using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAcceso
{
    public class Saldo
    {
        private int _id_saldo;
        private string _descripcion;
        private int _cantidad;       
        private float _precio;
        private int _id_producto;
        private string _estado;

        public Saldo() { }

        public int Id_Saldo
        {
            get
            {
                return _id_saldo;
            }
            set
            {
                _id_saldo = value;
            }
        }
        public string Descripcion
        {
            get
            {
                return _descripcion;
            }
            set
            {
                _descripcion = value;
            }
        }
        public int Cantidad
        {
            get
            {
                return _cantidad;
            }
            set
            {
                _cantidad = value;
            }
        }
        
        public float Precio
        {
            get
            {
                return _precio;
            }
            set
            {
                _precio = value;
            }
        }
        public int Producto_ID
        {
            get
            {
                return _id_producto;
            }
            set
            {
                _id_producto = value;
            }
        }
        public string Estado
        {
            get
            {
                return _estado;
            }
            set
            {
                _estado = value;
            }
        }

    }

    
}
