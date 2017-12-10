using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;

namespace ConsoleAppBach3.Metier
{
    /// <summary>
    /// Class qui représente Contact
    /// </summary>
    /// <seealso cref="IComparable{T}"/>
    [Table("Contact")]
    public class Contact : IComparable<Contact>
    {
        private const char SEPARATEUR = ';';

        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.IDENTITY)]

        private string nom;
        /// <summary>
        /// Nom du contact
        /// </summary>
        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value.ToUpper(); }
        }

        public string Prenom { get; set; }

        private string mail;

        /// <summary>
        /// Mail du contact
        /// </summary>
        /// <exception cref="ArgumentException">Si le format du mail est incorrect.</exception>
        public string Mail
        {
            get { return mail; }
            set {
                Regex reg =  new Regex(@"^[a-zA-Z0-9.-_]+@{1,1}[a-zA-Z0-9.-_]{2,}\.[a-zA-Z0-9.]{2,6}$");
                if (reg.IsMatch(value))
                    mail = value;
                else
                    throw new ArgumentException($"La valeur {value} n'est pas une adresse mail.");
            }
        }

        public DateTime DateNaissance { get; set; }


        public override string ToString()
        {
            return $"{this.nom}{SEPARATEUR}{this.Prenom}{SEPARATEUR}{this.mail}{SEPARATEUR}{this.DateNaissance.ToString("dd/MM/yyyy")}";
            //return base.ToString();
        }

        public static List<Contact> Lister()
        {
            List<Contact> list = new List<Contact>();

            IEnumerable<string> lignesFichier = File.ReadLines(ConfigurationManager.AppSettings["FICHIER"], Encoding.UTF8);
            foreach (var ligne in lignesFichier)
            {
                string[] tab = ligne.Split(SEPARATEUR);
                Contact c = new Contact
                {
                    Nom = tab[0],
                    Prenom = tab[1],
                    Mail = tab[2],
                    DateNaissance = Convert.ToDateTime(tab[3])
                };
                list.Add(c);
            }

            return list;
        }

        public void Enregistrer()
        {
            string nomFichier = ConfigurationManager.AppSettings["FICHIER"];
            using (StreamWriter sw = new StreamWriter(nomFichier, true, Encoding.UTF8))
            {
                sw.WriteLine(this.ToString());
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Contact other)
        {
            return this.Nom.CompareTo(other.Nom) != 0 ?
                this.Nom.CompareTo(other.Nom) :
                this.Prenom.CompareTo(other.Prenom);
        }
    }
}
