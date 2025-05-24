using System.Diagnostics;
using Utils.EstructuraDeDatos.ListasEnlazadas.Models;


namespace Utils.EstructuraDeDatos.ListasEnlazadas.Pilas
{
    public class Pila
    {

        public NodoLista? actual { get; set; }

        public Pila()
        {
            actual = null;
        }

        public bool isEmpty()
        {
            return actual == null;
        }


        public void add(object client)
        {
            NodoLista newNode = new NodoLista(client);
            newNode.setNext(actual);
            actual = newNode;
        }


        public object remove()
        {
            if (isEmpty())
            {
                return null;
            }
            else
            {
                object client = actual.getItem();
                actual = actual.getNext();
                return client;
            }
        }


        public object getActualItem()
        {
            if (isEmpty())
            {
                return null;
            }
            else
            {
                return actual.getItem();
            }
        }


        public int getTotalItems()
        {
            int count = 0;
            NodoLista current = actual;
            while (current != null)
            {
                count++;
                current = current.getNext();
            }
            return count;
        }
    }
}
