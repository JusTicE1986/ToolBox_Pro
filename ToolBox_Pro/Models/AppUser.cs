
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public partial class AppUser : ObservableObject
    {
        public string Username { get; set; } // von Environment.UserName
        public string DisplayName { get; set; } // Optional vom Admin ergänzt
        public UserRole Role { get; set; } = UserRole.NormalUser;
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsConfirmed { get; set; } = false;

        [ObservableProperty]
        private DateTime lastSeen;

        public bool IsCurrentlyOnline => LastSeen >= DateTime.Now.AddMinutes(-5);
        public string LastSeenDisplay => IsCurrentlyOnline ? "Online" : $"Zuletzt aktiv: {LastSeen:G}";

        [ObservableProperty]
        public int featureCounter;

        public string RankText
        {
            get
            {
                if (FeatureCounter >= 150)
                    return "Diamant 💎✨ – Exzellenz erreicht – dein Engagement setzt Maßstäbe.";
                if (FeatureCounter >= 100)
                    return "Platin 💎 – Herausragende Aktivität – du bist ein Power-User.";
                if (FeatureCounter >= 75)
                    return "Gold 🥇 – Ausgezeichnet – du gehörst zu den aktiven Nutzern.";
                if (FeatureCounter >= 50)
                    return "Silber 🥈 – Starke Leistung – du entwickelst dich weiter.";
                if (FeatureCounter >= 25)
                    return "Bronze 🥉 – Erste Meilensteine erreicht.";
                return "Eisen 💪 – Solider Start – weiter so";
            }
        }


        public UserRank Rank
        {
            get
            {
                if (FeatureCounter >= 150) return UserRank.Diamant;
                if (FeatureCounter >= 100) return UserRank.Platin;
                if (FeatureCounter >= 75) return UserRank.Gold;
                if (FeatureCounter >= 50) return UserRank.Silber;
                if (FeatureCounter >= 25) return UserRank.Bronze;
                return UserRank.Eisen;

            }
        }
        public enum UserRank
        {
            Eisen,
            Bronze,
            Silber,
            Gold,
            Platin,
            Diamant
        }
    }
}
