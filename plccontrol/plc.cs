using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using NModbus; // plc 연결

namespace plccontrol
{
    internal class plc
    {

        private TcpClient client;
        private IModbusMaster master;  // NModbus4의 Modbus Master
        private bool isRunning;
        string[] switchlist = new string[] { "airspray Pressure", "paintflow Pressure" };
        /// <summary>
        /// Modbus TCP 서버(LS PLC)에 연결 (Slave ID는 PLC 설정값에 따라 조정)
        /// </summary>
        public void ConnectToServer(string serverIp, int port, byte slaveId = 1)
        {
            try
            {
                // 1) PLC(IP: serverIp, Port: port)와 TcpClient로 연결
                client = new TcpClient(serverIp, port);

                // 2) NModbus Factory를 이용해 Master 생성
                var factory = new ModbusFactory();
                master = factory.CreateMaster(client);

                isRunning = true;
                Console.WriteLine($"Modbus TCP 연결 성공: {serverIp}:{port}, Slave ID={slaveId}");
            }
            catch (Exception ex)
            {
                LogError($"Failed to connect: {ex.Message}");
                Console.WriteLine("Retrying connection in 5 seconds...");
                Thread.Sleep(5000);
                // 재시도
                ConnectToServer(serverIp, port, slaveId);
            }
        }

        /// <summary>
        /// 현재 센서상태 콘솔 출력 (원하면 주석 해제)
        /// </summary>
        public void DisplayStatus(string[] arr)
        {
            Console.WriteLine("=================================");
            Console.WriteLine($"switch : {switchlist[int.Parse(arr[0])]}");
            Console.WriteLine($"ON/OFF : {arr[1]}");
            Console.WriteLine("=================================\n");
        }

        /// <summary>
        /// Modbus Master를 통해 PLC Holding Register에 쓰기
        /// </summary>
        public void SendData(string[] arr, byte slaveId = 1, ushort startAddress = 0, bool isON = true)
        {
            try
            {
                // 한 번만 수행하면 되어서 굳이 while x
                // 2) 콘솔 출력(주석 해제 가능)
                this.DisplayStatus(arr);
                master.WriteSingleCoil(slaveId, startAddress, isON);
                Console.WriteLine("success~~");
            }
            catch (Exception ex)
            {
                LogError($"Error during data transmission: {ex.Message}");
                isRunning = false;
            }
        }

        private void LogError(string message)
        {
            File.AppendAllText("error.log", $"{DateTime.Now}: {message}\n");
        }
    }
}
