using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PiholeDashboard.Controls
{
    public partial class TopClients : ContentView
    {
        public TopClients()
        {
            InitializeComponent();
            _context.BindingContext = this;
        }

        // HEADING
        public static readonly BindableProperty ClientsProperty =
            BindableProperty.Create(
                nameof(Clients),
                typeof(Dictionary<string, int>),
                typeof(TopClients),
                default(Dictionary<string, int>),
                propertyChanged: OnClientsChanged);

        private static void OnClientsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Console.WriteLine("BINDABLE CHANGED!!");
        }

        public Dictionary<string,int> Clients
        {
            get
            {
                return (Dictionary<string, int>)GetValue(ClientsProperty);
            }

            set
            {
                SetValue(ClientsProperty, value);
            }
        }
    }
}
