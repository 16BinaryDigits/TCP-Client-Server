using System;
using TCPS;

namespace ServerConsole
{
    class Program
    {
        static Clone server = new Clone();

        static void Main(string[] args)
        {
            ConfigureConsole();

            server.Start(new Write { Message = "OK- From Server", ID = DateTime.Now.Ticks.ToString() });

            Info();
            Loop();
        }

        static void Loop()
        {
            Console.Read();
            Loop();
        }

        static void ConfigureConsole()
        {
            Console.Title = $"{General.GetLocalIP(System.Net.Sockets.AddressFamily.InterNetwork)}:{server.Config.Port}";
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.CursorVisible = false;
        }

        static void Info()
        {
            General.WriteToConsole($"*******************************");
            General.WriteToConsole($"          TCP SERVER           ");
            General.WriteToConsole($"*******************************");
            General.WriteToConsole($"Server IP    : {General.GetLocalIP(System.Net.Sockets.AddressFamily.InterNetwork)}");
            General.WriteToConsole($"Server Port  : {server.Config.Port}");
            General.WriteToConsole($"OS           : {Environment.OSVersion}");
            General.WriteToConsole($".Net         : {Environment.Version}");
            General.WriteToConsole($"*******************************");
            General.WriteToConsole($"");
            General.WriteToConsole($"Server is running and waiting for connections ...");
            General.WriteToConsole($"");
        }
    }

    class Clone : Server
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
