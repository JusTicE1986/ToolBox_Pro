using System;
using System.Diagnostics;

namespace ToolBox_Pro.Services
{
    public class CleanupService
    {
        public void CleanupFolder(string folderPath)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c del /q \"{folderPath}\\*.*\" & for /D %a in (\"{folderPath}\\*.*\") do rd /q /s \"%a\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(error))
                    Debug.WriteLine($"Fehler beim Bereinigen von {folderPath}: {error}");
                else
                    Debug.WriteLine($"Erfolg beim Bereinigen von {folderPath}: {output}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception beim Bereinigen von {folderPath}: {ex.Message}");
            }
        }
    }
}
