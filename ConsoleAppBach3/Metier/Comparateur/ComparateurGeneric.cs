using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppBach3.Metier.Comparateur
{
    class ComparateurGeneric<T> : IComparer<T>
    {
        private string[] champs;
        public ComparateurGeneric(params string[] champs)
        {
            this.champs = champs;
        }

        public int Compare(T x, T y)
        {
            int resultat = 0;
            foreach (var ch in this.champs)
            {
                if (typeof(T).GetProperty(ch) != null)
                {
                    if (typeof(T).GetProperty(ch).PropertyType.Name is IComparable)
                    {

                        IComparable valeurX = (IComparable)typeof(T).GetProperty(ch).GetValue(x);
                        IComparable valeurY = (IComparable)typeof(T).GetProperty(ch).GetValue(y);
                        resultat = valeurX.CompareTo(valeurY);
                    }
                    else
                        throw new InvalidCastException($"Le type {typeof(T).GetProperty(ch).PropertyType.Name} n'implémente pas IComparable");

                    #region switch
                    /*switch (ch)
                    {
                        case "nom":
                            resultat = x.Nom.CompareTo(y.Nom);
                            break;
                        case "prenom":
                            resultat = x.Prenom.CompareTo(y.Prenom);
                            break;
                        case "mail":
                            resultat = x.Mail.CompareTo(y.Mail);
                            break;
                        case "datenaissance":
                            resultat = x.DateNaissance.CompareTo(y.DateNaissance);
                            break;
                    }*/
                #endregion
                    if (resultat != 0)
                        return resultat;
                }
                else
                    throw new ArgumentException($"Le champ {ch} n'éxiste pas.");
            }
            return 0;
        }
    }
}
