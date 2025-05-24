using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.enums;
using Utils.EstructurasDeDatos.Interfaces;
using Utils.models;

namespace Utils.dtos
{
    public class ClientsDPI : Cliente, Comparador
    {

        bool Comparador.igualQue(object obj)
        {
            if (obj == null || !(obj is ClientsDPI)) return false;
            ClientsDPI other = (ClientsDPI)obj;
            return this.dpi.Equals(other.dpi);
        }

        bool Comparador.mayorIgualQue(object obj)
        {
            ClientsDPI other = (ClientsDPI)obj;
            return string.Compare(this.dpi, other.dpi) >= 0;
        }

        bool Comparador.mayorQue(object obj)
        {
            ClientsDPI other = (ClientsDPI)obj;
            return string.Compare(this.dpi, other.dpi) > 0;
        }

        bool Comparador.menorIgualQue(object obj)
        {
            ClientsDPI other = (ClientsDPI)obj;
            return string.Compare(this.dpi, other.dpi) <= 0;
        }

        bool Comparador.menorQue(object obj)
        {
            ClientsDPI other = (ClientsDPI)obj;
            return string.Compare(this.dpi, other.dpi) < 0;
        }
    }
}
