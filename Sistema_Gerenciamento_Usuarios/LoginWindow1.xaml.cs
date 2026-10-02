using MySql.Data.MySqlClient;
using Sistema_Gerenciamento_Usuarios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BCryptNet = BCrypt.Net.BCrypt;

namespace WpfApp1
{
    /// <summary>
    /// Lógica interna para LoginWindow1.xaml
    /// </summary>
    public partial class LoginWindow1 : Window
    {
        public string connectionString = "Server=localhost;Database=login;Uid=root;Pwd=;";

        public LoginWindow1()
        {
            InitializeComponent();


        }
        private void btn_login_cadastro_Click(object sender, RoutedEventArgs e)
        {
            //====================
            //VERIFICAR SE É ADMIN
            //====================

            bool inativo = false;

            string usuario = digita_usuario.Text.Trim();
            string senha = digita_senha.Password.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Preencha todos os campos para entrar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
               

                using (MySqlConnection conexao = new MySqlConnection(connectionString))
                {

                    conexao.Open();

                    string query = "SELECT senha, IsAdmin, id, status FROM usuarios WHERE usuario = @usuario";

                    using (MySqlCommand comando = new MySqlCommand(query, conexao))
                    {


                        comando.Parameters.AddWithValue("@usuario", usuario);


                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {


                            if (reader.Read())
                            {
                                int idUsuarioLogado = reader.GetInt32("id");
                                string senhaHashBanco = reader.GetString("senha");
                                bool isAdmin = reader.GetBoolean("IsAdmin");
                                string VerificarStatus = reader.GetString ("status");

                                if (BCryptNet.Verify(senha, senhaHashBanco))
                                {
                                    if (VerificarStatus != "Ativo")
                                    {
                                        MessageBox.Show("ERRO DE SEU USUÁRIO É INATIVO");
                                        return;
                                    }
                                    
                                    MessageBox.Show("Login realizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                                    if (isAdmin)
                                            
                                    {
                                        MessageBox.Show("Seja bem vindo a sua tela admin!");
                                        Tela_Inicial_admin TelaAdmin = new Tela_Inicial_admin();
                                        TelaAdmin.Show();
                                        this.Close();
                                    }
                                    
                                    
                                    else 
                                    {
                                        Read TelaUsuario = new Read();
                                        TelaUsuario.Show();
                                        this.Close();
                                    }

                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Usuário ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                                    digita_senha.Password = string.Empty;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Usuário ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                                digita_senha.Password = string.Empty;

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar ao banco de dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }




    }
}
