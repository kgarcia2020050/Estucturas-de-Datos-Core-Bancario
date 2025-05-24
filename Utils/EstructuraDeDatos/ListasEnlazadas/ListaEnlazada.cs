
using System.Diagnostics;
using Utils.EstructuraDeDatos.ListasEnlazadas.Models;
using Utils.models;

namespace Utils.EstructuraDeDatos.ListasEnlazadas
{
    public class ListaEnlazada
    {
        public NodoLista? cabeza;

        public ListaEnlazada()
        {
            cabeza = null;
        }

        public void Insertar(object item)
        {
            NodoLista nuevoNodo = new NodoLista(item);
            if (cabeza == null)
            {
                cabeza = nuevoNodo;
            }
            else
            {
                NodoLista actual = cabeza;
                while (actual.getNext() != null)
                {
                    actual = actual.getNext();
                }
                actual.setNext(nuevoNodo);
            }
        }


        public void Actualizar(object item, object nuevoItem)
        {
            NodoLista actual = cabeza;
            while (actual != null)
            {
                if (actual.getItem().Equals(item))
                {

                    actual.setItem(nuevoItem);
                    return;
                }
                actual = actual.getNext();
            }
        }


        public void Eliminar(object item)
        {
            if (cabeza == null) return;

            if (cabeza.getItem().Equals(item))
            {
                cabeza = cabeza.getNext();
                return;
            }
            NodoLista actual = cabeza;

            while (actual.getNext() != null && !actual.getNext().getItem().Equals(item))
            {
                actual = actual.getNext();
            }
            if (actual.getNext() != null)
            {
                actual.setNext(actual.getNext().getNext());
            }
        }

        public bool Buscar(object item)
        {
            NodoLista actual = cabeza;
            while (actual != null)
            {
                if (actual.getItem().Equals(item))
                {
                    return true;
                }
                actual = actual.getNext();
            }
            return false;
        }

        public void Imprimir()
        {
            NodoLista actual = cabeza;
            while (actual != null)
            {
                //Debug.WriteLine(IsEmpty() ? "Lista vacía" : actual.getItem().ToString());
                actual = actual.getNext();
            }
        }

        public void Limpiar()
        {
            cabeza = null;
        }

        public bool IsEmpty()
        {
            return cabeza == null;
        }

        public int Contar()
        {
            int contador = 0;
            NodoLista actual = cabeza;
            while (actual != null)
            {
                contador++;
                actual = actual.getNext();
            }
            return contador;
        }

        public object Obtener(int index)
        {
            if (index < 0 || index >= Contar())
            {
                throw new ArgumentOutOfRangeException("Index fuera de rango");
            }
            NodoLista actual = cabeza;
            for (int i = 0; i < index; i++)
            {
                actual = actual.getNext();
            }
            return actual.getItem();
        }

        public void Reemplazar(int index, object nuevoItem)
        {
            if (index < 0 || index >= Contar())
            {
                throw new ArgumentOutOfRangeException("Index fuera de rango");
            }
            NodoLista actual = cabeza;
            for (int i = 0; i < index; i++)
            {
                actual = actual.getNext();
            }
            actual.setItem(nuevoItem);
        }


        public void EliminarEnPosicion(int index)
        {
            if (index < 0 || index >= Contar())
            {
                throw new ArgumentOutOfRangeException("Index fuera de rango");
            }
            if (index == 0)
            {
                cabeza = cabeza.getNext();
            }
            else
            {
                NodoLista actual = cabeza;
                for (int i = 0; i < index - 1; i++)
                {
                    actual = actual.getNext();
                }
                actual.setNext(actual.getNext().getNext());
            }
        }


        public void EliminarPorClave(object clave, string propiedad)
        {
            NodoLista actual = cabeza;
            NodoLista anterior = null;

            while (actual != null)
            {
                var item = actual.getItem();
                var propInfo = item.GetType().GetProperty(propiedad);

                if (propInfo != null)
                {
                    var valor = propInfo.GetValue(item);
                    if (valor != null && valor.Equals(clave))
                    {
                        // Nodo encontrado: eliminarlo
                        if (anterior == null)
                        {
                            cabeza = actual.getNext(); // Nodo en la cabeza
                        }
                        else
                        {
                            anterior.setNext(actual.getNext());
                        }
                        return;
                    }
                }

                anterior = actual;
                actual = actual.getNext();
            }
        }


        public NodoLista searchItem(object item)
        {
            NodoLista actual = cabeza;
            Tarjeta existingTarjeta = (Tarjeta)item;
            while (actual != null)
            {

                Tarjeta tarjeta = (Tarjeta)actual.getItem();

                if ( existingTarjeta.numeroTarjeta == tarjeta.numeroTarjeta && existingTarjeta.pin == tarjeta.pin )
                {
                    return actual;
                }
                actual = actual.getNext();
            }
            return null;
        }

        public NodoLista obtenerCabeza()
        {
            return cabeza;
        }

        public NodoLista BuscarNodo(object key, string property = "nit")
        {
            NodoLista actual = cabeza;

            while (actual != null)
            {
                object item = actual.getItem();

                var propInfo = item.GetType().GetProperty(property);

                if (propInfo != null)
                {
                    object valorPropiedad = propInfo.GetValue(item);

                    if (valorPropiedad != null && valorPropiedad.Equals(key))
                    {
                        return actual; 
                    }
                }

                actual = actual.getNext();
            }

            return null; 
        }


    }

}