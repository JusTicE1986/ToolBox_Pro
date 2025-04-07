using System;
using System.Globalization;
using System.Text.Json.Serialization;
using Timetracker.Helper;

namespace ToolBoxPro.Models
{
    public class ArbeitszeitTag
    {
        public DateTime Datum { get; set; } = DateTime.Today;
        public TimeSpan Start { get; set; } = TimeSpan.Zero;
        public TimeSpan Ende { get; set; } = TimeSpan.Zero;
        public TimeSpan Pause { get; set; } = TimeSpan.Zero;
        public string Notiz { get; set; } = string.Empty;

        [JsonIgnore]
        public string Wochentag => Datum.ToString("dddd", new CultureInfo("de-DE"));

        [JsonIgnore]
        public bool IstWochenende => Datum.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

        [JsonIgnore]
        public bool IstFeiertag => FeiertagsHelper.GetFeiertage(Datum.Year).ContainsKey(Datum.Date);


        [JsonIgnore]
        public bool IstErfasst => Ende > Start && BerechneteGearbeiteteZeit > TimeSpan.Zero;


        [JsonIgnore]
        public string Besonderheit
        {
            get
            {
                if (IstFeiertag) return "Feiertag";
                if (IstWochenende) return "Wochenende";
                return string.IsNullOrEmpty(Notiz) ? "Keine Besonderheit" : Notiz;
            }
        }

        [JsonIgnore]
        public TimeSpan BerechnetePause => Pause;

        [JsonIgnore]
        public TimeSpan BerechneteGearbeiteteZeit =>
            (Ende - Start - Pause) > TimeSpan.Zero
                ? Ende - Start - Pause
                : TimeSpan.Zero;
    }
}
