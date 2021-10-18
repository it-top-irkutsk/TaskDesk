using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLib.BaseClasses
{
    public class Logger : ILogger
    {
        private static string _path { get; set; }
        public void MakeLog(string taskInfo)
        {
            taskInfo = $"{DateTime.Today} : {taskInfo ?? throw new Exception("Переданный лог неверен")}";
            _path = @"\Logs";
            DirectoryInfo dir = new DirectoryInfo(_path);
            if (!dir.Exists)
            {
                dir.Create();
            }
            using FileStream fstream = new FileStream($@"{_path}\logs.txt", FileMode.OpenOrCreate);
            byte[] array = System.Text.Encoding.Default.GetBytes(taskInfo);
            fstream.Write(array, 0, array.Length);
        }
    }
}
