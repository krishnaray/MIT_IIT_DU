namespace UDPSlidingWindow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int windowSize = 2, totalPackets = 10;
            Console.WriteLine("Window Size: ");
            if(int.TryParse(Console.ReadLine(), out int ws))
                windowSize = ws;
            Console.WriteLine("Total PAckets: ");
            if (int.TryParse(Console.ReadLine(), out int tp))
                totalPackets = tp;
            Console.WriteLine("Message: ");
            string? msg = Console.ReadLine();
            Miths.Com.UDPSlidingWindow uDPSlidingWindow = new Miths.Com.UDPSlidingWindow(windowSize, totalPackets, msg);
        }
    }
}
