using System.Collections.Concurrent;
using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace MqttHub.Bus;

// public class MqttBus(ISender mediator,IMqttClient mqttClient,IServiceScopeFactory scopeFactory,IManagedMqttClient managedMqttClient):IMqttBus
// {
//     private readonly ConcurrentBag<string> _messages;
//     public readonly ConcurrentBag<string>  _subscribedTopics;
//
//     private readonly MqttFactory _mqttFactory=new() ;
//     
//     public Task SendCommand<T>(T command) where T : Commands.Command
//     {
//         return mediator.Send(command);
//     }
//     public async Task ManagedMqttPublish<T>(T message, string topic) where T : class
//     {
//         var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
//
//         if (await ConnectMqttClientAsync() || mqttClient!= null && mqttClient.IsConnected )
//         {
//
//             await mqttClient.PublishAsync(new MqttApplicationMessage()
//             {
//                 Topic = topic,
//                 PayloadSegment = data,
//                 QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
//                 Retain = true
//             });
//         }
//     }
//
//     public async Task SubscribeToTopic(string topic)
//     {
//         if (await ConnectMqttClientAsync() || mqttClient!=null && mqttClient.IsConnected)
//         {
//             mqttClient.ApplicationMessageReceivedAsync += async e =>
//             {
//                 var message=e.ApplicationMessage.ConvertPayloadToString();
//                 //_messages.Add(message);
//                 if (MessageReceived != null)
//                     await MessageReceived(message, e.ApplicationMessage.Topic);
//                 await Task.CompletedTask;
//             };
//             await mqttClient.SubscribeAsync(topic);
//             
//         }
//     }
//     public IEnumerable<string> GetMessages()
//     {
//         return _messages;
//     }
//     public IEnumerable<string> GetSubscribedTopics()
//     {
//         return _subscribedTopics.Distinct();
//     }
//
//     public event Func<string, string, Task>? MessageReceived;
//
//     private async Task<bool> ConnectManagedMqttClientAsync()
//     {
//         var result = false;
//         try
//         {
//             if (!managedMqttClient.IsConnected)
//             {
//                 var managedClientOption = new MqttClientOptionsBuilder()
//                     .WithClientId("AdminManagedMqttClient")
//                     .WithTcpServer("localhost", 1883)
//                     .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
//                     .WithCleanSession(true)
//                     .WithKeepAlivePeriod(TimeSpan.FromHours(2));
//                 managedMqttClient = _mqttFactory.CreateManagedMqttClient();
//
//                 var managedMqttClientOption = new ManagedMqttClientOptionsBuilder()
//                     .WithClientOptions(managedClientOption)
//                     .WithMaxPendingMessages(10)
//                     //.WithPendingMessagesOverflowStrategy(MqttPendingMessagesOverflowStrategy.DropOldestQueuedMessage)
//                     .WithAutoReconnectDelay(TimeSpan.FromSeconds(1))
//                     .Build();
//                 await managedMqttClient.StartAsync(managedMqttClientOption);
//                 result = true;
//             }
//         }
//         catch (Exception ex)
//         {
//             result = false;
//         }
//         return result;
//     }
//     private async Task<bool> ConnectMqttClientAsync()
//     {
//         var  result=false;
//         try
//         {
//             if (!mqttClient.IsConnected)
//             {
//                 var options = new MqttClientOptionsBuilder()
//                     .WithTcpServer("localhost", 1883)
//                     .WithClientId("AdminSubscribeClient")
//                     .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
//                     .WithKeepAlivePeriod(TimeSpan.FromMinutes(60))
//                     .WithCleanSession()
//                     .Build();
//                 mqttClient = _mqttFactory.CreateMqttClient();
//                 await mqttClient.ConnectAsync(options);
//                 result = true;
//             }
//         }
//         catch (Exception e)
//         {
//             result=false;
//         }
//         return result;
//     }
//    
//     private async Task SubscribeToTopics(List<string> topics)
//     {
//         foreach (var topic in topics)
//         {
//              await managedMqttClient.SubscribeAsync(topic);
//             _subscribedTopics.Add(topic);
//         }
//     }
//
// }
public class MqttBus : IMqttBus
    {
        private readonly ISender _mediator;
        private readonly IMqttClient _mqttClient;
        private readonly IManagedMqttClient _managedMqttClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConcurrentBag<string> _messages = [];
        private readonly ConcurrentBag<string> _subscribedTopics = [];
        private readonly MqttFactory _mqttFactory = new();
        private readonly MqttClientOptions _options;
        public MqttBus(ISender mediator, IMqttClient mqttClient, IServiceScopeFactory scopeFactory, IManagedMqttClient managedMqttClient)
        {
            _mediator = mediator;
            _mqttClient = mqttClient;
            _scopeFactory = scopeFactory;
            _managedMqttClient = managedMqttClient;
            _options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .WithClientId("AdminSubscribeClient")
                .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithKeepAlivePeriod(TimeSpan.FromMinutes(60))
                // .WithCleanSession()
                .Build();
            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var message = e.ApplicationMessage.ConvertPayloadToString();
                _messages.Add(message);
                if (MessageReceived != null)
                    await MessageReceived(message, e.ApplicationMessage.Topic);
                await Task.CompletedTask;
            };
            
            _mqttClient.ConnectAsync(_options, CancellationToken.None);
        
            _mqttClient.DisconnectedAsync += async e =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5)); // Wait before reconnecting
                try
                {
                    await _mqttClient.ConnectAsync(_options, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Reconnecting failed: " + ex.Message);
                }
            };
            // _managedMqttClient.ApplicationMessageReceivedAsync += async e =>
            // {
            //     var message = e.ApplicationMessage.ConvertPayloadToString();
            //     _messages.Add(message);
            //     if (MessageReceived != null)
            //         await MessageReceived(message, e.ApplicationMessage.Topic);
            //     await Task.CompletedTask;
            // };
        }
        private bool IsConnected => _mqttClient.IsConnected;

        private async Task ConnectClient()
        {
            try
            {
                await _mqttClient.ConnectAsync(_options, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }
        }
        public Task SendCommand<T>(T command) where T : Commands.Command
        {
            return _mediator.Send(command);
        }

        public async Task ManagedMqttPublish<T>(T message, string topic) where T : class
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            if (!IsConnected)
            {
                await ConnectClient(); // Connect if not connected
            }

            if (IsConnected)
            {
                await _mqttClient.PublishAsync(new MqttApplicationMessage()
                {
                    Topic = topic,
                    Payload = data,
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
                    Retain = true
                });
            }
            
            // if (await ConnectMqttClientAsync() || _mqttClient != null && _mqttClient.IsConnected)
            // {
            //     await _mqttClient.PublishAsync(new MqttApplicationMessage()
            //     {
            //         Topic = topic,
            //         Payload = data,
            //         QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
            //         Retain = true
            //     });
            // }
        }

        public async Task SubscribeToTopic(string topic)
        {
            // if (await ConnectMqttClientAsync() || _mqttClient != null && _mqttClient.IsConnected)
            // {
            //     _mqttClient.ApplicationMessageReceivedAsync += async e =>
            //     {
            //         var message = e.ApplicationMessage.ConvertPayloadToString();
            //         _messages.Add(message);
            //         if (MessageReceived != null)
            //             await MessageReceived(message, e.ApplicationMessage.Topic);
            //         await Task.CompletedTask;
            //     };
            //     await _mqttClient.SubscribeAsync(topic);
            //     _subscribedTopics.Add(topic);
            // }
            if (!IsConnected)
            {
                await ConnectClient();
            }
            if (IsConnected)
            {
                await _mqttClient.SubscribeAsync(topic);
                _subscribedTopics.Add(topic);
            }
            else
            {
                throw new InvalidOperationException("MQTT client is not connected.");
            }
        }

        public IEnumerable<string> GetMessages()
        {
            return _messages;
        }

        public IEnumerable<string> GetSubscribedTopics()
        {
            return _subscribedTopics.Distinct();
        }

        public event Func<string, string, Task>? MessageReceived;

        private async Task<bool> ConnectManagedMqttClientAsync()
        {
            var result = false;
            try
            {
                if (!_managedMqttClient.IsConnected)
                {
                    var managedClientOption = new MqttClientOptionsBuilder()
                        .WithClientId("AdminManagedMqttClient")
                        .WithTcpServer("localhost", 1883)
                        .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                        .WithCleanSession(true)
                        .WithKeepAlivePeriod(TimeSpan.FromHours(2))
                        .Build();

                    var managedMqttClientOption = new ManagedMqttClientOptionsBuilder()
                        .WithClientOptions(managedClientOption)
                        .WithMaxPendingMessages(10)
                        .WithAutoReconnectDelay(TimeSpan.FromSeconds(1))
                        .Build();

                    await _managedMqttClient.StartAsync(managedMqttClientOption);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, etc.)
                result = false;
            }
            return result;
        }

        private async Task<bool> ConnectMqttClientAsync()
        {
            var result = false;
            try
            {
                if (!_mqttClient.IsConnected)
                {
                    var options = new MqttClientOptionsBuilder()
                        .WithTcpServer("localhost", 1883)
                        .WithClientId("AdminSubscribeClient")
                        .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                        .WithKeepAlivePeriod(TimeSpan.FromMinutes(60))
                        .WithCleanSession()
                        .Build();

                    await _mqttClient.ConnectAsync(options);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, etc.)
                result = false;
            }
            return result;
        }

        private async Task SubscribeToTopics(List<string> topics)
        {
            foreach (var topic in topics)
            {
                await _managedMqttClient.SubscribeAsync(topic);
                _subscribedTopics.Add(topic);
            }
        }
    }