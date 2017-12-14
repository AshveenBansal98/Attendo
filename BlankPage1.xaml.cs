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
using Microsoft.WindowsAzure.MobileServices;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Attendo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://attendoapp.azurewebsites.net");
        FaceServiceClient faceServiceClient = new FaceServiceClient("51465113ce1a4f0ba199cc6292482c91", "https://southeastasia.api.cognitive.microsoft.com/face/v1.0");
        public class TodoItem
        {
            public string Id { get; set; }
            public string Date { get; set; }
            public string Roll { get; set; }
        }

        public BlankPage1()
        {
            this.InitializeComponent();

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Roll.Text = "Roll no.(s) of Identified Persons: ";
            VisualizationCanvas.Children.Clear();
            FileOpenPicker photoPicker = new FileOpenPicker();
            photoPicker.ViewMode = PickerViewMode.Thumbnail;
            photoPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
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

            using (var stream = await photoFile.OpenAsync(FileAccessMode.Read))
            {
                
                var faces = await faceServiceClient.DetectAsync(stream.AsStream());
                var faceIds = faces.Select(face => face.FaceId).ToArray();
                var results = await faceServiceClient.IdentifyAsync("student", faceIds);

                foreach (var identifyResult in results)
                {
                    //Console.WriteLine("Result of face: {0}", identifyResult.FaceId);
                    if (identifyResult.Candidates.Length == 0)
                    {
                        string s = Roll.Text;
                        Roll.Text = s + "Not identified, ";
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        Person[] x = await faceServiceClient.ListPersonsAsync("student");
                        Candidate name = identifyResult.Candidates[0];
                        var candidateId = name.PersonId;
                        int p = 0;
                        for(int i = 0; i < x.Length; i++)
                        {
                            if (x[i].PersonId == candidateId)
                            {
                                p = 1;
                                break;
                            }
                        }
                        if (p == 0)
                        {
                            Roll.Text = Roll.Text + " Not identified, "; 
                            continue;
                        }
                        var person = await faceServiceClient.GetPersonAsync("student", candidateId);
                        string s = Roll.Text;
                        Roll.Text = s + person.Name + ", ";
                        string day = DateTime.Today.Day.ToString();
                        string month = DateTime.Today.Month.ToString();
                        string year = DateTime.Today.Year.ToString();
                        string date = month + "/" + day + "/" + year;
                        TodoItem item = new TodoItem {
                            Roll = person.Name,
                            Date = date
                        };
                        await MobileService.GetTable<TodoItem>().InsertAsync(item);
                    }
                }

                BitmapImage bitmapSource = new BitmapImage();

                IRandomAccessStream fileStream = await photoFile.OpenAsync(FileAccessMode.Read);
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);

                BitmapTransform transform = new BitmapTransform();
                const float sourceImageHeightLimit = 1280;

                if (decoder.PixelHeight > sourceImageHeightLimit)
                {
                    float scalingFactor = (float)sourceImageHeightLimit / (float)decoder.PixelHeight;
                    transform.ScaledWidth = (uint)Math.Floor(decoder.PixelWidth * scalingFactor);
                    transform.ScaledHeight = (uint)Math.Floor(decoder.PixelHeight * scalingFactor);
                }

                SoftwareBitmap sourceBitmap = await decoder.GetSoftwareBitmapAsync(decoder.BitmapPixelFormat, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage);

                const BitmapPixelFormat faceDetectionPixelFormat = BitmapPixelFormat.Gray8;

                SoftwareBitmap convertedBitmap;

                if (sourceBitmap.BitmapPixelFormat != faceDetectionPixelFormat)
                {
                    convertedBitmap = SoftwareBitmap.Convert(sourceBitmap, faceDetectionPixelFormat);
                }
                else
                {
                    convertedBitmap = sourceBitmap;
                }

                SolidColorBrush lineBrush = new SolidColorBrush(Windows.UI.Colors.Yellow);
                double lineThickness = 2.0;
                SolidColorBrush fillBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);

                ImageBrush brush = new ImageBrush();
                SoftwareBitmapSource bitmapsource = new SoftwareBitmapSource();
                await bitmapsource.SetBitmapAsync(sourceBitmap);
                brush.ImageSource = bitmapsource;
                brush.Stretch = Stretch.Fill;
                this.VisualizationCanvas.Background = brush;
                double widthScale = sourceBitmap.PixelWidth / this.VisualizationCanvas.ActualWidth;
                double heightScale = sourceBitmap.PixelHeight / this.VisualizationCanvas.ActualHeight;

                foreach (var face in faces)
                {
                    // Create a rectangle element for displaying the face box but since we're using a Canvas
                    // we must scale the rectangles according to the image’s actual size.
                    // The original FaceBox values are saved in the Rectangle's Tag field so we can update the
                    // boxes when the Canvas is resized.
                    Rectangle box = new Rectangle
                    {
                        Tag = face.FaceRectangle,
                        Width = (uint)(face.FaceRectangle.Width / widthScale),
                        Height = (uint)(face.FaceRectangle.Height / heightScale),
                        Fill = fillBrush,
                        Stroke = lineBrush,
                        StrokeThickness = lineThickness,
                        Margin = new Thickness((uint)(face.FaceRectangle.Left / widthScale), (uint)(face.FaceRectangle.Top / heightScale), 0, 0)
                    };
                    this.VisualizationCanvas.Children.Add(box);
                }
            }
        }

    }
}
