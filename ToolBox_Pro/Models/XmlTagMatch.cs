using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models;
public class XmlTagMatch
{
    public int LineNumber { get; set; }
    public string OriginalLine { get; set; } = string.Empty;
    public List<string> MatchedTags { get; set; } = new();
}
