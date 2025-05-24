using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.EstructurasDeDatos.Interfaces;
using Utils.models;

namespace Utils.dtos
{
    public class CardsNumber : Tarjeta, Comparador
    {
        bool Comparador.igualQue(object q)
        {
            if (q == null || !(q is CardsNumber)) return false;
            CardsNumber other = (CardsNumber)q;
            return this.numeroTarjeta.Equals(other.numeroTarjeta);
        }

        bool Comparador.mayorIgualQue(object q)
        {
            if (q == null || !(q is CardsNumber)) return false;
            CardsNumber other = (CardsNumber)q;
            return string.Compare(this.numeroTarjeta, other.numeroTarjeta) >= 0;
        }

        bool Comparador.mayorQue(object q)
        {
            if (q == null || !(q is CardsNumber)) return false;
            CardsNumber other = (CardsNumber)q;
            return string.Compare(this.numeroTarjeta, other.numeroTarjeta) > 0;
        }

        bool Comparador.menorIgualQue(object q)
        {
            if (q == null || !(q is CardsNumber)) return false;
            CardsNumber other = (CardsNumber)q;
            return string.Compare(this.numeroTarjeta, other.numeroTarjeta) <= 0;
        }

        bool Comparador.menorQue(object q)
        {
            if (q == null || !(q is CardsNumber)) return false;
            CardsNumber other = (CardsNumber)q;
            return string.Compare(this.numeroTarjeta, other.numeroTarjeta) < 0;
        }


        public override string ToString()
        {
            return $"Tarjeta: {numeroTarjeta}, Tipo: {tipo}, Saldo Actual: {saldoActual}, Limite de Credito: {limiteCredito}, CVV: {cvv}, PIN: {pin}, Fecha de Expiracion: {fechaExpiracion}";
        }

        public CardsNumber clone()
        {
            return new CardsNumber
            {
                numeroTarjeta = this.numeroTarjeta,
                pin = this.pin,
                saldoActual = this.saldoActual,
                limiteCredito = this.limiteCredito,
                cvv = this.cvv,
                fechaExpiracion = this.fechaExpiracion,
                tipo = this.tipo,
                estado = this.estado,
            };
        }


    }
}
