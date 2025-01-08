using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TP3;

namespace TP3
{
    public class VolCharter : Vol
    {
        // declaration des attibuts, methodes d'accès  et de modification
        public Avion InfoAvion { get; set; }
        public bool ServiceBar { get; set; }
        public bool Divertissement { get; set; }
        public bool ServicesPayant { get; set; }
        public bool PriseAlimentation { get; set; }
        public  bool Wifi { get; set; }

        // declaration de constructeur
        [JsonConstructor]
        public VolCharter(int numeroVol, string destination, Date dateDepart, int totalReservation)
            : base(numeroVol, destination, dateDepart, totalReservation)
        {
        }

       
        public VolCharter(int numeroVol, string destination, Date dateDepart, int totalReservation, Avion typeAvion, 
            bool serviceBar, bool divertissement, bool servicesPayants, bool priseAlimentation, bool wifi) 
            : base(numeroVol, destination, dateDepart, totalReservation)
        {
            this.InfoAvion= typeAvion;
            this.ServiceBar= serviceBar;
            this.Divertissement= divertissement;
            this.ServicesPayant= servicesPayants;
            this.PriseAlimentation= priseAlimentation;
            this.Wifi= wifi;
        }

        public override string ToString()
        {
            return base.ToString() + this.InfoAvion.ToString();
        }


    }
}
