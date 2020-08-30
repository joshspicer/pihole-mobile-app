using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PiholeDashboard.Controls
{
    public partial class StatusIndicator : ContentView
    {
        public StatusIndicator()
        {
            InitializeComponent();
            _context.BindingContext = this;
        }

        async void Interact_Clicked(object sender, EventArgs e)
        {
            //BG.ShiftColorTo(new Color(1, 0, 0), new Color(0, 1, 0), color => BG.BackgroundColor = color);
        }

        public static readonly BindableProperty StatusProperty =
            BindableProperty.Create(
                nameof(Status),
                typeof(string),
                typeof(StatusIndicator),
                default(string),
                propertyChanged: OnClientsChanged);

        private static void OnClientsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Console.WriteLine("Status indicator CHANGED!!");
        }


        public Color Status
        {
            get
            {
                var strStatus = GetValue(StatusProperty) as string;
                if (strStatus is string && strStatus != "")
                {
                    switch (strStatus.ToLower())
                    {
                        case "enabled":
                            return (Xamarin.Forms.Color)App.Current.Resources["green"];
                        case "disabled":
                        default:
                            return (Xamarin.Forms.Color)App.Current.Resources["red"];
                    }
                }
                return (Xamarin.Forms.Color)App.Current.Resources["orange"];
            }

            set
            {
                SetValue(StatusProperty, value);
            }
        }
    }
}

public static class ViewExtensions
{
    // gist.github.com/danvanderboom/0319256fa3f1f2f50be2
    public static void ShiftColorTo(this VisualElement view, Color sourceColor, Color targetColor, Action<Color> setter, uint length = 250, Easing easing = null)
    {
        view.Animate("ShiftColorTo",
            x =>
            {
                var red = sourceColor.R + (x * (targetColor.R - sourceColor.R));
                var green = sourceColor.G + (x * (targetColor.G - sourceColor.G));
                var blue = sourceColor.B + (x * (targetColor.B - sourceColor.B));
                var alpha = sourceColor.A + (x * (targetColor.A - sourceColor.A));

                setter(Color.FromRgba(red, green, blue, alpha));
            },
            length: length,
            easing: easing);
    }
}