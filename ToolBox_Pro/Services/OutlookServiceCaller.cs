// OutlookServiceCaller.cs - Originalzustand
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ToolBox_Pro.Services
{
    public class OutlookServiceCaller
    {
        public void ExportAttachments(string senderEmail, string exportPath)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "OutlookMailService.exe",
                        Arguments = $"\"{senderEmail}\" \"{exportPath}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                string result = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (!string.IsNullOrEmpty(result))
                {
                    Console.WriteLine($"[INFO] {result}");
                }

                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine($"[ERROR] {error}");
                }

                MessageBox.Show("Anhänge erfolgreich exportiert und Mails verschoben.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aufruf des Outlook-Services:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
