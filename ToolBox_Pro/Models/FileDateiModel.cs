using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models;
public class FileDateiModel
{
    public string FilePath { get; set; }
    public string FileName => Path.GetFileName(FilePath);
    public long FileSizeBytes { get; set; }
    public string FileSizeMB => $"{FileSizeBytes / (1024 * 1024)} MB";
}
