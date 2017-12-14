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
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford.Common.Contract;
using Windows.Media.Capture;
using Windows.Graphics.Display;
using Windows.Storage.Streams;
using Windows.Media.MediaProperties;
using Windows.UI.Xaml.Media.Imaging;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Graphics.Imaging;
using Windows.Media.FaceAnalysis;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Attendo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class BlankPage4 : Page
    {
        FaceServiceClient faceServiceClient = new FaceServiceClient("51465113ce1a4f0ba199cc6292482c91", "https://southeastasia.api.cognitive.microsoft.com/face/v1.0");
        public BlankPage4()
        {
            this.InitializeComponent();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string roll = Roll.Text;
            Guid value;
            Person[] y;
            //Person d = null;
            int p = -1;
            y = await faceServiceClient.ListPersonsAsync("student");
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i].Name == roll)
                {
                    p = 0;
                    value = y[i].PersonId;
                   // d = y[i];
                    break;
                }
            }
            if (p == -1)
            {
                Roll.Text = "Cannot delete as given roll number does not exist";
            }
            else
            {
                /*if (d == null)
                    return;
                
                for(int i =0; i < d.PersistedFaceIds.Length; i++)
                {
                    await faceServiceClient.DeleteFaceFromFaceListAsync(d.Name, d.PersistedFaceIds[i]);
                }*/
                
                await faceServiceClient.DeletePersonAsync("student", value);
                Roll.Text = "Deleted Successfully";
            }
            
        }
    }
}
