using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace HomeExam.AlertManagementAPI.RabbitMQ.RabbitMQMessageSender
{
    public class RabbitMQPushNotificationsMessageSender : IRabbitMQPushNotificationsMessageSender
    {
        private const string _hostName = "localhost";
        private const string _username = "guest";
        private const string _password = "guest";

        private readonly string _pushNotificationsQueueName;
        private IConfiguration _configuration;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMQPushNotificationsMessageSender(IConfiguration configuration) 
        {
            _configuration = configuration;
            _pushNotificationsQueueName = _configuration.GetValue<string>("QueuesNames:PushNotificationsQueueName");
        }

        private async Task CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _username,
                    Password = _password,
                };

                _connection = await factory.CreateConnectionAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task<bool> ConnectionExists()
        {
            if (_connection == null)
            {
                await CreateConnection();
            }

            return true;
        }

        public async Task SendMessage(object message)
        {
            if (await ConnectionExists())
            {
                _channel = await _connection.CreateChannelAsync();

                await _channel.QueueDeclareAsync(_pushNotificationsQueueName, false, false, false, null);

                var json = JsonConvert.SerializeObject(message);

                var body = Encoding.UTF8.GetBytes(json);

                await _channel.BasicPublishAsync(exchange: "", routingKey: _pushNotificationsQueueName, false, body: body);
            }
        }
    }
}
