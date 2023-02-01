using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.ServiceBus;
using System.Diagnostics;

namespace SBSendReceiveDelete
{
    public class QueueService
    {
        private const string _connectionString = "TODO";

        /// <summary>
        /// Sends up to three messages entered by the user
        /// </summary>
        /// <param name="queueName">The queue name</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task SendMessageAsync(string queueName)
        {
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("Missing queue name. Please provide one and try again!");

            try
            {
                // Create a service bus client
                var client = new ServiceBusClient(_connectionString);

                // Create a sender for the queue.
                var sender = client.CreateSender(queueName);

                var lines = new List<string> { "first", "second", "third" };
                var messageTasks = new List<Task>();

                lines.ForEach(line =>
                {
                    Console.WriteLine($"Please enter the {line} message you want to send end press enter button");
                    var inputLine = Console.ReadLine();
                    Console.Clear();

                    if (!string.IsNullOrEmpty(inputLine))
                    {
                        var message = new ServiceBusMessage(inputLine);
                        messageTasks.Add(Task.Run(async () => await sender.SendMessageAsync(message)));
                    }
                });

                if (messageTasks.Any())
                {
                    await using (client)
                    {
                        await Task.WhenAll(messageTasks).ConfigureAwait(false); // The completion order of tasks in Task.WhenAll does not match the order in which they were added.
                                                                                // This means that the order of messages sent to a queue may not be preserved.
                                                                                // Use only in cases where message order is not crucial."

                        var count = messageTasks.Count;
                        var output = count > 1 ? $"{count} messages were successfully sent!" : "One message has been successfully sent";

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(output + Environment.NewLine);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Missing inputs. Messages have not been sent!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong!");
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Receives and deletes messages
        /// </summary>
        /// <param name="queueName">The queue name</param>
        /// <param name="maxMessages">The maximum number of messages that will be received.</param>
        /// <returns></returns>
        public async Task DeleteMessagesInQueue(string queueName, ushort maxMessages)
        {
            try
            {
                var client = new ServiceBusClient(_connectionString);

                var receiver = client.CreateReceiver(queueName, new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

                var messages = await receiver.ReceiveMessagesAsync(maxMessages).ConfigureAwait(false);

                /*
                 * Important!
                  The ServiceBusReceivedMessage method may not return exactly maxMessages messages even if there are maxMessages messages available in the queue or topic due to
                  the possibility of race conditions and other factors. A race condition occurs when two or more processes access the same resource simultaneously 
                  and try to modify it at the same time, leading to unpredictable results.
                  Additionally, other factors such as network latency, server load, and message processing time can also impact the number of messages returned by the ServiceBusReceivedMessage method
                */

                var output = messages.Count > 1 ? $"Messages to delete: {messages.Count}" : "One message to delete";

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{output}" + Environment.NewLine);
                Console.WriteLine("The process of deleting has started.." + Environment.NewLine );

                messages.ToList().ForEach(msg =>
                {
                    Thread.Sleep(1000); // just for funn
                    Console.WriteLine($"Deleted message: {msg?.Body?.ToString()}");
                });

                Console.WriteLine();
                Console.WriteLine("The process of deleting is complete..");
                Console.ForegroundColor = ConsoleColor.White;

                }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(e.Message);
            }
        }
    }
}
