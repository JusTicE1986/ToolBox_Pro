using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Interfaces;
public interface IFileScannerService
{
    IEnumerable<FileDateiModel> GetLargeFiles(string folderPath, long minFileSizeBytes, bool recursive = false, bool excludePdf = false);
}
