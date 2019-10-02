using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

/*A FAIRE
 * - Try catch pour chaque fonctions
 * */



namespace M2_Gestion_Flexible_Chariot
{
    /// <summary>
    /// Classe qui contient le programme principale.
    /// </summary>
    class Program
    {
        static char choixMenuPrincipale = ' ';
        static string nomRecette = "";

        /// <summary>
        /// Point d'entrée du programme.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            GestionBaseDeDonnée.ConnectToDB("gestion_flexible_chariot", "root", "");

            AffichageMenuPrincipale();
            ChoixMenuPrincipale();

            GestionBaseDeDonnée.DisconnectFromDB();
        }

        /*----------------------------------------------------------------------------------- MENU PRINCIPALE ------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Permet d'afficher le menu principale.
        /// </summary>
        static void AffichageMenuPrincipale()
        {
            Console.Clear();
            Console.WriteLine("*************** Menu principale ***************\n");
            Console.WriteLine("1. Pas");
            Console.WriteLine("2. Recettes");
            Console.WriteLine("3. Lots\n");
            Console.WriteLine("4. Quitter le programme\n");
        }

        /// <summary>
        /// Permet de saisir le choix de l'utilisateur en fonction du menu principale.
        /// </summary>
        static void ChoixMenuPrincipale()
        {
            do
            {
                try
                {
                    Console.Write("Quel est votre choix ? : ");
                    choixMenuPrincipale = char.Parse(Console.ReadLine());
                }

                catch (System.FormatException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }

                switch (choixMenuPrincipale)
                    {
                        case '1':
                            AffichageMenuPas();
                            ChoixMenuPas();
                            break;

                        case '2':
                            AffichageMenuRecettes();
                            ChoixMenuRecettes();
                            break;

                        case '3':
                            AffichageMenuLots();
                            ChoixMenuLots();
                            break;

                        case '4':
                            FinProgramme();
                            break;

                        default:
                            //ErreurSaisieMenuPrincipale();
                            break;
                    }
            } while (choixMenuPrincipale != '4');
        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        static void ErreurSaisieMenuPrincipale()
        {
            Console.Write("\nErreur de saisie, veuillez appuyer sur une touche pour recommencer la saisie... ");
            Console.WriteLine(Console.ReadKey());
            Console.Clear();
            AffichageMenuPrincipale();
            ChoixMenuPrincipale();
        }

        /*------------------------------------------------------------------------------------------ MENU DES PAS -------------------------------------------------------------------------------------*/

        /// <summary>
        /// Permet d'afficher le menu des pas.
        /// </summary>
        static void AffichageMenuPas()
        {
            Console.Clear();
            Console.WriteLine("******** Menu pas ***********");
            Console.WriteLine("\n1. Création de pas");
            Console.WriteLine("2. Affichage des pas");
            Console.WriteLine("3. Effacer des pas");
            Console.WriteLine("\n4. Revenir au menu principale\n");
        }

