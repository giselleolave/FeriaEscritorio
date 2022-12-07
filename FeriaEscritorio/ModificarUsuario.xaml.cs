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
    /// Lógica de interacción para ModificarUsuario.xaml
    /// </summary>
    public partial class ModificarUsuario : Window
    { 
        OracleConnection conn = null;
        public ModificarUsuario()
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
            Application.Current.Shutdown();
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
            Application.Current.Shutdown();
        }

        private void btncargaru_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM USUARIO WHERE ID like ('" + txtIdMod.Text + "%')";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvusuariosMod.ItemsSource = dt.DefaultView;
            dr.Close();
            
        }

 
        private void gvusuariosMod_Loaded_1(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM USUARIO";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gvusuariosMod.ItemsSource = dt.DefaultView;
            dr.Close();

        }

        private void gvusuariosMod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtIdMod.Text = dr["ID"].ToString();
                txtNombreMod.Text = dr["NOMBRE_COMPLETO"].ToString();
                txtEmailusMod.Text = dr["EMAIL"].ToString();
                txtTelefonoMod.Text = dr["TELEFONO"].ToString();
                txtContrasenaMod.Text = dr["PASSWORD"].ToString();
            }
        }
      
        private void btnActualizarU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("fn_actualizarusu", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("ID", OracleDbType.Int32).Value = Convert.ToInt32(txtIdMod.Text);
                cmd.Parameters.Add("NOM", OracleDbType.Varchar2).Value = txtNombreMod.Text;
                cmd.Parameters.Add("EMA", OracleDbType.Varchar2).Value = txtEmailusMod.Text;
                cmd.Parameters.Add("NOM", OracleDbType.Varchar2).Value = txtTelefonoMod.Text;
                cmd.Parameters.Add("CONT", OracleDbType.Varchar2).Value = txtContrasenaMod.Text;
                cmd.Parameters.Add("TIP", OracleDbType.Int32).Value = Convert.ToInt32(CbxTipoUMod.SelectedValue);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuario Modificado");
            }
            catch (Exception)
            {

                MessageBox.Show("Error al modificar usuario");
            }
        }

        

        private void CbxTipoUMod_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand comm = new OracleCommand("select id, nombre from TIPO_USUARIO ", conn);
            OracleDataReader tipo = comm.ExecuteReader();
            while (tipo.Read())
            {
                string _id = tipo["id"].ToString();
                CbxTipoUMod.Items.Add(new { nombre = tipo["nombre"].ToString(), id = int.Parse(_id) });
            }
        }
    }
}
