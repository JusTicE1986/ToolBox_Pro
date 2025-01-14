using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public class PDFDataModel
    {
        public string Materialnummer { get; set; }
        public string Format { get; set; }
        public int Seitenzahl { get; set; }
        public double Gewicht { get; set; }
        public string AusgabeDatum { get; set; }
        public string Typ {  get; set; }
        public string Model {  get; set; }
        public string Language {  get; set; }
        public string Version {  get; set; }
    }

}
