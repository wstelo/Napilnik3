using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            List<ILogger> _loggers = new List<ILogger>();
            _loggers.Add(new ConsoleLogWritter());
            _loggers.Add(new SecureConsoleLogWritter(new FileLogWritter()));
            Pathfinder combinedLogPathfinder = new Pathfinder(new CombinedLogWritter(_loggers));
            
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
        private ILogger _logger;
        private string text = "Что-то";

        public Pathfinder(ILogger logger)
        {
            _logger = logger;
        }

        public void Find()
        {
            _logger.WriteError(text);
        }
    }

    class CombinedLogWritter : ILogger
    {
        private List<ILogger> _loggers;

        public CombinedLogWritter(List<ILogger> loggers)
        {
            _loggers = loggers;
        }

        public void WriteError(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.WriteError(message);
            }
        }
    }

    class FileLogWritter : ILogger
    {
        public void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class ConsoleLogWritter : ILogger
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class SecureConsoleLogWritter : ILogger
    {
        private ILogger _logger;
        private DayOfWeek _friday = DayOfWeek.Friday;
        private DateTime _dateTime = DateTime.Now;

        public SecureConsoleLogWritter(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteError(string message)
        {
            if (_dateTime.DayOfWeek == _friday)
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
