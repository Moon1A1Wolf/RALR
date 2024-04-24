using System.Net;

namespace RALR
{
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            HideLoading();
        }

        private async void Connect_Button_Clicked(object sender, EventArgs e)
        {
            await ShowLoading();
            bool retry = true;

            while (retry)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverAddress.Text);

                using (HttpWebResponse resp = (HttpWebResponse)await request.GetResponseAsync())
                {
                    await HideLoading();
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                        {
                            await DisplayAlert("Response", sr.ReadToEnd(), "Ok");

                            await DisplayAlert("Response", "Connected successfully.", "Ok");

                            App.Current.MainPage = new HostsPage();

                        }
                    }
                    else
                    {
                        retry = await DisplayAlert("Invalid credentials", "Please ensure your server address, username and password are correct.", "Retry", "Ok");
                    }
                }
                retry = false;
            }
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
