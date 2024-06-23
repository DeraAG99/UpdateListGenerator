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
            string targetDirectory = "./s4league/";

            // Create update list
            List<string> updateList = new List<string>();

            // Recursively traverse the target directory and its subfolders
            ProcessDirectory(targetDirectory, updateList);

            // Write the update list to a file
            using (StreamWriter writer = new StreamWriter("updatelist.txt"))
            {
                writer.WriteLine(string.Join("\n", updateList));
            }

            Console.WriteLine("Update list generated successfully.");
        }

        static void ProcessDirectory(string directoryPath, List<string> updateList)
        {
            // Get all files in the current directory
            foreach (string file in Directory.GetFiles(directoryPath))
            {
                // Calculate MD5 hash of the file
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(File.ReadAllBytes(file));
                    string md5Hash = BitConverter.ToString(hash).Replace("-", "");

                    // Add entry to the update list with full file path relative to targetDirectory
                    string relativePath = file.Substring(file.IndexOf("s4league") + "s4league".Length + 1);
                    updateList.Add($"{relativePath}|{md5Hash}");
                }
            }

            // Recursively process subdirectories
            foreach (string subDirectory in Directory.GetDirectories(directoryPath))
            {
                ProcessDirectory(subDirectory, updateList);
            }
        }
    }
}
