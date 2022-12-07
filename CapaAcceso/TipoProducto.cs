using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAcceso
{
    public class TipoProducto
    {
        private int _id_categoria;
        private string _nombre_categoria;

        public TipoProducto()
        {

        }
        public int Id_Categoria
        {
            get
            {
                return _id_categoria;
            }
            set
            {
                _id_categoria = value;
            }
        }
        public string Nombre_Categoria
        {
            get
            {
                return _nombre_categoria;
            }
            set
            {
                _nombre_categoria = value;
            }
        }




    }
}
