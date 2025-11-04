using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services;
public class XmlTagFilterService
{
    private static readonly string[] Tags = { "<image_not_used", "<textNode_not_used" };

    public static List<XmlTagMatch> FilteringMatchingLines(string filePath)
    {
        var matches = new List<XmlTagMatch>();
        var lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var cells = line.Split(';');
            var foundTags = new List<string>();

            foreach (var cell in cells)
            {
                var cleaned = cell.Trim().Trim('"'); // Whitespace & Anführungszeichen entfernen

                foreach (var tag in Tags)
                {
                    if (cleaned.Contains(tag))
                    {
                        foundTags.Add(tag);
                        break; // optional: nur den ersten Treffer pro Zelle erfassen
                    }
                }
            }

            if (foundTags.Any())
            {
                matches.Add(new XmlTagMatch
                {
                    LineNumber = i + 1,
                    OriginalLine = line.Trim().Trim('"'),
                    MatchedTags = foundTags
                });
            }

            // Debug-Ausgaben zur Kontrolle
            Debug.WriteLine($"Zeile {i + 1}: {line}");
            Debug.WriteLine($"Treffer: {string.Join(", ", foundTags)}");
        }

        return matches;
    }

    public static void SaveMatchesToFile(IEnumerable<XmlTagMatch> matches, string path)
    {
        File.WriteAllLines(path, matches.Select(m => m.OriginalLine));
    }

}
