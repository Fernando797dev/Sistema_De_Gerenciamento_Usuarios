using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sistema_Gerenciamento_Usuarios
{
    /// <summary>
    /// Lógica interna para Update.xaml
    /// </summary>
    public partial class Update : Window
    {
        string connectionString = "Server=localhost;Database = login;Uid = root;Pwd=;";
        public Update()
        {
            InitializeComponent();
        }


        private void botao_procurar_Click1(object sender, RoutedEventArgs e)
        {
            string email = email_alvo.Text.Trim();
            string senha = digita_senha.Text.Trim();
            string nomeCompleto = digitar_nome_completo.Text.Trim();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }




}

