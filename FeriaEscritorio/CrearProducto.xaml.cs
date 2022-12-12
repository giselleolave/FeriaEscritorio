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
    /// Lógica de interacción para CrearProducto.xaml
    /// </summary>
    public partial class CrearProducto : Window
    {
        OracleConnection conn = null;
        public CrearProducto()
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

                throw new Exception("Error de conexión");
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminProductos menuproduc = new AdminProductos();
            menuproduc.Show();
        }

        private void btnSalirE_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void CbxTipoP_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand comm = new OracleCommand("select id, DESCRIP_PRO from TIPO_PRODUCTO ", conn);
            OracleDataReader tipoP = comm.ExecuteReader();
            while (tipoP.Read())
            {
                string _idP = tipoP["id"].ToString();
                CbxTipoP.Items.Add(new { descripcion = tipoP["DESCRIP_PRO"].ToString(), id = int.Parse(_idP) });
            }

        }


        private void btnGuardarP_Click(object sender, RoutedEventArgs e)
        {
            string calidad = null;
            
            if (rbNormal.IsChecked == true & rbAlta.IsChecked == false)
            {
                calidad = "NORMAL";
            }
            if (rbNormal.IsChecked == false & rbAlta.IsChecked == true)
            {
                calidad = "ALTA";
            }
            try
            {
                OracleCommand cmd = new OracleCommand("fn_insertarpro", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("NOMP", OracleDbType.Varchar2).Value = txtNombrePro.Text;
                cmd.Parameters.Add("CANP", OracleDbType.Int32).Value = Convert.ToInt32(txtCantidad.Text);
                cmd.Parameters.Add("CALP", OracleDbType.Varchar2).Value = calidad.ToString();
                cmd.Parameters.Add("FECP", OracleDbType.Date).Value = Convert.ToDateTime(dtFecha.ToString());
                cmd.Parameters.Add("PRUP", OracleDbType.Int32).Value = Convert.ToInt32(txtPrecioU.Text);
                cmd.Parameters.Add("IDTP", OracleDbType.Int32).Value = Convert.ToInt32(CbxTipoP.SelectedValue);
                cmd.Parameters.Add("PRMP", OracleDbType.Int32).Value = Convert.ToInt32(txtPrecioM.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Producto Insertado", "INSERTAR PRODUCTO", MessageBoxButtons.OK);
            }
            catch (Exception error)
            {

                MessageBox.Show("Error al insertar producto", "ERROR INSERTAR PRODUCTO", MessageBoxButtons.OK);
            }
            
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarProducto();
        }

        private void LimpiarProducto()
        {
            txtNombrePro.Clear();
            txtCantidad.Clear();
            txtPrecioU.Clear();
            txtPrecioM.Clear();
        }
    }
}
