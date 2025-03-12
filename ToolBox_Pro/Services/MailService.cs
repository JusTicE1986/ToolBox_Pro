using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Outlook = Microsoft.Office.Interop.Outlook;


namespace ToolBox_Pro.Services
{
    public class MailService
    {
        public List<string> ExtractPDFsFromMails(string senderEmail, string destinationFolder)
        {
            List<string> savedFiles = new List<string>();
            Outlook.Application outlookApp = new Outlook.Application();
            Outlook.MAPIFolder inbox = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            Outlook.Items items = inbox.Items.Restrict("[SenderEmailAddress] = transeng@e-kern.com");

            // Der Zielordner, in den die E-Mails verschoben werden
            Outlook.MAPIFolder targetFolder = GetSubFolder(inbox, "KERN COTI Angebote");
            if (targetFolder == null)
            {
                targetFolder = inbox.Folders.Add("KERN COTI Angebote", Type.Missing) as Outlook.MAPIFolder;
            }

            // Überprüfung, ob das Zielverzeichnis existiert
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
                System.Windows.MessageBox.Show($"Der Ordner {destinationFolder} wurde erstellt.");
            }
            else
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(destinationFolder);
                foreach (var file in directoryInfo.GetFiles())
                {
                    file.Delete(); // Löscht alle vorhandenen Dateien
                }
            }

            // Verarbeitung der E-Mails
            foreach (object item in items)
            {
                if (item is Outlook.MailItem mailItem)
                {
                    try
                    {
                        foreach (Outlook.Attachment attachment in mailItem.Attachments)
                        {
                            if (attachment.FileName.EndsWith(".pdf") && attachment.FileName.StartsWith("attr"))
                            {
                                string savePath = Path.Combine(destinationFolder, attachment.FileName);
                                attachment.SaveAsFile(savePath);
                                savedFiles.Add(savePath);
                            }
                        }

                        // Mail in Zielordner verschieben
                        if (targetFolder != null)
                        {
                            mailItem.Move(targetFolder);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Der Zielordner konnte nicht erstellt werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"Fehler bei Mailverarbeitung: {ex.Message}");
                    }
                }
            }
            return savedFiles;
        }

        private Outlook.MAPIFolder GetSubFolder(Outlook.MAPIFolder parentFolder, string folderName)
        {
            foreach (Outlook.MAPIFolder folder in parentFolder.Folders)
            {
                if (folder.Name == folderName)
                {
                    return folder;
                }
            }
            return null; // Ordner wurde nicht gefunden
        }
    }
}

