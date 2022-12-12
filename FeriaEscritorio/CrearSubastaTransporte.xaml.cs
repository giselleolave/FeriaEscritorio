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
    /// Lógica de interacción para CrearSubastaTransporte.xaml
    /// </summary>
    public partial class CrearSubastaTransporte : Window
    {
        OracleConnection conn = null;
        string tipo = null;
        public CrearSubastaTransporte()
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
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnSalirE_Click(object sender, RoutedEventArgs e)
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
            SubastaTransporte menusubasta = new SubastaTransporte();
            menusubasta.Show();
        }

       
        //PROCESOS PARA SUBASTA LOCAL
         private void ListarProcresosLocales()
        {
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT PEDIDO.ID,PEDIDO.NUMERO_PEDIDO,PEDIDO.ESTADO,PEDIDO.SALDO_ID,DETALLE_PEDIDO.ID AS DETALLE_ID,DETALLE_PEDIDO.DIRECCION FROM PEDIDO JOIN DETALLE_PEDIDO ON PEDIDO.NUMERO_PEDIDO = DETALLE_PEDIDO.NUMERO_PEDIDO WHERE PEDIDO.ESTADO = 'pendiente'";
                //cmd.CommandText = "SELECT SOLICITUD_PRO.ID,SOLICITUD_PRO.CANTIDAD,TIPO_PRODUCTO.DESCRIP_PRO,ESTADO.ESTADO,SOLICITUD_PRO.USUARIO_ID,SOLICITUD_PRO.DIRECCION FROM SOLICITUD_PRO JOIN ESTADO ON SOLICITUD_PRO.ESTADO_ID = ESTADO.ID JOIN TIPO_PRODUCTO ON SOLICITUD_PRO.PRODUCTO = TIPO_PRODUCTO.ID WHERE ESTADO.ID = 6";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                gvLocal.ItemsSource = dt.DefaultView;
                dr.Close();

            }
            catch (Exception)
            {

                MessageBox.Show("Error al cargar datos");
            }
        }
        private void gvlocal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtIDDETLO.Text = dr["DETALLE_ID"].ToString();
                txtDireccionLO.Text = dr["DIRECCION"].ToString();             
            }
        }

        private void cbxTipoTrans_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand comm = new OracleCommand("SELECT ID,NOMBRE FROM TIPO_TRANS", conn);
            OracleDataReader tipoT = comm.ExecuteReader();
            while (tipoT.Read())
            {
                string _idt = tipoT["ID"].ToString();
                cbxTipoTransLO.Items.Add(new { descrip_tipo = tipoT["NOMBRE"].ToString(), idtipo = int.Parse(_idt) });
            }
        }
        private void btnLocal_Click(object sender, RoutedEventArgs e)
        {
            tipo = "LOCAL";
            gvexterno.IsEnabled = false;
            gvLocal.IsEnabled = true;
            btnBuscarEx.IsEnabled = false;
            btnBuscarLo.IsEnabled = true;
            btnTodosEx.IsEnabled = false;
            btnTodosLo.IsEnabled = true;
            txtbusproE.IsEnabled = false;
            txtbusproL.IsEnabled = true;
            dtFecha_iniciosLO.IsEnabled = true;
            dtFecha_iniciosEX.IsEnabled = false;
            cbxTipoTransLO.IsEnabled = true;
            cbxTipoTransEX.IsEnabled = false;
            txtDireccionLO.IsEnabled = true;
            txtDireccionEX.IsEnabled = false;
            txtIDDETLO.IsEnabled = true;
            txtIDPROEX.IsEnabled = false;
            btnGuardarLo.IsEnabled = true;
            btnLimpiarLO.IsEnabled = true;
            btnGuardarEX.IsEnabled = false;
            btnLimpiarEX.IsEnabled = false;
            ListarProcresosLocales();
        }
        private void btnGuardarLo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("FN_INSERTARSUBTLO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("DIRC", OracleDbType.Varchar2).Value = txtDireccionLO.Text;
                cmd.Parameters.Add("FECIN", OracleDbType.Date).Value = Convert.ToDateTime(dtFecha_iniciosLO.ToString());
                //cmd.Parameters.Add("TRID", OracleDbType.Int32).Value = Convert.ToInt32(cbxTipoTransLO.SelectedValue);               
                cmd.Parameters.Add("TIPO", OracleDbType.Varchar2).Value = tipo;
                cmd.Parameters.Add("DPID", OracleDbType.Int32).Value = Convert.ToInt32(txtIDDETLO.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Subasta Creada");
                
            }
            catch (Exception)
            {

                MessageBox.Show("Error al crear subasta_trans");
            }
        }


        private void btnBuscarLo_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT PROCESO_VEN.ID,PROCESO_VEN.ESTADO,PROCESO_VEN.SOLICITUD_PROCESO_ID,SOLICITUD_PRO.DIRECCION FROM PROCESO_VEN JOIN SOLICITUD_PRO ON PROCESO_VEN.SOLICITUD_PROCESO_ID = SOLICITUD_PRO.ID WHERE ESTADO = 'activo' AND SOLICITUD_PRO.PAIS_ID = 1 AND PROCESO_VEN.ID LIKE ('" + txtbusproL.Text + "%')";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvLocal.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnTodosLo_Click(object sender, RoutedEventArgs e)
        {
            ListarProcresosLocales();
        }
        // PROCESOS SUBASTA EXTERNA
        private void btnExterno_Click(object sender, RoutedEventArgs e)
        {
            tipo = "EXTERNO";
            gvexterno.IsEnabled = true;
            gvLocal.IsEnabled = false;
            btnBuscarEx.IsEnabled = true;
            btnBuscarLo.IsEnabled = false;
            btnTodosEx.IsEnabled = true;
            btnTodosLo.IsEnabled = false;
            txtbusproE.IsEnabled = true;
            txtbusproL.IsEnabled = false;
            dtFecha_iniciosLO.IsEnabled = false;
            dtFecha_iniciosEX.IsEnabled = true;
            cbxTipoTransLO.IsEnabled = false;
            cbxTipoTransEX.IsEnabled = true;
            txtDireccionLO.IsEnabled = false;
            txtDireccionEX.IsEnabled = true;
            txtIDDETLO.IsEnabled = false;
            txtIDPROEX.IsEnabled = true;
            btnGuardarEX.IsEnabled = true;
            btnLimpiarEX.IsEnabled = true;
            btnGuardarLo.IsEnabled = false;
            btnLimpiarLO.IsEnabled = false;
            ListarProcresosExternos();
        }

        private void btnTodosEx_Click(object sender, RoutedEventArgs e)
        {
            ListarProcresosExternos();
        }

        private void ListarProcresosExternos()
        {
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT PROCESO_VEN.ID,PROCESO_VEN.ESTADO,PROCESO_VEN.SOLICITUD_PROCESO_ID,PROCESO_PRODUCTO.ID AS PROCESO_ID,SOLICITUD_PRO.DIRECCION FROM PROCESO_VEN JOIN PROCESO_PRODUCTO ON PROCESO_VEN.ID = PROCESO_PRODUCTO.PROCESO_VEN_ID JOIN SOLICITUD_PRO ON PROCESO_VEN.SOLICITUD_PROCESO_ID = SOLICITUD_PRO.ID WHERE PROCESO_VEN.ESTADO = 'subastando' AND SOLICITUD_PRO.PAIS_ID !=47";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                gvexterno.ItemsSource = dt.DefaultView;
                dr.Close();

            }
            catch (Exception)
            {

                MessageBox.Show("Error al cargar datos");
            }
        }
        private void gvexterno_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtIDPROEX.Text = dr["PROCESO_ID"].ToString();
                txtDireccionEX.Text = dr["DIRECCION"].ToString();
            }
        }

        private void btnGuardarEX_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("FN_INSERTARSUBTEX", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("DIRC", OracleDbType.Varchar2).Value = txtDireccionEX.Text;
                cmd.Parameters.Add("FECIN", OracleDbType.Date).Value = Convert.ToDateTime(dtFecha_iniciosEX.ToString());
                //cmd.Parameters.Add("TRID", OracleDbType.Int32).Value = Convert.ToInt32(cbxTipoTransLO.SelectedValue);               
                cmd.Parameters.Add("TIPO", OracleDbType.Varchar2).Value = tipo;
                cmd.Parameters.Add("PPID", OracleDbType.Int32).Value = Convert.ToInt32(txtIDPROEX.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Subasta Creada");

            }
            catch (Exception)
            {

                MessageBox.Show("Error al crear subasta_trans");
            }
        }

        private void btnBuscarEx_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT PROCESO_VEN.ID,PROCESO_VEN.ESTADO,PROCESO_VEN.SOLICITUD_PROCESO_ID,SOLICITUD_PRO.DIRECCION FROM PROCESO_VEN JOIN SOLICITUD_PRO ON PROCESO_VEN.SOLICITUD_PROCESO_ID = SOLICITUD_PRO.ID WHERE ESTADO = 'activo' AND SOLICITUD_PRO.PAIS_ID != 1 AND PROCESO_VEN.ID LIKE ('" + txtbusproE.Text + "%')";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvexterno.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void cbxTipoTransEX_Loaded_1(object sender, RoutedEventArgs e)
        {
            OracleCommand comm = new OracleCommand("SELECT ID,NOMBRE FROM TIPO_TRANS", conn);
            OracleDataReader tipoT = comm.ExecuteReader();
            while (tipoT.Read())
            {
                string _idt = tipoT["ID"].ToString();
                cbxTipoTransEX.Items.Add(new { descrip_tipo = tipoT["NOMBRE"].ToString(), idtipo = int.Parse(_idt) });
            }
        }
    }
}
