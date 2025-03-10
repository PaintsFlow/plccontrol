using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace plccontrol
{
    internal class rabbitmq
    {
        /// <summary>
        /// 통신하기
        /// </summary>
        ConnectionFactory factory;
        IChannel channel;
        IConnection connection;
        static rabbitmq staticrabbitmq;
        public event EventHandler plccontrol; // plc 이벤트 컨트롤 등록

        public static rabbitmq Instance()
        {
            if (staticrabbitmq == null)
                staticrabbitmq = new rabbitmq();
            return staticrabbitmq;
        }
        private async Task _connect()
        {
            try
            {
                factory = new ConnectionFactory
                {
                    HostName = "211.187.0.113",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672
                };
                this.connection = await this.factory.CreateConnectionAsync();
                this.channel = await this.connection.CreateChannelAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // 큐 메세지 기다리는 함수
        public async void readONOFF()
        {
            try
            {
                // rabbit mq 메세지 sub하기
                await this._connect();
                QueueDeclareOk queueDeclareResult = await this.channel.QueueDeclareAsync();
                string queuename = queueDeclareResult.QueueName;
                await this.channel.QueueBindAsync(queue: queuename, exchange: "control", routingKey: string.Empty);
                var consumer = new AsyncEventingBasicConsumer(this.channel);
                
                consumer.ReceivedAsync += (model, ea) =>
                {
                    byte[] body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    
                    Console.WriteLine($"message : {message}");
                    string[] switchData = message.Split(", ");

                    // 기계 off시 수행
                    plccontrol?.Invoke(switchData, EventArgs.Empty);
                    
                    return Task.CompletedTask;
                };
                await this.channel.BasicConsumeAsync(queuename, autoAck: true, consumer: consumer);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        
    }
}
