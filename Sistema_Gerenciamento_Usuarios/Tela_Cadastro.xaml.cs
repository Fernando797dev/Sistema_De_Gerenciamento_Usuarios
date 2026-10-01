using MySql.Data.MySqlClient;
using System;
using System.Windows;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Sistema_Gerenciamento_Usuarios
{
    public partial class Tela_Cadastro : Window
    {
        public string Conexao = "Server=localhost;Database=login;Uid=root;Pwd=;";

        private bool criandoAdmin = false;

        private string AvatarEscolhido = "";

        public Tela_Cadastro()
        {
            InitializeComponent();
            ConfigurarCadastroUsuario();
        }

        private void ConfigurarCadastroUsuario()
        {
            criandoAdmin = false;

            txtTitulo.Text = "CADASTRO DE USUÁRIOS";
            txtTipoCadastro.Text = "USUÁRIO";

            btn_login_cadastro.Content = "CADASTRAR USUÁRIO";

            lblSenhaAdmin.Visibility = Visibility.Collapsed;
            digita_senha_admin.Visibility = Visibility.Collapsed;
            btnCriarAdmin.Visibility = Visibility.Visible;
        }

        private void btnCriarAdmin_Click(object sender, RoutedEventArgs e)
        {
            criandoAdmin = true;

            txtTitulo.Text = "CRIAR ADMINISTRADOR";
            txtTipoCadastro.Text = "ADMINISTRADOR";

            btn_login_cadastro.Content = "CRIAR ADMIN";

            lblSenhaAdmin.Visibility = Visibility.Visible;
            digita_senha_admin.Visibility = Visibility.Visible;
            btnCriarAdmin.Visibility = Visibility.Collapsed;
        }

        private void btn_login_cadastro_Click(object sender, RoutedEventArgs e)
        {
            string email = digita_email.Text.Trim();
            string usuario = digita_usuario.Text.Trim();
            string senha = digita_senha.Password;
            string confirmarSenha = digita_confirmar_senha.Password;
            string nomeCompleto = digita_nome_completo.Text.Trim();

            if (string.IsNullOrWhiteSpace(AvatarEscolhido))
            {
                MessageBox.Show("Selecione uma imagem de perfil",
                "",
                MessageBoxButton.OK);

                return;
            }

            if (string.IsNullOrWhiteSpace(nomeCompleto))
            {
                MessageBox.Show("Digite o nome completo.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(usuario))
            {
                MessageBox.Show("Digite o nome de usuário.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (usuario.Length < 3)
            {
                MessageBox.Show("O nome de usuário deve conter 3 ou mais caracteres.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Digite o e-mail.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("O e-mail digitado não é válido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Digite uma senha.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (senha.Length < 8)
            {
                MessageBox.Show("A senha deve conter 8 ou mais caracteres.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (senha != confirmarSenha)
            {
                MessageBox.Show("As senhas não coincidem.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (criandoAdmin)
            {
                CriarAdministrador(nomeCompleto, email, usuario, senha);
                return;
            }

            CriarUsuario(nomeCompleto, email, usuario, senha);
        }

        private void CriarUsuario(string nomeCompleto, string email, string usuario, string senha)
        {
            if (string.IsNullOrWhiteSpace(AvatarEscolhido))
            {
                MessageBox.Show("Selecione uma imagem de perfil",
                "",
                MessageBoxButton.OK);

                return;
            }
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Conexao))
                {
                    connection.Open();

                    string verificarUsuario = "SELECT COUNT(*) FROM usuarios WHERE usuario = @usuario";

                    using (MySqlCommand command = new MySqlCommand(verificarUsuario, connection))
                    {
                        command.Parameters.AddWithValue("@usuario", usuario);
                        int quantidade = Convert.ToInt32(command.ExecuteScalar());

                        if (quantidade > 0)
                        {
                            MessageBox.Show("Esse usuário já está cadastrado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    string verificarEmail = "SELECT COUNT(*) FROM usuarios WHERE email = @email";

                    using (MySqlCommand command = new MySqlCommand(verificarEmail, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        int quantidade = Convert.ToInt32(command.ExecuteScalar());

                        if (quantidade > 0)
                        {
                            MessageBox.Show("Esse e-mail já está cadastrado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    string senhaCriptografada = BCryptNet.HashPassword(senha);

                    string queryInsert = "INSERT INTO usuarios (imagem_perfil, nome_completo, email, usuario, senha, IsAdmin) VALUES (@imagem_perfil, @nome_completo, @email, @usuario, @senha, 0)";

                    using (MySqlCommand command = new MySqlCommand(queryInsert, connection))
                    {
                        command.Parameters.AddWithValue("imagem_perfil", AvatarEscolhido);
                        command.Parameters.AddWithValue("@nome_completo", nomeCompleto);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@usuario", usuario);
                        command.Parameters.AddWithValue("@senha", senhaCriptografada);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimparCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar usuário:\n\n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CriarAdministrador(string nomeCompleto, string email, string usuario, string senha)
        {
            if (string.IsNullOrWhiteSpace(AvatarEscolhido))
            {
                MessageBox.Show("Selecione uma imagem de perfil",
                "",
                MessageBoxButton.OK);

                return;
            }
            if (string.IsNullOrWhiteSpace(digita_senha_admin.Password))
            {
                MessageBox.Show("Digite a senha do administrador atual.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Conexao))
                {
                    connection.Open();

                    string queryAdmin = "SELECT senha FROM usuarios WHERE IsAdmin = 1 LIMIT 1";
                    string senhaHashAdmin = null;

                    using (MySqlCommand command = new MySqlCommand(queryAdmin, connection))
                    {
                        object resultado = command.ExecuteScalar();

                        if (resultado == null)
                        {
                            MessageBox.Show("Nenhum administrador cadastrado no sistema.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        senhaHashAdmin = resultado.ToString();
                    }

                    bool senhaCorreta = BCryptNet.Verify(digita_senha_admin.Password, senhaHashAdmin);

                    if (!senhaCorreta)
                    {
                        MessageBox.Show("A senha do administrador está incorreta.", "Acesso negado", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string verificarUsuario = "SELECT COUNT(*) FROM usuarios WHERE usuario = @usuario";

                    using (MySqlCommand command = new MySqlCommand(verificarUsuario, connection))
                    {
                        command.Parameters.AddWithValue("@usuario", usuario);
                        int quantidade = Convert.ToInt32(command.ExecuteScalar());

                        if (quantidade > 0)
                        {
                            MessageBox.Show("Esse usuário já está cadastrado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    string verificarEmail = "SELECT COUNT(*) FROM usuarios WHERE email = @email";

                    using (MySqlCommand command = new MySqlCommand(verificarEmail, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        int quantidade = Convert.ToInt32(command.ExecuteScalar());

                        if (quantidade > 0)
                        {
                            MessageBox.Show("Esse e-mail já está cadastrado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    string senhaCriptografada = BCryptNet.HashPassword(senha);

                    string queryInsert = "INSERT INTO usuarios (imagem_perfil, nome_completo, email, usuario, senha, IsAdmin) VALUES (@imagem_perfil, @nome_completo, @email, @usuario, @senha, 1)";

                    using (MySqlCommand command = new MySqlCommand(queryInsert, connection))
                    {
                        command.Parameters.AddWithValue("@nome_completo", nomeCompleto);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@usuario", usuario);
                        command.Parameters.AddWithValue("@senha", senhaCriptografada);
                        command.Parameters.AddWithValue("@imagem_perfil", AvatarEscolhido);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Administrador criado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                    LimparCampos();
                    ConfigurarCadastroUsuario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar administrador:\n\n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LimparCampos()
        {
            digita_email.Clear();
            digita_usuario.Clear();
            digita_senha.Clear();
            digita_confirmar_senha.Clear();
            digita_senha_admin.Clear();
            digita_nome_completo.Clear();
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