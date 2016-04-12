using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SampleMe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            InitalizeFilePicker();
        }

        private void InitalizeFilePicker()
        {
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");
        }

        private async Task<string> ReadFile(StorageFile file)
        {
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            string textBody;
            ulong size = stream.Size;
            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                {
                    dataReader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    textBody = dataReader.ReadString(numBytesLoaded);
                    Debug.WriteLine(textBody.Trim());
                }
            }
            return textBody;
        }
        
        private async void fileButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var fileContents = ReadFile(file);
                if (fileContents != null)
                    statusBox.Text = "File read OK!";
            }

            else

            {
                statusBox.Text = "Not a valid file.";
            }

        }
    }
}
