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
    /// Lógica de interacción para CrearTipoProducto.xaml
    /// </summary>
    public partial class CrearTipoProducto : Window
    {
        OracleConnection conn = null;
        public CrearTipoProducto()
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
            Application.Current.Shutdown();
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminProductos menuproduc = new AdminProductos();
            menuproduc.Show();
        }

        private void btnSalirE_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void gvcategoria_Loaded(object sender, RoutedEventArgs e)
        {
            ListarCategorias();
        }

        private void ListarCategorias()
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_listartipo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                List<TipoProducto> listacate = new List<TipoProducto>();
                OracleParameter output = cmd.Parameters.Add("ltp_cursor", OracleDbType.RefCursor);
                output.Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                OracleDataReader reader = ((OracleRefCursor)output.Value).GetDataReader();

                while (reader.Read())
                {

                    TipoProducto cat = new TipoProducto();
                    cat.Id_Categoria = reader.GetInt32(0);
                    cat.Nombre_Categoria = reader.GetString(1);
                   
                    listacate.Add(cat);
                   
                }
                gvcategoria.ItemsSource = listacate;
            }
            catch (Exception)
            {

                MessageBox.Show("Error al cargar datos");
            }
        }

        private void btnBuscarCat_Click(object sender, RoutedEventArgs e)
        {
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ID,DESCRIP_PRO FROM TIPO_PRODUCTO WHERE DESCRIP_PRO like ('" + txtBuscarCat.Text + "%')";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                gvcategoria.ItemsSource = dt.DefaultView;
                dr.Close();
            }
        }

        private void btnAgregarTipo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_insertacat", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("NOMCAT", OracleDbType.Varchar2).Value = txtNombreCat.Text;
                
                cmd.ExecuteNonQuery();
                MessageBox.Show("Categoría Insertada");
            }
            catch (Exception)
            {

                MessageBox.Show("Error al insertar categoría");
            }
        }
    }
}
