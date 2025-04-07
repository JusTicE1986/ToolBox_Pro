using System;
using System.Diagnostics;
using System.Globalization;

namespace ToolBoxPro.Helper
{
    public static class KulturHelper
    {
        private static readonly CultureInfo kulturInfo = new("de-DE");

        public static int GetKalenderwoche(DateTime datum)
            => kulturInfo.Calendar.GetWeekOfYear(datum, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        public static DateTime GetStartDerKalenderwoche(int jahr, int kw)
        {
            Debug.WriteLine($"[KW-BERECHNUNG] Jahr: {jahr}, KW: {kw}");

            if (kw < 1 || kw > 53)
                throw new ArgumentOutOfRangeException(nameof(kw), $"Ungültige Kalenderwoche: {kw}");

            var jan4 = new DateTime(jahr, 1, 4);
            int delta = DayOfWeek.Monday - jan4.DayOfWeek;
            if (delta > 0) delta -= 7;
            var start = jan4.AddDays(delta);

            var result = start.AddDays((kw - 1) * 7);

            Debug.WriteLine($"[KW-BERECHNUNG] Ergebnis: {result:yyyy-MM-dd}");
            return result;
        }

    }
}
