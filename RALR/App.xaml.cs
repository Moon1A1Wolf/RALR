namespace RALR
{
    public partial class App : Application
    {
        public static string currIP;
        public static string connnection;
        public App()
        {
            InitializeComponent();
            ModifyEntry();
            MainPage = new LoginPage();
        }

        void ModifyEntry()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {
            #if ANDROID
            handler.PlatformView.SetSelectAllOnFocus(true);
            #elif WINDOWS
            handler.PlatformView.GotFocus += (s, e) =>
            {
                handler.PlatformView.SelectAll();
            };
            #endif
            });
        }
    }
}
