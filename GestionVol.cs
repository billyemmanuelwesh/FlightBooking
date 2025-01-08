/* 
   Nom du projet : TP3.GestionVol
   Auteurs :  Billy Emmanuel WESH / IFT 1179 / Hiver 2023
   Matricule : 20227876
    
       Objectifs
     Composition d’objets
     Héritage
     Interfaces
     Fichiers Objects
     Classe abstraites
     Conteneurs
   
	Derniere mise a jour : 05-04-2023
*/

using TP3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace TP3
{
    public class GestionVol
    { 
        // declaration des constantes de la classe
        public const int MAX_PLACES = 340; // nombre de place d'un avion
         
        //declaration d'attribut 
        static int noChoix = 0;

        // dictionary de Vol vide
        Dictionary<int, Vol> ListeDictionaryVols = new Dictionary<int, Vol>();

        String nomCompagnie = "";

        // methode pour afficher le menu, avec le nom de la compagnie
        void AffficherMenu()
        {
            do
            {
                Console.WriteLine("GESTION DES VOLS DE LA COMPAGNIE {0}\n",nomCompagnie);
                Console.WriteLine("1.Liste des vols");
                Console.WriteLine("2.Ajout d'un vol");
                Console.WriteLine("3.Retrait d'un vol");
                Console.WriteLine("4.Modification de la date de départ");
                Console.WriteLine("5.Réservation d'un vol");
                Console.WriteLine("0.Terminer");
                // pour demander a l'utilisateur de faire un choix et en recuperer la valeur
                Console.WriteLine("\t\tFaites votre choix:");
                String choixTempo = Console.ReadLine();
                if (choixTempo != null)
                {
                    noChoix = Int32.Parse(choixTempo);
                }
                // on parcour le choix recuperer, dependement de sa valeur on appelle une methode
                switch (noChoix)
                {
                    case 1: ListeVols("CieAirRelax"); break;
                    case 2: InsererVol(); break;
                    case 3: RetirerVol(); break;
                    case 4: ModifierDate(); break;
                    case 5: ReserverVol(); break;
                    case 0: EcrireFichier("CieAirRelax.obj"); break;
                    default:
                        Console.WriteLine("Désolé! Veuillez rentrer un chiffre entre 0 et 5");
                        break;
                }
                Console.WriteLine("\n\n\n");
            } while (noChoix != 0);

        }


        // methode pour ouvrir, lire le fichier et remplir le tableau tabVol d'objet Vol
        void ChargerVol()
        {
            string ligne;
            string monFichierTxt = String.Empty;
            string monFichierObj = String.Empty;

            // on recupere tous les fichiers du repertoire de l'application
            foreach (String fileName in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                // si on trouve un fichier terminant par .txt alors:
                if (fileName.EndsWith(".txt"))
                {
                    // on utilise ce fichier et on recupere le nom de la compagnie
                    monFichierTxt = fileName;
                    nomCompagnie = Path.GetFileName(fileName);
                }
                if (fileName.EndsWith(".obj"))
                {
                    // on utilise ce fichier et on recupere le nom de la compagnie
                    monFichierObj = fileName;
                    nomCompagnie = Path.GetFileName(fileName);
                }
            }

            if (monFichierObj != String.Empty)
            {
                // s'apprete a lire dans un ficher
                StreamReader fichierObj = new StreamReader(monFichierObj);
                String json = fichierObj.ReadToEnd();
                // converti un string JSON en un dictionary de vol
                ListeDictionaryVols = JsonSerializer.Deserialize<Dictionary<int, Vol>>(json);
                fichierObj.Close();
            }
            else if (monFichierTxt != String.Empty)
            {
                // s'apprete a lire dans un ficher
                StreamReader fichier = new StreamReader(monFichierTxt);

                while ((ligne = fichier.ReadLine()) != null)
                {
                    // decoupage des valeurs du fichier sous forme se sous chaine,
                    var sousChaine1 = ligne.Substring(0, 5);
                    var sousChaine2 = ligne.Substring(6, 18).Trim();
                    var sousChaine4 = ligne.Substring(27, 10).Trim();
                    var sousChaine3 = ligne.Substring(38, ligne.Length - 38).Trim();
                    var jourChaine = sousChaine4.Substring(0, 2);
                    var moisChaine = sousChaine4.Substring(3, 2);
                    var anneeChaine = sousChaine4.Substring(6, 4);

                    // conversion de string en entier , sauf pour destination
                    var numeroSerie = int.Parse(sousChaine1);
                    var destination = sousChaine2;
                    var reservations = int.Parse(sousChaine3);
                    var leJour = int.Parse(jourChaine);
                    var leMois = int.Parse(moisChaine);
                    var lAnnee = int.Parse(anneeChaine);

                    var dateDepart = new Date(leJour, leMois, lAnnee);

                    // ajout de nouvels objets vol au dictionary vol de vol

                    var unObjVol = new Vol(numeroSerie, destination, dateDepart, reservations);
                    ListeDictionaryVols.Add(numeroSerie, unObjVol);
                }
                fichier.Close();
            }
        }
        
        // methode pour inserer un nouveau vol au dictionary vols
        public void InsererVol() 
        {
            Console.WriteLine("SAISI D'UN NOUVEAU VOL\n");
            Console.WriteLine("Quelle catégorie de vol voulez-vous saisir:\n");
            Console.WriteLine("1 : Pour les vols PRIVÉS\n2 : Pour les vols CHARTERS\n" +
                              "3 : Pour les vols RÉGULIERS\n4 : Pour les vols BAS PRIX\n");

            String rep = Console.ReadLine();
            if (int.Parse(rep) >= 1 && int.Parse(rep) <= 4)
            {
                Console.WriteLine("Numéro du vol : ");
                String repNumVolStr = Console.ReadLine();
                if (repNumVolStr.Length == 5) // si le numero de vol comtient 5 caracteres
                {
                    int repNumVol = int.Parse(repNumVolStr);
                    if (!RechercherVol(repNumVol)) // si on ne trouve pas le numero de vol, on va le creer
                    {
                        Console.WriteLine("Entrez la DESTINATION:");
                        String repDestination = Console.ReadLine();
                        if (repDestination.Length <= 20)
                        {
                            bool estValide;
                            bool continuer = false;
                            do
                            {
                                Console.WriteLine("Entrer la date, format date (jj/MM/AAAA) : ");
                                String repNouvDate = Console.ReadLine();

                                // on extrait separement le jour, le mois et l'annee

                                var maDate = ConvertirEnDate(repNouvDate);
                                estValide = maDate.VerifierEntrerDate();
                                //TODO: modifier les string pour passer des int a la fonction
                                if (estValide)
                                {
                                    // creation d'un nouveau vol ( nbr de reservation 0)

                                    switch (int.Parse(rep))
                                    {
                                        case 1:
                                            VolPrive monvolPrive = new VolPrive(repNumVol, repDestination, maDate, 0);
                                            RemplirPrive(monvolPrive);
                                            ListeDictionaryVols.Add(monvolPrive.NumeroVol, monvolPrive);
                                            Console.WriteLine("\n\nLe vol prive {0} réservation(s) a été inséré avec succès!\n", monvolPrive);
                                            break;
                                        case 2:
                                            VolCharter monvolCharter = new VolCharter(repNumVol, repDestination, maDate, 0);
                                            RemplirCharter(monvolCharter);
                                            ListeDictionaryVols.Add(monvolCharter.NumeroVol, monvolCharter);
                                            Console.WriteLine("\n\nLe vol Charter {0} réservation(s) a été inséré avec succès!\n", monvolCharter);
                                            break;
                                        case 3:
                                            VolRegulier monvolRegulier = new VolRegulier(repNumVol, repDestination, maDate, 0);
                                            RemplirRegulier(monvolRegulier);
                                            ListeDictionaryVols.Add(monvolRegulier.NumeroVol, monvolRegulier);
                                            Console.WriteLine("\n\nLe vol Régulier {0} réservation(s) a été inséré avec succès!\n", monvolRegulier);
                                            break;
                                        case 4:
                                            VolBasPrix monvolBasPrix = new VolBasPrix(repNumVol, repDestination, maDate, 0);
                                            RemplirBasPrix(monvolBasPrix);
                                            ListeDictionaryVols.Add(monvolBasPrix.NumeroVol, monvolBasPrix);
                                            Console.WriteLine("\n\nLe vol Bas Prix {0} réservation(s) a été inséré avec succès!\n", monvolBasPrix);
                                            break;
                                        default:
                                            Console.WriteLine("Désolé! Veuillez rentrer un chiffre de 1 et 4");
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("La date n'a pas été ajouté car elle contenait une erreur!");
                                    Console.WriteLine("Voulez-vous continuer (O-Oui autre pour - Non ? ");
                                    continuer = Console.ReadLine().Trim().ToUpper()[0] == 'O';
                                }
                            } while (!estValide && continuer);
                        }
                        else
                        {
                            Console.WriteLine("Veuillez entrez au plus 20 caractères");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Désolé, ce vol existe déja");
                    }
                }
                else
                {
                    Console.WriteLine("Désolé, le numéro de vol doit contenir 5 chiffres");
                }
            }
            else
            {
                Console.WriteLine("Désolé, Entrez un chiffre de 1 à 4");
            }
        }

        public void RemplirAvion(Avion avion)
        {
            Console.WriteLine(" Entrez le Type d'avion :\n1 : BOEING_400\n2 : BOEING_450\n3 : AIRBUS_10\n4 : AIRBUS_20\n");
            avion.TypeAvion = Int32.Parse(Console.ReadLine());
            // le nombre de place par defaut des avions est de 340
            avion.NombreDePlace = MAX_PLACES;
            Console.WriteLine("Entrez le Rayon d'action de l'avion :\n1 : COURT-COURRIER\n2 : MOYEN-COURRIER\n3 : LONG-COURRIER\n");
            avion.RayonAction = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Entrez la Catégorie de l'avion :\n1 : AVION AFFAIRE\n2 : AVION LÉGER\n3 : AVION ULTRA LÉGER\n");
            avion.Categorie = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Classe de l'avion :\n1 : PREMIÈRE CLASSE\n2 : CLASSE AFFAIRE\n3 : CLASSE ECONOMIQUE\n");
            avion.TypeClasse = Int32.Parse(Console.ReadLine());
        }



        public void RemplirPrive(VolPrive volPrive)
        {
            volPrive.InfoAvion = new Avion();
            RemplirAvion(volPrive.InfoAvion);

            //remplir les informations des methodes de vol prive
            Console.WriteLine("Service Bar (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volPrive.ServiceBar = true;
            else
                volPrive.ServiceBar = false;

            Console.WriteLine("Repas Fourni (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volPrive.RepasFourni = true;
            else
                volPrive.RepasFourni = false;

            Console.WriteLine("Divertissement (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volPrive.Divertissement = true;
            else
                volPrive.Divertissement = false;

            Console.WriteLine("Prise d'alimentation (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volPrive.PriseAlimentation = true;
            else
                volPrive.PriseAlimentation = false;

            Console.WriteLine("Wifi (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volPrive.Wifi = true;
            else
                volPrive.Wifi = false;
        }
        public void RemplirCharter(VolCharter volCharter)
        {
            volCharter.InfoAvion = new Avion();
            RemplirAvion(volCharter.InfoAvion);

            //remplir les informations des methodes de vol prive
            Console.WriteLine("Service Bar (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volCharter.ServiceBar = true;
            else
                volCharter.ServiceBar = false;

            Console.WriteLine("Divertissement (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volCharter.Divertissement = true;
            else
                volCharter.Divertissement = false;

            Console.WriteLine("Services payant (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volCharter.ServicesPayant = true;
            else
                volCharter.ServicesPayant = false;

            Console.WriteLine("Prise d'alimentation (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volCharter.PriseAlimentation = true;
            else
                volCharter.PriseAlimentation = false;

            Console.WriteLine("Wifi (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volCharter.Wifi = true;
            else
                volCharter.Wifi = false;
        }

        public void RemplirRegulier(VolRegulier volRegulier)
        {
            volRegulier.InfoAvion = new Avion();
            RemplirAvion(volRegulier.InfoAvion);

            //remplir les informations des methodes de vol prive
            Console.WriteLine("Service Bar (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volRegulier.RepasFourni = true;
            else
                volRegulier.RepasFourni = false;

            Console.WriteLine("Service Bar (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volRegulier.ServiceBar = true;
            else
                volRegulier.ServiceBar = false;

            Console.WriteLine("Divertissement (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volRegulier.Divertissement = true;
            else
                volRegulier.Divertissement = false;

            Console.WriteLine("Prise d'alimentation (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volRegulier.PriseAlimentation = true;
            else
                volRegulier.PriseAlimentation = false;

            Console.WriteLine("Service Bar (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volRegulier.ReserverSiege = true;
            else
                volRegulier.ReserverSiege = false;

            Console.WriteLine("Service Bar (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volRegulier.ServicePayant = true;
            else
                volRegulier.ServicePayant = false;

        }

        public void RemplirBasPrix(VolBasPrix volBasPrix)
        {
            volBasPrix.InfoAvion = new Avion();
            RemplirAvion(volBasPrix.InfoAvion);

            //remplir les informations des methodes de vol prive
            Console.WriteLine("Prise d'alimentation (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volBasPrix.PriseAlimentation = true;
            else
                volBasPrix.PriseAlimentation = false;

            Console.WriteLine("Service Bar (O - pour Oui ,  N - pour Non");
            if (Console.ReadLine() == "O")
                volBasPrix.ServicesPayant = true;
            else
                volBasPrix.ServicesPayant = false;
        }

        // methode pour rechercher  un  vol 
        public bool RechercherVol(int numVol)
        {
            foreach (var item in ListeDictionaryVols)
            {
                if (numVol == item.Key)
                {
                    return true;
                }
            }
            return false;
        }

        // methode pour retirer un  vol 
        public void RetirerVol()
        {
            Console.WriteLine("RETIRER UN VOL\n");
            Console.WriteLine("Numéro du vol:");
            String repNumVolStr = Console.ReadLine();

            int repNumVol = int.Parse(repNumVolStr);

            if (RechercherVol(repNumVol)) // si le numero de vol existe, on fera ce qui suit
            {

                Console.WriteLine("Désirez-vous vraiment retirer ce vol (O/N) ?");
                String ouiOuNon = Console.ReadLine().ToUpper();

                switch (ouiOuNon)
                {
                    case "O":
                        {
                            ListeDictionaryVols.Remove(repNumVol);
                            Console.WriteLine("Le vol {0} a été supprimé avec succès!\n", repNumVol);
                        }
                        break;
                    case "N": Console.WriteLine("Vous avez changé d'avis, pas de suppression"); break;
                    default: Console.WriteLine("Erreur! Veillez rentrer O pour OUI et N pour NON"); break;
                }
            }
            else
            {
                Console.WriteLine("Désolé, ce numéro de vol n'existe pas dans la liste");
            }
        }

        // methode pour modifier la date d'un vol 
        public void ModifierDate()
        {
            Console.WriteLine("MODIFICATION DATE DE DEPART\n");
            Console.WriteLine("Numéro du vol:");
            String repNumVolStr = Console.ReadLine();
            int repNumVol = int.Parse(repNumVolStr);
            if (RechercherVol(repNumVol)) // si le numero de vol existe, on peut modifier ce dernier
            {
                foreach (var item in ListeDictionaryVols)
                {
                    if (repNumVol == item.Key)
                    {

                        // affiche la destination,la date de depart et demande a l'utilisateur de renter une date
                        Console.WriteLine("Destination : {0}\nDate de départ : {1}\n", item.Value.Destination, item.Value.DateDepart);
                        bool estValide;
                        bool continuer = false;
                        do
                        {
                            Console.WriteLine("Entrer la nouvelle date, format date (jj/MM/AAAA) : ");
                            String repNouvDate = Console.ReadLine();

                            // ConvertirEnDate extrait separement le jour, le mois et l'annee et les retournent en format Date
                            // on remplace l'ancienne date par la nouvelle
                            var maDate = ConvertirEnDate(repNouvDate);
                            estValide = maDate.VerifierEntrerDate();
                            if (estValide)
                            {
                                item.Value.DateDepart = maDate;
                                Console.WriteLine("La date a été modifiée avec succès!\n");
                            }
                            else
                            {
                                Console.WriteLine("La date n'a pas été modifié car elle contenait une erreur!\n");
                                Console.WriteLine("Voulez-vous continuer (O-Oui autre pour - Non ? ");
                                continuer = Console.ReadLine().Trim().ToUpper()[0] == 'O';
                            }                          
                        } while (!estValide && continuer);
                    }
                }
            }
            else
            {
                Console.WriteLine("Désolé, ce numéro de vol n'existe pas dans la liste");
            }
        }

        // methode pour convertir les valeurs rentrées par l'utilisateur, en Date
        public Date ConvertirEnDate(String repNouvDate)
        {
            // on extrait separement le jour, le mois et l'annee
            var j = repNouvDate.Substring(0, 2);
            var m = repNouvDate.Substring(3, 2);
            var a = repNouvDate.Substring(6, repNouvDate.Length - 6);

            var aVerif = a.Length;
            int leJour = int.Parse(j);
            int leMois = int.Parse(m);
            int lAnne = 0;

            if (aVerif == 4)
            {
                lAnne = int.Parse(a);
            }
            else
            { 
                Console.WriteLine("Incorrecte,l'année doit contenir 4 chiffres");
            }
            Date date = new Date(leJour,leMois,lAnne);
            return date;
        }
      
        // methode pour reserver un vol 
        public void ReserverVol()
        {
            Console.WriteLine("RESERVATION D'UN VOL\n");
            Console.WriteLine("Numéro du vol:");
            String repNumVolStr = Console.ReadLine();
            int repNumVol = int.Parse(repNumVolStr);
            if (RechercherVol(repNumVol)) //si le numero de vol existe.
            {
                foreach (var item in ListeDictionaryVols)
                {
                    int placeRestante;
                    if (repNumVol == item.Key)
                    {
                        if (MAX_PLACES > item.Value.TotalReservation)
                        {
                            // affiche les info du vol

                            Console.WriteLine("Voici les info du vol : ");
                            Console.WriteLine("Destination : {0}\nDate de départ : {1}\nNombre de place restante : {2}",
                             item.Value.Destination, item.Value.DateDepart, placeRestante = MAX_PLACES - item.Value.TotalReservation);

                            // demande a l'utilisateur de renter le nombre de place à reserver.
                            Console.WriteLine("Entrer le nombre de place que le client désire réserver : ");
                            String repPlace = Console.ReadLine();
                            int repNbPlace = int.Parse(repPlace);

                            // on fait la reservation
                            if (item.Value.TotalReservation + repNbPlace > MAX_PLACES)
                            {
                                Console.WriteLine("Désolé, trop de place demandé, pas assez de place");
                            }
                            else
                            {
                                item.Value.TotalReservation += repNbPlace;
                                Console.WriteLine("Vol(s) réservé(s) avec succès!\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Désolé, il ne reste plus de place disponible");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Désolé, ce numéro de vol n'existe pas dans la liste");
            }
        }

        // methode qui fait afficher soit tous les vols existants, soit les Catégories de vols (et le nom de la compagnie).
        public void ListeVols(String nomCompagnie)
        {
            Console.WriteLine("Entrez:\n0 : Pour TOUS les vols\n1 : Pour les vols PRIVÉS\n2 : Pour les vols CHARTERS\n" +
                              "3 : Pour les vols RÉGULIERS\n4 : Pour les vols BAS PRIX");
            String repListe = Console.ReadLine();
 
            switch (int.Parse(repListe))
            {
                case 0:
                    Console.WriteLine("\t\t\t\t\tLISTE DES VOLS DE LA COMPAGNIE {0}\n", nomCompagnie);
                    Console.WriteLine("\t\t\t\t\tNUMÉRO\tDESTINATION\tDATE_DÉPART\tRÉSERVATION");

                    foreach (var item in ListeDictionaryVols)
                    {
                        Console.WriteLine("{0}", item.Value.AfficherInfosDeBase());
                    }
                    Console.WriteLine("\n\n\n");
                    break;
                case 1:
                    Console.WriteLine("\t\t\t\t\tLISTE DES VOLS PRIVÉS DE LA COMPAGNIE {0}\n", nomCompagnie);
                    Console.WriteLine("NUMÉRO\tDESTINATION\tDATE_DÉPART\tNb_RES" +
                        "\tTYP_AV\t\tNB_PLACE\tRAYON\t\tCATÉGORIE\tCLASSE");
                    foreach (var item in ListeDictionaryVols)
                    {
                        if (item.Value is VolPrive)
                        {
                            Console.WriteLine("{0}", ((VolPrive)item.Value).ToString());
                        }
                    }
                    Console.WriteLine("\n\n\n");
                    break;
                case 2:
                    Console.WriteLine("\t\t\t\t\tLISTE DES VOLS CHARTERS DE LA COMPAGNIE {0}\n", nomCompagnie);
                    Console.WriteLine("NUMÉRO\tDESTINATION\tDATE_DÉPART\tNB_RES" +
                        "\tTYP_AV\t\tNB_PLACE\tRAYON\t\tCATÉGORIE\tCLASSE");
                    foreach (var item in ListeDictionaryVols)
                    {
                        if (item.Value is VolCharter)
                        {
                            Console.WriteLine("{0}", ((VolCharter)item.Value).ToString());
                        }
                    }
                    Console.WriteLine("\n\n\n");
                    break;
                case 3:
                    Console.WriteLine("\t\t\t\t\tLISTE DES VOLS RÉGULIERS DE LA COMPAGNIE {0}\n", nomCompagnie);
                    Console.WriteLine("NUMÉRO\tDESTINATION\tDATE_DÉPART\tNB_RES" +
                        "\tTYP_AV\t\tNB_PLACE\tRAYON\t\tCATÉGORIE\tCLASSE");
                    foreach (var item in ListeDictionaryVols)
                    {
                        if (item.Value is VolRegulier)
                        {
                            Console.WriteLine("{0}", ((VolRegulier)item.Value).ToString());
                        }
                    }
                    Console.WriteLine("\n\n\n");
                    break;
                case 4:
                    Console.WriteLine("\t\t\t\t\tLISTE DES VOLS BAS PRIX DE LA COMPAGNIE {0}\n", nomCompagnie);
                    Console.WriteLine("NUMÉRO\tDESTINATION\tDATE_DÉPART\tNB_RES" +
                        "\tTYP_AV\t\tNB_PLACE\tRAYON\t\tCATÉGORIE\tCLASSE");
                    foreach (var item in ListeDictionaryVols)
                    {
                        if (item.Value is VolBasPrix)
                        {
                            Console.WriteLine("{0}", ((VolBasPrix)item.Value).ToString());
                        }
                    }
                    Console.WriteLine("\n\n\n");
                    break;
                default:
                    Console.WriteLine("Désolé! Veuillez rentrer un chiffre de 0 et 4");
                    break;
                }
                
               
        }


        // methode qui ouvre le fichier de la compagnie en ecriture et écrit le tableau d'objet
        public void EcrireFichier(string nomFichier)
        {


            //TEst ajouter manuellement un nouveau vol bas prix
            //ListeDictionaryVols.Add(11111, new VolBasPrix(2342, "test", new Date(2,2,2022), 0, new Avion("test", 340, 0, 0, 0), false, false));


            // converti ton dictionnaire en string JSON
            //string jsonString = JsonSerializer.Serialize(ListeDictionaryVols, new JsonSerializerOptions() { IncludeFields = true });


            //converti un string JSON en dictionnaire de vol
            //Dictionary<int, Vol> test = JsonSerializer.Deserialize<Dictionary<int, Vol>>(jsonString);



            //Console.WriteLine(jsonString);


           
            if (ListeDictionaryVols.Count() > 0) // si le dictionary des vols n'est pas vide fait ceci
            {
                StreamWriter fichier = new StreamWriter(nomFichier, false);
                // converti le dictionary en string JSON
                string jsonString = JsonSerializer.Serialize(ListeDictionaryVols, new JsonSerializerOptions() { IncludeFields = true });
                fichier.Write(jsonString);
                Console.WriteLine("AU REVOIR!!!");
                Console.WriteLine("Appuyer sur la touche ENTER pour fermer");
                Console.ReadKey();
                fichier.Close();
            }
            else
            {
                Console.WriteLine("Tableau des vols est vide et ne sera pas enregistrer");
            }
          
        }

        static void Main(string[] args)
        {

            GestionVol gestion =new GestionVol();
            gestion.ChargerVol();
            gestion.AffficherMenu();
        }
    }
}
