using System;

namespace TCPC
{
    // modify to match your needs
    public class Read
    {
        public string ID { get; set; } = string.Empty;

        public string IP { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public int ReceivedBufferSize { get; set; } = 0;

        public int CoreID { get; set; } = 0;

        public int ThreadID { get; set; } = 0;

        public Exception Exception { get; set; } = null;
    }
}
