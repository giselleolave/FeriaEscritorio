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
    /// Lógica de interacción para CrearUsuario.xaml
    /// </summary>
    public partial class CrearUsuario : Window
    {
        OracleConnection conn = null;
        public CrearUsuario()
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
            AdminUsuarios volvermenuU = new AdminUsuarios();
            volvermenuU.Show();
        }

        private void btnSalirE_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnGuardarU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_insertarusu", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("NOM", OracleDbType.Varchar2).Value = txtNombre.Text;
                cmd.Parameters.Add("EMA", OracleDbType.Varchar2).Value = txtEmailus.Text;
                cmd.Parameters.Add("NOM", OracleDbType.Varchar2).Value = txtTelefono.Text;
                cmd.Parameters.Add("CONT", OracleDbType.Varchar2).Value = txtContrasena2.Text;
                cmd.Parameters.Add("TIP", OracleDbType.Int32).Value = Convert.ToInt32(CbxTipoU.SelectedValue);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuario Insertado", "INSERTAR USUARUARIO", MessageBoxButtons.OK);
            }
            catch (Exception)
            {

                MessageBox.Show("Error al insertar usuario", "ERROR INSERTA USUARIO", MessageBoxButtons.OK);
            }

        }

        private void CbxTipoU_Loaded(object sender, RoutedEventArgs e)
        {

            OracleCommand comm = new OracleCommand("SELECT ID,NOMBRE FROM TIPO_USUARIO ", conn);
            OracleDataReader tipoU = comm.ExecuteReader();
            while (tipoU.Read())
            {
                string _id = tipoU["ID"].ToString();
                CbxTipoU.Items.Add(new { nombre = tipoU["NOMBRE"].ToString(), id = int.Parse(_id) });
            }


        }

    }
}
