using System.Text;

namespace TCPC
{
    // modify to match your needs
    public class Config
    {
        public string IP { get; set; } = string.Empty;

        public int Port { get; set; } = 8888;

        public int ReadBufferSize { get; set; } = 1024;

        public int MaxReceivedBufferSize { get; set; } = 1048576;

        public Encoding Encoding { get; set; } = Encoding.ASCII;
    }
}
