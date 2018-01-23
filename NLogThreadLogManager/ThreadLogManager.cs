﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Config;

namespace NLogThreadLogManager
{
   public class ThreadLogManager
   {
      private static readonly ThreadLocal<string> ThreadAlias = new ThreadLocal<string>();

      public static LoggingConfiguration Configuration = new LoggingConfiguration();
      public static string ThreadName
      {
         get
         {
            string threadName = "";
            if (ThreadAlias.IsValueCreated)
               threadName = ThreadAlias.Value;
            else
               threadName = Thread.CurrentThread.Name;

            if (threadName == null)
               threadName = Thread.CurrentThread.ToString();

            return threadName;
         }

         set { ThreadAlias.Value = value; }
      }

      /// <summary>
      /// This was taken from NLog.Internal.StackTraceUsageUtils
      /// Gets the fully qualified name of the class invoking the calling method, including the 
      /// namespace but not the assembly.    
      /// </summary>
      [MethodImpl(MethodImplOptions.NoInlining)]
      public static string GetCallingClassFullName(int framesToSkip = 2)
      {
         string className;

         Type declaringType;

         do
         {

            StackFrame frame = new StackFrame(framesToSkip, false);
            MethodBase method = frame.GetMethod();
            declaringType = method.DeclaringType;
            if (declaringType == null)
            {
               className = method.Name;
               break;
            }

            framesToSkip++;
            className = declaringType.FullName;
         } while (className.StartsWith("System.", StringComparison.Ordinal));

         return className;
      }

      public static ThreadLoggerFactory GetCurrentClassLoggerFactory()
      {
         return new ThreadLoggerFactory(GetCallingClassFullName());
      }
   }
}
