using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.EstructuraDeDatos.Arboles.Models;
using Utils.EstructurasDeDatos.Interfaces;

namespace Utils.EstructuraDeDatos.Arboles.AVL
{
    public class AVL
    {
        protected NodoAVL raiz;

        public AVL()
        {
            raiz = null;
        }
        public NodoAVL raizArbol()
        {
            return raiz;
        }

        private NodoAVL rotacionII(NodoAVL n, NodoAVL n1)
        {
            n.setIzq(n1.getDer());
            n1.setDer(n);
            if (n1.fe == -1)
            {
                n.fe = 0;
                n1.fe = 0;
            }
            else
            {
                n.fe = -1;
                n1.fe = 1;
            }
            return n1;
        }

        private NodoAVL rotacionDD(NodoAVL n, NodoAVL n1)
        {
            n.setDer(n1.getIzq());
            n1.setIzq(n);
            if (n1.fe == +1)
            {
                n.fe = 0;
                n1.fe = 0;
            }
            else
            {
                n.fe = +1;
                n1.fe = -1;
            }
            return n1;
        }

        private NodoAVL rotacionID(NodoAVL n, NodoAVL n1)
        {
            NodoAVL n2 = (NodoAVL)n1.getDer();
            n.setIzq(n2.getDer());
            n2.setDer(n);
            n1.setDer(n2.getIzq());
            n2.setIzq(n1);
            if (n2.fe == +1)
                n1.fe = -1;
            else
                n1.fe = 0;
            if (n2.fe == -1)
                n.fe = 1;
            else
                n.fe = 0;
            n2.fe = 0;
            return n2;
        }

        private NodoAVL rotacionDI(NodoAVL n, NodoAVL n1)
        {
            NodoAVL n2 = (NodoAVL)n1.getIzq();
            n.setDer(n2.getIzq());
            n2.setIzq(n);
            n1.setIzq(n2.getDer());
            n2.setDer(n1);
            if (n2.fe == +1)
                n.fe = -1;
            else
                n.fe = 0;
            if (n2.fe == -1)
                n1.fe = 1;
            else
                n1.fe = 0;
            n2.fe = 0;
            return n2;
        }

        public void insertar(object valor)
        {
            Comparador dato = (Comparador)valor;
            Logical h = new Logical(false);
            raiz = insertarAvl(raiz, dato, h);
        }

        private NodoAVL insertarAvl(NodoAVL raiz, Comparador dt, Logical h)
        {
            NodoAVL n1;
            if (raiz == null)
            {
                raiz = new NodoAVL(dt);
                h.setLogical(true);
            }
            else if (dt.menorQue(raiz.getDato()))
            {
                NodoAVL iz = insertarAvl((NodoAVL)raiz.getIzq(), dt, h);
                raiz.setIzq(iz);
                if (h.booleanValue())
                {
                    switch (raiz.fe)
                    {
                        case 1:
                            raiz.fe = 0;
                            h.setLogical(false);
                            break;
                        case 0:
                            raiz.fe = -1;
                            break;
                        case -1:
                            n1 = (NodoAVL)raiz.getIzq();
                            if (n1.fe == -1)
                                raiz = rotacionII(raiz, n1);
                            else
                                raiz = rotacionID(raiz, n1);
                            h.setLogical(false);
                            break;
                    }
                }
            }
            else if (dt.mayorQue(raiz.getDato()))
            {
                NodoAVL dr = insertarAvl((NodoAVL)raiz.getDer(), dt, h);
                raiz.setDer(dr);
                if (h.booleanValue())
                {
                    switch (raiz.fe)
                    {
                        case 1:
                            n1 = (NodoAVL)raiz.getDer();
                            if (n1.fe == +1)
                                raiz = rotacionDD(raiz, n1);
                            else
                                raiz = rotacionDI(raiz, n1);
                            h.setLogical(false);
                            break;
                        case 0:
                            raiz.fe = +1;
                            break;
                        case -1:
                            raiz.fe = 0;
                            h.setLogical(false);
                            break;
                    }
                }
            }
            else
                throw new Exception("No puede haber claves repetidas ");
            return raiz;
        }

        public void eliminar(object valor)
        {
            Comparador dato = (Comparador)valor;
            Logical flag = new Logical(false);
            raiz = borrarAvl(raiz, dato, flag);
        }


        public void update(object valor)
        {
            Comparador dato = (Comparador)valor;
            Logical flag = new Logical(false);
            raiz = borrarAvl(raiz, dato, flag);
            raiz = insertarAvl(raiz, dato, flag);
        }


