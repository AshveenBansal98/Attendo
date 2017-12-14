using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
using Microsoft.WindowsAzure.MobileServices;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Attendo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        FaceServiceClient faceServiceClient = new FaceServiceClient("51465113ce1a4f0ba199cc6292482c91", "https://southeastasia.api.cognitive.microsoft.com/face/v1.0");
        Face[] faces;                   // The list of detected faces.
        String[] faceDescriptions;      // The list of descriptions for the detected faces.
        double resizeFactor;            // The resize factor for the displayed image.

        public MainPage()
        {
            this.InitializeComponent();

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage1));
        }

        private void HyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage2));
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage3));
        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage4));
        }

        private void HyperlinkButton_Click_3(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage5));
        }
    }
}
