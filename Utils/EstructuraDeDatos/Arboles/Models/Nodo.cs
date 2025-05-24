namespace Utils.EstructuraDeDatos.Arboles.Models
{
    public class Nodo
    {
        protected object dato;
        protected Nodo izq;
        protected Nodo der;

        public Nodo(object dato)
        {
            this.dato = dato;
            izq = null;
            der = null;
        }



        public Nodo(Nodo izq, object dato, Nodo der)
        {
            this.izq = izq;
            this.dato = dato;
            this.der = der;
        }
        public object getDato()
        {
            return dato;
        }

        public Nodo getIzq()
        {
            return izq;
        }
        public Nodo getDer()
        {
            return der;

        }

        public void setDato(object dato)
        {
            this.dato = dato;
        }

        public void setIzq(Nodo izq)
        {
            this.izq = izq;
        }

        public void setDer(Nodo der)
        {
            this.der = der;
        }

        public override string ToString()
        {
            return dato.ToString();
        }
    }
}
