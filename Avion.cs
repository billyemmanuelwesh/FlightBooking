using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TP3
{
    public class Avion
    {
        private int typeAvion; // Boeing_400 (1), Boeing450 (2), Airbus_10 (3), Airbus_20 (4)
        private int nombreDePlace;
        private int rayonAction; // court-courier(1), moyen-courier(2), long-courier(3)
        private int categorie; // AvionAffaire(1), AvionLeger(2), AvionUltraLeger(3)
        private int typeClasse; // premiereClasse(1), classeAffaire(2), classeEconomique(3)

        // creation de tableaux pour stocker les differente valeurs que peut prendre chaque attribut
        // on pourra afficher (avec toString) ces valeurs en utilisant leur numero d'indice, puisqu'à l'enregistrement d'un vol on rentrera le chiffre associe a l'indice .
        public static String[] typesAvion = new String[] { "", "BOEING_4", "BOEING_5", "AIRBUS_1", "AIRBUS_2" };
        public static String[] rayonsAction = new String[] { "", "court_c", "moyen_c", "long_c"};
        public static String[] categories = new String[] { "", "affaire", "léger", "ult_léger"};
        public static String[] classes = new String[] { "", "première", "affaire", "écono."};
        
        public int TypeAvion
        {
            get { return typeAvion; }
            set { typeAvion = value; }
        }
        public int NombreDePlace
        {
            get { return nombreDePlace; }
            set { nombreDePlace = value; }
        }
        public int RayonAction
        {
            get { return rayonAction; }
            set { rayonAction = value; }
        }
        public int Categorie
        {
            get { return categorie; }
            set { categorie = value; }
        }
        public int TypeClasse
        {
            get { return typeClasse; }
            set { typeClasse = value; }
        }

        [JsonConstructor]
        public Avion(int typeAvion, int nombreDePlace, int rayonAction, int categorie, int typeClasse)
        {
            this.typeAvion = typeAvion;
            this.nombreDePlace = nombreDePlace;
            this.rayonAction = rayonAction;
            this.categorie = categorie;
            this.typeClasse = typeClasse;
        }
        public Avion()
        {
        }
        public override string ToString()
        {
            return String.Format("\t{0}\t{1}\t\t{2}\t\t{3}\t{4}",
              typesAvion[this.TypeAvion], this.NombreDePlace, rayonsAction[this.RayonAction], categories[this.Categorie], classes[this.TypeClasse]);
        }
    }
}
