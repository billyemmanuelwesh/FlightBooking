using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TP3
{
    public class Date
    {
        // declaration des attributs d'instance
        private int jour;
        private int mois;
        private int annee;

        // declaration des methodes d'accès et de modification
       
        public int Jour
        {
            get { return jour; }
            private set { jour = value; }
        }
        public int Mois
        {
            get { return mois; }
            private set { mois = value; }
        }
        public int Annee
        {
            get { return annee; }
            private set { annee = value; }
        }

        // declaration du constructeur a trois parametres
        [JsonConstructor]
        public Date(int jour, int mois, int annee)
        {
            this.Jour = jour;
            this.Mois = mois;
            this.Annee = annee;
        }

       
        // redefinition de la methode ToString
        public override String ToString()
        {
            return String.Format("{0:00}/{1:00}/{2:00}",this.jour, this.mois, this.annee);
        }

        // methode pour verifier et valider les dates rentrer par l'utilisateur
        public bool VerifierEntrerDate()
        {
         
            if (mois > 12 || mois < 1)
            {
                Console.WriteLine("Incorrecte, le mois doit etre un chiffre de 1 à 12");
                return false;
            }
            if (jour < 1)
            {
                Console.WriteLine("Incorrecte, le jour ne peut pas etre inferieur à 1");
                return false;
            }
            if (annee < 1)
            {
                Console.WriteLine("Incorrecte, l'année ne peut pas etre inferieur à 1");
                return false;
            }
            //mois de 31 jours
            if (mois == 1 || mois == 3 || mois == 5 || mois == 7 || mois == 8 || mois == 10 || mois == 12)
            {
                if (jour > 31 || jour < 1)
                {
                    Console.WriteLine("Incorrecte, le jour doit être supérieur à 1 et inférieur à 32");
                    return false;
                }
            }
            //mois de fevrier
            else if (mois == 2)
            {
                if (DateTime.IsLeapYear(annee)) // si c'est un année bissextile
                {
                    if (jour > 29 || jour < 1)
                    {
                        Console.WriteLine("Incorrecte, le jour doit être supérieur à 1 et inférieur 30");
                        return false;
                    }
                }
                else // si l'année n'est pas bissextile
                {
                    if (jour > 28 || jour < 1)
                    {
                        Console.WriteLine("Incorrecte, le jour doit être supérieur à 1 et inférieur à 29");
                        return false;
                    }
                }
            }
            //mois de 30 jours
            else
            {
                if (jour > 30 || jour < 1)
                {
                    Console.WriteLine("Incorrecte, le jour doit être supérieur à 1 et inférieur à 31");
                    return false;
                }
            }
            return true;
        }
    }
}
