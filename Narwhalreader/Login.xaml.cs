using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using Reddit;

namespace Narwhalreader
{
    public partial class Login : PhoneApplicationPage
    {
        public Login()
        {
            InitializeComponent();

            RedditClient.LoginSuccess += new RedditLoginSuccessHandler(Login_Success);
            RedditClient.LoginFailure += new RedditLoginErrorHandler(Login_Error);
        }
		
		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
            RedditClient.Login(Username.Text, Password.Password);
		}

        private void Login_Success()
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Login_Error(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButton.OK);
        }
    }
}
