using System;
using RabbitMQ.Client;
using plccontrol;

namespace receiveSwitch
{
    class Program
    {
        static void Main(string[] args)
        {
            rabbitmq rbmq = rabbitmq.Instance(); // 객체 생성 (+mq 연결)
            rbmq.plccontrol += plcControl; 
            
            Thread readmessage = new Thread(new ThreadStart(rbmq.readONOFF));
            readmessage.Start();

            Thread.Sleep(-1);
        }
        // 버튼 이벤트 등록
        static void plcControl(object sender, EventArgs e)
        {
            string[] x = (string[])sender;
            Console.WriteLine($"이벤트 발생 : {x[0]}");
        }
    }
}