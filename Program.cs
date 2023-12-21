using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace UpdateListGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create update list
            List<string> updateList = new List<string>();

            // Recursively traverse the current directory and its subfolders
            ProcessDirectory(".", updateList);

            // Write the update list to a file
            using (StreamWriter writer = new StreamWriter("updatelistXyrotec.txt"))
            {
                // Replace all dots with empty strings
                updateList = updateList.Select(item => item.Substring(0+1)).ToList();

                writer.WriteLine(string.Join("\n", updateList));
            }
        }

        static void ProcessDirectory(string directoryPath, List<string> updateList)
        {
            // Get all files in the current directory
            foreach (string file in Directory.GetFiles(directoryPath))
            {
                // Calculate MD5 hash of the file
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(File.ReadAllBytes(file));
                string md5Hash = BitConverter.ToString(hash).Replace("-", "");

                // Add entry to the update list with full file path
                updateList.Add($"{file}|{md5Hash}");
            }

            // Recursively process subdirectories
            foreach (string subDirectory in Directory.GetDirectories(directoryPath))
            {
                ProcessDirectory(subDirectory, updateList);
            }
        }
    }
}