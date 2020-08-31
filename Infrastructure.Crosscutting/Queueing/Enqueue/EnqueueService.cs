using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Queueing;
using Application.Contracts.Infrastructure.Queueing.Enqueue;
using Application.Infrastructure.Queueing;
using Newtonsoft.Json;

namespace Infrastructure.Crosscutting.Queueing.Enqueue
{
    public class EnqueueService : IEnqueueService
    {
        private readonly IAppSettingsService _appSettingsService;

        public EnqueueService(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }

        public void Execute(IQueueable queueable, QueueItemType type)
        {
            using (var context = new QueueContext(_appSettingsService.ConnectionString))
            {
                var queueItem = new QueueItem(type, JsonConvert.SerializeObject(queueable));

                context.QueueItems.Add(queueItem);
                context.SaveChanges();
            }
        }
    }
}