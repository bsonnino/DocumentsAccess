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
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DocumentsAccess
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void GetFolder(object sender, RoutedEventArgs e)
        {
            var folder = await SelectFolderAsync();
            if (folder != null)
            {
                DocsGrid.ItemsSource = null;
                StatusTxt.Text = "";
                var sw = new Stopwatch();
                sw.Start();
                var queryOptions = new QueryOptions
                {
                    FolderDepth = FolderDepth.Deep,
                    IndexerOption = IndexerOption.UseIndexerWhenAvailable
                };
                var query = folder.CreateFileQueryWithOptions(queryOptions);
                var allFiles = await query.GetFilesAsync();
                DocsGrid.ItemsSource = allFiles;
                sw.Stop();
                StatusTxt.Text = $"Ellapsed time: {sw.ElapsedMilliseconds}  {allFiles.Count} files";
            }
        }

        private static async Task<StorageFolder> SelectFolderAsync()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop
            };
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            return folder;
        }

        private async void GetFolderSlow(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = await SelectFolderAsync();
            if (folder != null)
            {
                DocsGrid.ItemsSource = null;
                StatusTxt.Text = "";
                var sw = new Stopwatch();
                sw.Start();
                _allFiles = new List<StorageFile>();
                await GetFilesInFolder(folder);
                DocsGrid.ItemsSource = _allFiles;
                sw.Stop();
                StatusTxt.Text = $"Ellapsed time: {sw.ElapsedMilliseconds}  {_allFiles.Count} files";
            }
        }

        private List<StorageFile> _allFiles;

        private async Task GetFilesInFolder(StorageFolder folder)
        {
            var items = await folder.GetItemsAsync();
            foreach (var item in items)
            {
                if (item is StorageFile)
                    _allFiles.Add(item as StorageFile);
                else
                    await GetFilesInFolder(item as StorageFolder);
            }
        }
    }
}
