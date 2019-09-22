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
        static int IDRecette = 0;

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

        /// <summary>
        /// Permet d'afficher le menu principale.
        /// </summary>
        static void AffichageMenuPrincipale()
        {
            Console.Clear();
            Console.WriteLine("*************** Menu principale ***************\n");
            Console.WriteLine("1. Recettes");
            Console.WriteLine("2. Lots");
            Console.WriteLine("3. Historique des lots\n");
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
                        AffichageMenuRecettes();
                        ChoixMenuRecettes();
                        break;

                    case '2':
                        CréationLots();
                        break;

                    case '3':
                        HistoriqueLots();
                        break;

                    case '4':
                        QuitterProgramme();
                        break;

                    default:
                        SwitchDefault();
                        break;
                }
            } while (choixMenuPrincipale != '4');
        }

        /// <summary>
        /// Permet d'afficher le menu recettes.
        /// </summary>
        static void AffichageMenuRecettes()
        {
            Console.Clear();
            Console.WriteLine("******** Menu recettes ***********");
            Console.WriteLine("\n1. Création de recettes");
            Console.WriteLine("2. Affichage des recettes créées");
            Console.WriteLine("3. Vider la table recette et pas");
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
                    ViderTables();
                    AffichageMenuRecettes();
                    ChoixMenuRecettes();
                    break;
                case '4':
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
            long lastInsertedId = -1;

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
                    Console.WriteLine("\nNombre d'enregistrements ajoutés : {0}\n", nbreAjout);
                    lastInsertedId = cmd.LastInsertedId;
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

        static void ViderTables()
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM recette; DELETE FROM pas";

                    int nbreEffacés = cmd.ExecuteNonQuery();
                    Console.WriteLine("Nombre de table effacés : {0}\n", nbreEffacés);
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
        /// Permet de créer un pas.
        /// </summary>
        static void CréationPas()
        {
            int IDPas = 0;
            int numéroPas = 0;
            string nomPas = " ";
            int positionPas = 0;
            int tempsPas = 0;
            bool quittancePas = false;
            long lastInsertedId = -1;
            char choixCréationRecette = ' ';
            int choixIDRecettePourPas = 0;

            do
            {
                Console.Write("Quel est le numéro du pas ? : ");
                numéroPas = int.Parse(Console.ReadLine());
                Console.Write("Quel est le nom du pas ? : ");
                nomPas = Console.ReadLine();
                Console.Write("Quel est la postion du pas (1 à 5) ? : ");
                positionPas = int.Parse(Console.ReadLine());
                Console.Write("Quel est la durée du pas (secondes) ? : ");
                tempsPas = int.Parse(Console.ReadLine());
                Console.Write("Est-ce qu'il est nécessaire de quittancer le pas (true ou false) ? : ");
                quittancePas = bool.Parse(Console.ReadLine());
                AffichageRecettes();
                Console.Write("\n\nQuelle est la recette qu'il faut associer pour le pas ? : ");
                choixIDRecettePourPas = int.Parse(Console.ReadLine());


                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO pas (PAS_Numero, PAS_Nom, PAS_Position, PAS_Temps, PAS_Quittance, REC_ID) VALUES (@numéro, @nomPas, @position, @temps, @quittance, @IDREC);";

                        cmd.Parameters.AddWithValue("@numéro", numéroPas);
                        cmd.Parameters.AddWithValue("@nomPas", nomPas);
                        cmd.Parameters.AddWithValue("@position", positionPas);
                        cmd.Parameters.AddWithValue("@temps", positionPas);
                        cmd.Parameters.AddWithValue("@quittance", quittancePas);
                        cmd.Parameters.AddWithValue("@IDREC", choixIDRecettePourPas);

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

        static void CréationLots()
        {

        }

        static void HistoriqueLots()
        {

        }

        static void SwitchDefault()
        {
            Console.Clear();
            Console.WriteLine("\nVeuillez appuyer sur une touche pour recommencer la saisie.");
            Console.WriteLine(Console.ReadKey());
            Console.Clear();
            AffichageMenuPrincipale();
        }

        static void QuitterProgramme()
        {
            choixMenuPrincipale = '4';
            Console.Clear();
            Console.WriteLine("\nMerci d'avoir utilisé cette application. :)\n");
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
