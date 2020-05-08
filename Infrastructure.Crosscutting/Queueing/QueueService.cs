using Infrastructure.Crosscutting.AppSettings;
using Newtonsoft.Json;

namespace Infrastructure.Crosscutting.Queueing
{
    public class QueueService : IQueueService
    {
        private readonly IAppSettingsService _appSettingsService;

        public QueueService(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }

        public void Enqueue(QueueItemType type, IQueueable queueable)
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