using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace SampleMe
{
    public sealed partial class MainPage
    {
        FileOpenPicker openPicker = new FileOpenPicker();
        string[] csvContent;
    }

    class OrderRecord
    {
        public string TransactionId { get; set; }
        public string ProductNumber { get; set; }
    }
}
