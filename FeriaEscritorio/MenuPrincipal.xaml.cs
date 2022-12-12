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
using System.Windows.Forms;

namespace FeriaEscritorio
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal      

    {
        OracleConnection conn = null;
        public MenuPrincipal()
        {
            abrirConexion();
            InitializeComponent();
        }

        private void abrirConexion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OracleDB"].ConnectionString;
            conn = new OracleConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión", "ERROR DE CONEXIÓN", MessageBoxButtons.OK);
                throw new Exception("Falla de conexión");
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Ingreso volvermenu = new Ingreso();
            volvermenu.Show();
        }

        private void btnSalirE_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminUsuarios menuusuario = new AdminUsuarios();
            menuusuario.Show();
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminProductos menuproduc = new AdminProductos();
            menuproduc.Show();
        }

        private void btnSaldos_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminSaldos menusaldos = new AdminSaldos();
            menusaldos.Show();
        }

        private void btnContratos_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminContratos menucontratos = new AdminContratos();
            menucontratos.Show();
        }

        private void btnProcesoVenta_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ProcesoVenta menuventas = new ProcesoVenta();
            menuventas.Show();
        }

        private void btnSubasta_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            SubastaTransporte menusubasta   = new SubastaTransporte();
            menusubasta.Show();
        }
    }
}
