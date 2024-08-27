using System;
using System.IO;
using UnityEngine;

namespace MainManagers
{
    public static class LogFile
    {
        public static string path = Application.persistentDataPath + "/logs/debugLog.txt";

        public static void Enable()
        {
            Application.logMessageReceived += LogMessage;
        }

        private static void Log(string msg)
        {
            using StreamWriter writer = File.AppendText(path);
            {
                //writer.WriteLine(DateTime.UtcNow.ToLongTimeString() + " | Log => " + (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name + " -> " + msg);
            }
            writer.Close();
        }
    
        private static void LogWarning(string msg)
        {
            using StreamWriter writer = File.AppendText(path);
            {
                //writer.WriteLine(DateTime.UtcNow.ToLongTimeString() + " | Warning => " + (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name + " -> " + msg);
            }
            writer.Close();
        }
    
        private static void LogError(string msg)
        {
            using StreamWriter writer = File.AppendText(path);
            {
                writer.WriteLine(DateTime.UtcNow.ToLongTimeString() + " | Error => " + (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name + " -> " + msg);
            }
            writer.Close();
        }
        
        public static void LogMessage(string condition, string stackTrace, LogType type)
        {
            using StreamWriter writer = File.AppendText(path);
            {
                
                writer.WriteLine($"{type}: {condition} \n Stack trace:  + {stackTrace}");
            }
            writer.Close();
        }
    }
}
