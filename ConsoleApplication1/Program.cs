using System;
using System.Diagnostics;
using log4net;
using log4net.Config;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;
            var logger = LogManager.GetLogger(typeof(Program));

            // this one works
            logger.Info("With Exception", new Exception("log me!")); 
            
            // this one throws (handled)
            logger.Info("Without Exception"); 
            
            /*
             Results in (debug log):
             
log4net:ERROR [AdoNetAppender] ErrorCode: GenericFailure. Exception while writing to database
System.NullReferenceException: Object reference not set to an instance of an object.
   at Devart.Data.Oracle.c9.a(aq A_0, Int32 A_1)
   at Devart.Data.Oracle.c9.a(Int32 A_0, eu A_1)
   at Devart.Data.Oracle.OracleCommand.InternalExecute(CommandBehavior behavior, IDisposable disposable, Int32 startRecord, Int32 maxRecords, Boolean nonQuery)
   at Devart.Common.DbCommandBase.ExecuteDbDataReader(CommandBehavior behavior, Boolean nonQuery)
   at Devart.Data.Oracle.OracleCommand.ExecuteNonQuery()
   at log4net.Appender.AdoNetAppender.SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
   at log4net.Appender.AdoNetAppender.SendBuffer(LoggingEvent[] events)


            Switching to ODP.Net (you have to remove the `Direct=true` from the connection string) solves the issue
             */
        }
    }
}