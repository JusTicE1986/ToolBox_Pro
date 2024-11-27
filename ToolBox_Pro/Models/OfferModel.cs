using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public class OfferModel
    {
        public string FileName { get; set; }
        public string Language { get; set; }
        public decimal Price { get; set; }

        public OfferModel(string fileName, decimal offerSum)
        {
            FileName = fileName;
            Language = Language;
            Price = offerSum;
        }
    }
}
