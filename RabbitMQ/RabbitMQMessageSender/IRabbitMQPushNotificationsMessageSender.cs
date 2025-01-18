namespace HomeExam.AlertManagementAPI.RabbitMQ.RabbitMQMessageSender
{
    public interface IRabbitMQPushNotificationsMessageSender
    {
        Task SendMessage(object message);
    }
}