        /// <summary>
        /// Permet de saisir le choix de l'utilisateur en fonction du menu pas.
        /// </summary>
        static void ChoixMenuPas()
        {
            char choixMenuPas = ' ';

            Console.Write("\nQuel est votre choix ? : ");
            choixMenuPas = char.Parse(Console.ReadLine());

            switch (choixMenuPas)
            {
                case '1':
                    CréationPas();
                    AffichageMenuPas();
                    ChoixMenuPas();
                    break;
                case '2':
                    AffichagePas();
                    AffichageMenuPas();
                    ChoixMenuPas();
                    break;
                case '3':
                    AffichagePas();
                    EffacerPas();
                    AffichageMenuPas();
                    ChoixMenuPas();
                    break;
                case '4':
                    AffichageMenuPrincipale();
                    ChoixMenuPrincipale();
                    break;
                default:
                    ErreurSaisieMenuPas();
                    break;
            }
        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        static void ErreurSaisieMenuPas()
        {
            Console.Write("\nErreur de saisie, veuillez appuyer sur une touche pour recommencer la saisie... ");
            Console.WriteLine(Console.ReadKey());
            Console.Clear();
            AffichageMenuPas();
            ChoixMenuPas();
        }


        /// <summary>
        /// Permet de créer un pas.
        /// </summary>
        static void CréationPas()
        {
            Console.Clear();

            int nbreAjout = 0;
            char choixCréationPas = ' ';
            int nombrePas = 1;
            int numéroPas = 0;
            string nomPas = "";
            int positionPas = 0;
            int tempsPas = 0;

            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("\tPas n° " + nombrePas);
                    Console.Write("\nQuel est le numéro du pas ? : ");
                    numéroPas = int.Parse(Console.ReadLine());
                    Console.Write("Quel est le nom du pas ? : ");
                    nomPas = Console.ReadLine();
                    positionPas = SaisiePositionPas();
                    tempsPas = SaisieDuréePas();

                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
                catch (FormatException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
                bool quittancePas = SaisieQuittancePas();
                try
                {
                    AffichageRecettes();
                    Console.Write("\n\nQuelle est la recette qu'il faut associer pour le pas ? : ");
                    int IDRecette = int.Parse(Console.ReadLine());


                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO pas (PAS_Numero, PAS_Nom, PAS_Position, PAS_Temps, PAS_Quittance, REC_ID) VALUES (@numéro, @nomPas, @position, @temps, @quittance, @IDREC);";

                        cmd.Parameters.AddWithValue("@numéro", numéroPas);
                        cmd.Parameters.AddWithValue("@nomPas", nomPas);
                        cmd.Parameters.AddWithValue("@position", positionPas);
                        cmd.Parameters.AddWithValue("@temps", tempsPas);
                        cmd.Parameters.AddWithValue("@quittance", quittancePas);
                        cmd.Parameters.AddWithValue("@IDREC", IDRecette);

                        Console.Write("\nVoulez-vous créer à nouveau un pas (O/N) ? : ");

                        choixCréationPas = char.Parse(Console.ReadLine().ToUpper());
                        Console.Write("\n");
                        nbreAjout = nbreAjout + cmd.ExecuteNonQuery();
                        nombrePas++;
                    }

                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
                catch (FormatException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
            } while (choixCréationPas != 'N');
            Console.WriteLine("Nombre de pas ajoutés : {0}\n", nbreAjout);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet de définir les valeurs de valadité d'une position.
        /// </summary>
        static int SaisiePositionPas()
        {
            int positionPas = 0;

            do
            {
                Console.Write("Quel est la position du pas (1 à 5) ? : ");
                positionPas = int.Parse(Console.ReadLine());
                if (positionPas < 1 || positionPas > 5)
                {
                    Console.WriteLine("\nLa valeur doit être comprise entre 1 et 5, veuillez recommencer la saisie.\n");
                }
            } while (positionPas < 1 || positionPas > 5);

            return positionPas;
        }

        /// <summary>
        /// Permet de saisir la durée d'un pas.
        /// </summary>
        static int SaisieDuréePas()
        {
            int tempsPas = 0;

            Console.Write("Quel est la durée du pas (secondes) ? : ");
            tempsPas = int.Parse(Console.ReadLine());
            if (tempsPas < -0)
            {
                tempsPas = 0;
            }

            return tempsPas;
        }

        /// <summary>
        /// Permet de saisir la quittance du pas.
        /// </summary>
        /// <returns> La quittance booléenne.</returns>
        static bool SaisieQuittancePas()
        {
            bool quittanceBooléen = false;
            char quittancePas = ' ';

            do
            {
                Console.Write("Est-ce qu'il est nécessaire de quittancer le pas 0(non) ou 1(oui) ? : ");
                quittancePas = char.Parse(Console.ReadLine());
                if (quittancePas == '0')
                {
                    quittanceBooléen = false;
                }
                else if (quittancePas == '1')
                {
                    quittanceBooléen = true;
                }
                else if (quittancePas != '0' && quittancePas != '1')
                {
                    Console.WriteLine("\nLa valeur saisie doit être 0 ou 1. Veuillez recommencer la saisie");
                }

            } while (quittancePas != '0' && quittancePas != '1');

            return quittanceBooléen;
        }

        /// <summary>
        /// Permet d'afficher la liste des pas.
        /// </summary>
        static void AffichagePas()
        {
            Console.Clear();
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM pas";
                    Console.WriteLine("PAS_ID\tPAS_Numéro\tPAS_Nom\tPAS_Position\tPAS_Temps\tPAS_Quittance\tREC_ID");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine("  {0}\t \t{1}\t \t\t{2}\t \t{3}\t {4}\t {5}\t {6}", reader["PAS_ID"], reader["PAS_Numero"], reader["PAS_Nom"],
                            reader["PAS_Position"], reader["PAS_Temps"], reader["PAS_Quittance"], reader["REC_ID"]);
                            compteur++;
                        }

                        Console.WriteLine("\n{0} pas affichés.\n", compteur);
                        Console.Write("Veuillez appuyer sur une touche pour continuer... ");
                        Console.ReadKey();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                Console.ReadKey();
                Console.Write("\n\n");
            }
        }

        /// <summary>
        /// Permet d'effacer un pas.
        /// </summary>
        static void EffacerPas()
        {
            int nbreEffacés = 0;
            char choixEffacerPas = ' ';

            do
            {
                Console.Write("\n\nVeuillez saisir l'ID du pas à effacer : ");
                int id = int.Parse(Console.ReadLine());

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM pas WHERE PAS_ID = @id;";
                        cmd.Parameters.AddWithValue("@id", id);

                        Console.Write("\nVoulez-vouz effacer un autre pas (O/N) ? : ");

                        choixEffacerPas = char.Parse(Console.ReadLine().ToUpper());
                        Console.Write("\n");
                        nbreEffacés += cmd.ExecuteNonQuery();
                    }

                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
            } while (choixEffacerPas != 'N');

            Console.WriteLine("Nombre d'enregistrements effacés : {0}\n", nbreEffacés);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }





