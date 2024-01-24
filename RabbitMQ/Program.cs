using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.RabbitMq;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllersWithViews();

services.AddScoped<IRabbitMqService, RabbitMqService>();

services.AddSwaggerGen();

services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


var factory = new ConnectionFactory() { HostName = "rabbitmq" };
// var factory = new ConnectionFactory() { HostName = "rabbitmq" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateChannel();

channel.QueueDeclare(queue: "MyFirstQueue",
    durable: false,
    exclusive: false, 
    autoDelete: false,
    arguments: null);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received message: {message}");
};

channel.BasicConsume(queue: "MyFirstQueue", autoAck: true, consumer: consumer);


app.Run();