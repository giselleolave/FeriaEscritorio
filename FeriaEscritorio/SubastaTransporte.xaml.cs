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
    /// Lógica de interacción para SubastaTransporte.xaml
    /// </summary>
    public partial class SubastaTransporte : Window
    {
        OracleConnection conn = null;
        public SubastaTransporte()
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

                throw new Exception("Problemas de conexión");
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

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MenuPrincipal volvermenu = new MenuPrincipal();
            volvermenu.Show();
        }

        private void btnSalirE_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnCreSu_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CrearSubastaTransporte menucreasubasta = new CrearSubastaTransporte();
            menucreasubasta.Show();
        }

        private void gvsubastas_Loaded(object sender, RoutedEventArgs e)
        {
            ListarSubastas();
        }

        private void ListarSubastas()
        {
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ID,DIRECCION,ESTADO,FECHA_INICIO,TIPO,PROCESO_PRODUCTO_ID,DETALLE_PEDIDO_ID FROM SUBASTA_TRANS";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                gvsubastas.ItemsSource = dt.DefaultView;
                dr.Close();

            }
            catch (Exception)
            {

                MessageBox.Show("Error al cargar datos", "ERROR MODIFICAR USUARIO", MessageBoxButtons.OK);
            }
        }


        private void btnBusSub2_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SUBASTA_TRANS WHERE ID like ('" + txtbuscarSu.Text + "%')";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvsubastas.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnelimsub_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_borrarsub", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtCambioSUB.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Subasta Eliminada", "ELIMINAR SUBASTA", MessageBoxButtons.OK);
                ListarSubastas();

            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar subasta", "ERROR ELIMINAR SUBASTA", MessageBoxButtons.OK);

            }
        }
    }
}
