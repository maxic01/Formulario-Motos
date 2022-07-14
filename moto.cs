using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motos11
{
    internal class moto
    {
        private int codigo;

        public int pCodigo
        {
            get { return codigo; }
            set { codigo = value; }
        }
        private string modelo;

        public string pModelo
        {
            get { return modelo; }
            set { modelo = value; }
        }
        private double precio;

        public double pPrecio
        {
            get { return precio; }
            set { precio = value; }
        }
        private int marca;

        public int pMarca
        {
            get { return marca; }
            set { marca = value; }
        }
        private DateTime fecha;

        public DateTime pFecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        public moto()
        {
            this.codigo = 0;
            this.modelo = "";
            this.precio = 0;
            this.marca = 0;
            this.fecha = DateTime.Today;
        }

        public moto(int codigo, string modelo, double precio, int marca, DateTime fecha)
        {
            this.codigo = codigo;
            this.modelo = modelo;
            this.precio = precio;
            this.marca = marca;
            this.fecha = fecha;
        }

        public override string ToString()
        {
            return codigo + " - " + modelo + " - " + precio;
        }
    }
}
