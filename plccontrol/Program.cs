using System;
using RabbitMQ.Client;
using plccontrol;

namespace receiveSwitch
{
    class Program
    {
        static plc p;
        static string serverIp = "192.168.10.200";
        static int serverPort = 502;       // Modbus TCP 기본 포트는 502
        static byte slaveId = 1;          // PLC에서 설정한 Slave ID
        static void Main(string[] args)
        {
            rabbitmq rbmq = rabbitmq.Instance(); // 객체 생성 (+mq 연결)
            p = new plc();
            rbmq.plccontrol += plcControl;
            //접속 정보 (IP, Port, Slave ID) 실제 PLC에 맞춰 조정
            
            p.ConnectToServer(serverIp, serverPort, slaveId); // server 연결

            Thread readmessage = new Thread(new ThreadStart(rbmq.readONOFF));
            readmessage.Start();

            Thread.Sleep(-1);
        }


        // 버튼 이벤트 등록
        static void plcControl(object sender, EventArgs e)
        {
            string[] switchdata = (string[])sender;
            // plc 객체 생성
            if (switchdata[0] == "0")
            {
                if (switchdata[1] == "1")
                {
                    //on
                    p.SendData(switchdata, slaveId, (Int32)134, true);
                }
                else
                {
                    //off
                    p.SendData(switchdata, slaveId, (Int32)134, false);
                }
            }
            else if (switchdata[0] == "1")
            {
                if (switchdata[1] == "1")
                {
                    //on
                    p.SendData(switchdata, slaveId, 135, true);
                }
                else
                {
                    //off
                    p.SendData(switchdata, slaveId, 135, false);
                }
            }
        }
    }
}