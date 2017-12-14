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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Attendo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage2 : Page
    {
        FaceServiceClient faceServiceClient = new FaceServiceClient("51465113ce1a4f0ba199cc6292482c91", "https://southeastasia.api.cognitive.microsoft.com/face/v1.0");
        Face[] faces;                   // The list of detected faces.
        String[] faceDescriptions;      // The list of descriptions for the detected faces.
        double resizeFactor;            // The resize factor for the displayed image.

        public BlankPage2()
        {
            this.InitializeComponent();
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            string roll = Roll.Text;
            if(Roll.Text.Length == 0){
                Roll.PlaceholderText = "Enter roll number first";
                return;
            }
            FileOpenPicker photoPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            photoPicker.FileTypeFilter.Add(".jpg");
            photoPicker.FileTypeFilter.Add(".jpeg");
            photoPicker.FileTypeFilter.Add(".png");
            photoPicker.FileTypeFilter.Add(".bmp");

            StorageFile photoFile = await photoPicker.PickSingleFileAsync();
            if (photoFile == null)
            {
                return;
            }

            string filePath = photoFile.Path;
            PersonGroup[] x = await faceServiceClient.ListPersonGroupsAsync();
            int p = -1;
            Guid value;
            Person[] y;
            CreatePersonResult friend1;
            y = await faceServiceClient.ListPersonsAsync("student");
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i].Name == roll)
                {
                    p = i;
                    break;
                }
            }
            if (p == -1)
            {
                friend1 = await faceServiceClient.CreatePersonAsync("student", roll);
                value = friend1.PersonId;
            }
            else
            {
                value = y[p].PersonId;
            }
            //using (Stream s = File.OpenRead(filePath)) // thsi statement doesn't work in windows UWP. It works only on windows desktop applications and console apps.
            using (var stream = await photoFile.OpenAsync(FileAccessMode.Read))
            {
                await faceServiceClient.AddPersonFaceAsync("student", value, stream.AsStream());
            }
            await faceServiceClient.TrainPersonGroupAsync("student");
            try
            {
                while (true)
                {
                    TrainingStatus trainingStatus = await faceServiceClient.GetPersonGroupTrainingStatusAsync("student");
                    Roll.Text = trainingStatus.Status.ToString();
                    if (trainingStatus.Status != Status.Running)
                    {
                        break;
                    }
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Roll.Text = ex.Message;
            }

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
