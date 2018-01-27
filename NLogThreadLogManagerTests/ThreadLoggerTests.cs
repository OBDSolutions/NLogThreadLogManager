using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLogThreadLogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace NLogThreadLogManager.Tests
{
    [TestClass()]
    public class ThreadLoggerTests
    {
        [TestMethod()]
        public void ThreadLoggerTest()
        {
            LogManager.Configuration = new LoggingConfiguration();
            ThreadLoggerFactory threadLoggerFactory = ThreadLogManager.GetCurrentClassLoggerFactory();

            MemoryTarget target1 = new MemoryTarget("Test123");
            LogManager.Configuration.AddRuleForAllLevels(target1, "Thread1.*");
            LogManager.ReconfigExistingLoggers();

            ThreadLogManager.ThreadName = "Thread1";
            threadLoggerFactory.Logger.Debug("Test Thread1");


            MemoryTarget target2 = new MemoryTarget("Test123");
            Thread thread = new Thread(() =>
            {

                LogManager.Configuration.AddRuleForAllLevels(target2, "Thread2.*");
                LogManager.ReconfigExistingLoggers();

                ThreadLogManager.ThreadName = "Thread2";
                threadLoggerFactory.Logger.Debug("Test Thread2");
            });

            thread.Start();

            while (thread.ThreadState != ThreadState.Stopped)
                Thread.Sleep(10);

            Assert.AreEqual(target1.Logs.Count, 1);
            Assert.AreEqual(target2.Logs.Count, 1);
        }
    }
}