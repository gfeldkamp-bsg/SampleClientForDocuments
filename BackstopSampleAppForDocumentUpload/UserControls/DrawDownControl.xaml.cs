using BackstopSampleAppForDocumentUpload.Authenticator;
using BackstopSampleAppForDocumentUpload.BackstopWidgetService;
using BackstopSampleAppForDocumentUpload.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Windows;

namespace BackstopSampleAppForDocumentUpload.UserControls
{
    /// <summary>
    /// Interaction logic for DrawDownControl.xaml
    /// </summary>
    public partial class DrawDownControl : Window
    {
        public DrawDownControl()
        {
            InitializeComponent();
        }

        private WidgetResult GetWidgetResult()
        {
            var remoteAddress = new EndpointAddress($"{ConnectionProperties.Uri}/backstop/services/BackstopWidgetService_1_0");
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            var client = new BackstopWidgetService_1_0PortTypeClient(binding, remoteAddress);
            var login = new LoginInfoType() { Username = ConnectionProperties.Username, Password = ConnectionProperties.Password.ToInsecureString() };
            var args = new Dictionary<string, object> { {"fundId", txtFundId.Text}, {"asOf", DateTime.Now} };
            var fvps = new List<FieldValuePair>();
            foreach (var x in args)
            {
                fvps.Add(
                    new FieldValuePair
                    {
                        field = x.Key,
                        value = (x.Value as DateTime?)?.ToString("yyyy-MM-dd") ?? x.Value.ToString()
                    });
            }

            var response = client.getWidget(login, "Drawdown Report", fvps.ToArray());
            return response;
            
        }

        public DataTable GetDrawDownData()
        {
            WidgetResult wr = GetWidgetResult();
            DataTable dt = new DataTable();
            for (int c = 0; c < wr.names.Length; c++)
            {
                Type t = wr.types[c] == "integer" ? typeof(int) : wr.types[c] == "date" ? typeof(DateTime) : typeof(string);
                dt.Columns.Add(wr.names[c], t);
            }
            for (int r = 0; r < wr.data.Length; r++)
            {
                object[] convertedData = wr.types.Zip(wr.data[r],
                    (type, value) => type == "integer" && value == string.Empty ? null : value).ToArray();
                dt.Rows.Add(convertedData);
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "Depth desc";
            DataTable sortedDT = dv.ToTable();
            return sortedDT;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = GetDrawDownData();

            var row = dataTable.Rows[0];

            DataTable table = new DataTable();
            table.Columns.Add("Key", typeof(string));
            table.Columns.Add("Value", typeof(string));

            var startDate = (row["Start Date"] as DateTime?)?.ToString("MMMM yy");
            var endDate = (row["End Date"] as DateTime?)?.ToString("MMMM yy");
            
            table.Rows.Add("Max Drawdown (Start / Recovery)", string.Concat(startDate, "-", endDate));
            table.Rows.Add("Max Drawdown Depth", row["Depth"]);
            table.Rows.Add("Months in Maximum Drawdown", row["Length"]);
            table.Rows.Add("Months to Recover", row["Recovery"]);
            dgTable.ItemsSource = table.DefaultView;
            dgDrawData.ItemsSource = dataTable.DefaultView;
        }
    }
}
