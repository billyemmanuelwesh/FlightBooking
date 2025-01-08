using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TP3;

namespace TP3
{
    public class VolPrive : Vol
    {
        // declaration des attibuts, methodes d'accès  et de modification
        public Avion InfoAvion { get; set; }
        public bool RepasFourni { get; set; }
        public bool ServiceBar { get; set; }
        public bool Divertissement { get; set; }
        public bool PriseAlimentation { get; set; }
        public bool Wifi { get; set; }

        [JsonConstructor]
        public VolPrive(int numeroVol, string destination, Date dateDepart, int totalReservation)
            : base(numeroVol, destination, dateDepart, totalReservation)
        { 
        }

       
        // declaration de constructeur
        public VolPrive(int numeroVol, string destination, Date dateDepart, int totalReservation, Avion typeAvion,
               bool repasFourni, bool serviceBar, bool divertissement, bool priseAlimentation, bool wifi)
               : base(numeroVol, destination, dateDepart, totalReservation)
        {
            this.InfoAvion = typeAvion;
            this.RepasFourni = repasFourni;
            this.ServiceBar = serviceBar;
            this.Divertissement = divertissement;
            this.PriseAlimentation = priseAlimentation;
            this.Wifi = wifi;
        }

        public override string ToString()
        {
            return base.ToString() + this.InfoAvion.ToString();
        }
    }
}
