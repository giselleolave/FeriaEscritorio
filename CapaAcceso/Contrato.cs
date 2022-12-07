using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAcceso
{
    public class Contrato
    {
        private int _id_contrato;
        private DateTime _fecha_firma;
        private DateTime _fecha_termino;
        private int _porcentaje;
        private char _is_active;

        public int Id_Contrato
        {
            get
            {
                return _id_contrato;
            }
            set
            {
                _id_contrato = value;
            }
        }
        public DateTime Fecha_firma
        {
            get
            {
                return _fecha_firma;
            }
            set
            {
                _fecha_firma = value;
            }
        }
        public DateTime Fecha_termino
        {
            get
            {
                return _fecha_termino;
            }
            set
            {
                _fecha_termino = value;
            }
        }
        public int Porcentaje
        {
            get
            {
                return _porcentaje;
            }
            set
            {
                _porcentaje = value;
            }
        }
        public char Is_active
        {
            get
            {
                return _is_active;
            }
            set 
            {
                _is_active = value;
            }
        }
        public Contrato()
        {

        }













    }
}
