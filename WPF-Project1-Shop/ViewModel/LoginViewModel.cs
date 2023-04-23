using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Security;
using System.Threading;
using System.Security.Principal;
using WPF_Project1_Shop.EFModel;
using WPF_Project1_Shop.EFCustomRepository;

namespace WPF_Project1_Shop.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        //Fields
        private string _userName;
        // private SecureString _password;
        private string _password;
        private string _errorMessage;
        private bool _isViewVisible = true;
        private bool _isRememberPassword = false;

        AccountRepository accountRepository = new AccountRepository(new RailwayContext());

        // Properties
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        //public SecureString Password
        //{
        //    get => _password; set
        //    {
        //        _password = value;
        //        OnPropertyChanged(nameof(Password));
        //    }
        //}

        public string Password
        {
            get => _password; set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage; 
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));

            }
        }
        public bool IsViewVisible
        {
            get => _isViewVisible; 
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible)); 
            }
        }


        public bool IsRememberPassword
        {
            get => _isRememberPassword;
            set
            {
                _isRememberPassword = value;
                OnPropertyChanged(nameof(IsRememberPassword));
            }
        }


        // Commands
        public ICommand LoginCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        // Constructor
        public LoginViewModel()
        {
            
            LoginCommand = new ViewModelCommand(ExecuteLogInCommand, CanExecuteLoginCommand);
            RememberPasswordCommand = new ViewModelCommand(ExecuteRememberPasswordCommand, CanExcuteRememberPasswordCommand);
            //ShowPasswordCommand = new ViewModelCommand(ExecuteLogInCommand, CanExecuteLoginCommand);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;

            if (string.IsNullOrWhiteSpace(UserName) || UserName.Length < 3 || Password == null || Password.Length < 3)
            {
                validData = false;
            }
            else
            {
                validData = true;
            }

            return validData;
        }

        private void ExecuteLogInCommand(object obj)
        {
            
            Account? account = accountRepository.GetAccount(UserName, Password);
            if (account != null)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserName, Password), null);
                IsViewVisible = false;
            } else
            {
                ErrorMessage = "* Invalid username or password";
            }

        }

        private bool CanExcuteRememberPasswordCommand(object obj)
        {

            return true;
        }

        private void ExecuteRememberPasswordCommand(object obj)
        {
            throw new NotImplementedException();
        }
    }
}