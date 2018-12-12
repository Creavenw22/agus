using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Entidades.SP
{
    public delegate bool DelegadoPrecio(string ruta, double precio);
    public class Cajon<T> : ISerializar where T : Fruta
    {
        #region Atributos
        protected int _capacidad;
        protected List<T> elementos;
        protected double _precioUnitario;
        public event DelegadoPrecio EventoPrecio;
        #endregion

        #region Propiedades
        public List<T> Elementos
        {
            get
            {
                return this.elementos;
            }
        }

        public double PrecioTotal
        {
            get
            {
                double precio = (this._precioUnitario * this.Elementos.Count);
                return precio;
            }
        }
        #endregion

        #region Constructor
        private Cajon()
        {
            this.elementos = new List<T>();
        }

        public Cajon(int capacidad) : this()
        {
            this._capacidad = capacidad;
        }

        public Cajon(double precioUnitario, int capacidad) : this(capacidad)
        {
            this._precioUnitario = precioUnitario;
        }
        #endregion

        #region Metodos
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\nCapacidad del cajon: {0}", this._capacidad);
            sb.AppendFormat("\nCantidad total de elementos: {0}", this.Elementos.Count);
            sb.AppendFormat("\nPrecio total: {0}\n\n", this.PrecioTotal);
            foreach (T item in this.Elementos)
            {
                sb.AppendLine(item.ToString()+"\n");
            }
            return sb.ToString();
        }

        public bool Xml(string ruta)
        {
            bool retorno = false;
            using (TextWriter writer = new StreamWriter(String.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ruta)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Cajon<T>));
                serializer.Serialize(writer, this);
                retorno = true;
            }
            return retorno;
        }
        #endregion

        #region Sobrecarga de operadores
        public static Cajon<T> operator +(Cajon<T> cajon, T fruta)
        {
            if(cajon.Elementos.Count <= cajon._capacidad)
            {
                if (cajon.PrecioTotal > 55)
                {
                    cajon.EventoPrecio((Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Precio.txt"), cajon.PrecioTotal);
                }
                else
                {
                    cajon.Elementos.Add(fruta);
                }
                return cajon;
            }
            else
            {
                throw new CajonLlenoException("\nLa cantidad de frutas agregada supera la capacidad del cajon");
            }
        }
        #endregion
    }
}
