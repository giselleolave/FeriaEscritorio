using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAcceso
{
    public class Producto
    {
        private int _id_producto;
        private string _nombre_producto;
        private int _cantidad;
        private string _calidad;      
        private DateTime _fecha_cosecha;  
        private int _preciou;
        
        private int _tipo_producto;
        private int _precio;


        public int Id_Producto
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
        public string Nombre_Producto
        {
            get
            {
                return _nombre_producto;
            }
            set
            {
                _nombre_producto = value;
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
        public string Calidad
        {
            get
            {
                return _calidad;
            }
            set
            {
                _calidad = value;
            }
        }
        
        public DateTime Fecha_cosecha
        {
            get
            {
                return _fecha_cosecha;
            }
            set
            {
                _fecha_cosecha = value;
            }
        }
        public int Precio_Unitario
        {
            get
            {
                return _preciou;
            }
            set
            {
                _preciou = value;
            }
        }
        
        
        public int Tipo_producto
        {
            get
            {
                return _tipo_producto;
            }
            set
            {
                _tipo_producto = value;
            }
        }
        public int Precio_Mayorista
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
        public Producto()
        {

        }


    }


 }
