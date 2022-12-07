using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAcceso
{
     public class TipoUsuario
    {
        public int _id_tipousuario;
        public string _tipo_usuario;

        public TipoUsuario()
        {
        }
        public int Id_TipoUsuario
        {
            get
            {
                return _id_tipousuario;
            }
            set
            {
                _id_tipousuario= value;
            }
        }
        public string Tipo_Usuario
        {
            get
            {
                return _tipo_usuario;
            }
            set
            {
                _tipo_usuario = value;
            }
        }
        
    }

}
