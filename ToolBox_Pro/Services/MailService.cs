using System.Collections.Generic;
using Microsoft.Office.Interop.Outlook;

namespace ToolBox_Pro.Services
{
    public class MailService
    {
        public List<string> ExtractPDFsFromMails(string senderEmail, string destinationFolder)
        {
            List<string> savedFiles = new List<string>();
            Application outlookApp = new Application();
            MAPIFolder inbox = outlookApp.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
            Items items = inbox.Items.Restrict($"[SenderEmailAddress] = '{senderEmail}'");

            // Der Zielordner, in den die E-Mails verschoben werden
            MAPIFolder targetFolder = GetSubFolder(inbox, "KERN Angebote");
            if (targetFolder == null)
            {
                System.Windows.MessageBox.Show("Der Ordner 'KERN Angebote' wurde nicht gefunden.");
                return savedFiles;
            }

            foreach (object item in items)
            {
                if (item is MailItem mailItem)
                {
                    // Anhänge speichern
                    foreach (Attachment attachment in mailItem.Attachments)
                    {
                        if (attachment.FileName.EndsWith(".pdf") && attachment.FileName.StartsWith("attr"))
                        {
                            string savePath = $"{destinationFolder}\\{attachment.FileName}";
                            attachment.SaveAsFile(savePath);
                            savedFiles.Add(savePath);
                        }
                    }

                    // E-Mail in den Zielordner verschieben
                    try
                    {
                        mailItem.Move(targetFolder); // E-Mail verschieben
                    }
                    catch (System.Exception ex)
                    {
                        // Fehlerbehandlung für das Verschieben der E-Mail
                        System.Windows.MessageBox.Show($"Fehler beim Verschieben der E-Mail: {ex.Message}");
                    }
                }
            }
            return savedFiles;
        }
        private MAPIFolder GetSubFolder(MAPIFolder parentFolder, string folderName)
        {
            foreach (MAPIFolder folder in parentFolder.Folders)
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
