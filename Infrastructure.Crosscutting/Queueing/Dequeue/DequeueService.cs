using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Queueing.Dequeue.Resolvers;

namespace Infrastructure.Crosscutting.Queueing.Dequeue
{
    public class DequeueService : IDequeueService
    {
        private readonly IAppSettingsService _appSettingsService;
        private readonly ILogService _logService;

        private readonly IList<IDequeueResolver> _dequeueResolvers;

        public DequeueService(
            IAppSettingsService appSettingsService, 
            ILogService logService, 
            ILogDequeueResolver logDequeueResolver,
            IAuditDequeueResolver auditDequeueResolver,
            IEmailDequeueResolver emailDequeueResolver
        )
        {
            _appSettingsService = appSettingsService;
            _logService = logService;

            _dequeueResolvers = new List<IDequeueResolver>
            {
                logDequeueResolver,
                auditDequeueResolver,
                emailDequeueResolver
            };
        }

        public void Execute()
        {
            foreach (var queueItemType in Enum.GetValues(typeof(QueueItemType)).Cast<QueueItemType>())
            {
                var dequeueResolver = GetDequeueResolverFor(queueItemType);

                if (dequeueResolver == null)
                {
                    _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Dequeue Resolver not found | queueItemType={queueItemType}");
                    continue;
                }

                using (var dbContext = new QueueContext(_appSettingsService.ConnectionString))
                {
                    try
                    {
                        var queueItemsJsonData = dbContext.QueueItems.Where(qi => qi.Type == queueItemType).Select(qi => qi.Data).ToList();
                        dequeueResolver.ResolveDequeue(queueItemsJsonData);

                        dbContext.QueueItems.RemoveRange(dbContext.QueueItems.Where(qi => qi.Type == queueItemType));
                        dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | An exception has occurred resolving queue items | ex={ex}");
                    }
                }
            }
        }

        private IDequeueResolver GetDequeueResolverFor(QueueItemType queueItemType)
        {
            return _dequeueResolvers.FirstOrDefault(dr => dr.ResolvesQueueItemType(queueItemType));
        }
    }
}