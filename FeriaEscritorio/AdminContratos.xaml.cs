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
using Application = System.Windows.Application;

namespace FeriaEscritorio
{
    /// <summary>
    /// Lógica de interacción para AdminContratos.xaml
    /// </summary>
    public partial class AdminContratos : Window
    {
        OracleConnection conn = null;
        public AdminContratos()
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
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MenuPrincipal volvermenu = new MenuPrincipal();
            volvermenu.Show();
        }

        private void btnSalirE_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnCrearC_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CrearContrato crearcon = new CrearContrato();
            crearcon.Show();
        }

        private void gvcontratos_Loaded(object sender, RoutedEventArgs e)
        {
            ListarContratos();
        }

        private void ListarContratos()
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID,FECHA_FIRMA,FECHA_TERMINO,PORC_COMISION,IS_ACTIVE FROM CONTRATO";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvcontratos.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnListarC_Click(object sender, RoutedEventArgs e)
        {
            ListarContratos();
        }

        private void btnBusCon_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM CONTRATO WHERE ID like ('" + txtbuscarC.Text + "%')";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvcontratos.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnActivarC_Click(object sender, RoutedEventArgs e)
        {
            char estadoCon = 'Y';
            try
            {
                OracleCommand cmd = new OracleCommand("FN_CAMBIARESTCON", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtCambioCon.Text);
                cmd.Parameters.Add("ESTC", OracleDbType.Char).Value = estadoCon;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Contrato Activado", "ACTIVACIÓN DE CONTRATO", MessageBoxButtons.OK);
                ListarContratos();

            }
            catch (Exception)
            {

                MessageBox.Show("Error al cambiar estado", "ERROR CAMBIO DE ESTADO", MessageBoxButtons.OK);
            }
        }

        private void btnTerminarC_Click(object sender, RoutedEventArgs e)
        {
            char estadoCon = 'F';
            try
            {
                OracleCommand cmd = new OracleCommand("FN_CAMBIARESTCON", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtCambioCon.Text);
                cmd.Parameters.Add("ESTC", OracleDbType.Char).Value = estadoCon;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Contrato Finalizado", "FINALIZACIÓN DE CONTRATO", MessageBoxButtons.OK);
                ListarContratos();

            }
            catch (Exception)
            {

                MessageBox.Show("Error al cambiar estado", "EROOR CAMBIO DE ESTADO", MessageBoxButtons.OK);
            }
        }
    }
}
