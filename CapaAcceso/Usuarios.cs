using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAcceso
{
   public class Usuarios
    {
        private int _id_usuario;
        private string _nombre_usuario;
        private string _apellido_usuario;

        public int Id_Usuario
        {
            get
            {
                return _id_usuario;
            }
            set
            {
                _id_usuario = value;
            }
        }
        public string Nombre_Usuario
        {
            get
            {
                return _nombre_usuario;
            }
            set
            {
                _nombre_usuario = value;
            }
        }
        public string Apellido_Usuario
        {
            get
            {
                return _apellido_usuario;
            }
            set
            {
                _apellido_usuario = value;
            }
        }

        public Usuarios() 
        {

        }



    }
}
