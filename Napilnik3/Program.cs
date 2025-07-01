using System;
using System.Collections.Generic;
using System.IO;

namespace Napilnik3
{
    class Program
    {
        static void Main(string[] args)
        {
            Pathfinder fileLogPathfinder = new Pathfinder(new FileLogWritter());
            Pathfinder сonsoleLogPathfinder = new Pathfinder(new ConsoleLogWritter());        
            Pathfinder fridayLogPathfinder = new Pathfinder(new SecureConsoleLogWritter(new FileLogWritter()));
            Pathfinder fridayConsoleLogPathfinder = new Pathfinder(new SecureConsoleLogWritter(new ConsoleLogWritter()));
            List<ILogger> loggers = new List<ILogger>();
            loggers.Add(new ConsoleLogWritter());
            loggers.Add(new SecureConsoleLogWritter(new FileLogWritter()));
            Pathfinder combinedLogPathfinder = new Pathfinder(new CombinedLogWritter(loggers));            
            fileLogPathfinder.Find();
            сonsoleLogPathfinder.Find();
            fridayLogPathfinder.Find();
            fridayConsoleLogPathfinder.Find();
            combinedLogPathfinder.Find();
            Console.ReadKey();
        }
    }

    class Pathfinder
    {
        private readonly ILogger _logger;
        private string _text = "Что-то";

        public Pathfinder(ILogger logger)
        {
            if(logger == null)
            {
                throw new ArgumentNullException(logger.ToString());
            }

            _logger = logger;
        }

        public void Find()
        {
            _logger.WriteError(_text);
        }
    }

    class CombinedLogWritter : ILogger
    {
        private readonly List<ILogger> _loggers;

        public CombinedLogWritter(List<ILogger> loggers)
        {
            if (loggers.Count == 0)
            {
                throw new ArgumentNullException(loggers.ToString());
            }

            _loggers = loggers;
        }

        public void WriteError(string message)
        {
            if(message == null)
            {
                throw new ArgumentNullException(message.ToString());
            }

            foreach (var logger in _loggers)
            {
                logger.WriteError(message);
            }
        }
    }

    class FileLogWritter : ILogger
    {
        private const string FileName = "log.txt";

        public void WriteError(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(message.ToString());
            }

            File.WriteAllText(FileName, message);
        }
    }

    class ConsoleLogWritter : ILogger
    {
        public void WriteError(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(message.ToString());
            }

            Console.WriteLine(message);
        }
    }

    class SecureConsoleLogWritter : ILogger
    {
        private readonly ILogger _logger;
        private DayOfWeek _requiredDayOfTheWeek = DayOfWeek.Friday;

        public SecureConsoleLogWritter(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(logger.ToString());
            }

            _logger = logger;
        }

        public void WriteError(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(message.ToString());
            }

            if (DateTime.Now.DayOfWeek == _requiredDayOfTheWeek)
            {
                _logger.WriteError(message);
            }
        }
    }

    interface ILogger
    {
        void WriteError(string message);
    }
}
