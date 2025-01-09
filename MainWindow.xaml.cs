using Infotools;
using Mysqlx.Crud;
using System;
using System.Windows;

namespace Infottols
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        

      

        private void Contacts_Click(object sender, RoutedEventArgs e)
        {
            RendezvousWindow contactsWindow = new RendezvousWindow();
            contactsWindow.Show();
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            ClientsWindow ClientsWindow = new ClientsWindow();
            ClientsWindow.Show();
        }

       
    }
}

