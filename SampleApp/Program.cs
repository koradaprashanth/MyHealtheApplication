using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    class Program
    {
        private static string result;
        static void Main(string[] args)
        {

            WriteSomething();
            Console.WriteLine(result);
            Console.ReadLine();
            
        }
        static async Task<string> WriteSomething()
        {
            await Task.Delay(5);
            result = "Print Hello world!";
            return "Something";
        }


        public void DoProcess()
        {
            log4net.LogManager.GetLogger(GetType()).Info("Start processing..."); // TODO: Should log an information message.
            try
            {
                log4net.LogManager.GetLogger(GetType()).Warn("Critical section..."); // TODO: Should log a warning.
                CriticalSection();
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger(GetType()).Error(ex); // TODO: Should log an exception.
            }
            log4net.LogManager.GetLogger(GetType()).Debug("Log message"); // TODO: Should log a message with a level of the log4net.Core.Level.Debug.
        }
    }
}
