using BackstopSampleAppForDocumentUpload.Authenticator;
using BackstopSampleAppForDocumentUpload.DataObjects;
using BackstopSampleAppForDocumentUpload.UserControls;
using BackstopSampleAppForDocumentUpload.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BackstopSampleAppForDocumentUpload
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

        public MainViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set { _mainViewModel = value; }
        }

        public MainWindow()
        {
            InitializeComponent();

            _mainViewModel = DataContext as MainViewModel;
            if (!ConnectionProperties.HasLoggedIn)
            {
                var loginWindow = new LoginControl();
                loginWindow.Width = 500;
                loginWindow.Height = 500;
                
                var loginClosed = loginWindow.ShowDialog();               
            }
            
        }

        private void SearchResultGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _mainViewModel.AttachTo = e.AddedItems[0] as Entity;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _mainViewModel.Initiliaze();
            }
            catch (System.Exception)
            {
            }
            
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var listView = e.Source as ListView;
            var selectedItems = new ObservableCollection<ActivityTag>();
            foreach (var item in listView.SelectedItems)
            {
                selectedItems.Add(item as ActivityTag);
            }

            _mainViewModel.SelectedActivityTags = selectedItems;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginControl();
            loginWindow.Owner = this;
            loginWindow.Width = 500;
            loginWindow.Height = 500;

            var loginClosed = loginWindow.ShowDialog();
        }

        private void DocumentList_Click(object sender, RoutedEventArgs e)
        {
            var documentWindow = new DocumentsControl();
            documentWindow.Owner = this;
            documentWindow.Width = 600;
            documentWindow.Height = 600;
            documentWindow.ShowDialog();
            
        }

        private void DrawDown_Click(object sender, RoutedEventArgs e)
        {
            var drawDown = new DrawDownControl();
            drawDown.Owner = this;
            drawDown.Width = 600;
            drawDown.Height = 600;
            drawDown.ShowDialog();
        }
    }
}
