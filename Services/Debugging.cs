﻿using System;
using Discord;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace lcsbot.Services
{
    public static class Debugging
    {
        private static string path = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        private static string fullPath = null;

        private static ConsoleColor GetColor(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return ConsoleColor.Magenta;
                case LogSeverity.Error:
                    return ConsoleColor.Red;
                case LogSeverity.Warning:
                    return ConsoleColor.Yellow;
                case LogSeverity.Info:
                    return ConsoleColor.White;
                case LogSeverity.Verbose:
                    return ConsoleColor.DarkYellow;
                case LogSeverity.Debug:
                    return ConsoleColor.DarkGray;
                default:
                    return ConsoleColor.White;
            }
        }

        private static void ResetColor()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void PrintMessage(LogMessage message)
        {
            Console.ForegroundColor = GetColor(message.Severity);
            Console.Write($"{DateTime.Now,-19} [{message.Severity,8}]");
            Console.ResetColor();
            Console.Write($" {message.Source}: {message.Message}; {message.Exception}\n");
        }

        private static void SetLogPath()
        {
            string datetime = DateTime.Now.ToString().Replace(":", ".").Replace(@"\", ".").Replace("/", ".");
            string newPath = Path.Combine(path, datetime + ".tenelog");

            try
            {
                if (!Directory.Exists(path))
                {
                    LogNoLog("Debugging.Log", $"No log directory found, creating at '{path}'", LogSeverity.Debug);
                    Directory.CreateDirectory(path);
                }

                fullPath = newPath;
                File.Create(fullPath).Dispose();
                LogNoLog("Debugging.Log", $"Created new log file for session at '{fullPath}'", LogSeverity.Debug);
            }
            catch (Exception e)
            {
                LogNoLog(new LogMessage(LogSeverity.Error, "SetLogPath", "Failed to create directory", e));
            }
        }

        private static void LogMessage(LogMessage message)
        {
            PrintMessage(message);
            LogToFile(message);
        }

        private static void LogToFile(LogMessage message)
        {
            if (fullPath == null)
                SetLogPath();
            else
                File.AppendAllText(fullPath, message.ToString() + "\n");
        }

        /// <summary>
        /// Prints a message on the debug console and writes to output log.
        /// </summary>
        /// <param name="message">Log message for the message.</param>
        public static Task Log(LogMessage message)
        {
            LogMessage(message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Prints a message on the debug console and writes to output log.
        /// </summary>
        /// <param name="source">Source where the message is coming from (parent class of function or similar).</param>
        /// <param name="message">Message to print to console.</param>
        public static void Log(string source, string message)
        {
            LogMessage(new LogMessage(LogSeverity.Info, source, message));
        }

        /// <summary>
        /// Prints a message on the debug console and writes to output log.
        /// </summary>
        /// <param name="source">Source where the message is coming from (parent class of function or similar).</param>
        /// <param name="message">Message to print to console.</param>
        /// <param name="severity">Message severity</param>
        public static void Log(string source, string message, LogSeverity severity = LogSeverity.Info)
        {
            LogMessage(new LogMessage(severity, source, message));
        }

        /// <summary>
        /// Prints a message on the debug console without writing it to output log.
        /// </summary>
        /// <param name="message">Log message for the message.</param>
        public static void LogNoLog(LogMessage message)
        {
            PrintMessage(message);
        }

        /// <summary>
        /// Prints a message on the debug console and writes to output log.
        /// </summary>
        /// <param name="source">Source where the message is coming from (parent class of function or similar).</param>
        /// <param name="message">Message to print to console.</param>
        public static void LogNoLog(string source, string message)
        {
            PrintMessage(new LogMessage(LogSeverity.Info, source, message));
        }

        /// <summary>
        /// Prints a message on the debug console and writes to output log.
        /// </summary>
        /// <param name="source">Source where the message is coming from (parent class of function or similar).</param>
        /// <param name="message">Message to print to console.</param>
        /// <param name="severity">Message severity</param>
        public static void LogNoLog(string source, string message, LogSeverity severity = LogSeverity.Info)
        {
            PrintMessage(new LogMessage(severity, source, message));
        }

        /// <summary>
        /// Outputs test messages in all severity colors.
        /// </summary>
        public static void TestLog()
        {
            LogMessage message = new LogMessage(LogSeverity.Critical, "Log testing", $"Lorem ipsum dolor sit amet");
            Log(message);

            message = new LogMessage(LogSeverity.Debug, "Log testing", $"Consectetur adipiscing elit");
            Log(message);

            message = new LogMessage(LogSeverity.Error, "Log testing", $"Nam in diam maximus");
            Log(message);

            message = new LogMessage(LogSeverity.Info, "Log testing", $"Bibendum nisi eu");
            Log(message);

            message = new LogMessage(LogSeverity.Verbose, "Log testing", $"Pulvinar lorem");
            Log(message);

            message = new LogMessage(LogSeverity.Warning, "Log testing", $"Morbi nulla sapien");
            Log(message);
        }

        /// <summary>
        /// Reads input from the console.
        /// </summary>
        /// <param name="comment">Additional comment for information to the user.</param>
        /// <returns>The inputted line.</returns>
        public static string Read(string comment)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{DateTime.Now,-19} [   Input] {comment} : ");
            Console.ResetColor();

            return Console.ReadLine();
        }

        /// <summary>
        /// Divides by zero, meant to cause an exception for testing try catch methods. Only to be used in debugging.
        /// </summary>
        public static void DivideByZero()
        {
            int zero = 0;
            int x = 1 / zero;
        }

        /// <summary>
        /// Checks if an HTTP url is reachable.
        /// </summary>
        /// <param name="url">Website url</param>
        /// <returns>If the host returns a success for the url.</returns>
        public static bool CheckHttpReachable(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 1000;
            request.Method = "HEAD";

            Log("CheckHttpReachable", $"Checking if '{url}' is reachable", LogSeverity.Debug);

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Log("CheckHttpReachable", $"'{url}' is reachable", LogSeverity.Debug);
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException e)
            {
                if (e.Message.Contains("404"))
                    Log("CheckHttpReachable", $"Remote server returned 404, not found", LogSeverity.Warning);
                else
                    Log("CheckHttpReachable", $"Web exception: {e.Message}", LogSeverity.Error);
                return false;
            }
        }
    }
}
