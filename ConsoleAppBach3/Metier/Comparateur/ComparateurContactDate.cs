using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppBach3.Metier.Comparateur
{
    class ComparateurContactDate : IComparer<Contact>
    {
        public int Compare(Contact x, Contact y)
        {
            
            return x.DateNaissance.CompareTo(y.DateNaissance);
        }


        public string ChampDeComparaison { get; set; }
    }
}
