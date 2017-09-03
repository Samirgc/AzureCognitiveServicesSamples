using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AzureCognitiveServicesSamples.FaceAPI
{
    /// <summary>
    /// Interaction logic for DetectAndVerify.xaml
    /// </summary>
    public partial class DetectAndVerify : Window
    {

        private string personImageUri;
        private string imageToCheckUri;
        public DetectAndVerify()
        {
            InitializeComponent();
        }

        private void PersonImageBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog(this) == true)
            {
                PersonImage.Source = new BitmapImage(new Uri(dialog.FileName));
                personImageUri = dialog.FileName;
            }
        }

        private void ImageToCheckBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog(this) == true)
            {
                ImageToCheck.Source = new BitmapImage(new Uri(dialog.FileName));
                imageToCheckUri = dialog.FileName;
            }

        }

        private async void VerifySimilarityButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VerifyResult.Content = "";
                // first we need to use detect api for both images 
                if (string.IsNullOrEmpty(personImageUri))
                {
                    MessageBox.Show("Choose person image first");
                    return;
                }
                if (string.IsNullOrEmpty(imageToCheckUri))
                {
                    MessageBox.Show("Choose the other image to compare");
                    return;
                }
                DisableButton();

                VerifyResult.Content = "Loading ...";

                var personImageResult = await FaceAPI.DetectAsync(personImageUri);
                var ImageToCheckResult = await FaceAPI.DetectAsync(imageToCheckUri);

                if (personImageResult.Count > 1)
                {
                    EnableButtons();
                    MessageBox.Show($"number of people in person image is {personImageResult.Count}");
                    return;
                }
                if (ImageToCheckResult.Count > 1)
                {
                    EnableButtons();
                    MessageBox.Show($"number of people in compare image is {ImageToCheckResult.Count}");
                    return;
                }

                // using the returned detect id for each image, we will call the verify api
                var result = await FaceAPI.VerifyAsync(personImageResult.Single().faceId, ImageToCheckResult.Single().faceId);

                VerifyResult.Content = $"Is Identical :{result.isIdentical} - Confidence :{result.confidence}";
                EnableButtons();
            }
            catch (Exception exc)
            {
                VerifyResult.Content = "Error Happended !";
                EnableButtons();
            }
        }

        private void EnableButtons()
        {
            VerifySimilarityButton.IsEnabled = true;
            PersonImageBrowseButton.IsEnabled = true;
            ImageToCheckBrowseButton.IsEnabled = true;

        }

        private void DisableButton()
        {
            VerifySimilarityButton.IsEnabled = false;
            PersonImageBrowseButton.IsEnabled = false;
            ImageToCheckBrowseButton.IsEnabled = false;
        }
    }
}
