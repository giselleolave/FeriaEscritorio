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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;
using System.Data;
using CapaAcceso;

namespace FeriaEscritorio
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OracleConnection conn = null;
       
        public MainWindow()
        {
            abrirConexion();
            //InitializeComponent();
        }

        private void abrirConexion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OracleDB"].ConnectionString;
            conn = new OracleConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión");
                throw new Exception("Falla de conexión");
            }
        }

        private void gvListado_Loaded(object sender, RoutedEventArgs e)
        {
            cargarGrilla();
        }

        private void cargarGrilla()
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_listar", conn);/*Se llama a la funcion*/
                cmd.CommandType = CommandType.StoredProcedure;/*Se estable que es de tipo almacenado*/
                List<Usuarios> Lista = new List<Usuarios>();/*se crea una lista vacia*/
                OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);/*Se crea una copia de la lista*/
                output.Direction = ParameterDirection.ReturnValue;/*Se establece que devuelve un retorno*/
                cmd.ExecuteNonQuery();/*Se ejecuta funcion y se carga la variable con datos*/
                OracleDataReader reader = ((OracleRefCursor)output.Value).GetDataReader();

                while (reader.Read())
                {
                    Usuarios usu = new Usuarios();
                    usu.Id_Usuario = reader.GetInt32(0);
                    usu.Nombre_Usuario = reader.GetString(1);
                    usu.Apellido_Usuario = reader.GetString(2);

                    Lista.Add(usu);
                }
                gvListado.ItemsSource = Lista;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al cargar datos");
            }
        }

        private void btnContinuar_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Ingreso ventana = new Ingreso();
            ventana.Show();
          
           
        }
    }
}
