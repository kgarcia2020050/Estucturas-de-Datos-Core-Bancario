using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utils.EstructuraDeDatos.Arboles.ArbolesBinarios;
using Utils.EstructuraDeDatos.Arboles.Models;
using Utils.EstructurasDeDatos.Interfaces;

namespace Utils.EstructuraDeDatos.Arboles.ABB
{
    public class ABB:ArbolBinario
    {
        public ABB() : base()
        {
        }


        public ABB(Nodo raiz) : base(raiz)
        {
        }


        public Nodo search(object value)
        {
            Comparador data = (Comparador)value;
            if (raiz == null)
                return null;
            else
                return search(getRaiz(), data);
        }

        public Nodo search(Nodo root, Comparador value)
        {
            if (root == null)
                return null;
            else if (value.igualQue(root.getDato()))
                return root;
            else if (value.menorQue(root.getDato()))
                return search(root.getIzq(), value);
            else
                return search(root.getDer(), value);
        }
        public Nodo buscarIterativo(object value)
        {
            Comparador data;
            bool encontrado = false;
            Nodo raizSub = raiz;
            data = (Comparador)value;
            while (!encontrado && raizSub != null)
            {
                if (data.igualQue(raizSub.getDato()))
                    encontrado = true;
                else if (data.menorQue(raizSub.getDato()))
                    raizSub = raizSub.getIzq();
                else
                    raizSub = raizSub.getDer();
            }
            return raizSub;
        }


        public void insert(object value)

        {
            Comparador data = (Comparador)value;
            raiz = insert(raiz, data);
        }
        protected Nodo insert(Nodo raizSub, Comparador dato)
        {
            if (raizSub == null)
                raizSub = new Nodo(dato);

            else if (dato.menorQue(raizSub.getDato()))
            {
                Nodo iz;
                iz = insert(raizSub.getIzq(), dato);
                raizSub.setIzq(iz);
            }

            else if (dato.mayorQue(raizSub.getDato()))
            {
                Nodo dr;
                dr = insert(raizSub.getDer(), dato);
                raizSub.setDer(dr);
            }

            else throw new Exception("Nodo duplicado");
            return raizSub;

        }

        public void delete(object valor)
        {
            Comparador dato = (Comparador)valor;
            raiz = delete(raiz, dato);
        }
        protected Nodo delete(Nodo raizSub, Comparador dato)
        {
            if (raizSub == null)
                throw new Exception("No encontrado el nodo con la clave");

            else if (dato.menorQue(raizSub.getDato()))
            {
                Nodo iz;
                iz = delete(raizSub.getIzq(), dato);
                raizSub.setIzq(iz);
            }

            else if (dato.mayorQue(raizSub.getDato()))
            {
                Nodo dr;
                dr = delete(raizSub.getDer(), dato);
                raizSub.setDer(dr);
            }
            else
            {
                Nodo q;
                q = raizSub;
                if (q.getIzq() == null)
                    raizSub = q.getDer();
                else if (q.getDer() == null)
                    raizSub = q.getIzq();
                else
                {
                    q = replace(q);
                }
                q = null;
            }
            return raizSub;
        }
        private Nodo replace(Nodo act)
        {
            Nodo a, p;
            p = act;
            a = act.getIzq();

            while (a.getDer() != null)
            {
                p = a;
                a = a.getDer();
            }

            act.setDato(a.getDato());
            if (p == act)
                p.setIzq(a.getIzq());
            else
                p.setDer(a.getDer());
            return a;

        }


        public void update(object value)
        {
            Comparador data = (Comparador)value;
            raiz = update(raiz, data);
        }

        protected Nodo update(Nodo raizSub, Comparador dato)
        {
            if (raizSub == null)
                throw new Exception("No encontrado el nodo con la clave");
            else if (dato.menorQue(raizSub.getDato()))
            {
                Nodo iz;
                iz = update(raizSub.getIzq(), dato);
                raizSub.setIzq(iz);
            }
            else if (dato.mayorQue(raizSub.getDato()))
            {
                Nodo dr;
                dr = update(raizSub.getDer(), dato);
                raizSub.setDer(dr);
            }
            else
            {
                raizSub.setDato(dato);
            }
            return raizSub;
        }

    }
}
