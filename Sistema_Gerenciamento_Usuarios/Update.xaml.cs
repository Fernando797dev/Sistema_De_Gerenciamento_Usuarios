using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Sistema_Gerenciamento_Usuarios
{
    public partial class Update : Window
    {
        private string connectionString = "Server=localhost;Database=login;Uid=root;Pwd=;";

        // Armazena o ID do usuário encontrado para garantir UPDATE na linha correta
        private int idUsuarioEncontrado = 0;

        public Update()
        {
            InitializeComponent();
        }

        // ==========================================
        // 1. ETAPA: PROCURAR O USUÁRIO PELO EMAIL
        // ==========================================
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

                    string queryProcurar = "SELECT id, nome_completo, email, senha, IsAdmin, status FROM usuarios WHERE email = @email LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(queryProcurar, con))
                    {
                        cmd.Parameters.AddWithValue("@email", emailBusca);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Salva o ID do usuário
                                idUsuarioEncontrado = Convert.ToInt32(reader["id"]);

                                // Preenche as caixas de texto
                                digitar_email.Text = reader["email"].ToString();
                                digitar_nome_completo.Text = reader["nome_completo"] != DBNull.Value ? reader["nome_completo"].ToString() : "";
                                digita_senha.Text = reader["senha"] != DBNull.Value ? reader["senha"].ToString() : "";

                                // Preenche o ComboBox Perfil (IsAdmin: 1 = Admin, 0 = Usuário)
                                int isAdmin = Convert.ToInt32(reader["IsAdmin"]);
                                SetComboBoxValue(cbPerfil, isAdmin == 1 ? "Admin" : "Usuário");

                                // Preenche o ComboBox Status ("Ativo" ou "Inativo")
                                string status = reader["status"].ToString();
                                SetComboBoxValue(cbStatus, status);

                                MessageBox.Show("Usuário encontrado! Faça as alterações desejadas.");
                            }
                            else
                            {
                                MessageBox.Show("Nenhum usuário foi encontrado com este e-mail.");
                                LimparCampos();
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

        // ==========================================
        // 2. ETAPA: SALVAR A ALTERAÇÃO (UPDATE)
        // ==========================================
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (idUsuarioEncontrado == 0)
            {
                MessageBox.Show("Por favor, busque um usuário válido antes de realizar o Update.");
                return;
            }

            string novoEmail = digitar_email.Text.Trim();
            string novoNome = digitar_nome_completo.Text.Trim();
            string novaSenha = digita_senha.Text.Trim();

            // Pega o valor selecionado no ComboBox de Perfil (IsAdmin)
            string perfilSelecionado = GetComboBoxContent(cbPerfil);
            int isAdmin = (perfilSelecionado == "Admin" || perfilSelecionado == "1") ? 1 : 0;

            // Pega o valor selecionado no ComboBox de Status
            string statusSelecionado = GetComboBoxContent(cbStatus);

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string queryUpdate = @"UPDATE usuarios 
                                          SET email = @email, 
                                              nome_completo = @nome, 
                                              senha = @senha, 
                                              IsAdmin = @isAdmin, 
                                              status = @status, 
                                              data_atualizacao = NOW() 
                                          WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(queryUpdate, con))
                    {
                        cmd.Parameters.AddWithValue("@email", novoEmail);
                        cmd.Parameters.AddWithValue("@nome", novoNome);
                        cmd.Parameters.AddWithValue("@senha", novaSenha);
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

        // Métodos auxiliares para ler e definir ComboBox no WPF de forma segura:
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

        private void LimparCampos()
        {
            idUsuarioEncontrado = 0;
            digitar_email.Clear();
            digitar_nome_completo.Clear();
            digita_senha.Clear();
            cbPerfil.SelectedIndex = -1;
            cbStatus.SelectedIndex = -1;
        }
    }
}