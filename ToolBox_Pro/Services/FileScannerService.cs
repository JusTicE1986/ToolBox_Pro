using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox_Pro.Interfaces;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services;
public class FileScannerService : IFileScannerService
{
    public IEnumerable<FileDateiModel> GetLargeFiles(string folderPath, long minFileSizeBytes, bool recursive = false, bool excludePdf = false)
    {
        var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

        return Directory.EnumerateFiles(folderPath, "*.*", searchOption)
            .Select(f => new FileInfo(f))
            .Where(fi => fi.Length > minFileSizeBytes)
            .Where(fi => !excludePdf || !fi.Extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(fi => fi.Length)
            .Select(fi => new FileDateiModel
            {
                FilePath = fi.FullName,
                FileSizeBytes = fi.Length
            });
            
    }
}
