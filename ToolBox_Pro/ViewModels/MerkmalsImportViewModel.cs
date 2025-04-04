using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public partial class MerkmalsImportViewModel : ObservableObject
    {
        private readonly MerkmalsImportService _importService = new();

        [ObservableProperty]
        private ObservableCollection<MerkmalsEintrag> eintraege = new();
        [ObservableProperty]
        private ObservableCollection<XmlMerkmal> xmlMerkmale = new();
        [ObservableProperty]
        private ObservableCollection<string> typen = new();
        [ObservableProperty]
        private string ausgewaehlterTyp;
        [ObservableProperty]
        private bool istBeschaeftigt;
        [ObservableProperty]
        private ICollectionView gefilterteEintraege;
        [ObservableProperty]
        private ObservableCollection<MerkmalModelEintrag> merkmalModelle = new();
        [ObservableProperty] MerkmalModelEintrag ausgewaehltesMerkmalModell = new();

        //private ICollectionView gefilterteEintraege;
        //public ICollectionView GefilterteEintraege => gefilterteEintraege;

        [RelayCommand]
        private void ExcelLaden()
        {
            var daten = _importService.LadeUndFiltereEintraege();
            Eintraege = new ObservableCollection<MerkmalsEintrag>(daten);

            Typen = new ObservableCollection<string>(
                Eintraege.Select(e => e.Type).Distinct().OrderBy(t => t));

            gefilterteEintraege = CollectionViewSource.GetDefaultView(Eintraege);
            gefilterteEintraege.Filter = FilterEintraege;
            GefilterteEintraege?.Refresh();
        }
        [RelayCommand]
        private void LadeXmlMerkmale()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "XML Merkmalsdatei (*.xml)|*.xml"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var liste = _importService.LadeUntersteXmlMerkmale(openFileDialog.FileName);
                XmlMerkmale = new ObservableCollection<XmlMerkmal>(liste);
            }
        }
        [RelayCommand]
        private void AbgleichIdsMitXml()
        {
            if (XmlMerkmale == null || XmlMerkmale.Count == 0 || Eintraege == null) return;

            foreach (var eintrag in Eintraege)
            {
                var match = XmlMerkmale.FirstOrDefault(x => x.Bezeichnung.StartsWith(eintrag.MerkmalNameUndWert));
                if (match != null)
                {
                    eintrag.Id = match.Id;
                }
            }
            OnPropertyChanged(nameof(Eintraege));
        }
        [RelayCommand]
        private void ExportiereXml()
        {
            var exportService = new ExportXmlService();

            //Nur Einträge mit zugewiesener ID
            var idListe = GefilterteEintraege
                .Cast<MerkmalsEintrag>()
                .Where(x => !string.IsNullOrWhiteSpace(x.Id))
                .Select(x => x.Id)
                .Distinct()
                .ToList();

            if (idListe.Count == 0) return;

            SaveFileDialog sfd = new()
            {
                Filter = "XML Dateien (*.xml)|*.xml",
                FileName = $"{DateTime.Today.ToString("yyyy-MM-dd")} {AusgewaehlterTyp}_{AusgewaehltesMerkmalModell.Model} Projektfilter",
                Title = "Projektfilter-XML speichern"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                exportService.ExportiereXmlFilterDatei(idListe, sfd.FileName);
            }
        }

        private bool FilterEintraege(object obj)
        {
            if (obj is not MerkmalsEintrag eintrag) return false;

            bool typOk = string.IsNullOrWhiteSpace(AusgewaehlterTyp)
                || eintrag.Type == AusgewaehlterTyp;

            string modellFilter = AusgewaehltesMerkmalModell?.Model;

            bool modellOk = string.IsNullOrWhiteSpace(modellFilter)
                || eintrag.Model == modellFilter
                || eintrag.Model == "*";

            return typOk && modellOk;
        }

        partial void OnAusgewaehlterTypChanged(string value)
        {
            var neueModelle = Eintraege
    .Where(e => e.Type == value && e.Model != "*") // 👈 "*" ignorieren
    .Select(e => new MerkmalModelEintrag
    {
        Model = e.Model,
        ModellBez = e.ModellBez
    })
    .Where(m => !string.IsNullOrWhiteSpace(m.Model) && !string.IsNullOrWhiteSpace(m.ModellBez))
    .DistinctBy(m => m.Model)
    .OrderBy(m => m.ModellBez ?? string.Empty)
    .ToList();
            MerkmalModelle = new ObservableCollection<MerkmalModelEintrag>(neueModelle);
            AusgewaehltesMerkmalModell = null;
            GefilterteEintraege?.Refresh();
        }

        partial void OnAusgewaehltesMerkmalModellChanged(MerkmalModelEintrag value)

        {
            GefilterteEintraege?.Refresh();
        }
    }
}
