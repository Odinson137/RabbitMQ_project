using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ.RabbitMq;

public class RabbitMqService : IRabbitMqService
{
    public void SendMessage(object obj)
    {
        var content = JsonConvert.SerializeObject(obj);
        SendMessage(content);
    }

    public void SendMessage(string message)
    {
        // var factory = new ConnectionFactory() { Uri = new Uri()}; // если через докер, иначе просто localhost
        var factory = new ConnectionFactory() { HostName = "rabbitmq" }; // если через докер, иначе просто localhost
        using var connection = factory.CreateConnection();
        using (var channel = connection.CreateChannel())
        {
            channel.QueueDeclare(queue: "MyFirstQueue",
                durable: false,
                exclusive: false, 
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            
            channel.BasicPublish(exchange: "",
                routingKey: "MyFirstQueue",
                body: body);
        };
        
    }
}