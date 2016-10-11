using BackstopSampleAppForDocumentUpload.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BackstopSampleAppForDocumentUpload.UserControls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : Window
    {
        private LoginViewModel _loginViewModel;

        public LoginViewModel LoginViewModel
        {
            get { return _loginViewModel; }
            set { _loginViewModel = value; }
        }

        public LoginControl()
        {
            InitializeComponent();
            _loginViewModel = DataContext as LoginViewModel;
            _loginViewModel.CloseAction = Close;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _loginViewModel.Password = ((PasswordBox)sender).SecurePassword;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_loginViewModel.CanClose) DialogResult = true;
        }
    }
}
