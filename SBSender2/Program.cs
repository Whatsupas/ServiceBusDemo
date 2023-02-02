// NuGet: Azure.Messaging.ServiceBus 

namespace SBSendReceiveDelete
{
    public class Program
    {
        private const string _queueName = "rtqueuedemo";
        private const string _topicName = "rttopicdemo";
        static void Main(string[] args)
        {
            var queueService = new ServiceBusService();

            // I'm using a single console terminal for testing purposes"
            queueService.SendMessageToQueueAsync(_queueName).GetAwaiter().GetResult();
            queueService.ReceiveAndDeleteMessagesFromQueue(_queueName, 2).GetAwaiter().GetResult();
            queueService.SendMessageToTopicAsync(_topicName).GetAwaiter().GetResult();
        }
    }
}