        /*------------------------------------------------------------------------------------------------ MENU DES RECETTES --------------------------------------------------------------------------*/

        /// <summary>
        /// Permet d'afficher le menu recettes.
        /// </summary>
        static void AffichageMenuRecettes()
        {
            Console.Clear();
            Console.WriteLine("******** Menu recettes ***********");
            Console.WriteLine("\n1. Création de recettes");
            Console.WriteLine("2. Affichage des recettes");
            Console.WriteLine("3. Effacer des recettes");
            Console.WriteLine("\n4. Revenir au menu principale\n");
        }

        /// <summary>
        /// Permet de saisir le choix de l'utilisateur en fonction du menu recettes.
        /// </summary>
        static void ChoixMenuRecettes()
        {
            char choixMenuRecettes = ' ';

            Console.Write("Quel est votre choix ? : ");
            choixMenuRecettes = char.Parse(Console.ReadLine());

            switch (choixMenuRecettes)
            {
                case '1':
                    CréationRecettes();
                    AffichageMenuRecettes();
                    ChoixMenuRecettes();
                    break;
                case '2':
                    AffichageRecettes();
                    AffichageMenuRecettes();
                    ChoixMenuRecettes();
                    break;
                case '3':
                    AffichageRecettes();
                    EffacerRecettes();
                    AffichageMenuRecettes();
                    ChoixMenuRecettes();
                    break;
                case '4':
                    AffichageMenuPrincipale();
                    ChoixMenuPrincipale();
                    break;
                default:
                    ErreurSaisieMenuRecette();
                    break;
            }

        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        static void ErreurSaisieMenuRecette()
        {
            Console.Write("\nErreur de saisie, veuillez appuyer sur une touche pour recommencer la saisie... ");
            Console.WriteLine(Console.ReadKey());
            Console.Clear();
            AffichageMenuRecettes();
            ChoixMenuRecettes();
        }

        /// <summary>
        /// Permet de créer une recette.
        /// </summary>
        static void CréationRecettes()
        {
            char choixAjouterRecette = ' ';
            int nombreRecette = 1;
            int nbreAjout = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("\t    Recette n° " + nombreRecette);
                Console.Write("\nVeuillez saisir le nom de la recette : ");
                nomRecette = Console.ReadLine();

                DateTime dateTime = DateTime.Now;

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO recette (REC_Nom, REC_DateCreation) VALUES (@nomRecette, @dateCréation);";

                        cmd.Parameters.AddWithValue("@nomRecette", nomRecette);
                        cmd.Parameters.AddWithValue("@dateCréation", dateTime);

                        Console.Write("\nVoulez-vouz ajouter une autre recette (O/N) ? : ");

                        choixAjouterRecette = char.Parse(Console.ReadLine().ToUpper());
                        Console.Write("\n");
                        nbreAjout += cmd.ExecuteNonQuery();
                        nombreRecette++;
                    }

                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
            } while (choixAjouterRecette != 'N');

