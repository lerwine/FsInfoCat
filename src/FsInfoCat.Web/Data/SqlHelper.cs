using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Web.Data
{
    public static class SqlHelper
    {
        public static string GetErrorMessages(SqlException exception)
        {
            IEnumerable<string> result;
            if (null != exception.InnerException && exception.InnerException is Win32Exception)
                result = exception.Errors.OfType<SqlError>().Select(error =>
                {
                    if (error.State == 0)
                    {
                        if (error.Number == -2 && ((Win32Exception)exception.InnerException).NativeErrorCode == 258)
                            return "Timeout elapsed while attempting to connect to the database.";
                        return "Communication error while attempting to connect to the database (error code " + error.Number.ToString() + ").";
                    }
                    if (string.IsNullOrWhiteSpace(error.Message))
                        return "An unexpected error (code " + error.Number.ToString() + ") occured while trying to access the database.";
                    return "An unexpected error (" + error.Number.ToString() + ") occured while trying to access the database: " + error.Message.Trim();
                });
            else
                result = exception.Errors.OfType<SqlError>().Select(error =>
                {
                    if (error.State == 0)
                    {
                        switch (error.Number)
                        {
                            case -1:
                            case 2:
                            case 53:
                                return "An error has occurred while establishing a connection to the server.";
                            case -2:
                                return "Timeout elapsed while attempting to connect to the database.";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(error.Message))
                        return "An unexpected error (code " + error.Number.ToString() + ") occured while trying to access the database.";
                    return "An unexpected error (" + error.Number.ToString() + ") occured while trying to access the database: " + error.Message.Trim();
                });
            string message = string.Join(Environment.NewLine + Environment.NewLine, result.ToArray());
            if (message.Trim().Length > 0)
                return message;
            if (string.IsNullOrWhiteSpace(exception.Message))
                return "An unexpected " + exception.GetType().Name + " (error code " + exception.Number.ToString() + ") occured while trying to access the database.";
            return "An unexpected error (" + exception.Number.ToString() + ") occured while trying to access the database: " + exception.Message.Trim();
        }
    }
}
