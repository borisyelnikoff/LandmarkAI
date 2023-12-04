using Microsoft.Win32;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LandmarkAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            var imageFolderPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\..\resources"));
            var dialog = new OpenFileDialog
            {
                Filter = "Image files (*.png; *.jpg)|*.png;*.jpg;*.jpeg|All files(*.*)|*.*",
                InitialDirectory = imageFolderPath
            };
            if (dialog.ShowDialog() == true)
            {
                var fileName = dialog.FileName;
                selectedImage.Source = new BitmapImage(new Uri(fileName));
                await MakePredictionAsync(fileName);
            }
        }

        private async Task MakePredictionAsync(string fileName)
        {
            var url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.1/Prediction/bf39d301-3888-43cf-91fe-1509ce5ac26a/image?iterationId=2cc8d120-36d9-417c-b3c3-213b910a3f20";
            var prediction_key = "fd63926c323344a0aacaa249ebd73fc6";
            var content_type = "application/octet-stream";
            var imageFile = File.ReadAllBytes(fileName);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", prediction_key);
            using var content = new ByteArrayContent(imageFile);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(content_type);
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}