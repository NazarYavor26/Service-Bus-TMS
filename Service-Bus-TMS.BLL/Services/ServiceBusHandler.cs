using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service_Bus_TMS.DAL.Entities;

namespace Service_Bus_TMS.BLL.Services;

public class ServiceBusHandler : IServiceBusHandler
{
    private const string QUEUE_NAME = "dev-queue1";
    private const int MILLISECONDS_DELAY_RECEIVE = 500;
    
    public void SendMessage(Task task)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(
            queue: QUEUE_NAME,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
            
        string taskMessage = JsonConvert.SerializeObject(task);

        var body = Encoding.UTF8.GetBytes(taskMessage);

        channel.BasicPublish(
            exchange: "",
            routingKey: QUEUE_NAME,
            basicProperties: null,
            body: body);
    }

    public List<Task> ReceiveMessage()
    {
        List<Task> tasks = new List<Task>();
        var factory = new ConnectionFactory { HostName = "localhost" };
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(
            queue: QUEUE_NAME,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, e) =>
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var task = JsonConvert.DeserializeObject<Task>(message);
            tasks.Add(task);
            Console.WriteLine(" Received message: {0}", message);
        };
        
        
        channel.BasicConsume(
            queue: QUEUE_NAME,
            autoAck: true,
            consumer: consumer);
        
        while (true)
        {
            Thread.Sleep(MILLISECONDS_DELAY_RECEIVE);
            
            var messageCount = channel.MessageCount(QUEUE_NAME);
            if (messageCount == 0)
            {
                break;
            }
        }

        return tasks;
    }
}