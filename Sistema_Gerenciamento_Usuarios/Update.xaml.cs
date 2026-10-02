using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using BCryptNet = BCrypt.Net.BCrypt;
namespace Sistema_Gerenciamento_Usuarios
{
    public partial class Update : Window
    {
        private string connectionString = "Server=localhost;Database=login;Uid=root;Pwd=;";

        private int idUsuarioEncontrado = 0;
        private string AvatarEscolhido = "";


        public Update()
        {
            InitializeComponent();
        }


        private void botao_procurar_Click1(object sender, RoutedEventArgs e)
        {
            string emailBusca = email_alvo.Text.Trim();

            if (string.IsNullOrEmpty(emailBusca))
            {
                MessageBox.Show("O e-mail para procurar não pode estar vazio.");
                return;
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string queryProcurar = "SELECT id, usuario, email, senha, IsAdmin, status, imagem_perfil FROM usuarios WHERE email = @email LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(queryProcurar, con))
                    {
                        cmd.Parameters.AddWithValue("@email", emailBusca);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {


                            if (reader.Read())
                            {
                                idUsuarioEncontrado = Convert.ToInt32(reader["id"]);

                                digitar_email.Text = reader["email"].ToString();
                                digitar_usuario.Text = reader["usuario"] != DBNull.Value ? reader["usuario"].ToString() : "";
                                digita_senha.Text = reader["senha"] != DBNull.Value ? reader["senha"].ToString() : "";

                                int isAdmin = Convert.ToInt32(reader["IsAdmin"]);
                                SetComboBoxValue(cbPerfil, isAdmin == 1 ? "Admin" : "Usuário");

                                string status = reader["status"].ToString();
                                SetComboBoxValue(cbStatus, status);

                                AvatarEscolhido = reader["imagem_perfil"] != DBNull.Value ? reader["imagem_perfil"].ToString() : "";

                                MessageBox.Show("Usuário encontrado! Faça as alterações desejadas.");
                            }
                            else
                            {
                                MessageBox.Show("Nenhum usuário foi encontrado com este e-mail.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao consultar o banco de dados: " + ex.Message);
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (idUsuarioEncontrado == 0)
            {
                MessageBox.Show("Por favor, busque um usuário válido antes de realizar o Update.");
                return;
            }

            string novoEmail = digitar_email.Text.Trim();
            string novoUsuario = digitar_usuario.Text.Trim();
            string novaSenha = digita_senha.Password.Trim();

            string perfilSelecionado = GetComboBoxContent(cbPerfil);
            int isAdmin = (perfilSelecionado == "Admin" || perfilSelecionado == "1") ? 1 : 0;

            string statusSelecionado = GetComboBoxContent(cbStatus);

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string queryUpdate = @"UPDATE usuarios 
                                          SET imagem_perfil = @imagem_perfil,
                                              email = @email, 
                                              usuario = @usuario, 
                                              senha = @senha, 
                                              IsAdmin = @isAdmin, 
                                              status = @status, 
                                              data_atualizacao = NOW() 
                                          WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(queryUpdate, con))
                    {
                        string senhaCriptografada = BCryptNet.HashPassword(novaSenha);

                        cmd.Parameters.AddWithValue("@imagem_perfil", AvatarEscolhido);
                        cmd.Parameters.AddWithValue("@email", novoEmail);
                        cmd.Parameters.AddWithValue("@usuario", novoUsuario);
                        cmd.Parameters.AddWithValue("@senha", senhaCriptografada);
                        cmd.Parameters.AddWithValue("@isAdmin", isAdmin);
                        cmd.Parameters.AddWithValue("@status", statusSelecionado);
                        cmd.Parameters.AddWithValue("@id", idUsuarioEncontrado);

                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Dados atualizados com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Nenhuma alteração foi realizada no registro.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar dados: " + ex.Message);
                }
            }
        }

        private string GetComboBoxContent(ComboBox cb)
        {
            if (cb.SelectedItem is ComboBoxItem item)
                return item.Content.ToString();
            return cb.SelectedItem != null ? cb.SelectedItem.ToString() : "";
        }

        private void SetComboBoxValue(ComboBox cb, string valor)
        {
            foreach (var item in cb.Items)
            {
                if (item is ComboBoxItem cbItem && cbItem.Content.ToString().Equals(valor, StringComparison.OrdinalIgnoreCase))
                {
                    cb.SelectedItem = cbItem;
                    return;
                }
                else if (item.ToString().Equals(valor, StringComparison.OrdinalIgnoreCase))
                {
                    cb.SelectedItem = item;
                    return;
                }
            }
        }
        private void Avatar1_Click(object sender, RoutedEventArgs e)
        {
            AvatarEscolhido = "Image/avatar1.jpg";
        }
        private void Avatar2_Click(object sender, RoutedEventArgs e)
        {
            AvatarEscolhido = "Image/avatar2.jpg";
        }
        private void Avatar3_Click(object sender, RoutedEventArgs e)
        {
            AvatarEscolhido = "Image/avatar3.jpg";

        }
        private void Avatar4_Click(object sender, RoutedEventArgs e)
        {
            AvatarEscolhido = "Image/avatar4.jpg";

        }
        private void Avatar5_Click(object sender, RoutedEventArgs e)
        {
            AvatarEscolhido = "Image/avatar5.jpg";

        }


    }

}