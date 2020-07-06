using Xamarin.Forms;

namespace PiholeDashboard
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false);
        }
    }
}
