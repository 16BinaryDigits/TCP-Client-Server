using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPS
{
    public class Server
    {
        // Extra: To get task processing cpu core for logging
        [DllImport("Kernel32.dll"), SuppressUnmanagedCodeSecurity]
        static extern int GetCurrentProcessorNumber();

        // Local variables:
        static TcpListener tcpListener = null;
        static Config tcpConfig = null;

        // Props:
        public Config Config { get { return tcpConfig; } }

        // Constructor:
        public Server() { tcpConfig = new Config { }; }

        public Server(Config config) { tcpConfig = config; }

        // Method:
        // Start accepting TCP connections
        public async void Start(Write serverResponse)
        {
            try
            {
                // Checks
                if (tcpConfig is null) tcpConfig = new Config { };
                if (tcpListener is null) tcpListener = new TcpListener(General.GetLocalIP(AddressFamily.InterNetwork), tcpConfig.Port);

                // Start up the TcpListener
                tcpListener.Start();

                // Loop waiting for client connections
                while (true)
                {
                    using (var client = await tcpListener.AcceptTcpClientAsync())
                    {
                        if (client.Connected) await ClientConnectionAsync(client, tcpConfig, serverResponse, BackgroundWork);
                    }
                }
            }
            catch { tcpListener = null; }
        }

        // Method:
        // Stop accepting TCP connections
        public void Stop()
        {
            try
            {
                if (tcpListener != null) tcpListener.Stop();
            }
            catch { tcpListener = null; }
        }

        // Method:
        // Handling connected clients instances
        async Task ClientConnectionAsync(TcpClient client, Config config, Write write, Action<Read> action = null)
        {
            try
            {
                var read = new Read();
                client.NoDelay = true;

                using (NetworkStream networkStream = new NetworkStream(client.Client))
                {

                    // handling stream writing
                    if (networkStream.CanWrite && write.Message != string.Empty)
                    {
                        var buffer = config.Encoding.GetBytes(General.SerializeToJson(write));
                        await networkStream.WriteAsync(buffer, 0, buffer.Length);
                    }

                    // handling stream reading
                    if (networkStream.CanRead)
                    {
                        int byteCount = 0;
                        StringBuilder stringBuilder = new StringBuilder();
                        var buffer = new byte[config.ReadBufferSize];

                        do
                        {
                            byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                            stringBuilder.AppendFormat("{0}", config.Encoding.GetString(buffer, 0, byteCount));

                            if (byteCount >= config.MaxReceivedBufferSize) break;
                        }
                        while (networkStream.DataAvailable);

                        // Setting the response type props ...
                        // You can modify the model to match your needs
                        read.IP = client.Client.RemoteEndPoint.ToString();
                        read.ID = write.ID;
                        read.Message = stringBuilder.ToString();
                        read.ReceivedBufferSize = byteCount;
                        read.CoreID = GetCurrentProcessorNumber();
                        read.ThreadID = Thread.CurrentThread.ManagedThreadId;

                        if (action != null) { action.Invoke(read); }
                    }
                }
            }
            catch (Exception exception) { action.Invoke(new Read { Exception = exception }); }
        }

        // Method:
        // Override this method to process the client request data
        public virtual void BackgroundWork(Read read)
        {

        }
    }
}
