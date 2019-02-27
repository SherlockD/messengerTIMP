using System.IO;
using System.Linq;

namespace Messenger
{
    static class LastExitSerialization
    {
        public static void Serialize(string port,string data)
        {
            if (!File.Exists("serialize.txt"))
            {
                var file = File.Create("serialize.txt");
                file.Close();
            }

            var readedLines = File.ReadAllLines("serialize.txt").ToList();
            for(int i = 0; i < readedLines.Count; i++)
            {
                if(readedLines[i].Substring(0,4) == port)
                {
                    readedLines[i] =  $"{port}:{data}";
                    File.WriteAllLines("serialize.txt", readedLines.ToArray());
                    return;
                }
            }
            using(var streamWriter = new StreamWriter("serialize.txt",true))
            {
                streamWriter.WriteLine($"{port}:{data}");
            }
        }

        public static string GetPortData(string port)
        {
            var streamReader = new StreamReader("serialize.txt");
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if(line.Substring(0,4) == port)
                {
                    streamReader.Close();
                    return line.Substring(5);
                }
            }
            streamReader.Close();
            return string.Empty;
        }
    }
}
