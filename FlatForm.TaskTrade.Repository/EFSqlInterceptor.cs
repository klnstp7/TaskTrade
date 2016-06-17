using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Peacock.PEP.Repository
{
    /// <summary>
    /// SQL命令拦截器
    /// </summary>
    public class EFSqlInterceptor : DbCommandInterceptor
    {
        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            if ((command.CommandText.ToUpper().Contains("UPDATE") || command.CommandText.ToUpper().Contains("DELETE"))
                && command.Connection.Database != "pep_lite")
            {
                command.Connection.Close();
                command.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["PepWriteConnection"].ToString();
                command.Connection.Open();
            }
        }
        public override void ScalarExecuting(DbCommand command,
            DbCommandInterceptionContext<object> interceptionContext)
        {
            command.Connection.Close();
            command.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["PepReadConnection"].ToString();
            command.Connection.Open();
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            if (command.CommandText.ToUpper().Contains("INSERT") && command.Connection.Database == ConfigurationManager.AppSettings["PEPDataBase"])
            {
                command.Connection.Close();
                command.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["PepWriteConnection"].ToString();
                command.Connection.Open();
            }
        }
    }
}
