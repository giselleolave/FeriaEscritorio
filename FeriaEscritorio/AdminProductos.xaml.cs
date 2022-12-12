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
    /// Lógica de interacción para AdminProductos.xaml
    /// </summary>
    public partial class AdminProductos : Window
    {
       
        OracleConnection conn = null;
        public AdminProductos()
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

        private void btnNuePro_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CrearProducto crearp = new CrearProducto();
            crearp.Show();
        }

        private void gvproductos_Loaded(object sender, RoutedEventArgs e)
        {
            ListarProductos();
        }

        private void ListarProductos()
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT PRODUCTO.ID,PRODUCTO.NOMBRE,PRODUCTO.CANTIDAD,PRODUCTO.CALIDAD,PRODUCTO.FECHA_COSECHA,PRODUCTO.PRECIO_UNITARIO,BODEGA.ESTADO FROM PRODUCTO LEFT OUTER JOIN BODEGA ON PRODUCTO.ID = BODEGA.PRODUCTO_ID";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvproductos.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnNueTipopro_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CrearTipoProducto crearti = new CrearTipoProducto();
            crearti.Show();
        }

        private void btnElimiP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_borrapro", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtbuscarProID.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Producto Eliminado", "ELIMINACIÓN DE PRODUCTO", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar producto", "ERROR ELIMINACIÓN", MessageBoxButtons.OK);

            }
        }

        private void btnBusPro_Click(object sender, RoutedEventArgs e)
        {
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM PRODUCTO WHERE NOMBRE like ('" + txtbuscarP.Text + "%')";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                gvproductos.ItemsSource = dt.DefaultView;
                dr.Close();
            }
        }

        private void btnListarP_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT PRODUCTO.ID,PRODUCTO.NOMBRE,PRODUCTO.CANTIDAD,PRODUCTO.CALIDAD,PRODUCTO.FECHA_COSECHA,PRODUCTO.PRECIO_UNITARIO,BODEGA.ESTADO FROM PRODUCTO LEFT OUTER JOIN BODEGA ON PRODUCTO.ID = BODEGA.PRODUCTO_ID";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvproductos.ItemsSource = dt.DefaultView;
            dr.Close();

        }

        private void gvproductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.DataGrid dag = sender as System.Windows.Controls.DataGrid;
            if (dag.SelectedItem is DataRowView data)
            {
                txtIDPRO.Text = data["ID"].ToString();
                txtDESCR.Text = data["Nombre"].ToString();
                txtCANTI.Text = data["Cantidad"].ToString();
            }
        }
        private void btnBodega_Click(object sender, RoutedEventArgs e)
        {
            //string ESTADO = "BODEGA";
            try
            {
                OracleCommand cmd = new OracleCommand("fn_insertarbod", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("CANB", OracleDbType.Int32).Value = Convert.ToInt32(txtCANTI.Text);
                cmd.Parameters.Add("DESB", OracleDbType.Varchar2).Value = txtDESCR.Text;
                cmd.Parameters.Add("IDPR", OracleDbType.Int32).Value = Convert.ToInt32(txtIDPRO.Text);
                cmd.Parameters.Add("ESTB", OracleDbType.Varchar2).Value = "BODEGA";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Producto enviado a bodega", "ENVIAR A BODEGA", MessageBoxButtons.OK);
            }
            catch (Exception E)
            {

                MessageBox.Show("Error al guardar producto en bodega" + E, "ERROR AL ENVIAR", MessageBoxButtons.OK);
            }
            ListarProductos();

        }

        
    }
}
