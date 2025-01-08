using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TP3;

namespace TP3
{
    public class VolBasPrix : Vol
    {
        // declaration des attibuts, methodes d'accès  et de modification
        public Avion InfoAvion { get; set; }
        public bool ServicesPayant { get; set; }
        public bool PriseAlimentation { get; set; }


        // declaration de constructeur
        [JsonConstructor]
        public VolBasPrix(int numeroVol, string destination, Date dateDepart, int totalReservation)
            : base(numeroVol, destination, dateDepart, totalReservation)
        {
        }


        public VolBasPrix(int numeroVol, string destination, Date dateDepart, int totalReservation, 
               Avion typeAvion, bool servicesPayants, bool priseAlimentation)
               : base(numeroVol, destination, dateDepart, totalReservation)
        {
            this.InfoAvion = typeAvion;
            this.ServicesPayant = servicesPayants;
            this.PriseAlimentation = priseAlimentation;
        }

        public override string ToString()
        {
            return base.ToString() + this.InfoAvion.ToString();
        }
    }
}
