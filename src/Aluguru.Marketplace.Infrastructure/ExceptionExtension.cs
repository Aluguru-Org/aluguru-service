using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Infrastructure
{
    public static class ExceptionExtension
    {
        public static string GetErrorMsg(this Exception ex)
        {
            StringBuilder sb = new StringBuilder(ex.Message);
            Exception inner = ex.InnerException;
            while (inner != null)
            {
                sb.AppendFormat(" - {0}", inner.Message);
                inner = inner.InnerException;
            }
            return sb.ToString();
        }

        public static List<string> GetErrorList(this Exception ex)
        {
            List<string> errorList = new List<string>();

            if (ex != null)
            {
                errorList.Add(ex.Message);
            }

            Exception inner = ex.InnerException;

            while (inner != null)
            {
                errorList.Add(inner.Message);
                inner = inner.InnerException;
            }

            return errorList;
        }
    }
}
