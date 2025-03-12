// Program.cs - Originalzustand
using System;

namespace OutlookMailService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Verwendung: OutlookMailService.exe <AbsenderEmail> <ExportPfad>");
                return;
            }

            string senderEmail = args[0];
            string exportPath = args[1];

            try
            {
                var processor = new MailProcessor();
                processor.ExportAttachments(senderEmail, exportPath);
                Console.WriteLine("Verarbeitung abgeschlossen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }
    }
}
