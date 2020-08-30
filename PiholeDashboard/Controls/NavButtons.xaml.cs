using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PiholeDashboard.Controls
{
    public partial class NavButtons : ContentView
    {
        public NavButtons()
        {
            InitializeComponent();
        }

        async void LeftButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///connected");
        async void CenterButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///browse");
        async void RightButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///settings");
    }
}
