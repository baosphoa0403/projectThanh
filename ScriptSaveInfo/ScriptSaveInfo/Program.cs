// See https://aka.ms/new-console-template for more information
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
        public string cpu { get; set; }
        public string ram { get; set; }
        public string disk { get; set; }
        public string os { get; set; }
        public string shutDownTime { get; set; }

        public Data(string hostName, string ipAddress, string anyDesk, string cpu, string ram, string disk, string os, string shutDownTime)
        {
            this.hostName = hostName;
            this.ipAddress = ipAddress;
            this.anyDesk = anyDesk;
            this.cpu = cpu;
            this.ram = ram;
            this.disk = disk;
            this.os = os;
            this.shutDownTime = shutDownTime;
        }
    }

    class Program
    {
        static async Task Main(string[] arg)
        {
            Console.WriteLine("Hello, World!");

            Console.WriteLine("Hihi");
            //string currentDirectory = Directory.GetCurrentDirectory();
            string scriptPath = "../../../../script.sh";
            //string relativePath = Path.GetRelativePath(currentDirectory, scriptPath);
            //Console.WriteLine(relativePath);
            // Create a new process and configure the start info
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "/bin/bash"; // Use the Bash shell to run the script
            startInfo.Arguments = scriptPath; // Pass the path to the script as an argument
            startInfo.RedirectStandardOutput = true; // Redirect the output of the script to our console
            startInfo.UseShellExecute = false; // Don't use the default shell to execute the script
           
            using var client = new HttpClient();
            string apiUrl = "http://localhost:3000/infoComputer";
            string hostName = "";
            string ipAddress = "";
            string anyDesk = "";
            string cpu = "";
            string ram = "";
            string disk = "";
            string os = "";
            string shutDownTime = "";

            string output = "";
            // Create a new StringContent object with the request body
            // Start the process and wait for it to exit
            using (Process process = Process.Start(startInfo))
            {
                // Read the output of the script one line at a time
                using (StreamReader reader = process.StandardOutput)
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        output += line + "&&&";
                    }
                }

                process.WaitForExit();
            }
            Console.WriteLine("Script output:");
            Console.WriteLine(output.Split("&&&"));
            string[] tmp = output.Split("&&&");
            if (tmp.Length > 0)
            {
                hostName = tmp[0];
                ipAddress = tmp[1];
                anyDesk = tmp[2];
                cpu = tmp[3];
                ram = tmp[0];
                disk = tmp[0];
                os = tmp[0];
                shutDownTime = tmp[0];
            }
            //var requestBody = "{\"username\": \"john_doe\", \"email\": \"john.doe@example.com\"}";
            var requestBody = new Data(hostName,
             ipAddress,
             anyDesk,
             cpu,
             ram,
             disk,
             os,
             shutDownTime);
            string json = JsonConvert.SerializeObject(requestBody);

            Console.WriteLine(json);

            var requestBodyContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, requestBodyContent);

            // Get the response content as a string
            var responseContent = await response.Content.ReadAsStringAsync();

            // Print the response content to the console
            Console.WriteLine(responseContent);
        }
    }
}