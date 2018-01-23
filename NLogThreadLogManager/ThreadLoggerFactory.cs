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
      private readonly ThreadLocal<string> _threadAlias = new ThreadLocal<string>();
      private readonly Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();
      private readonly string _fullClassName;

      public ILogger Logger
      {
         get
         {
            string threadName = ThreadLogManager.ThreadName;

            lock (_loggers)
            {
               if (!_loggers.ContainsKey(threadName))
                  _loggers[threadName] = LogManager.GetLogger(threadName + "." + _fullClassName);

               return _loggers[threadName];
            }


         }
      }

      public ThreadLoggerFactory(string fullClassName)
      {
         this._fullClassName = fullClassName;
      }
   }
}
