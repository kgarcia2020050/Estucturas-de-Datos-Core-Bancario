using System;
using System.Diagnostics;
using Utils.EstructuraDeDatos.ListasEnlazadas;
using Utils.EstructuraDeDatos.ListasEnlazadas.Models;

namespace Utils.EstructuraDeDatos.DispercionesHash
{
    public class Hash
    {
        public static readonly int Tam_tabla = 997;
        public static readonly double R = 0.618034;
        public ListaEnlazada[] tabla = new ListaEnlazada[Tam_tabla];

        public long transformaClave(string clave)
        {
            //Debug.WriteLine($"Transformando clave: {clave}");
            long d = 0;
            for (int j = 0; j < Math.Min(clave.Length, 10); j++)
            {
                d = d * 27 + (int)clave[j];
            }
            return d < 0 ? -d : d;
        }

        public int dispersion(long x)
        {
            double t = R * x - Math.Floor(R * x);
            return (int)(Tam_tabla * t);
        }

        public int PosMod(int x)
        {
            int pos = x % Tam_tabla;
            return pos < 0 ? pos + Tam_tabla : pos;
        }

        public int returnPosicion(string clave)
        {
            return dispersion(transformaClave(clave));
        }

        public int returnPosicion(int clave)
        {
            return PosMod(clave);
        }

        public void Insertar(object dato, int clave)
        {
            int pos = returnPosicion(clave);

            //Debug.WriteLine($"Insertando en la posición: {pos} con clave: {clave}");

            if (tabla[pos] == null)
                tabla[pos] = new ListaEnlazada();

            tabla[pos].Insertar(dato);
        }

        public void Insertar(object dato, string clave)
        {
            int pos = returnPosicion(clave);

            if (tabla[pos] == null)
                tabla[pos] = new ListaEnlazada();

            tabla[pos].Insertar(dato);
        }

        public void Eliminar(int clave, string propiedad)
        {
            int pos = returnPosicion(clave);

            //Debug.WriteLine($"Eliminando en la posición: {pos} con clave: {clave}");

            if (tabla[pos] != null)
                tabla[pos].EliminarPorClave(clave, propiedad);
        }

        public void Eliminar(string clave, string propiedad)
        {
            int pos = returnPosicion(clave);

            if (tabla[pos] != null)
                tabla[pos].EliminarPorClave(clave, propiedad);
        }

        public object Buscar(int clave, string property)
        {
            int pos = returnPosicion(clave);

            if (tabla[pos] == null)
                return null;

            NodoLista nodo = tabla[pos].BuscarNodo(clave, property);
            return nodo != null ? nodo.getItem() : null;
        }

        public object Buscar(string clave, string property)
        {
            int pos = returnPosicion(clave);

            if (tabla[pos] == null)
                return null;

            NodoLista nodo = tabla[pos].BuscarNodo(clave, property);
            return nodo != null ? nodo.getItem() : null;
        }


        public void Actualizar(object nuevoDato, object clave, string property)
        {
            int pos;

            if (clave is int claveInt)
            {
                pos = returnPosicion(claveInt);
            }
            else if (clave is string claveStr)
            {
                pos = returnPosicion(claveStr);
            }
            else
            {
                throw new ArgumentException("La clave debe ser de tipo int o string.");
            }

            if (tabla[pos] != null)
            {
                NodoLista nodo = tabla[pos].BuscarNodo(clave, property);
                if (nodo != null)
                {
                    nodo.setItem(nuevoDato);
                }
                else
                {
                    throw new Exception($"Elemento con clave '{clave}' no encontrado para la propiedad '{property}'.");
                }
            }
            else
            {
                throw new Exception($"No existe ninguna lista en la posición de la clave '{clave}'.");
            }
        }

        public void imprimir()
        {
            for (int i = 0; i < Tam_tabla; i++)
            {
                if (tabla[i] != null)
                {
                    //Debug.WriteLine($"Posición {i}:");
                    tabla[i].Imprimir();
                }
            }
        }

    }
}
