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
        
        private async void fileButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                ParseFile(file);
            }
            else
            {
                statusBox.Text = "Not a valid file.";
            }
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            var xxx = GenerateEdges(csvContent);
        }
    }
}
