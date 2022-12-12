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
    /// Lógica de interacción para AdminUsuarios.xaml
    /// </summary>
    public partial class AdminUsuarios : Window
    {
        OracleConnection conn = null;
        public AdminUsuarios()
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
            MenuPrincipal volvermenu = new MenuPrincipal();
            volvermenu.Show();
        }

        private void btnSalirE_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnCrearU_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CrearUsuario crearu = new CrearUsuario();
            crearu.Show();
        }

        private void gvusuarios_Loaded(object sender, RoutedEventArgs e)
        {
            ListarUsuarios();
        }

        private void ListarUsuarios()
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_listarusu",conn);
                cmd.CommandType = CommandType.StoredProcedure;
                List<Usuarios> lista = new List<Usuarios>();
                OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
                output.Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                OracleDataReader reader = ((OracleRefCursor)output.Value).GetDataReader();

                while (reader.Read())
                {
                    Usuarios usu = new Usuarios();
                    usu.Id_Usuario = reader.GetInt32(0);
                    usu.Nombre_Usuario = reader.GetString(1);
                    usu.Apellido_Usuario = reader.GetString(2);
                    lista.Add(usu);
                }
                gvusuarios.ItemsSource = lista;
            }
            catch (Exception)
            {

                MessageBox.Show("Error al cargar datos", "ERROR DE CONEXIÓN", MessageBoxButtons.OK);
            }
        }

        private void btnElimiU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              OracleCommand cmd = new OracleCommand("fn_borrarusu", conn);
              cmd.CommandType = CommandType.StoredProcedure;
              cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtbuscarID.Text);
              cmd.ExecuteNonQuery();
              MessageBox.Show("Usuario Eliminado", "ELIMINAR USUARIO", MessageBoxButtons.OK);
              ListarUsuarios();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar usuario", "ERROR ELIMINAR USUARIO", MessageBoxButtons.OK);

            }
           
        }

        private void btnListarU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_listarusu", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                List<Usuarios> lista = new List<Usuarios>();
                OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
                output.Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                OracleDataReader reader = ((OracleRefCursor)output.Value).GetDataReader();

                while (reader.Read())
                {
                    Usuarios usu = new Usuarios();
                    usu.Id_Usuario = reader.GetInt32(0);
                    usu.Nombre_Usuario = reader.GetString(1);
                    usu.Apellido_Usuario = reader.GetString(2);

                    lista.Add(usu);
                }
                gvusuarios.ItemsSource = lista;
            }
            catch (Exception)
            {

                MessageBox.Show("Error al cargar datos", "ERROR DE CONEXIÓN", MessageBoxButtons.OK);
            }
        }

        private void btnModiU_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ModificarUsuario modiu = new ModificarUsuario();
            modiu.Show();
        }

        private void btnBusNom_Click(object sender, RoutedEventArgs e)
        {
            
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM USUARIO WHERE NOMBRE_COMPLETO like ('" + txtbuscarU.Text + "%')";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                gvusuarios.ItemsSource = dt.DefaultView;
                dr.Close();
            }
           
            
        }
    }
    }

