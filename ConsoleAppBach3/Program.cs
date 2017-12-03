using ConsoleAppBach3.Metier;
using ConsoleAppBach3.Metier.Comparateur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppBach3.Outils;
using System.Xml.Linq;

namespace ConsoleAppBach3
{
    class Program
    {

        //private static DateTime debut;
        //private static DateTime fin;

        static void Main(string[] args)
        {
           
            while (true)
            {
                afficherMenu();
                Console.WriteLine("Votre choix :");
                string choix = Console.ReadLine();
                switch (choix.ToLower())
                {
                    case "1":
                        ajouterContact();
                        break;
                    case "2":
                        afficherContact();
                        break;
                    case "3":
                        trierContacts();
                        break;
                    case "4":
                        trierContactsParDate();
                        break;
                    case "5":
                            triGeneric();
                        break;
                    case "6":
                        extension();
                        break;
                    case "7":
                        rechercheContactParNom();
                        break;
                    case "8":
                        rechercheContactParDate();
                        break;
                    case "9":
                        linq();
                        break;
                    case "10":
                        TPLinq();
                        break;
                    case "q":
                        return;

                    default:
                        Console.WriteLine("Erreur dans le choix");
                        break;
                }
            }

        }

        private static void TPLinq()
        {
            Console.WriteLine("Date début ?");
            DateTime debut = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("Date fin ?");
            DateTime fin = Convert.ToDateTime(Console.ReadLine());

            List<Contact> list = Contact.Lister();
            XDocument doc = XDocument.Load(@"c:\contacts.xml");
            IEnumerable<XElement> elements = doc.Root.Elements();

            var resultat = from c in list
                           join e in elements
                           on c.Mail equals e.Attribute("email").Value
                           where c.DateNaissance >= debut && c.DateNaissance <= fin
                           orderby c.Nom
                           select new
                           {
                               c.Prenom,
                               c.Nom,
                               Rue = e.Element("Adresse").Element("Rue").Value,
                               CodePostal = e.Element("Adresse").Element("Cp").Value,
                               Ville = e.Element("Adresse").Element("Ville").Value
                           };

            var resultatBis = list.Where(c => c.DateNaissance >= debut && c.DateNaissance <= fin)
                                    .Join(elements,
                                        c => c.Mail,
                                        e => e.Attribute("email").Value,
                                        (c, e) => new
                                        {
                                            c.Prenom,
                                            c.Nom,
                                            Rue = e.Element("Adresse").Element("Rue").Value,
                                            CodePostal = e.Element("Adresse").Element("Cp").Value,
                                            Ville = e.Element("Adresse").Element("Ville").Value
                                        });

            foreach (var item in resultat)
            {
                Console.WriteLine(item.Nom + " " + item.Ville);
            }

        }

        private static void linq()
        {

            List<Contact> list = Contact.Lister();
            /*list.OrderBy(delegate (Contact c)
            {
                return c.Prenom;
            });*/
            var resultat = list.Where(c => c.Nom.Contains("T")).OrderBy(c => c.Prenom);

            var resultatBis = from c in list
                              where c.Nom.Contains("T")
                              orderby c.Prenom
                              select c;
        }

        private static void rechercheContactParDate()
        {
            Console.WriteLine("Date début ?");
            DateTime debut = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("Date fin ?");
            DateTime fin = Convert.ToDateTime(Console.ReadLine());

            List<Contact> list = Contact.Lister();
            //List<Contact> resultat = list.FindAll(methodeRechercheParDate);
            List<Contact> resultat = list.FindAll(delegate (Contact c)
            {
                return c.DateNaissance >= debut && c.DateNaissance <= fin;
            });

            foreach (var item in resultat)
            {
                Console.WriteLine(item.ToString());
            }
        }

        /*private static bool methodeRechercheParDate(Contact c)
        {
            return c.DateNaissance >= debut && c.DateNaissance <= fin;
        }*/

        private static void rechercheContactParNom()
        {
            List<Contact> list = Contact.Lister();
            List<Contact> resultat = list.FindAll(methodeDeRecherche);
            foreach (var item in resultat)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static bool methodeDeRecherche(Contact c)
        {
            return c.Nom.Contains("T");
        }

        private static void extension()
        {            
            DateTime dt = DateTime.Now;
            //Extension.PremierJourSemaine(dt);
            Console.WriteLine(  dt.PremierJourSemaine());
        }

        private static void triGeneric()
        {
            List<Contact> list = Contact.Lister();
            ComparateurGeneric<Contact> comp = new ComparateurGeneric<Contact>("Mail", "DateNaissance");
            list.Sort(comp);
            foreach (var item in list)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static void trierContactsParDate()
        {
            List<Contact> list = Contact.Lister();
            ComparateurContactDate comp = new ComparateurContactDate();
            list.Sort(comp);
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Prenom} {item.Nom} - {item.DateNaissance}");
            }
        }

        private static void trierContacts()
        {
            List<Contact> list = Contact.Lister();
            list.Sort();
            
            
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Prenom} {item.Nom}");
            }
        }
        
        

        private static void afficherContact()
        {
            List<Contact> list = Contact.Lister();
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Prenom} {item.Nom}");
            }
        }

        private static void ajouterContact()
        {
            Contact c = new Contact();
            Console.WriteLine("Nom ?");
            c.Nom = Console.ReadLine();

            Console.WriteLine("Prénom ?");
            c.Prenom = Console.ReadLine();

            Console.WriteLine("Mail ?");
            c.Mail = Console.ReadLine();
            while (true)
            {
                try
                {
                    Console.WriteLine("Date de naissance ?");
                    c.DateNaissance = Convert.ToDateTime(Console.ReadLine());
                    break;
                }catch(Exception e)
                {
                    Console.WriteLine($"Erreur : {e.Message}");
                }
            }
            c.Enregistrer();
            //Console.WriteLine(c.ToString());
        }

        private static void afficherMenu()
        {
            Console.WriteLine("-- MENU --");
            Console.WriteLine("1- Ajouter un contact");
            Console.WriteLine("2- Afficher les contacts");
            Console.WriteLine("3- Trier les contacts");
            Console.WriteLine("4- Trier les contacts par date");
            Console.WriteLine("5- Tri généric");
            Console.WriteLine("6- Méthode d'extension");
            Console.WriteLine("7- Recherche par nom");
            Console.WriteLine("8- Recherche par dates");
            Console.WriteLine("9- Linq");
            Console.WriteLine("10- TPLinq");
            Console.WriteLine("Q- Quitter");
        }
    }
}
