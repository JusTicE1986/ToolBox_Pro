using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Outlook;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookMailService
{
    public class MailProcessor
    {
        public void ExportAttachments(string senderEmail, string exportPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(exportPath);
            List<MailItem> mails = new List<MailItem>();

            Outlook.Application outlookApp = new Outlook.Application();
            MAPIFolder inBox = outlookApp.ActiveExplorer().Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
            Items inBoxItems = inBox.Items;

            inBoxItems = inBoxItems.Restrict($"[SenderEmailAddress] = '{senderEmail}'");

            Folder folder = outlookApp.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox) as Folder;
            Folder root = outlookApp.Session.DefaultStore.GetDefaultFolder(OlDefaultFolders.olFolderInbox) as Folder;
            Folders folders = folder.Folders;
            int childs = EnumerateFolders(root);

            try
            {
                // Verzeichnis anlegen
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                    Console.WriteLine($"📂 Ordner erstellt: {directoryInfo.FullName}");
                }
                else
                {
                    // Ordner leeren
                    foreach (var file in directoryInfo.GetFiles())
                    {
                        file.Delete();
                    }
                }

                // Anhänge speichern
                foreach (object collectionItem in inBoxItems)
                {
                    MailItem newEmail = collectionItem as MailItem;
                    if (newEmail != null && newEmail.Attachments.Count > 0)
                    {
                        for (int i = 1; i <= newEmail.Attachments.Count; i++)
                        {
                            if (newEmail.Attachments[i].FileName.EndsWith(".pdf") &&
                                newEmail.Attachments[i].FileName.StartsWith("attr"))
                            {
                                string filePath = Path.Combine(exportPath, newEmail.Attachments[i].FileName);
                                newEmail.Attachments[i].SaveAsFile(filePath);
                                mails.Add(newEmail);
                                Console.WriteLine($"📄 Anhang gespeichert: {filePath}");
                            }
                        }
                    }
                }

                // Ordner anlegen, falls nicht vorhanden
                if (childs == 0)
                {
                    Folder newFolder = folders.Add("KERN COTI Angebote", Type.Missing) as Folder;
                    Console.WriteLine("📂 Ordner 'KERN COTI Angebote' wurde erstellt.");
                }

                // E-Mails verschieben
                int mailCount = 0;
                foreach (MailItem mail in mails)
                {
                    mail.UnRead = false;
                    mail.Save();
                    mail.Move(inBox.Folders["KERN COTI Angebote"]);
                    mailCount++;
                }

                Console.WriteLine($"✅ {mailCount} Angebote erfolgreich verschoben.");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"❌ Fehler bei der Verarbeitung: {ex.Message}");
            }
        }

        private int EnumerateFolders(Folder folder)
        {
            Folders childFolders = folder.Folders;
            int result = 0;

            if (childFolders.Count > 0)
            {
                foreach (Folder childFolder in childFolders)
                {
                    if (childFolder.FolderPath.ToString().Contains("KERN COTI Angebote"))
                    {
                        result++;
                    }
                }
                return result;
            }
            return 0;
        }
    }
}
