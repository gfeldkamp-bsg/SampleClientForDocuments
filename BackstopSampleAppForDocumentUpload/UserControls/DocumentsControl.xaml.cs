using BackstopSampleAppForDocumentUpload.Authenticator;
using BackstopSampleAppForDocumentUpload.BackstopCrm;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using BackstopSampleAppForDocumentUpload.Extensions;
using BackstopSampleAppForDocumentUpload.Services;
using BackstopSampleAppForDocumentUpload.DataObjects;
using System.Threading.Tasks;

namespace BackstopSampleAppForDocumentUpload.UserControls
{
    /// <summary>
    /// Interaction logic for DocumentsControl.xaml
    /// </summary>
    public partial class DocumentsControl : Window
    {
        public DocumentsControl()
        {
            InitializeComponent();            
        }

        private async Task GetDocuments()
        {
            var remoteAddress = new EndpointAddress($"{ConnectionProperties.Uri}/backstop/services/BackstopCrmService_1_6");
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            var client = new BackstopCrm.BackstopCrmService_1_6PortTypeClient(binding, remoteAddress);
            var login = new LoginInfoType() { Username = ConnectionProperties.Username, Password = ConnectionProperties.Password.ToInsecureString() };
            var docs = client.getAllDocuments(login);
            var relatedLinks = client.getAllDocumentRelations(login);
            var docService = new DocumentService();
            
            var documents = new List<DataObjects.DocumentInformation>();
            int index = 0;
            foreach (var item in docs)
            {                
                var newDocument = new DataObjects.DocumentInformation()
                {
                    Id = item.id.Value,
                    FileName = item.filename,
                    Title = item.title,
                    CreatedDate = item.createdDate,
                    Links = new List<Link>()
                };

                //now we have to go thru each document and get its metadata. 
                //this will take time since we go thru each and every document one at a time.

                try
                {
                    var result = await docService.GetDocument(newDocument.Id);
                    newDocument.Links = result.Data.Links;
                }
                catch (System.Exception ex)
                {
                    //we need to log this error.
                }
                
                documents.Add(newDocument);
                index++;
                if (index == 20) break;
            }

            DocGrid.ItemsSource = documents;
            
        }

        private async void Btn_Click(object sender, RoutedEventArgs e)
        {
            await GetDocuments();
        }
    }
}
