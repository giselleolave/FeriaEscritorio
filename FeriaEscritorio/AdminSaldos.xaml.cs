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
    /// Lógica de interacción para AdminSaldos.xaml
    /// </summary>
    public partial class AdminSaldos : Window
    {
        OracleConnection conn = null;
        public AdminSaldos()
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
            Application.Current.Shutdown();
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

        private void gvsaldos_Loaded(object sender, RoutedEventArgs e)
        {
            ListarSaldos();
        }

        private void ListarSaldos()
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_listarsal", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                List<Saldo> listadosal = new List<Saldo>();
                OracleParameter output = cmd.Parameters.Add("lsa_cursor", OracleDbType.RefCursor);
                output.Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                OracleDataReader reader = ((OracleRefCursor)output.Value).GetDataReader();

                while (reader.Read())
                {

                    Saldo sal = new Saldo();
                    sal.Id_Saldo = reader.GetInt32(0);
                    sal.Cantidad = reader.GetInt32(1);
                    sal.Descripcion = reader.GetOracleClob(2).ToString();
                    sal.Precio = reader.GetFloat(3);
                    sal.Estado = reader.GetString(4);
                    sal.Producto_ID = reader.GetInt32(5);
                    

                    listadosal.Add(sal);
                    Console.WriteLine("listaProductos", listadosal);
                }
                gvsaldos.ItemsSource = listadosal;

            }
            catch (Exception)
            {

                MessageBox.Show("Error al cargar datos");
            }
        }

        private void btnListarS_Click(object sender, RoutedEventArgs e)
        {
            ListarSaldos();
        }

        private void btnBusSal_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID,CANTIDAD,DESCRIPCION,PRECIO,PRODUCTO_ID,ESTADO FROM SALDO WHERE DESCRIPCION like ('" + txtbuscarSA.Text + "%')";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvsaldos.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void btnActivas_Copy_Click(object sender, RoutedEventArgs e)
        {
            string publicada = "PUBLICADO";
            try
            {
                OracleCommand cmd = new OracleCommand("FN_CAMBIARESTSA", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtCambioSA.Text);
                cmd.Parameters.Add("EST", OracleDbType.Varchar2).Value = publicada;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Saldo publicado");
                ListarSaldos();
            }
            catch (Exception)
            {

                MessageBox.Show("Error al publicar saldo");
            }
        }

        private void btnAnuladas_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_borrarsal", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtCambioSA.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Saldo Eliminado");
                ListarSaldos();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar saldo");

            }
        }
    }
}
