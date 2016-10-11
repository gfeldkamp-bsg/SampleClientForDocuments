using BackstopSampleAppForDocumentUpload.Extensions;
using System.ComponentModel;
using System.Windows.Input;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using BackstopSampleAppForDocumentUpload.Services;
using BackstopSampleAppForDocumentUpload.DataObjects;
using System.Collections.Generic;
using BackstopSampleAppForDocumentUpload.Authenticator;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO;

namespace BackstopSampleAppForDocumentUpload.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SearchCommand => new RelayCommand<object>(Search, CanExecute);
        public ICommand FilePickerCommand => new RelayCommand<object>(FilePicker);
        public ICommand UploadCommand => new RelayCommand<object>(Uplod);
        
        #region Members
        private string _searchTerm;
        private ObservableCollection<Entity> _searchResults;
        private bool _isLoggedIn;
        private Entity _attachTo;
        private ObservableCollection<ActivityTag> _activityTags;
        private ObservableCollection<ActivityTag> _selectedActivityTags;
        private string _uploadStatus;
        private string _selectedFile;
        #endregion

        #region Properties            
        public Entity AttachTo
        {
            get { return _attachTo; }
            set { _attachTo = value; OnPropertyChanged(); }
        }
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; OnPropertyChanged(); }
        }
        public string UploadStatus
        {
            get { return _uploadStatus; }
            set { _uploadStatus = value; OnPropertyChanged(); }
        }
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ActivityTag> ActivityTags
        {
            get { return _activityTags; }
            set { _activityTags = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ActivityTag> SelectedActivityTags
        {
            get { return _selectedActivityTags; }
            set { _selectedActivityTags = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Entity> SearchResults
        {
            get { return _searchResults; }
            set { _searchResults = value; OnPropertyChanged(); }
        }
        public string SelectedFile
        {
            get { return _selectedFile; }
            set { _selectedFile = value; OnPropertyChanged(); }
        }
        public LoginViewModel LoginModel { get; set; }
        
        #endregion


        public MainViewModel()
        {
            IsLoggedIn = ConnectionProperties.HasLoggedIn;
            SearchResults = new ObservableCollection<Entity>();
            SelectedFile = "Please click the button to select a file...";
        }

        #region Methods

        public async Task Initiliaze()
        {
            var tagService = new ActivityTagService();
            var result = await tagService.GetActivityTagsAsync();
            ActivityTags = new ObservableCollection<ActivityTag>(result.Data);
            
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Search(object obj)
        {
            var quickSearcService = new QuickSearchService();
            var result = await quickSearcService.SearchAsync(SearchTerm, 100);
            foreach (var item in result.Data)
            {
                SearchResults.Add(item.ToEntity());
            }
            
        }

        private void FilePicker(object obj)
        {
            var filePicker = new OpenFileDialog();
            var result = filePicker.ShowDialog();
            if(result == DialogResult.OK)
            {
                SelectedFile = filePicker.FileName;                
            }
        }

        private async void Uplod(object obj)
        {
            var docService = new DocumentService();
            var fileInfo = new FileInfo(SelectedFile);
            var document = new DocumentUpload()
            {
                AttachedTo = new AttachedTo() { EntityId = AttachTo.Id, EntityType = AttachTo.EntityType },
                FileName = fileInfo.Name,
                Title = fileInfo.Name,
                ActivityTags = SelectedActivityTags,
                Links = new Entity[] { },
                EffectiveDate = DateTime.Today,
                Bytes = File.ReadAllBytes(SelectedFile)
            };

            var result = await docService.CreateDocumentAsync(document);
            if(result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                UploadStatus = "Document successfully uploaded.";
            }else
            {
                UploadStatus = "There was a problem uploading the file.";
            }
        }

        private bool CanExecute(object obj)
        {
            return true;
        }
        #endregion
    }
}
