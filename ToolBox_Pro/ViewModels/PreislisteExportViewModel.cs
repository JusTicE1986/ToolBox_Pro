using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public class PreislisteExportViewModel : BaseViewModel
    {
        private readonly MerkmalCsvExportService _exportService;

        public PreislisteExportViewModel()
        {
            _exportService = new MerkmalCsvExportService();
            CsvSpeichernCommand = new RelayCommand(CsvSpeichern);
            CsvMerkmaleSpeichernCommand = new RelayCommand(CsvMerkmaleSpeichern);
            PreislistenExportierenCommand = new RelayCommand(PreislistenExportieren);
        }

        public ICommand CsvSpeichernCommand { get; }
        public ICommand CsvMerkmaleSpeichernCommand { get; }
        public ICommand PreislistenExportierenCommand { get; }

        private void CsvSpeichern()
        {
            string excelPfad = @"C:\Users\LNZNEUMA\Desktop\Kopie von Master PL.xlsm";
            string zielPfad = @"S:\03_PGK\Excels\2024 Preislisten\06 Test";

            _exportService.ExportiereMerkmale(excelPfad, zielPfad, "Master", "VC ALT");
            _exportService.ExportiereMerkmale(excelPfad, zielPfad, "Master", "VC NEU");
        }

        private void CsvMerkmaleSpeichern()
        {
            string excelPfad = @"C:\Users\LNZNEUMA\Desktop\Kopie von Master PL.xlsm";
            string zielPfad = @"S:\03_PGK\Excels\2024 Preislisten\06 Test";

            _exportService.ExportiereMerkmaleMitBezeichnung(excelPfad, zielPfad, "Master"); 
        }

        private void PreislistenExportieren()
        {
            string excelPfad = @"C:\Users\LNZNEUMA\Desktop\Kopie von Master PL.xlsm";
            string zielPfad = @"S:\03_PGK\Excels\2024 Preislisten\06 Test";

            _exportService.ErstellePreislisteNachArtNr(excelPfad, zielPfad, "Master", "VC ALT");
            _exportService.ErstellePreislisteNachArtNr(excelPfad, zielPfad, "Master", "VC NEU");
        }
    }
}