            Console.WriteLine("Nombre de recette ajoutés : {0}\n", nbreAjout);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet d'afficher la liste des recettes.
        /// </summary>
        static void AffichageRecettes()
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM recette";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine("\n{0} {1} {2}", reader["REC_ID"], reader["REC_Nom"], reader["REC_DateCreation"]);
                            compteur++;
                        }

                        Console.WriteLine("\n{0} recettes affichés.\n", compteur);
                        Console.Write("Veuillez appuyer sur une touche pour continuer... ");
                        Console.ReadKey();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                Console.ReadKey();
                Console.Write("\n\n");
            }
        }

        /// <summary>
        /// Permet d'effacer une recette.
        /// </summary>
        static void EffacerRecettes()
        {
            char choixEffacerRecette = ' ';
            int nbreEffacés = 0;

            do
            {
                Console.Write("\n\nVeuillez saisir l'ID de la recette à effacer : ");
                int id = int.Parse(Console.ReadLine());

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM recette WHERE REC_ID = @id;";
                        cmd.Parameters.AddWithValue("@id", id);

                        Console.Write("\nVoulez-vouz effacer une autre recette (O/N) ? : ");

                        choixEffacerRecette = char.Parse(Console.ReadLine().ToUpper());
                        nbreEffacés += cmd.ExecuteNonQuery();
                    }

                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
            } while (choixEffacerRecette != 'N');

            Console.WriteLine("\nNombre de recettes effacés : {0}\n", nbreEffacés);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }




        /*------------------------------------------------------------------------------------------- MENU DES LOTS -----------------------------------------------------------------------------------*/

        /// <summary>
        /// Permet d'afficher le menu des lots.
        /// </summary>
        static void AffichageMenuLots()
        {
            Console.Clear();
            Console.WriteLine("******** Menu lots ***********");
            Console.WriteLine("\n1. Création de lots");
            Console.WriteLine("2. Historique des lots");
            Console.WriteLine("3. Effacer des lots");
            Console.WriteLine("\n4. Revenir au menu principale\n");
        }

        /// <summary>
        /// Permet de saisir le choix de l'utilisateur en fonction du menu lots.
        /// </summary>
        static void ChoixMenuLots()
        {
            char choixMenuLots = ' ';

            Console.Write("Quel est votre choix ? : ");
            choixMenuLots = char.Parse(Console.ReadLine());

            switch (choixMenuLots)
            {
                case '1':
                    CréationLots();
                    AffichageMenuLots();
                    ChoixMenuLots();

                    break;
                case '2':
                    AffichageLots();
                    AffichageMenuLots();
                    ChoixMenuLots();
                    break;
                case '3':
                    AffichageLots();
                    EffacerLots();
                    AffichageMenuLots();
                    ChoixMenuLots();
                    break;
                case '4':
                    AffichageMenuPrincipale();
                    ChoixMenuPrincipale();
                    break;
                default:
                    ErreurSaisieMenuLots();
                    break;
            }

        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        static void ErreurSaisieMenuLots()
        {
            Console.Write("\nErreur de saisie, veuillez appuyer sur une touche pour recommencer la saisie... ");
            Console.WriteLine(Console.ReadKey());
            Console.Clear();
            AffichageMenuLots();
            ChoixMenuLots();
        }



        /// <summary>
        /// Permet de créer un lot.
        /// </summary>
        static void CréationLots()
        {
            int nbreLots = 1;
            int nbreAjout = 0;
            int qtePièceRéalisée = 0;
            int qtePièceAProduire = 0;
            int IDRecette = 0;
            int IDStatut = 0;
            char choixCréationLots = ' ';

            DateTime dateTime = DateTime.Now;

            do
            {
                Console.Clear();

                Console.WriteLine("               Lot n° " + nbreLots);
                Console.Write("\nQuelle est la quantité de pièce réalisée ? : ");
                qtePièceRéalisée = int.Parse(Console.ReadLine());
                Console.Write("Quelle est la quantité de pièce à produire ? : ");
                qtePièceAProduire = int.Parse(Console.ReadLine());
                AffichageRecettes();
                Console.Write("\n\nQuelle est l'ID de la recette a associer à ce lot ? : ");
                IDRecette = int.Parse(Console.ReadLine());
                AffichageStatut();
                Console.Write("\n\nQuelle est l'ID du statut a associer à ce lot ? : ");
                IDStatut = int.Parse(Console.ReadLine());


                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO lot (LOT_QtePieceRealisee, LOT_QtePieceAProduire, LOT_DateCreation, REC_ID, STA_ID) VALUES (@pièceRéalisée, @pièceProduire, @date, @RECID, @STAID);";

                        cmd.Parameters.AddWithValue("@pièceRéalisée", qtePièceRéalisée);
                        cmd.Parameters.AddWithValue("@pièceProduire", qtePièceAProduire);
                        cmd.Parameters.AddWithValue("@date", dateTime);
                        cmd.Parameters.AddWithValue("@RECID", IDRecette);
                        cmd.Parameters.AddWithValue("@STAID", IDStatut);

                        Console.Write("\nVoulez-vous créer un nouveau lot ? (O/N) ");

                        choixCréationLots = char.Parse(Console.ReadLine().ToUpper());

                        nbreAjout += cmd.ExecuteNonQuery();
                        nbreLots++;
                    }
                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
            } while (choixCréationLots != 'N');

            Console.WriteLine("\nNombre de lots ajoutés : {0}", nbreAjout);
            Console.Write("\nVeuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet d'afficher la liste des lots.
        /// </summary>
        static void AffichageLots()
        {
            Console.Clear();

            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM lot";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine("\n{0} {1} {2} {3} {4} {5}", reader["LOT_ID"], reader["LOT_QtePieceRealisee"], reader["LOT_QtePieceAProduire"], reader["LOT_DateCreation"], reader["REC_ID"], reader["STA_ID"]);
                            compteur++;
                        }

                        Console.WriteLine("\n{0} lots affichés.\n", compteur);
                        Console.Write("Veuillez appuyer sur une touche pour continuer... ");
                        Console.ReadKey();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                Console.ReadKey();
                Console.Write("\n\n");
            }
        }

        /// <summary>
        /// Permet d'effacer un lot.
        /// </summary>
        static void EffacerLots()
        {
            char choixEffacerLot = ' ';
            int nbreEffacés = 0;

            do
            {
                Console.Write("\n\nVeuillez saisir l'ID du lot à effacer : ");
                int id = int.Parse(Console.ReadLine());

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM lot WHERE LOT_ID = @id;";
                        cmd.Parameters.AddWithValue("@id", id);

                        Console.Write("\nVoulez-vouz effacer un autre lot (O/N) ? : ");

                        choixEffacerLot = char.Parse(Console.ReadLine().ToUpper());
                        nbreEffacés += cmd.ExecuteNonQuery();
                    }

                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
            } while (choixEffacerLot != 'N');

            Console.WriteLine("\nNombre de lots effacés : {0}\n", nbreEffacés);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

        /*--------------------------------------------------------------------------------- FIN DE L'APPLICATION --------------------------------------------------------------------------------------*/

        /// <summary>
        /// Permet d'afficher un message d'aurevoir lors de l'arrêt de l'application
        /// </summary>
        static void FinProgramme()
        {
            Console.Clear();
            Console.WriteLine("\nMerci d'avoir utilisé notre application. :)\n");
        }

        /*------------------------------------------------------------------------------------- STATUTS -----------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Permet d'afficher la liste des statuts.
        /// </summary>
        static void AffichageStatut()
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM statut";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine("\n{0} {1}", reader["STA_ID"], reader["STA_Libelle"]);
                            compteur++;
                        }

                        Console.WriteLine("\n{0} statuts affichés.\n", compteur);
                        Console.Write("Veuillez appuyer sur une touche pour continuer... ");
                        Console.ReadKey();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                Console.ReadKey();
                Console.Write("\n\n");
            }

        }

        /*------------------------------------------------------------------------------------- TABLES -----------------------------------------------------------------------------------------------*/


        /// <summary>
        /// Permet de vider la table Pas.
        /// </summary>
        static void ViderTablePas()
        {

            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM pas";

                    int nbreEffacés = cmd.ExecuteNonQuery();
                    Console.WriteLine("\nNombre de pas effacés : {0}\n", nbreEffacés);

                    GestionBaseDeDonnée.connexion.Close();
                }

                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    GestionBaseDeDonnée.connexion.Open();

                    cmd.CommandText = "UPDATE pas SET PAS_ID = '0'";

                    int nbreEffacés = cmd.ExecuteNonQuery();
                    Console.WriteLine("\nNombre de pas effacés : {0}\n", nbreEffacés);
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                Console.ReadKey();
                Console.Write("\n\n");
            }

            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

        static void AjouterTableRecette()
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE recette (REC_ID INT(11),REC_Nom varchar(50),REC_DateCreation datetime)";

                    int nbreEffacés = cmd.ExecuteNonQuery();
                    Console.WriteLine("\nNombre de table effacée : {0}\n", nbreEffacés);
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                Console.ReadKey();
                Console.Write("\n\n");
            }

            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }


        /// <summary>
        /// Permet de supprimmer la table des recettes.
        /// </summary>
        static void SupprimmerTableRecette()
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "DROP TABLE recette";

                    int nbreEffacés = cmd.ExecuteNonQuery();
                    Console.WriteLine("\nNombre de table effacée : {0}\n", nbreEffacés);
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                Console.ReadKey();
                Console.Write("\n\n");
            }

            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }


        static void AfficherTable()
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM `statut`";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1}",
                                                reader["STA_ID"],
                                                reader["STA_Libelle"]);
                            compteur++;
                        }

                        Console.WriteLine("{0} statuts affichés.", compteur);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                Console.ReadKey();
                Console.Write("\n\n");
            }
        }



























        /*------------------------------------------------------------------------------------- EXEMPLES PROF -----------------------------------------------------------------------------------------------*/

        static void MettreÀJourContact(long id, string nom, string prénom)
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "UPDATE contact SET CONT_Nom = @nom, CONT_Prenom = @prenom WHERE CONT_Id = @id;";
                    cmd.Parameters.AddWithValue("@nom", nom);
                    cmd.Parameters.AddWithValue("@prenom", prénom);
                    cmd.Parameters.AddWithValue("@id", id);

                    int nbreModif = cmd.ExecuteNonQuery();
                    Console.WriteLine("Nombre d'enregistrements modifiés : {0}", nbreModif);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
            }
        }

        static long AjouterContact(string nom, string prénom)
        {
            long lastInsertedId = -1;
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO contact (CONT_Nom, CONT_Prenom) VALUES (@nom, @prenom);";
                    cmd.Parameters.AddWithValue("@nom", nom);
                    cmd.Parameters.AddWithValue("@prenom", prénom);

                    int nbreAjout = cmd.ExecuteNonQuery();
                    Console.WriteLine("Nombre d'enregistrements ajoutés : {0}", nbreAjout);
                    lastInsertedId = cmd.LastInsertedId;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
            }
            return lastInsertedId;
        }

        static void EffacerContact(long id)
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM contact WHERE CONT_Id = @id;";
                    cmd.Parameters.AddWithValue("@id", id);

                    int nbreEffacés = cmd.ExecuteNonQuery();
                    Console.WriteLine("Nombre d'enregistrements effacés : {0}", nbreEffacés);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
            }

        }

        static int NbreContact(string filtre)
        {
            int count = -1;

            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM contact WHERE CONT_Nom LIKE @nom;";
                    cmd.Parameters.AddWithValue("@nom", filtre);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
            }
            return count;
        }
    }

}


