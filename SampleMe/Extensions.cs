using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SampleMe
{
    public sealed partial class MainPage
    {
        FileOpenPicker openPicker = new FileOpenPicker();
        List<OrderRecord> csvContent = new List<OrderRecord>();

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
                }
            }
            return textBody;
        }

        private async void ParseFile(StorageFile file)
        {
            var fileContents = await ReadFile(file);
            if (fileContents != null)
            {
                statusBox.Text = String.Format("File read OK! {0} Characters.", fileContents.Length);
                foreach (var line in fileContents.Split('\n'))
                {
                    var lineContent = line.Split(',');
                    if (lineContent.Length == 2)
                    {
                        csvContent.Add(new OrderRecord(lineContent));
                    }
                }
                var distinctTransactions = csvContent.Select(x => x.TransactionId).Distinct().Count();
                var distinctProducts = csvContent.Select(x => x.ProductNumber).Distinct().Count();
                statusBox.Text = String.Format("Transctions: {0}, Products: {1}", distinctTransactions, distinctProducts);
            }
        }

        private List<ProductEdges> GenerateEdges(List<OrderRecord> orders)
        {
            var transArray = orders.Select(x => x.TransactionId).Distinct().ToArray();
            var productArray = orders.Select(x => x.ProductNumber).Distinct().ToList();
            var productSums = orders.GroupBy(x => x.ProductNumber).Select(g => new { Symbol = g.Key, Count = g.Count() });
            int purchasesMade = orders.Count;
            int productCount = productArray.Count();
            var freqMat = new int[productCount][];
            var probMat = new double[productCount][];
            var confMat = new double[productCount][];
            for (int i = 0; i < productCount; i++)
            {
                freqMat[i] = new int[productCount];
                probMat[i] = new double[productCount];
                confMat[i] = new double[productCount];
            }

            var edges = new List<ProductEdges>();


            Parallel.For(0, transArray.Length, i =>
           {
               var basket = orders.Where(x => x.TransactionId == transArray[i]);
               var products = basket.Select(x => x.ProductNumber).ToArray();

               // Som en Cross Apply.
               foreach (var product in products)
               {
                   int xindex = productArray.IndexOf(product);
                   foreach (var product2 in products)
                   {
                       int yindex = productArray.IndexOf(product2);
                       freqMat[xindex][yindex] += 1;
                   }
               }
           });
            int ProdSum = 0;

            Parallel.For(0, freqMat.Length, i =>
           {
               for (int j = 0; j < freqMat[i].Length; j++)
               {
                   probMat[i][j] = freqMat[i][j] / purchasesMade;
               }
           });

            statusBox.Text = String.Format("Products in basket {0}", ProdSum);
            return edges;
        }
    }
}