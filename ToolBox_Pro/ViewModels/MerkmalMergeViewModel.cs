using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace ToolBox_Pro.ViewModels
{
    public partial class MerkmalMergeViewModel : ObservableObject

    {
        [ObservableProperty]
        private ObservableCollection<MerkmalEintrag> eintraege = new();

        [RelayCommand]
        private void LadeMerkmalDatei()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Excel-Datei (*.xlsx)|*.xlsx",
                Title = "Wähle die Excel-Datei mit den Merkmalen"
            };

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                var daten = LeseMerkmaleAusExcel(dialog.FileName);
                Eintraege = new ObservableCollection<MerkmalEintrag>(daten);
            }
        }

        [RelayCommand]
        private void LadeGruppenDatei()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Excel-Datei (*.xlsx) | *.xlsx",
                Title = "Wähle die Gruppentabelle"
            };

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                var gruppenMap = LeseGruppenAusExcel(dialog.FileName);

                foreach (var eintrag in Eintraege)
                {
                    if (gruppenMap.TryGetValue(eintrag.Merkmalname, out string gruppe))
                    {
                        eintrag.Gruppe = gruppe;
                    }
                    else
                    {
                        eintrag.Gruppe = "UNBEKANNT";
                    }
                }
                Eintraege = new ObservableCollection<MerkmalEintrag>(Eintraege);
            }
        }

        [RelayCommand]
        private void Reset()
        {
            Eintraege.Clear();
        }

        [RelayCommand]
        private void Exportieren()
        {
            var dialog = new SaveFileDialog
            {
                Title = "Exportieren als Excel",
                Filter = "Excel-Datei (*.xlsx)|*.xlsx",
                FileName = "Merkmale_Gemerged.xlsx"
            };

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Export");


            }
        }

        private List<MerkmalEintrag> LeseMerkmaleAusExcel(string pfad)
        {
            var liste = new List<MerkmalEintrag>();
            using var wb = new XLWorkbook(pfad);
            var ws = wb.Worksheet(1);

            foreach (var row in ws.RowsUsed().Skip(1))
            {
                string merkmalname = row.Cell(3).GetString().Trim();       // C
                string wert = row.Cell(4).GetString().Trim();              // D
                string vc = row.Cell(6).GetString().Trim();                // E
                string merkmalBez = row.Cell(8).GetString().Trim();        // G
                string bezeichWert = row.Cell(9).GetString().Trim();       // H

                var e = new MerkmalEintrag
                {
                    Merkmalname = merkmalname,
                    VcAktion = vc,
                    Merkmal = vc == "D" ? merkmalname : $"{merkmalname}_{wert}",
                    Merkmalswert = vc == "D" ? "" : wert,
                    BezeichMerkmalwert = vc == "D" ? "" : bezeichWert,
                    Merkmalbezeichnung = merkmalBez
                };
                liste.Add(e);
            }

            return liste;
        }

        private Dictionary<string, string> LeseGruppenAusExcel(string pfad)
        {
            var map = new Dictionary<string, string>();
            using var wb = new XLWorkbook(pfad);
            var ws = wb.Worksheet(1);

            foreach (var row in ws.RowsUsed().Skip(1))
            {
                string gruppe = row.Cell(1).GetString().Trim();
                string merkmal = row.Cell(2).GetString().Trim();

                if (!string.IsNullOrEmpty(merkmal))
                {
                    map[merkmal] = gruppe;
                }
            }

            return map;
        }

        public class MerkmalEintrag
        {
            public string Gruppe { get; set; }
            public string Merkmal { get; set; }
            public string Merkmalname { get; set; }
            public string Merkmalswert { get; set; }
            public string VcAktion { get; set; }
            public string Merkmalbezeichnung { get; set; }
            public string BezeichMerkmalwert { get; set; }
            public string Merkmalname_Merkmalswert
            {
                get
                {
                    return string.IsNullOrWhiteSpace(Merkmalswert)
                        ? Merkmalname
                        : $"{Merkmalname}_{Merkmalswert}";
                }
            }
        }
    }
}
