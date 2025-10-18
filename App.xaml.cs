using AROMADICOFFE.Pages;

namespace AROMADICOFFE
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
