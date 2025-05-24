using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utils.EstructuraDeDatos.Arboles.Models;

namespace Utils.EstructuraDeDatos.Arboles.ArbolesBinarios
{
   public class ArbolBinario
    {
        protected Nodo raiz;


        public ArbolBinario()
        {
            raiz = null;
        }

        public ArbolBinario(Nodo raiz)
        {
            this.raiz = raiz;
        }

        public Nodo getRaiz()
        {
            return raiz;
        }

        public void setRaiz(Nodo raiz)
        {
            this.raiz = raiz;
        }

        public bool esVacio()
        {
            return raiz == null;
        }
        public static Nodo nuevoArbol(Nodo ramaIzqda, object dato, Nodo ramaDrcha)
        {
            return new Nodo(ramaIzqda, dato, ramaDrcha);
        }


        public static List<Nodo> preOrden(Nodo raiz)
        {
            List<Nodo> nodos = new List<Nodo>();
            preOrdenM(raiz, nodos);
            return nodos;
        }

        private static void preOrdenM(Nodo nodo, List<Nodo> lista)
        {
            if (nodo != null)
            {
                lista.Add(nodo);
                preOrdenM(nodo.getIzq(), lista);
                preOrdenM(nodo.getDer(), lista);
            }
        }


        public static List<Nodo> inOrden(Nodo raiz)
        {
            List<Nodo> nodos = new List<Nodo>();
            inOrdenM(raiz, nodos);
            return nodos;
        }

        private static void inOrdenM(Nodo nodo, List<Nodo> lista)
        {
            if (nodo != null)
            {
                inOrdenM(nodo.getIzq(), lista);
                lista.Add(nodo);
                inOrdenM(nodo.getDer(), lista);
            }
        }

        public static List<Nodo> postOrden(Nodo raiz)
        {
            List<Nodo> nodos = new List<Nodo>();
            postOrdenM(raiz, nodos);
            return nodos;
        }

        private static void postOrdenM(Nodo nodo, List<Nodo> lista)
        {
            if (nodo != null)
            {
                postOrdenM(nodo.getIzq(), lista);
                postOrdenM(nodo.getDer(), lista);
                lista.Add(nodo);
            }
        }

        public static int numNodos(Nodo raiz)
        {
            if (raiz == null)
                return 0;
            else
                return 1 + numNodos(raiz.getIzq()) +
                numNodos(raiz.getDer());
        }
    }
}
