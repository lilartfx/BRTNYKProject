using System;
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

            try
            {
                directoryEntry = App.TryLogin(UsernameTextBox.Text, PasswordBox.Password);
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
            throw new NotImplementedException();
        }
    }
}