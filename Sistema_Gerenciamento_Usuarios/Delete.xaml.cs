using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace Sistema_Gerenciamento_Usuarios
{
    public partial class Delete : Window
    {
        string connectionsString = "Server=localhost;Database=login;Uid=root;Pwd=;";


        public Delete()
        {
            InitializeComponent();
        }

        private void ConfirmarDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
