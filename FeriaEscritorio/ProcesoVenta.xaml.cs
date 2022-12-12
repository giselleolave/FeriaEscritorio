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
using Prism.Services.Dialogs;
using System.Windows.Forms;

namespace FeriaEscritorio
{
    /// <summary>
    /// Lógica de interacción para ProcesoVenta.xaml
    /// </summary>
    public partial class ProcesoVenta : Window
    {
        OracleConnection conn = null;
        public ProcesoVenta()
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

        private void gvsolicitudes_Loaded(object sender, RoutedEventArgs e)
        {
            ListarSolicitudes();
        }

        private void ListarSolicitudes()
        {
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT SOLICITUD_PRO.ID,SOLICITUD_PRO.PRODUCTO,SOLICITUD_PRO.CANTIDAD,SOLICITUD_PRO.DIRECCION,SOLICITUD_PRO.USUARIO_ID,ESTADO.ESTADO FROM SOLICITUD_PRO JOIN ESTADO ON SOLICITUD_PRO.ESTADO_ID = ESTADO.ID";
               // cmd.CommandText = "SELECT SOLICITUD_PRO.ID,SOLICITUD_PRO.PRODUCTO,SOLICITUD_PRO.CANTIDAD,ESTADO.ESTADO,SOLICITUD_PRO.USUARIO_ID,SOLICITUD_PRO.DIRECCION FROM SOLICITUD_PRO JOIN ESTADO ON SOLICITUD_PRO.ESTADO_ID = ESTADO.ID JOIN TIPO_PRODUCTO ON SOLICITUD_PRO.PRODUCTO = TIPO_PRODUCTO.ID ";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                gvsolicitudes.ItemsSource = dt.DefaultView;
                dr.Close();

            }
            catch (Exception)
            {

                MessageBox.Show("Error al cargar datos", "ERROR MODIFICAR USUARIO", MessageBoxButtons.OK);
            }
        }

        private void btnAnular_Click(object sender, RoutedEventArgs e)
        {
            int anulado = 7;
            try
            {
                OracleCommand cmd = new OracleCommand("FN_CAMBIAREST", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtCambio.Text);
                cmd.Parameters.Add("EST", OracleDbType.Varchar2).Value = Convert.ToInt32(anulado);              
                cmd.ExecuteNonQuery();
                MessageBox.Show("Solicitud Anulada", "ANULAR SOLICITUD", MessageBoxButtons.OK);
                ListarSolicitudes();
            }
            catch (Exception)
            {

                MessageBox.Show("Error al modificar solicitud", "MODIFICAR SOLICITUD", MessageBoxButtons.OK);
            }
        }

        private void btnActivar_Click(object sender, RoutedEventArgs e)
        {
            int activa =8;
            try
            {
                OracleCommand cmd = new OracleCommand("FN_CAMBIAREST", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtCambio.Text);
                cmd.Parameters.Add("EST", OracleDbType.Varchar2).Value = Convert.ToInt32(activa);

                cmd.ExecuteNonQuery();
                if (MessageBox.Show("¿Desea crear proceso de venta?", "Proceso de venta", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        OracleCommand cmdd = new OracleCommand("FN_INSERTARPROVEN", conn);
                        cmdd.CommandType = CommandType.StoredProcedure;
                        cmdd.Parameters.Add("ESTPV", OracleDbType.Varchar2).Value = "activo";
                        cmdd.Parameters.Add("IDPV", OracleDbType.Int32).Value = Convert.ToInt32(txtCambio.Text);
                        cmdd.ExecuteNonQuery();
                        MessageBox.Show("Proceso creado", "Message", MessageBoxButtons.OK);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error al crear proceso de venta", "ERROR CREAR PROCESO DE VENTA", MessageBoxButtons.OK);
                    }
                    
                }
                ListarSolicitudes();
             
            }
            catch (Exception)
            {

                MessageBox.Show("Error al modificar solicitud", "ERROR MODIFICAR SOLICITUD", MessageBoxButtons.OK);
            }
        }
        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            int activa = 6;
            try
            {
                OracleCommand cmd = new OracleCommand("FN_CAMBIAREST", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtCambio.Text);
                cmd.Parameters.Add("EST", OracleDbType.Varchar2).Value = Convert.ToInt32(activa);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Solicitud Activada, lista para Subasta de transporte", "ACTIVACIÓN SOLICITUD", MessageBoxButtons.OK);
                ListarSolicitudes();
            }
            catch (Exception)
            {

                MessageBox.Show("Error al modificar solicitud", "ERROR MODIFICAR SOLICITUD", MessageBoxButtons.OK);
            }
        }

        private void btnSolicitadas_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT SOLICITUD_PRO.ID,SOLICITUD_PRO.PRODUCTO,SOLICITUD_PRO.CANTIDAD,SOLICITUD_PRO.DIRECCION,SOLICITUD_PRO.USUARIO_ID,ESTADO.ESTADO FROM SOLICITUD_PRO JOIN ESTADO ON SOLICITUD_PRO.ESTADO_ID = ESTADO.ID WHERE ESTADO_ID = 5";
           
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvsolicitudes.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnActivas_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT SOLICITUD_PRO.ID,SOLICITUD_PRO.PRODUCTO,SOLICITUD_PRO.CANTIDAD,SOLICITUD_PRO.DIRECCION,SOLICITUD_PRO.USUARIO_ID,ESTADO.ESTADO FROM SOLICITUD_PRO JOIN ESTADO ON SOLICITUD_PRO.ESTADO_ID = ESTADO.ID WHERE ESTADO_ID = 6";
            
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvsolicitudes.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnAnuladas_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT SOLICITUD_PRO.ID,SOLICITUD_PRO.PRODUCTO,SOLICITUD_PRO.CANTIDAD,SOLICITUD_PRO.DIRECCION,SOLICITUD_PRO.USUARIO_ID,ESTADO.ESTADO FROM SOLICITUD_PRO JOIN ESTADO ON SOLICITUD_PRO.ESTADO_ID = ESTADO.ID WHERE ESTADO_ID = 7";
            
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvsolicitudes.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnBusPro_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT SOLICITUD_PRO.ID,SOLICITUD_PRO.PRODUCTO,SOLICITUD_PRO.CANTIDAD,SOLICITUD_PRO.DIRECCION,SOLICITUD_PRO.USUARIO_ID,ESTADO.ESTADO FROM SOLICITUD_PRO JOIN ESTADO ON SOLICITUD_PRO.ESTADO_ID = ESTADO.ID WHERE USUARIO_ID like ('" + txtbuscarSU.Text + "%')";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvsolicitudes.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnBusPro1_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT SOLICITUD_PRO.ID,SOLICITUD_PRO.PRODUCTO,SOLICITUD_PRO.CANTIDAD,SOLICITUD_PRO.DIRECCION,SOLICITUD_PRO.USUARIO_ID,ESTADO.ESTADO FROM SOLICITUD_PRO JOIN ESTADO ON SOLICITUD_PRO.ESTADO_ID = ESTADO.ID WHERE ID like ('" + txtbuscarSI.Text + "%')";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvsolicitudes.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnSolicitudes_Click(object sender, RoutedEventArgs e)
        {
            ListarSolicitudes();
        }

       
    }
}
