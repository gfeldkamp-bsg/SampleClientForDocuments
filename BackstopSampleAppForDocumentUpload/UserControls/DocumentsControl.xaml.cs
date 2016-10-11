using BackstopSampleAppForDocumentUpload.Authenticator;
using BackstopSampleAppForDocumentUpload.BackstopCrm;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using BackstopSampleAppForDocumentUpload.Extensions;

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
            GetDocuments();
        }

        private void GetDocuments()
        {
            var remoteAddress = new EndpointAddress($"{ConnectionProperties.Uri}/backstop/services/BackstopCrmService_1_6");
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            var client = new BackstopCrm.BackstopCrmService_1_6PortTypeClient(binding, remoteAddress);
            var login = new LoginInfoType() { Username = ConnectionProperties.Username, Password = ConnectionProperties.Password.ToInsecureString() };
            var docs = client.getAllDocuments(login);
            var documents = new List<DataObjects.DocumentInformation>();
            foreach (var item in docs)
            {
                documents.Add(new DataObjects.DocumentInformation() { Id = item.id.Value, FileName = item.filename, Title = item.title, CreatedDate = item.createdDate });
            }
            
            DocGrid.ItemsSource = documents;

        }
        
    }
}
