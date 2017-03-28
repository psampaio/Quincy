using System;
using System.Diagnostics;
using Caliburn.Micro;

namespace Quincy
{
    public class CaliburnMicroDebugLogger : ILog
    {
        private readonly Type type;

        public CaliburnMicroDebugLogger(Type type)
        {
            this.type = type;
        }

        public void Error(Exception exception)
        {
            Debug.WriteLine(CreateLogMessage(exception.ToString()), "ERROR");
        }

        public void Info(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args), "INFO");
        }

        public void Warn(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args), "WARN");
        }

        private string CreateLogMessage(string format, params object[] args)
        {
            return $"[{DateTime.Now:o}] ({type.FullName}) {string.Format(format, args)}";
        }
    }
}