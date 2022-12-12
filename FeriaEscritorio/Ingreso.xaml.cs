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
using Application = System.Windows.Application;

namespace FeriaEscritorio
{
    /// <summary>
    /// Lógica de interacción para Ingreso.xaml
    /// </summary>
    public partial class Ingreso 
    {
        OracleConnection conn = null;
        public Ingreso()
        {
            abrirConexion();
            InitializeComponent();
        }

        private void abrirConexion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["oracleDB"].ConnectionString;
            conn = new OracleConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión", "Message", MessageBoxButtons.OK);
                
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

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand comand = new OracleCommand("SELECT ID_TIPO_USUARIO FROM USUARIO WHERE EMAIL like('" + txtEmail.Text + "') AND PASSWORD  like('" + txtContrasena.Text + "')", conn);
            comand.CommandType = CommandType.Text;
            comand.Parameters.Add(":EMAIL", txtEmail.Text);
            comand.Parameters.Add(":PASSWORD", txtContrasena.Text);

            OracleDataReader reader = comand.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            if (txtEmail.Text.Length == 0) { MessageBox.Show("Debe escribir un email asociado a un perfil de ADMINISTRADOR", "EMAIL", MessageBoxButtons.OK); }
            else if (txtContrasena.Text.Length == 0) { MessageBox.Show("Debe escribir una contraseña", "CONTRASEÑA", MessageBoxButtons.OK); }
            else if (dt.Rows.Count == 0)
            {
                MessageBox.Show("NO ESTÁ AUTORIZADO PARA ENTRAR AL SISTEMA", "ACCESO DENEGADO", MessageBoxButtons.OK);
                
            }
            else if (dt.Rows.ToString() != "")
            {
                if (dt.Rows[0][0].ToString() != "1" && dt.Rows[0][0].ToString() != "2" || dt.Rows[0][0].ToString() == "")
                {
                    MessageBox.Show("NO ESTÁ AUTORIZADO PARA ENTRAR AL SISTEMA", "ACCESO DENEGADO", MessageBoxButtons.OK);
                
                }  
                else if (dt.Rows[0][0].ToString() == "1") 
                {
                    this.Hide();
                    MenuPrincipal menuPrincipal = new MenuPrincipal();
                    menuPrincipal.Show();
                }
                else if (dt.Rows[0][0].ToString() == "2") 
                {
                this.Hide();
                MenuConsultor menuConsultor = new MenuConsultor();
                menuConsultor.Show();
                }

            }
                     
           
            
           

            else {MessageBox.Show("NO ESTÁ AUTORIZADO PARA ENTRAR AL SISTEMA", "ACCESO DENEGADO", MessageBoxButtons.OK);}
        }
    }
}
