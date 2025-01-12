using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Miths.Com
{
    public class UDPSlidingWindow
    {
        static int WindowSize = 4; // Sliding window size
        static int PacketSize = 1024; // Size of each packet
        static int TotalPackets = 10; // Total number of packets to send
        static string Message = "Packet";

        public UDPSlidingWindow(int windowSize, int totalPackets, string msg)
        {
            WindowSize = windowSize;
            if(totalPackets > 8) TotalPackets = totalPackets;
            if (!string.IsNullOrEmpty(msg) && msg.Length > 1)
                Message = msg;
            Thread receiverThread = new Thread(Receiver);
            receiverThread.Start();

            Thread.Sleep(1000); // Give the receiver a moment to start

            Sender();

            receiverThread.Join();
        }

        static void Sender()
        {
            UdpClient sender = new UdpClient();
            IPEndPoint receiverEndpoint = new IPEndPoint(IPAddress.Loopback, 11000);

            int baseIndex = 0;
            int nextSeqNum = 0;
            byte[][] packets = new byte[TotalPackets][];

            // Prepare packets
            for (int i = 0; i < TotalPackets; i++)
            {
                string message = Message + " " + i;// $"Packet {i}";
                packets[i] = Encoding.UTF8.GetBytes(message);
            }

            Console.WriteLine("[Sender] Starting transmission...");

            while (baseIndex < TotalPackets)
            {
                // Send packets within the window
                while (nextSeqNum < baseIndex + WindowSize && nextSeqNum < TotalPackets)
                {
                    sender.Send(packets[nextSeqNum], packets[nextSeqNum].Length, receiverEndpoint);
                    Console.WriteLine($"[Sender] Sent: Packet {nextSeqNum}");
                    nextSeqNum++;
                }

                // Wait for acknowledgment
                try
                {
                    sender.Client.ReceiveTimeout = 2000; // Timeout in milliseconds
                    byte[] ackData = sender.Receive(ref receiverEndpoint);
                    string ackMessage = Encoding.UTF8.GetString(ackData);

                    if (ackMessage.StartsWith("ACK"))
                    {
                        int ackNum = int.Parse(ackMessage.Split(' ')[1]);
                        Console.WriteLine($"[Sender] Received: {ackMessage}");

                        if (ackNum >= baseIndex)
                        {
                            baseIndex = ackNum + 1;
                        }
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("[Sender] Timeout waiting for ACK. Resending window...");
                    nextSeqNum = baseIndex;
                }
            }

            Console.WriteLine("[Sender] Transmission complete.");
            sender.Close();
        }

        static void Receiver()
        {
            UdpClient receiver = new UdpClient(11000);
            IPEndPoint senderEndpoint = new IPEndPoint(IPAddress.Any, 0);

            bool[] received = new bool[TotalPackets];
            Console.WriteLine("[Receiver] Ready to receive packets...");

            while (Array.Exists(received, r => !r))
            {
                byte[] packet = receiver.Receive(ref senderEndpoint);
                string message = Encoding.UTF8.GetString(packet);

                Console.WriteLine($"[Receiver] Received: {message}");
                string[] msgPrts = message.Split(' ');
                int packetNum = int.Parse(msgPrts[msgPrts.Length - 1]);// int.Parse(message.Split(' ')[1]);
                if (!received[packetNum])
                {
                    received[packetNum] = true;

                    // Send acknowledgment
                    string ackMessage = $"ACK {packetNum}";
                    byte[] ackData = Encoding.UTF8.GetBytes(ackMessage);
                    receiver.Send(ackData, ackData.Length, senderEndpoint);

                    Console.WriteLine($"[Receiver] Sent: {ackMessage}");
                }
            }

            Console.WriteLine("[Receiver] All packets received.");
            receiver.Close();
        }
    }
}
