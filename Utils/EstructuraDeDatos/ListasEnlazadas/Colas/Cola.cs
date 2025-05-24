
using Utils.EstructuraDeDatos.ListasEnlazadas.Models;
using Utils.models;

namespace Utils.EstructuraDeDatos.ListasEnlazadas.Colas
{
    public class Cola
    {

        public NodoLista? actual;
        public NodoLista? next;


        public Cola()
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
            if (isEmpty())
            {
                actual = newNode;
            }
            else
            {
                next = actual;
                while (next.getNext() != null)
                {
                    next = next.getNext();
                }
                next.setNext(newNode);
            }
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


        public void moveNext()
        {
            if (!isEmpty())
            {
                actual = actual.getNext();
            }
        }


        public object getNextItem()
        {
            if (isEmpty())
            {
                return null;
            }
            else
            {
                if (actual.getNext() != null)
                {
                    return actual.getNext().getItem();
                }
                else
                {
                    return null;
                }
            }
        }


        public int size()
        {
            int size = 0;
            NodoLista temp = actual;
            while (temp != null)
            {
                size++;
                temp = temp.getNext();
            }
            return size;
        }

    }

}