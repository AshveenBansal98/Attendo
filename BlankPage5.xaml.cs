using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Attendo
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    
    /// </summary>
    public sealed partial class BlankPage5 : Page
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://attendoapp.azurewebsites.net");
        public BlankPage5()
        {
            this.InitializeComponent();
            
        }
        public class TodoItem
        {
            public string Id { get; set; }
            public string Date { get; set; }
            public string Roll { get; set; }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Picker.Date == null)
                return;
            string day = Picker.Date.Value.Day.ToString();
            string month = Picker.Date.Value.Month.ToString();
            string year = Picker.Date.Value.Year.ToString();
            string date = month + "/" + day + "/" + year;

            List<TodoItem> l =await MobileService.GetTable<TodoItem>().ToListAsync();
            TodoItem t = new TodoItem
            {
                Date=date
            };
            string s = "Date: " + date + "\n" ;
            TodoItem[] x1 = l.ToArray();
            int p = 0;
            for (int i = 1; i <= 10; i++)
            {
                p = 0;
                t.Roll = i.ToString();
                for (int j = 0; j < x1.Length; j++)
                {
                    if (x1[j].Roll == t.Roll && x1[j].Date==t.Date)
                    {
                        p = 1;
                        break;
                    }
                }
              
                if (p==1)
                {
                    s = s + i.ToString() + ": Present\n";
                }
                else
                {
                    s = s + i.ToString() + ": Absent\n";
                }
            }
            Roll.Text = s;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
