using BackstopSampleAppForDocumentUpload.Authenticator;
using BackstopSampleAppForDocumentUpload.Extensions;
using BackstopSampleAppForDocumentUpload.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace BackstopSampleAppForDocumentUpload.ViewModels
{
    public class LoginViewModel : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand LoginCommand => new RelayCommand<object>(Login, CanExecute);
        public ICommand CancelCommand => new RelayCommand<object>(Cancel);

        public static readonly DependencyProperty HasLoggedInProperty = 
            DependencyProperty.Register("HasLoggedIn", typeof(bool), typeof(LoginViewModel), 
                new FrameworkPropertyMetadata(false));

        public enum LoginNotificationStatus
        {
            GeneralError,
            UserPassIncorrect,
            DeviceNotRegistered,
            TwoFactorRequired,
            Success,
            SecondFactorIncorrect
        }
        #region Members        
        private string _username;
        private SecureString _password;
        private bool _isValidating;
        private Dictionary<string, string> _errors = new Dictionary<string, string>();
        private bool _isTestingConnection;
        private bool _canClose;
        private string _loginErrorMessage;
        private bool _hasLoggedIn;
        #endregion

        #region Properties
        public SecureString Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        public bool IsValidating
        {
            get { return _isValidating; }
            set { _isValidating = value; OnPropertyChanged(); }
        }
        public Dictionary<string, string> Errors
        {
            get { return _errors; }
            set { _errors = value; OnPropertyChanged(); }
        }       
        public bool IsTestingConnection
        {
            get { return _isTestingConnection; }
            set { _isTestingConnection = value; OnPropertyChanged(); }
        }        
        public bool CanClose
        {
            get { return _canClose; }
            set { _canClose = value; OnPropertyChanged(); }
        }
        
        public string LoginErrorMessage
        {
            get { return _loginErrorMessage; }
            set { _loginErrorMessage = value; OnPropertyChanged(); }
        }
        public bool HasLoggedIn
        {
            get { return _hasLoggedIn; }
            set { _hasLoggedIn = value; OnPropertyChanged(); }
        }
        
        public string Error { get; set; }
        public Action CloseAction { get; set; }
        public bool OverrideDomain { get; set; }
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            
        }
        #endregion

        #region Methods
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CanExecute(object obj)
        {
            return !IsTestingConnection;
        }
        private async void Login(object obj)
        {
            if (string.IsNullOrWhiteSpace(Username) || Password?.Length == 0)
                return;
            IsTestingConnection = true;
            DecodeUsername();
            ConnectionProperties.Password = Password;
            var loginService = new LoginService();
            var response = await loginService.LoginAsync();
            ValidateResponse(response);
            IsTestingConnection = false;
        }

        private void DecodeUsername()
        {
            var parts = Username?.Split('@');
            if (parts != null)
            {
                ConnectionProperties.Username = parts[0];
                if (parts.Length <= 1) return;
                var domain = parts[1];
                if (!string.IsNullOrEmpty(domain))
                {
                    ConnectionProperties.Uri = new Uri($"{"https://"}{domain}{".backstopsolutions.com"}");
                    OverrideDomain = true;
                }
            }
        }
        private void Cancel(object obj)
        {
            CloseAction();
        }
        
        private void ValidateResponse(ServiceResponse<string> response)
        {
            if (!string.IsNullOrEmpty(response.Data) && response.StatusCode == HttpStatusCode.OK)
            {
                ConnectionProperties.HasLoggedIn = true;

                var token = new SecureString();
                Array.ForEach(response.Data.ToArray(), token.AppendChar);
                token.MakeReadOnly();

                ConnectionProperties.Token = token;
                if (!OverrideDomain)
                {
                    string location;
                    response.Headers.TryGetValue("Location", out location);
                    if (!string.IsNullOrEmpty(location))
                        ConnectionProperties.Uri = new Uri(location);
                }

                CanClose = true;
                HasLoggedIn = true;
                CloseAction();

            }
        }
        
        #endregion
    }
}
