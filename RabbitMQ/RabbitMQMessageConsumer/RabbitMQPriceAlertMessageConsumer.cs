using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using HomeExam.AlertManagementAPI.Services.IServices;
using System.Text;
using Newtonsoft.Json;
using HomeExam.AlertManagementAPI.Models.Dto;

namespace HomeExam.AlertManagementAPI.RabbitMQ.RabbitMQMessageConsumer
{
    public class RabbitMQPriceAlertMessageConsumer : BackgroundService
    {
        private const string _hostName = "localhost";
        private const string _username = "guest";
        private const string _password = "guest";

        private readonly string _priceAlertQueueName;
        private IConnection _connection;
        private IConfiguration _configuration;
        private IChannel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQPriceAlertMessageConsumer(IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
            _priceAlertQueueName = _configuration.GetValue<string>("QueuesNames:PriceAlertQueueName");
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var priceService = scope.ServiceProvider.GetRequiredService<IPriceService>();

                if (await ConnectionExists())
                {
                    _channel = await _connection.CreateChannelAsync();

                    await _channel.QueueDeclareAsync(_priceAlertQueueName, false, false, false, null);

                    stoppingToken.ThrowIfCancellationRequested();

                    var priceAlertConsumer = new AsyncEventingBasicConsumer(_channel);

                    priceAlertConsumer.ReceivedAsync += async (channel, eventArgument) =>
                    {
                        var content = Encoding.UTF8.GetString(eventArgument.Body.ToArray());

                        IEnumerable<PriceAlertDto> priceAlertDtoList = JsonConvert.DeserializeObject<IEnumerable<PriceAlertDto>>(content);

                        if (priceAlertDtoList != null && priceAlertDtoList.Any())
                        {
                            await priceService.UpdatePrice(priceAlertDtoList);
                        }

                        await _channel.BasicAckAsync(eventArgument.DeliveryTag, false);
                    };

                    await _channel.BasicConsumeAsync(_priceAlertQueueName, false, priceAlertConsumer);
                }
            }
        }
    }
}
