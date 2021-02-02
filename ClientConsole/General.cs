using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace TCPC
{
    internal class General
    {
        internal static void WriteToConsole(string data, ConsoleColor textColor = ConsoleColor.White, ConsoleColor textBackground = ConsoleColor.Black)
        {
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = textBackground;
            Console.WriteLine(data);
        }

        internal static IPAddress GetLocalIP(AddressFamily addressFamily)
        {
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(address => address.AddressFamily == addressFamily).FirstOrDefault();
            }
            catch { return null; }
        }

        internal static string SerializeToJson(object obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj);
            }
            catch { return string.Empty; }
        }

        internal static object DeserializeToObject<type>(string data)
        {
            try
            {
                return JsonSerializer.Deserialize<type>(data);
            }
            catch { return null; }
        }
    }
}
