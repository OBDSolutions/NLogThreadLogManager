using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace NLogThreadLogManager
{
    public class ThreadLoggerFactory
    {
        private readonly Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();
        private readonly string _fullClassName;

        public ILogger Logger
        {
            get
            {
                lock (_loggers)
                {
                    if (ThreadLogManager.ThreadName == null)
                        return LogManager.GetLogger(_fullClassName);

                    if (!_loggers.ContainsKey(ThreadLogManager.ThreadName))
                        _loggers[ThreadLogManager.ThreadName] = LogManager.GetLogger(ThreadLogManager.ThreadName + "." + _fullClassName);

                    return _loggers[ThreadLogManager.ThreadName];
                }
            }
        }

        public ThreadLoggerFactory(string fullClassName)
        {
            this._fullClassName = fullClassName;
        }
    }
}
