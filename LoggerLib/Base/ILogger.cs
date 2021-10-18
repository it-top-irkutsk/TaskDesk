using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLib
{
    public interface ILogger
    {
        public void MakeLog(string taskInfo);
    }
}
