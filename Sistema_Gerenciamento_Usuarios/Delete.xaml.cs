```csharp
using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace Sistema_Gerenciamento_Usuarios
{
    public partial class Delete : Window
    {
        private string connectionString = "Server=localhost;Database=login;Uid=root;Pwd=;";
        private int idUsuarioLogado;
        private bool isAdmin;

        public Delete(int idUsuarioLogado, bool isAdmin)
        {
            InitializeComponent();

            this.idUsuarioLogado = idUsuarioLogado;
            this.isAdmin = isAdmin;
        }

        private void ConfirmarDelete_Click(object sender, RoutedEventArgs e)
        {
            string email = Digitar_delete_Email.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Digite o email do usuário que deseja excluir.");
                return;
            }

            if (!isAdmin)
            {
                MessageBox.Show("Somente administradores podem excluir usuários.");
                return;
            }

            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    string queryBusca = "SELECT id, IsAdmin FROM usuarios WHERE email = @email LIMIT 1";

                    using (MySqlCommand comando = new MySqlCommand(queryBusca, conexao))
                    {
                        comando.Parameters.AddWithValue("@email", email);

                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Nenhum usuário foi encontrado com este email.");
                                return;
                            }

                            int idUsuario = Convert.ToInt32(reader["id"]);

                            if (idUsuario == idUsuarioLogado)
                            {
                                MessageBox.Show("Você não pode excluir o próprio usuário.");
                                return;
                            }
                        }
                    }

                    MessageBoxResult confirmacao = MessageBox.Show(
                        "Deseja realmente excluir este usuário?",
                        "Confirmar exclusão",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (confirmacao != MessageBoxResult.Yes)
                    {
                        return;
                    }

                    string queryDelete = "DELETE FROM usuarios WHERE email = @email";

                    using (MySqlCommand comandoDelete = new MySqlCommand(queryDelete, conexao))
                    {
                        comandoDelete.Parameters.AddWithValue("@email", email);

                        int linhasAfetadas = comandoDelete.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Usuário excluído com sucesso.");
                            Digitar_delete_Email.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível excluir o usuário.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao excluir usuário: " + ex.Message);
                }
            }
        }
    }
}
```
