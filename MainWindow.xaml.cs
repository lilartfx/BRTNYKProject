using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using LdapForNet;

namespace BRTNYKBNCProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string? Result { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            
            DirectoryEntry? directoryEntry = null;

            if (UsernameTextBox.Text == "" || PasswordBox.Password == "")
            {
                return;
            }
            
            try
            {
                var cleanUn = new string(UsernameTextBox.Text.Where(char.IsLetterOrDigit).ToArray());
                directoryEntry = App.TryLogin(cleanUn, PasswordBox.Password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (directoryEntry?.Attributes == null) return;

            LoginScreen.Visibility = Visibility.Hidden;
            DataScreen.Visibility = Visibility.Visible;
            
            foreach (var entry in directoryEntry.Attributes!)
            {
                Result = Result + entry.Name + ": " + entry.GetValue<string>() + "\n";
            }
            
            DataBox.Text = Result;
        }

        private void LoginOut_Click(object sender, RoutedEventArgs e)
        {
            UsernameTextBox.Text = "";
            PasswordBox.Password = "";
            LoginScreen.Visibility = Visibility.Visible;
            DataScreen.Visibility = Visibility.Hidden;
        }
    }
}