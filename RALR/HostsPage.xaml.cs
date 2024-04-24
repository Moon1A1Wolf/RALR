using System.Net;
using System.Windows.Input;

namespace RALR
{
    public partial class HostsPage : ContentPage
    {
        public static List<string> hosts = new List<string> { "gg", "hh"};
        public ICommand ManageCommand { get; private set; }
        public HostsPage()
        {
            InitializeComponent();
            listView.ItemsSource = hosts;
            HideLoading();
            ManageCommand = new Command<string>(
            async (string ip) =>
            {
                await DisplayAlert("GG", ip, "Ok");
                await ShowLoading();
                App.currIP = ip;
                App.Current.MainPage = new ConnectedNavShell();
            });

            BindingContext = this;
        }

        
        private async Task ShowLoading()
        {
            LoadingShroud.IsVisible = true;
            LoadingIndicator.IsVisible = true;
        }
        private async Task HideLoading()
        {
            LoadingShroud.IsVisible = false;
            LoadingIndicator.IsVisible = false;
        }

    }

}
