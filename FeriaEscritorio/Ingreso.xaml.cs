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
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;
using System.Data;
using CapaAcceso;

namespace FeriaEscritorio
{
    /// <summary>
    /// Lógica de interacción para Ingreso.xaml
    /// </summary>
    public partial class Ingreso 
    {
        OracleConnection conn = null;
        public Ingreso()
        {
            abrirConexion();
            InitializeComponent();
        }

        private void abrirConexion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["oracleDB"].ConnectionString;
            conn = new OracleConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión");
                throw new Exception("Falla de conexión" + ex);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand comand = new OracleCommand("SELECT * FROM USUARIO WHERE EMAIL like('" + txtEmail.Text + "') AND PASSWORD  like('" + txtContrasena.Text + "') AND ID_TIPO_USUARIO = 1", conn);
            comand.Parameters.Add(":EMAIL", txtEmail.Text);
            comand.Parameters.Add(":PASSWORD", txtContrasena.Text);
            Console.WriteLine("CONTRASEÑA" + txtContrasena.Text);
            Console.WriteLine("EMAIL" + txtEmail.Text);
            OracleDataReader reader = comand.ExecuteReader();

            if (reader.Read())
            {
                this.Hide();
                MenuPrincipal menuPrincipal = new MenuPrincipal();
                menuPrincipal.Show();

            }
            else
            {
                
                MessageBox.Show("Acceso Denegado");
            }
        }
    }
}
