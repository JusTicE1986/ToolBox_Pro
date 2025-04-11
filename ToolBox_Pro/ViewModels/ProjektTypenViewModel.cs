using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.ViewModels
{
    public partial class ProjektTypenViewModel : ObservableObject
    {
        public ObservableCollection<ProjektTypEntry> ProjektTypen { get; } = new()
        {
            new ProjektTypEntry
            {
                Typ = "A04-01",
                BezWM = "2060",
                BezWN = "WL750",
                BezKramerBau = "",
                BezKramerLand = "",
                SOPNullserie = DateTime.Today.AddDays(10),
                SODNullserie = null,
                SOPSerie = DateTime.Today.AddMonths(1),
                SODSerie = DateTime.Today.AddMonths(3),
                IstAktiv = true
            },
            new ProjektTypEntry
            {
                Typ = "RL40LP",
                BezWM = "2080LP",
                BezWN = "WL34",
                BezKramerBau = "",
                BezKramerLand = "",
                SOPNullserie = null,
                SOPSerie = null,
                IstAktiv = false
            }
        };

        public IEnumerable<ProjektTypEntry> ForecastSOPs =>
            ProjektTypen
                .Where(p => p.IstAktiv && p.IstSOPIn2Monaten)
                .OrderBy(p => p.SOPNullserie ?? p.SOPSerie);

        public IEnumerable<ProjektTypEntry> AktiveProjektTypen =>
            ProjektTypen.Where(p => p.IstAktiv);
    }
}
