using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToolBox_Pro.ViewModels
{
    public class UserAccessViewModel:INotifyPropertyChanged
    {
        private string _currentUser;
        public string CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public Visibility AdminButtonVisibility => IsAdmin() ? Visibility.Visible : Visibility.Collapsed;
        public Visibility UserButtonVisibility => !IsAdmin() ? Visibility.Visible : Visibility.Collapsed;

        public UserAccessViewModel()
        {
            CurrentUser = GetCurrentWindowsUser();
        }

        private string GetCurrentWindowsUser() 
        {
            return Environment.UserName;
        }

        private bool IsAdmin() 
        {
            return CurrentUser.Equals("LNZNEUMA", StringComparison.OrdinalIgnoreCase) || CurrentUser.Equals("JusTicE1986", StringComparison.OrdinalIgnoreCase);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
