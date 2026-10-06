using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;

namespace Sistema_Gerenciamento_Usuarios
{
    public partial class Read : Window
    {
        private string connectionString = "Server=localhost;Database=login;Uid=root;Pwd=;";

        public Read()
        {
            InitializeComponent();
            CarregarUsuarios();
        }

        private void Filtro_Changed(object sender, EventArgs e)
        {
            CarregarUsuarios();
        }

        private void CarregarUsuarios()
        {
            if (panelCards == null) return;
            panelCards.Children.Clear();

            string busca = txtFiltro != null ? txtFiltro.Text.Trim() : "";
            string perfilSel = (cbPerfil?.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Todos";
            string statusSel = (cbStatus?.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Todos";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "SELECT nome_completo, usuario, email, senha, IsAdmin, status, imagem_perfil FROM usuarios WHERE 1=1";

                    if (!string.IsNullOrEmpty(busca))
                    {
                        query += " AND (nome_completo LIKE @busca OR usuario LIKE @busca OR email LIKE @busca)";
                    }

                    if (perfilSel == "Admin")
                    {
                        query += " AND IsAdmin = 1";
                    }
                    else if (perfilSel == "Usuário")
                    {
                        query += " AND IsAdmin = 0";
                    }

                    if (statusSel != "Todos")
                    {
                        query += " AND status = @status";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        if (!string.IsNullOrEmpty(busca))
                        {
                            cmd.Parameters.AddWithValue("@busca", "%" + busca + "%");
                        }
                        if (statusSel != "Todos")
                        {
                            cmd.Parameters.AddWithValue("@status", statusSel);
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nome = reader["nome_completo"] != DBNull.Value ? reader["nome_completo"].ToString() : "";
                                string usuario = reader["usuario"] != DBNull.Value ? reader["usuario"].ToString() : "";
                                string email = reader["email"] != DBNull.Value ? reader["email"].ToString() : "";
                                int isAdmin = reader["IsAdmin"] != DBNull.Value ? Convert.ToInt32(reader["IsAdmin"]) : 0;
                                string status = reader["status"] != DBNull.Value ? reader["status"].ToString() : "";
                                string imgPath = reader["imagem_perfil"] != DBNull.Value ? reader["imagem_perfil"].ToString() : "";

                                Border card = CriarCardUsuario(nome, usuario, email, isAdmin == 1 ? "Admin" : "Usuário", status, imgPath);
                                panelCards.Children.Add(card);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar usuários: " + ex.Message);
                }
            }
        }

        private Border CriarCardUsuario(string nome, string usuario, string email, string perfil, string status, string imgPath)
        {
            Border card = new Border
            {
                Width = 220,
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                Background = Brushes.White,
                CornerRadius = new CornerRadius(8),
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1)
            };

            StackPanel sp = new StackPanel();

            Image img = new Image
            {
                Width = 70,
                Height = 70,
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            try
            {
                if (!string.IsNullOrEmpty(imgPath))
                {
                    img.Source = new BitmapImage(new Uri(imgPath, UriKind.RelativeOrAbsolute));
                }
            }
            catch
            {
            }

            sp.Children.Add(img);

            sp.Children.Add(new TextBlock { Text = "Nome: " + nome, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap });
            sp.Children.Add(new TextBlock { Text = "Usuário: " + usuario, Margin = new Thickness(0, 2, 0, 0) });
            sp.Children.Add(new TextBlock { Text = "E-mail: " + email, Margin = new Thickness(0, 2, 0, 0), TextWrapping = TextWrapping.Wrap });
            sp.Children.Add(new TextBlock { Text = "Perfil: " + perfil, Margin = new Thickness(0, 2, 0, 0) });
            sp.Children.Add(new TextBlock { Text = "Status: " + status, Margin = new Thickness(0, 2, 0, 0), FontWeight = FontWeights.SemiBold });

            card.Child = sp;
            return card;
        }
    }
}