using ProcessControl.FileClickActions;
using ProcessControl.Helpers;
using ProcessControl.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace ProcessControl
{
    class Program
    {
        const string DEFAULT_CONFIG_FILE_NAME = ".pcconfig";

        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLowerInvariant())
                {
                    case "-c":
                        if (args.Length > i + 1)
                            Start(args[i + 1]);
                        else
                        {
                            OutputError("Config file name not provided");
                            return;
                        }
                        break;
                    case "-rego":
                        RegisterDefaultConfigOpenOnDClick();
                        break;
                    case "-regr":
                        RegisterDefaultConfigRunOnDClick();
                        break;
                    default:
                        OutputHelp();
                        return;
                }
            }
            Start(DEFAULT_CONFIG_FILE_NAME);
        }
        private static void Start(string configFileName)
        {
            FileInfo file = new FileInfo(configFileName);
            if (!file.Exists)
            {
                OutputError($"Config file '{configFileName}' was not found");
                return;
            }
            Console.WriteLine("Starting Process control..");
            List<ExtendedProcess> processList = new List<ExtendedProcess>();
            using (var reader = new StreamReader(file.OpenRead()))
            {
                string cmdLine = null;
                while ((cmdLine = reader.ReadLine()) != null)
                {
                    ProcessData data = ParseProcessData(cmdLine);
                    if (data.Process == null)
                        continue;
                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.RedirectStandardOutput = true;
                        startInfo.RedirectStandardError = false;
                        startInfo.FileName = data.Process;
                        startInfo.Arguments = data.Arguments;
                        ExtendedProcess process = new ExtendedProcess();
                        process.StartInfo = startInfo;
                        if (process.StartInfo.RedirectStandardError)
                            process.ErrorDataReceived += Process_ErrorDataReceived;
                        if (process.StartInfo.RedirectStandardOutput)
                            process.OutputDataReceived += Process_OutputDataReceived;
                        process.Start();
                        if (process.StartInfo.RedirectStandardOutput)
                            process.BeginOutputReadLine();
                        if (process.StartInfo.RedirectStandardError)
                            process.BeginErrorReadLine();
                        processList.Add(process);
                        Console.WriteLine($"Process '{process.StartInfo.FileName}' started.");
                    }
                    catch(Exception e) { OutputError(e.Message + e.StackTrace); continue; }
                }
            }
            PauseApp();
            Console.WriteLine("Stopping Process control..");
            foreach (ExtendedProcess process in processList)
            {
                process.CloseMainWindow();
                process.Close();
                if (process.StartInfo.RedirectStandardError)
                    process.ErrorDataReceived -= Process_ErrorDataReceived;
                if (process.StartInfo.RedirectStandardOutput)
                    process.OutputDataReceived -= Process_OutputDataReceived;
                Console.WriteLine($"Process '{process.StartInfo.FileName}' closed.");
            }
            Console.WriteLine("Process control stopped.");
        }
        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ExtendedProcess process = (ExtendedProcess)sender;
            Console.WriteLine(process.StoredProcessName + ": " + e.Data);
        }
        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ExtendedProcess process = (ExtendedProcess)sender;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(process.StoredProcessName + " error: " + e.Data);
            Console.ResetColor();
        }
        private static void RegisterDefaultConfigOpenOnDClick()
        {
            if (OSHelper.GetOSType() == OSPlatform.Windows)
                WindowsFileClickActions.RegisterForFileExtension(DEFAULT_CONFIG_FILE_NAME, "notepad");
        }
        private static void RegisterDefaultConfigRunOnDClick()
        {
            if (OSHelper.GetOSType() == OSPlatform.Windows)
                WindowsFileClickActions.RegisterForFileExtension(DEFAULT_CONFIG_FILE_NAME, $"{Process.GetCurrentProcess().MainModule.FileName} -c");
        }
        private static ProcessData ParseProcessData(string cmdLine)
        {
            string process = null, arguments = "";
            if (cmdLine.StartsWith('"'))
            {
                int processIndexEnd = cmdLine.IndexOf('"', 1);
                if (processIndexEnd < 0)
                    return new ProcessData(process, arguments);
                process = cmdLine.Substring(1, processIndexEnd - 1);
                if (cmdLine.Length > processIndexEnd + 2)
                    arguments = cmdLine.Substring(processIndexEnd + 2, cmdLine.Length - processIndexEnd - 2);
            }
            else
            {
                int processIndexEnd = cmdLine.IndexOf(' ', 1);
                if (processIndexEnd > 0)
                {
                    process = cmdLine.Substring(0, processIndexEnd);
                    if (cmdLine.Length > processIndexEnd + 1)
                        arguments = cmdLine.Substring(processIndexEnd + 1, cmdLine.Length - processIndexEnd - 1);
                }
                else process = cmdLine;
            }
            return new ProcessData(process, arguments);
        }
        private static void OutputError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + message);
            Console.ResetColor();
        }
        private static void OutputHelp()
        {
            Console.WriteLine("Welcome to ProcessControl!");
            Console.WriteLine($"If you run this application without additional commands default config file '{DEFAULT_CONFIG_FILE_NAME}' of current directory will be used.");
            Console.WriteLine("");
            Console.WriteLine("Available commands:");
            Console.WriteLine("-c <config_file_name> - Starts program with custom config file");
            Console.WriteLine($"-rego - Register default config file '{DEFAULT_CONFIG_FILE_NAME}' to open on double click");
            Console.WriteLine($"-regr - Register default config file '{DEFAULT_CONFIG_FILE_NAME}' to run on double click");
        }
        private static void PauseApp()
        {
            Console.ReadKey(true);
        }
    }
}
