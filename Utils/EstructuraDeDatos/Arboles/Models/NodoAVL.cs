namespace Utils.EstructuraDeDatos.Arboles.Models
{
   public class NodoAVL : Nodo
    {
        public int fe;
        public NodoAVL(object valor) : base(valor)
        {
            fe = 0;
        }

        public NodoAVL(object valor, NodoAVL ramaIzdo, NodoAVL ramaDcho) : base(ramaIzdo, valor, ramaDcho)
        {
            fe = 0;
        }
    }
}
