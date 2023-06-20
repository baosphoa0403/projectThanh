﻿// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace scriptSaveInfo {
    public class Data
    {
        public string hostName { get; set; }
        public string ipAddress { get; set; }
        public string anyDesk { get; set; }
        public string ultraview { get; set; }
        public string cpu { get; set; }
        public string ram { get; set; }
        public string mac { get; set; }
        public string os { get; set; }
        public string shutDownTime { get; set; }

        public Data(string hostName, string ipAddress, string anyDesk, string ultraview ,string cpu, string ram, string mac, string os, string shutDownTime)
        {
            this.hostName = hostName;
            this.ipAddress = ipAddress;
            this.anyDesk = anyDesk;
            this.cpu = cpu;
            this.ram = ram;
            this.mac = mac;
            this.os = os;
            this.shutDownTime = shutDownTime;
            this.ultraview = ultraview;
        }
    }

    class Program
    {
        static async Task Main(string[] arg)
        {
            Console.WriteLine("Hello, World!");

            Console.WriteLine("Hihi");
            
             string command = @"
                # get hostname
                hostname 
                # get ip address
                ipconfig | findstr IPv4
                # anydesk
                @echo off
                pause
                # CPU
                wmic cpu get name 
                # Mac
                wmic bios get serialnumber
                # OS
                ver
                # ShutDownTime
                wevtutil qe system “/q:*[System [(EventID=1074)]]” /rd:true /f:text /c:1
             ";

        // Create a new process to execute the Bash command
        Process process = new Process();

        // Configure the process to execute the Bash command with the -c option and the command as an argument
        process.StartInfo.FileName = "bash";
        process.StartInfo.Arguments = "-c \"" + command + "\"";

        // Redirect the standard output and error streams to the console
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        // Start the process
        process.Start();

        // Read the standard output and error streams and print them to the console
        // string output = process.StandardOutput.ReadLine();
        // string error = process.StandardError.ReadToEnd();
        string output = "";
        using (StreamReader reader = process.StandardOutput)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                output += line + "&&&";
            }
            process.WaitForExit();
        }


        Console.WriteLine("Standard output:");
        Console.WriteLine(output);
        using var client = new HttpClient();
        string apiUrl = "http://10.151.85.232:3000/infoComputer";
        string hostName = "";
        string ipAddress = "";
        string anyDesk = "";
        string cpu = "";
        string ram = "";
        string mac = "";
        string os = "";
        string shutDownTime = "";
        string ultraview = "";
      

         Console.WriteLine(output.Split("&&&"));
            string[] tmp = output.Split("&&&");
            if (tmp.Length > 0)
            {
                hostName = tmp[0];
                ipAddress = tmp[1];
                anyDesk = tmp[2];
                cpu = tmp[3];
                ram = tmp[4];
                mac = tmp[5];
                os = tmp[6];
                shutDownTime = tmp[7];
                ultraview = tmp[8];
            }
            //var requestBody = "{\"username\": \"john_doe\", \"email\": \"john.doe@example.com\"}";
            var requestBody = new Data(hostName,
             ipAddress,
             anyDesk,
             cpu,
             ram,
             mac,
             os,
             shutDownTime,ultraview);
            string json = JsonConvert.SerializeObject(requestBody);

            Console.WriteLine(json);

            var requestBodyContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, requestBodyContent);

            // Get the response content as a string
            var responseContent = await response.Content.ReadAsStringAsync();

            // Print the response content to the console
            Console.WriteLine(responseContent);
            //string currentDirectory = Directory.GetCurrentDirectory();
            // string scriptPath = "../scriptWin.sh";
            // //string relativePath = Path.GetRelativePath(currentDirectory, scriptPath);
            // //Console.WriteLine(relativePath);
            // // Create a new process and configure the start info
            // ProcessStartInfo startInfo = new ProcessStartInfo();
            // startInfo.FileName = "/bin/bash"; // Use the Bash shell to run the script
            // startInfo.Arguments = scriptPath; // Pass the path to the script as an argument
            // startInfo.RedirectStandardOutput = true; // Redirect the output of the script to our console
            // startInfo.UseShellExecute = false; // Don't use the default shell to execute the script
           
            // using var client = new HttpClient();
            // string apiUrl = "http://10.151.85.232:3000/infoComputer";
            // string hostName = "";
            // string ipAddress = "";
            // string anyDesk = "";
            // string cpu = "";
            // string ram = "";
            // string mac = "";
            // string os = "";
            // string shutDownTime = "";
            // string ultraview = "";

            // string output = "";
            // // Create a new StringContent object with the request body
            // // Start the process and wait for it to exit
            // using (Process process = Process.Start(startInfo))
            // {
            //     // Read the output of the script one line at a time
            //     using (StreamReader reader = process.StandardOutput)
            //     {
            //         string line;
            //         while ((line = reader.ReadLine()) != null)
            //         {
            //             output += line + "&&&";
            //         }
            //     }

            //     process.WaitForExit();
            // }
            // Console.WriteLine("Script output:");
           
        }
    }
}