using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAcceso
{
    public class Solicitud
    {
        private int _id_solicitud;
        private int _cantidad_sol;
        private string _producto_sol;
        private string _estado_sol;
        private int _id_usuario_sol;
        private string _direccion;

        public int Id_Solicitud
        {
            get
            {
                return _id_solicitud;
            }
            set
            {
                _id_solicitud = value;
            }
        }
        public int Cantidad_Sol
        {
            get
            {
                return _cantidad_sol;
            }
            set
            {
                _cantidad_sol = value;
            }
        }
        public string Producto_Sol
        {
            get
            {
                return _producto_sol;
            }
            set
            {
                _producto_sol = value;
            }
        }
        public string Estado_Sol
        {
            get
            {
                return _estado_sol;
            }
            set
            {
                _estado_sol = value;
            }
        }
        public int Id_Usario_Sol
        {
            get
            {
                return _id_usuario_sol;
            }
            set
            {
                _id_usuario_sol = value;
            }
        }
        public string Direccion
        {
            get
            {
                return _direccion;
            }
            set
            {
                _direccion = value;
            }
        }
        public Solicitud()
        {

        }












    }
}
