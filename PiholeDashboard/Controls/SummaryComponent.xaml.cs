using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PiholeDashboard.Controls
{
    public partial class SummaryComponent : ContentView
    {

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(SummaryComponent),
                default(string),
                Xamarin.Forms.BindingMode.OneWay);

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        public SummaryComponent()
        {
            InitializeComponent();
        }
    }
}
