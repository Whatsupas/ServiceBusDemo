// NuGet: Azure.Messaging.ServiceBus 

namespace SBSendReceiveDelete
{
    public class Program
    {
        private const string _queueName = "rtqueuedemo";
        private const string _topicName = "rttopicdemo";
        static void Main(string[] args)
        {
            var sbService = new ServiceBusService();

            // I'm using a single console terminal for testing purposes
            sbService.SendMessageToQueueAsync(_queueName).GetAwaiter().GetResult();
            sbService.ReceiveAndDeleteMessagesFromQueue(_queueName, 2).GetAwaiter().GetResult();
            sbService.SendMessageToTopicAsync(_topicName).GetAwaiter().GetResult();
        }
    }
}