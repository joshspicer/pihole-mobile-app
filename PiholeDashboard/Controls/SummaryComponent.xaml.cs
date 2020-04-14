using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PiholeDashboard.Controls
{
    public partial class SummaryComponent : ContentView
    {

        // HEADING
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Heading),
                typeof(string),
                typeof(SummaryComponent),
                default(string));

        public string Heading
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


        // VALUE
        public static readonly BindableProperty MyValProperty =
            BindableProperty.Create("MyVal", typeof(string),
                typeof(SummaryComponent), string.Empty);

        public string MyVal
        {
            get { return (string)GetValue(MyValProperty); }
            set { SetValue(MyValProperty, value); }
        }


        // COLOR
        public static readonly BindableProperty ColorProperty =
     BindableProperty.Create(
         nameof(BgColor),
         typeof(Color),
         typeof(SummaryComponent),
         default(Color),
         Xamarin.Forms.BindingMode.TwoWay);

        public Color BgColor
        {
            get
            {
                return (Color)GetValue(ColorProperty);
            }

            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public SummaryComponent()
        {
            InitializeComponent();
            _context.BindingContext = this;
        }
    }
}
