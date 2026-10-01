using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Lógica interna para Tela_Inicial_admin.xaml
    /// </summary>
    public partial class Tela_Inicial_admin : Window
    {
        public Tela_Inicial_admin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Tela_Cadastro telaCadastroUsuario = new Tela_Cadastro();
            telaCadastroUsuario.Show();
            this.Close();
        }

        private void Botao_Listar_usuario_Click(object sender, RoutedEventArgs e)
        {
            Read AbrirRead = new Read();
            AbrirRead.Show();
            this.Close();
        }

        private void Botao_Editar_usuario_Click(object sender, RoutedEventArgs e)
        {
            Update AbrirUpdate = new Update();
            AbrirUpdate.Show();
            this.Close();
        }

        private void Botao_Apagar_usuarios_Click(object sender, RoutedEventArgs e)
        {
            Delete AbrirDelete = new Delete();
            AbrirDelete.Show();
            this.Close();
        }

        private void Botao_ver_LOG_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
