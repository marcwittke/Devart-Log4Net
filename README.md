# Devart-Log4Net

Reproduction of a `NullReferenceException` when using dotConnect for Oracle 9.14 together with log4net and an ADO.Net appender

### Steps
- Create an Oracle database `LOGGING` via `CreateSchema.sql`
- change the connection string in `App.config` accordingly
- run the program in debug mode
- verify that only the log event _with_ an exception is logged correctly to the `LOGGING.APPLICATION_LOG` table
- see the exception in the debug output (it is caught and swallowed by log4net)
```
log4net:ERROR [AdoNetAppender] ErrorCode: GenericFailure. Exception while writing to database
System.NullReferenceException: Object reference not set to an instance of an object.
   at Devart.Data.Oracle.c9.a(aq A_0, Int32 A_1)
   at Devart.Data.Oracle.c9.a(Int32 A_0, eu A_1)
   at Devart.Data.Oracle.OracleCommand.InternalExecute(CommandBehavior behavior, IDisposable disposable, Int32 startRecord, Int32 maxRecords, Boolean nonQuery)
   at Devart.Common.DbCommandBase.ExecuteDbDataReader(CommandBehavior behavior, Boolean nonQuery)
   at Devart.Data.Oracle.OracleCommand.ExecuteNonQuery()
   at log4net.Appender.AdoNetAppender.SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
   at log4net.Appender.AdoNetAppender.SendBuffer(LoggingEvent[] events)
```
- switch the `connectionType` to ODP.Net `Oracle.ManagedDataAccess.Client.OracleConnection` (remove `Direct=True` from the connection string)
- repeat and see no exception and all events accordingly logged to the database table


### possible workaround
By preventing the `exception` param to have a `DbNull` value you can mitigate the issue:

```xml
<parameter>
  <parameterName value="exception" />
  <dbType value="String" />
  <size value="32000" />
  <layout type="log4net.Layout.PatternLayout">
    <conversionPattern value="%exception " /> <!-- note the trailing whitespace! -->
  </layout>
</parameter>
```
