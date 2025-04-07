using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public partial class CleanupViewModel : ObservableObject
    {
        private readonly CleanupService _cleanupService;

        public CleanupViewModel()
        {
            _cleanupService = new CleanupService();
        }

        [ObservableProperty]
        private bool isBusy;

        [RelayCommand]
        private async Task CleanupST4PdfAsync()
        {
            await CleanupAsync(@"\\lnzdfs1.wnad.net\SAP-Shares\KMR\ST4\ST4_pdf_Automated_Production");
        }

        [RelayCommand]
        private async Task CleanupDIRSuccessAsync()
        {
            await CleanupAsync(@"\\lnzdfs1.wnad.net\SAP-Shares\KMR\ST4\DIR_success");
        }

        [RelayCommand]
        private async Task CleanupDIRFailedAsync()
        {
            await CleanupAsync(@"\\lnzdfs1.wnad.net\SAP-Shares\KMR\ST4\DIR_failed");
        }

        private async Task CleanupAsync(string path)
        {
            if (IsBusy) return;

            IsBusy = true;

            await Task.Run(() => _cleanupService.CleanupFolder(path));

            IsBusy = false;
        }
    }
}
