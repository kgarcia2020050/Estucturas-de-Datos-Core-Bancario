namespace Utils.EstructuraDeDatos.ListasEnlazadas.Models

{
    public class NodoLista
    {

        public object item { get; set; }
        public NodoLista next { get; set; }

        public NodoLista(object item)
        {
            this.item = item;
            next = null;
        }

        public object getItem()
        {
            return item;
        }

        public NodoLista getNext()
        {
            return next;
        }

        public void setNext(NodoLista next)
        {
            this.next = next;
        }

        public void setItem(object item)
        {
            this.item = item;
        }


        }
}