        private NodoAVL borrarAvl(NodoAVL r, Comparador clave, Logical cambiaAltura)
        {
            if (r == null)
                throw new Exception("No se puede borrar un nodo nulo");
            //Debug.WriteLine("No se puede borrar un nodo nulo");
            else if (clave.menorQue(r.getDato()))
            {
                NodoAVL iz = borrarAvl((NodoAVL)r.getIzq(), clave, cambiaAltura);
                r.setIzq(iz);
                if (cambiaAltura.booleanValue())
                    r = equilibrar1(r, cambiaAltura);
            }
            else if (clave.mayorQue(r.getDato()))
            {
                NodoAVL dr = borrarAvl((NodoAVL)r.getDer(), clave, cambiaAltura);
                r.setDer(dr);
                if (cambiaAltura.booleanValue())
                    r = equilibrar2(r, cambiaAltura);
            }
            else
            {
                NodoAVL q = r;
                if (q.getIzq() == null)
                {
                    r = (NodoAVL)q.getDer();
                    cambiaAltura.setLogical(true);
                }
                else if (q.getDer() == null)
                {
                    r = (NodoAVL)q.getIzq();
                    cambiaAltura.setLogical(true);
                }
                else
                {   
                    NodoAVL iz = reemplazar(r, (NodoAVL)r.getIzq(), cambiaAltura);
                    r.setIzq(iz);
                    if (cambiaAltura.booleanValue())
                        r = equilibrar1(r, cambiaAltura);
                }
                q = null;
            }
            return r;
        }

        private NodoAVL reemplazar(NodoAVL n, NodoAVL act, Logical cambiaAltura)
        {
            if (act.getDer() != null)
            {
                NodoAVL d = reemplazar(n, (NodoAVL)act.getDer(), cambiaAltura);
                act.setDer(d);
                if (cambiaAltura.booleanValue())
                    act = equilibrar2(act, cambiaAltura);
            }
            else
            {
                n.setDato(act.getDato());
                NodoAVL temp = act;
                act = (NodoAVL)act.getIzq();
                temp = null;
                cambiaAltura.setLogical(true);
            }
            return act;
        }

        private NodoAVL equilibrar1(NodoAVL n, Logical cambiaAltura)
        {
            NodoAVL n1;
            switch (n.fe)
            {
                case -1:
                    n.fe = 0;
                    break;
                case 0:
                    n.fe = 1;
                    cambiaAltura.setLogical(false);
                    break;
                case +1:
                    n1 = (NodoAVL)n.getDer();
                    if (n1.fe >= 0)
                    {
                        if (n1.fe == 0)
                            cambiaAltura.setLogical(false);
                        n = rotacionDD(n, n1);
                    }
                    else
                        n = rotacionDI(n, n1);
                    break;
            }
            return n;
        }

        private NodoAVL equilibrar2(NodoAVL n, Logical cambiaAltura)
        {
            NodoAVL n1;
            switch (n.fe)
            {
                case -1:
                    n1 = (NodoAVL)n.getIzq();
                    if (n1.fe <= 0)
                    {
                        if (n1.fe == 0)
                            cambiaAltura.setLogical(false);
                        n = rotacionII(n, n1);
                    }
                    else
                        n = rotacionID(n, n1);
                    break;
                case 0:
                    n.fe = -1;
                    cambiaAltura.setLogical(false);
                    break;
                case +1:
                    n.fe = 0;
                    break;
            }
            return n;
        }

        bool esVacio()
        {
            return raiz == null;
        }




        public static List<NodoAVL> preOrden(NodoAVL raiz)
        {
            List<NodoAVL> nodos = new List<NodoAVL>();
            preOrdenM(raiz, nodos);
            return nodos;
        }

        private static void preOrdenM(NodoAVL nodo, List<NodoAVL> lista)
        {
            if (nodo != null)
            {
                lista.Add(nodo);
                preOrdenM((NodoAVL)nodo.getIzq(), lista);
                preOrdenM((NodoAVL)nodo.getDer(), lista);
            }
        }

        public static List<NodoAVL> inOrden(NodoAVL raiz)
        {
            List<NodoAVL> nodos = new List<NodoAVL>();
            inOrdenM(raiz, nodos);
            return nodos;
        }

        private static void inOrdenM(NodoAVL nodo, List<NodoAVL> lista)
        {
            if (nodo != null)
            {
                inOrdenM((NodoAVL)nodo.getIzq(), lista);
                lista.Add(nodo);
                inOrdenM((NodoAVL)nodo.getDer(), lista);
            }
        }

        public static List<NodoAVL> postOrden(NodoAVL raiz)
        {
            List<NodoAVL> nodos = new List<NodoAVL>();
            postOrdenM(raiz, nodos);
            return nodos;
        }

        private static void postOrdenM(NodoAVL nodo, List<NodoAVL> lista)
        {
            if (nodo != null)
            {
                postOrdenM((NodoAVL)nodo.getIzq(), lista);
                postOrdenM((NodoAVL)nodo.getDer(), lista);
                lista.Add(nodo);
            }
        }



        public NodoAVL search(object valor)
        {
            Comparador dato = (Comparador)valor;
            return search(raiz, dato);
        }

        private NodoAVL search(NodoAVL raizSub, Comparador dato)
        {
            if (raizSub == null)
                return null;
            else if (dato.igualQue(raizSub.getDato()))
                return raizSub;
            else if (dato.menorQue(raizSub.getDato()))
                return search((NodoAVL)raizSub.getIzq(), dato);
            else
                return search((NodoAVL)raizSub.getDer(), dato);
        }

    }
}
