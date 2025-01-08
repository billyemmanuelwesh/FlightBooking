using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TP3
{
    //Attributs permettant de serialiser les classes filles de vol
    [JsonDerivedType(typeof(VolBasPrix), "VolBasPrix")]
    [JsonDerivedType(typeof(VolCharter), "VolCharter")]
    [JsonDerivedType(typeof(VolPrive), "VolPrive")]
    [JsonDerivedType(typeof(VolRegulier), "VolRegulier")]
    public class Vol
    {
        // declaration des attributs d'instance
        private int numeroVol;
        private String destination;
        private Date dateDepart;
        private int totalReservation;

        // declaration des methodes d'accès  et de modification
        public int NumeroVol
        {
            get { return numeroVol; }
        }
        public String Destination
        {
            get { return destination; }
        }
        public Date DateDepart
        {
            get { return dateDepart; }
            set { dateDepart = value; }
        }
        public int TotalReservation
        {
            get { return totalReservation; }
            set { totalReservation = value; }
        }

        // declaration du constructeur
        [JsonConstructor]
        public Vol(int numeroVol, String destination, Date dateDepart, int totalReservation)
        {
            this.numeroVol = numeroVol;
            this.destination = destination;
            this.dateDepart = dateDepart;
            this.totalReservation = totalReservation; 
        }


        //  redefinition de la methode ToString
        public override String ToString()
        {
            return String.Format("{0}\t{1}\t{2}\t{3}", this.numeroVol, this.destination.PadRight(15).Substring(0,15), this.dateDepart.ToString(), this.totalReservation);
        }


        //  pour pourvoir afficher les vols de la classe de base et les classe enfants tous dans un même format quand on liste tous les films
        public String AfficherInfosDeBase()
        {
            return String.Format("\t\t\t\t\t{0}\t{1}\t{2}\t{3}", this.numeroVol, this.destination.PadRight(15).Substring(0, 15), this.dateDepart.ToString(), this.totalReservation);
        }

    }
}
