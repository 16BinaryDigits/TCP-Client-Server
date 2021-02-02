using System;
using TCPC;

namespace ClientConsole
{
    class Program
    {
        static string _ip;
        static int _port;

        static void Main(string[] args)
        {
            _ip = General.GetLocalIP(System.Net.Sockets.AddressFamily.InterNetwork).ToString();
            _port = 8888;

            ConfigureConsole();

            Info();
            Loop();
        }

        static void Loop()
        {
            Console.ReadLine();

            new Clone().Connect(new Config { IP = _ip, Port = _port }, new Write { Message = $"OK- From Client", ID = DateTime.Now.Ticks.ToString() });

            Loop();
        }

        static void ConfigureConsole()
        {
            Console.Title = $"TCP Client";
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.CursorVisible = false;
        }

        static void Info()
        {
            General.WriteToConsole($"*******************************");
            General.WriteToConsole($"          TCP Client           ");
            General.WriteToConsole($"*******************************");
            General.WriteToConsole($"Server IP    : {_ip}");
            General.WriteToConsole($"Server Port  : {_port}");
            General.WriteToConsole($"OS           : {Environment.OSVersion}");
            General.WriteToConsole($".Net         : {Environment.Version}");
            General.WriteToConsole($"*******************************");
            General.WriteToConsole($"");
            General.WriteToConsole("Press Enter to send data to the server :");
        }
    }

    class Clone : Client
    {
        public Clone() : base()
        {

        }

        public Clone(Config config) : base(config)
        {

        }

        public override void BackgroundWork(Read read)
        {
            try
            {
                base.BackgroundWork(read);
                if (read.Exception is null)
                {
                    General.WriteToConsole(data: $"IP-[{read.IP}] RID-[{read.ID}] Core-[{read.CoreID:00}] Thread-[{read.ThreadID:00}] Buffer-[{read.ReceivedBufferSize:0000000}] MSG-[{read.Message}]");
                    return;
                }

                General.WriteToConsole(data: $"Exception-[{read.Exception.Message}]", textColor: ConsoleColor.Red);
            }
            catch { }
        }
    }
}
