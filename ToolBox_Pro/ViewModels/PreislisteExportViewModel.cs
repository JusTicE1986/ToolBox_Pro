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
            string excelPfad = @"\\wnad.net\local\PGK\DATA\Product_Management\PM_Preisliste\Masterliste\Master PL.xlsm";
            string zielPfad = @"S:\03_PGK\Excels\2024 Preislisten\01 Listen Modellierung";

            _exportService.ExportiereMerkmale(excelPfad, zielPfad, "Master", "VC ALT");
            _exportService.ExportiereMerkmale(excelPfad, zielPfad, "Master", "VC NEU");
        }

        private void CsvMerkmaleSpeichern()
        {
            string excelPfad = @"\\wnad.net\local\PGK\DATA\Product_Management\PM_Preisliste\Masterliste\Master PL.xlsm";
            string zielPfad = @"S:\03_PGK\Excels\2024 Preislisten\02 Listen Merkmale und Bezeichnung";

            _exportService.ExportiereMerkmaleMitBezeichnung(excelPfad, zielPfad, "Master"); 
        }

        private void PreislistenExportieren()
        {
            string excelPfad = @"\\wnad.net\local\PGK\DATA\Product_Management\PM_Preisliste\Masterliste\Master PL.xlsm";
            string zielPfad = @"S:\03_PGK\Excels\2024 Preislisten\04 Listen Preise";

            _exportService.ErstellePreislisteNachArtNr(excelPfad, zielPfad, "Master", "VC ALT");
            _exportService.ErstellePreislisteNachArtNr(excelPfad, zielPfad, "Master", "VC NEU");
        }
    }
}
