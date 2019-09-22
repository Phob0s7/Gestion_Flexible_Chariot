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

        static void Main(string[] args)
        {
            GestionBaseDeDonnée.ConnectToDB("gestion_flexible_chariot", "root", "");

            AffichageMenuPrincipale();
            ChoixMenuPrincipale();

            GestionBaseDeDonnée.DisconnectFromDB();
        }

        static void AffichageMenuPrincipale()
        {
            Console.Clear();
            Console.WriteLine("*************** Menu principale ***************\n");
            Console.WriteLine("1. Recettes");
            Console.WriteLine("2. Lots");
            Console.WriteLine("3. Historique des lots\n");
            Console.WriteLine("4. Quitter le programme\n");
        }

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

        static void AffichageMenuRecettes()
        {
            char choixMenuRecettes = ' ';

            Console.Clear();
            Console.WriteLine("******** Menu recettes ***********");
            Console.WriteLine("\n1. Création de recettes");
            Console.WriteLine("2. Affichage des recettes créées");
            Console.WriteLine("3. Effacer une recette");
            Console.WriteLine("\n4. Revenir au menu principale");
            Console.Write("\nQuel est votre choix ? ");
            choixMenuRecettes = char.Parse(Console.ReadLine());
            Console.Write("\n");

            switch (choixMenuRecettes)
            {
                case '1':
                    CréationRecettes();
                    break;
                case '2':
                    AffichageRecettes();
                    AffichageMenuRecettes();
                    break;
                case '3':
                    EffacerRecettes();
                    AffichageMenuRecettes();
                    break;
                case '4':
                    AffichageMenuPrincipale();
                    ChoixMenuPrincipale();
                    break;
                default:
                    break;
            }
        }

        static void CréationRecettes()
        {
            Console.Clear();
            Console.Write("Quel est l'ID de la recette : ");
            IDRecette = int.Parse(Console.ReadLine());
            Console.Write("Veuillez saisir le nom de la recette : ");
            nomRecette = Console.ReadLine();

            DateTime dateTime = DateTime.Now;

            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO recette (REC_ID, REC_Nom, REC_DateCreation) VALUES (@IDRecette, @nomRecette, @dateCréation);";

                    cmd.Parameters.AddWithValue("@IDRecette", IDRecette);
                    cmd.Parameters.AddWithValue("@nomRecette", nomRecette);
                    cmd.Parameters.AddWithValue("@dateCréation", dateTime);
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
                            Console.WriteLine("{0} {1} {2}", reader["REC_ID"], reader["REC_Nom"], reader["REC_DateCreation"]);
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
                Console.Write("Quel est l'ID du pas : ");
                IDPas = int.Parse(Console.ReadLine());
                Console.Write("Quel est le numéro du pas ? : ");
                numéroPas = int.Parse(Console.ReadLine());
                Console.Write("Quel est le nom du pas ? : ");
                nomPas = Console.ReadLine();
                Console.Write("Quel est la postion du pas (1 à 5) ? : ");
                positionPas = int.Parse(Console.ReadLine());
                Console.Write("Quel est la durée du pas (secondes) ? : ");
                tempsPas = int.Parse(Console.ReadLine());
                Console.Write("Est-ce qu'il est nécessaire de quittancer le pas ? : ");
                quittancePas = bool.Parse(Console.ReadLine());
                Console.Write("A quelle recette le pas doit-il être associé ? : ");
                choixIDRecettePourPas = int.Parse(Console.ReadLine());

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO pas (PAS_ID, PAS_Numero, PAS_Nom, PAS_Position, PAS_Temps, PAS_Quittance, REC_ID) VALUES (@IDPas, @numéro, @nomPas, @position, @temps, @quittance, @IDREC);";
                        cmd.Parameters.AddWithValue("@IDPas", IDPas);
                        cmd.Parameters.AddWithValue("@numéro", numéroPas);
                        cmd.Parameters.AddWithValue("@nomPas", nomPas);
                        cmd.Parameters.AddWithValue("@position", positionPas);
                        cmd.Parameters.AddWithValue("@temps", positionPas);
                        cmd.Parameters.AddWithValue("@quittance", quittancePas);
                        cmd.Parameters.AddWithValue("@IDREC", choixIDRecettePourPas);

                        int nbreAjout = cmd.ExecuteNonQuery();
                        Console.WriteLine("Nombre d'enregistrements ajoutés : {0}", nbreAjout);
                        lastInsertedId = cmd.LastInsertedId;

                        Console.Write("\nVoulez-vous créer à nouveau une recette ? (O/N) ");

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

            Console.Clear();
            AffichageMenuRecettes();
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
