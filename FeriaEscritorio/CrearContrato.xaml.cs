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
    /// Lógica de interacción para CrearContrato.xaml
    /// </summary>
    public partial class CrearContrato : Window
    {
        OracleConnection conn = null;

        public CrearContrato()
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
            catch (Exception)
            {

                throw new Exception("Horror!!");
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
            System.Windows.Application.Current.Shutdown();
        }

        private void btnSalirE_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminContratos volvermenuC = new AdminContratos();
            volvermenuC.Show();
        }

        private void btnGuardarP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("FN_INSERTARCON", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PORC", OracleDbType.Int32).Value = Convert.ToInt32(txtPorcen.Text);
                cmd.Parameters.Add("FECF", OracleDbType.Date).Value = Convert.ToDateTime(dtFecha_firma.ToString());
                cmd.Parameters.Add("FECT", OracleDbType.Date).Value = Convert.ToDateTime(dtFecha_termino.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Contrato Creado", "CREAR CONTRATO", MessageBoxButtons.OK);
            }
            catch (Exception error)
            {

                MessageBox.Show("Error al crear contrato", "ERROR CREAR CONTRATO", MessageBoxButtons.OK);
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtPorcen.Clear();
        }
    }
}
