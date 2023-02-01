// NuGet: Azure.Messaging.ServiceBus 

namespace SBSendReceiveDelete
{
    public class Program
    {
        private const string _queueName = "rtqueuedemo";
        static void Main(string[] args)
        {
            var queueService = new QueueService();
            queueService.SendMessageAsync(_queueName).GetAwaiter().GetResult();
            queueService.DeleteMessagesInQueue(_queueName, 100).GetAwaiter().GetResult();
        }
    }
}