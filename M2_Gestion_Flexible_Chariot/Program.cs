using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace M2_Gestion_Flexible_Chariot
{
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
                Console.Write("Quel est votre choix ? : ");
                choixMenuPrincipale = char.Parse(Console.ReadLine());

                switch (choixMenuPrincipale)
                {
                    case '1':
                        AffichageMenuPas();
                        ChoixMenuPas();
                        break;

                    case '2':

                        break;

                    case '3':

                        break;

                    case '4':
                        FinProgramme();
                        break;

                    default:
                        SwitchDefault();
                        break;
                }
            } while (choixMenuPrincipale != '4');
        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        static void SwitchDefault()
        {
            Console.Write("\nVeuillez appuyer sur une touche pour recommencer la saisie... ");
            Console.WriteLine(Console.ReadKey());
            Console.Clear();
            AffichageMenuPrincipale();
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



                    break;
                default:
                    break;
            }
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

            do
            {
                Console.Clear();
                Console.WriteLine("\tRecette n° " + nombrePas);
                Console.Write("\nQuel est le numéro du pas ? : ");
                int numéroPas = int.Parse(Console.ReadLine());
                Console.Write("Quel est le nom du pas ? : ");
                string nomPas = Console.ReadLine();
                Console.Write("Quel est la postion du pas (1 à 5) ? : ");
                int positionPas = int.Parse(Console.ReadLine());
                Console.Write("Quel est la durée du pas (secondes) ? : ");
                int tempsPas = int.Parse(Console.ReadLine());
                Console.Write("Est-ce qu'il est nécessaire de quittancer le pas (true ou false) ? : ");
                bool quittancePas = bool.Parse(Console.ReadLine());
                AffichageRecettes();
                Console.Write("\n\nQuelle est la recette qu'il faut associer pour le pas ? : ");
                int IDRecette = int.Parse(Console.ReadLine());

                try
                {
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
                    Console.WriteLine("Attention il y a eu le problème suivant : ");
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
            } while (choixCréationPas != 'N');
            Console.WriteLine("Nombre de pas ajoutés : {0}\n", nbreAjout);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
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

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine("\n{0} {1} {2} {3} {4} {5} {6}", reader["PAS_ID"], reader["PAS_Numero"], reader["PAS_Nom"], reader["PAS_Position"], reader["PAS_Temps"], reader["PAS_Quittance"], reader["REC_ID"]);
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
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
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
                Console.Write("Veuillez saisir l'ID du pas à effacer : \n");
                int id = int.Parse(Console.ReadLine());

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM recette WHERE PAS_ID = @id;";
                        cmd.Parameters.AddWithValue("@id", id);

                        Console.Write("\nVoulez-vouz effacer un autre pas (O/N) ? : ");

                        choixEffacerPas = char.Parse(Console.ReadLine().ToUpper());
                        Console.Write("\n");
                        nbreEffacés += cmd.ExecuteNonQuery();
                    }

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Attention il y a eu le problème suivant : ");
                    Console.WriteLine(ex.Message);
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

            Console.Write("\nQuel est votre choix ? : ");
            choixMenuRecettes = char.Parse(Console.ReadLine());

            switch (choixMenuRecettes)
            {
                case '1':
                    CréationRecettes();
                    CréationPas();
                    AffichageMenuRecettes();
                    ChoixMenuRecettes();
                    break;
                case '2':
                    AffichageRecettes();
                    AffichageMenuRecettes();
                    ChoixMenuRecettes();
                    break;
                case '3':
                    ViderTablePas();
                    AffichageMenuRecettes();
                    ChoixMenuRecettes();
                    break;
                case '4':
                    SupprimmerTableRecette();
                    ; AjouterTableRecette();
                    AffichageMenuRecettes();
                    ChoixMenuRecettes();
                    break;
                case '5':
                    AffichageMenuPrincipale();
                    ChoixMenuPrincipale();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Permet de créer une recette.
        /// </summary>
        static void CréationRecettes()
        {
            Console.Clear();
            Console.Write("Veuillez saisir le nom de la recette : ");
            nomRecette = Console.ReadLine();

            DateTime dateTime = DateTime.Now;

            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO recette (REC_Nom, REC_DateCreation) VALUES (@nomRecette, @dateCréation);";

                    cmd.Parameters.AddWithValue("@nomRecette", nomRecette);
                    cmd.Parameters.AddWithValue("@dateCréation", dateTime);

                    int nbreAjout = cmd.ExecuteNonQuery();
                    Console.WriteLine("\nNombre de recette ajoutés : {0}\n", nbreAjout);
                    Console.Write("Veuillez appuyer sur une touche pour continuer... ");
                    Console.ReadKey();
                    Console.WriteLine("\n");
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
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
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Permet d'effacer une recette.
        /// </summary>
        static void EffacerRecettes()
        {
            Console.Write("Veuillez saisir l'ID à effacer : \n");
            int id = int.Parse(Console.ReadLine());

            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM recette WHERE REC_ID = @id;";
                    cmd.Parameters.AddWithValue("@id", id);

                    int nbreEffacés = cmd.ExecuteNonQuery();
                }

                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "UPDATE recette SET REC_ID = '1'";

                    int nbreEffacés = cmd.ExecuteNonQuery();
                    Console.WriteLine("Nombre d'enregistrements effacés : {0}\n", nbreEffacés);
                }


            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
            }

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
            Console.WriteLine("2. Affichage des lots");
            Console.WriteLine("3. Effacer des lots");
            Console.WriteLine("\n4. Revenir au menu principale\n");
        }

        /// <summary>
        /// Permet de créer un lot.
        /// </summary>
        static void CréationLots()
        {
            int qtePièceRéalisée = 0;
            int qtePièceAProduire = 0;
            int IDRecette = 0;
            int IDStatut = 0;
            char choixCréationRecette = ' ';
            long lastInsertedId = 0;

            DateTime dateTime = DateTime.Now;

            do
            {
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

                        int nbreAjout = cmd.ExecuteNonQuery();
                        Console.WriteLine("Nombre d'enregistrements ajoutés : {0}", nbreAjout);
                        lastInsertedId = cmd.LastInsertedId;

                        Console.Write("\nVoulez-vous créer à nouveau une recette ? (O/N) \n");

                        choixCréationRecette = char.Parse(Console.ReadLine());
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Attention il y a eu le problème suivant : ");
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
            } while (choixCréationRecette != 'N');
        }






        /*--------------------------------------------------------------------------------- FIN DE L'APPLICATION --------------------------------------------------------------------------------------*/

        /// <summary>
        /// Permet d'afficher un message d'aurevoir lors de l'arrêt de l'application
        /// </summary>
        static void FinProgramme()
        {
            choixMenuPrincipale = '4';
            Console.Clear();
            Console.WriteLine("\nMerci d'avoir utilisé cette application. :)\n");
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
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
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
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
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
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
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
                Console.WriteLine("Attention il y a eu le problème suivant : ");
                Console.WriteLine(ex.Message);
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


