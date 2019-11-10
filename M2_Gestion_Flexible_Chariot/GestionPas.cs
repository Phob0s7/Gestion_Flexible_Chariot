using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace M2_Gestion_Flexible_Chariot
{
    /// <summary>
    /// Classe qui contient la gestion des pas.
    /// </summary>
    public class GestionPas
    {
        /// <summary>
        /// Permet de créer un pas.
        /// </summary>
        public static void CréationPas(ref string IDRecette)
        {
            int nbreAjout = 0;
            string choixCréationPas = "";
            int numéroPas = 0;
            string nomPas = "";
            int positionPas = 0;
            int tempsPas = 0;
            bool quittancePas = false;
            int nombrePas = 1;

            do
            {
                Console.WriteLine("\t      Pas n° " + nombrePas + "\n");

                numéroPas = SaisieNuméroPas();
                nomPas = VérifierSaisieNom("Nom du pas : ");
                positionPas = SaisiePositionPas();
                tempsPas = SaisieDuréePas();
                quittancePas = SaisieQuittancePas();

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

                        choixCréationPas = ErreurSaisirChoix("\nVoulez-vous créer à nouveau un pas (O/N) ? : ", choixCréationPas);
                        Console.WriteLine("");
                        nbreAjout += cmd.ExecuteNonQuery();
                        nombrePas++;
                    }
                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(ex.Message);
                    Console.ResetColor();
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }

                catch (FormatException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(ex.Message);
                    Console.ResetColor();
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }

                if (nombrePas == 11)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Vous ne pouvez plus créer de pas supplémentaire pour cette recette\n");
                    Console.ResetColor();
                }

            } while (choixCréationPas != "N" && nombrePas < 11);

            Console.WriteLine("Nombre de pas ajoutés : {0}\n", nbreAjout);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Répète l'erreur de saisie O ou N.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="choixCréation"></param>
        public static string ErreurSaisirChoix(string message, string choixCréation)
        {
            bool saisieValide = false;
            do
            {
                Console.Write(message);
                choixCréation = Console.ReadLine().ToUpper();

                if (choixCréation != "O" && choixCréation != "N")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVeuillez saisir o ou n.");
                    Console.ResetColor();
                }

                else
                {
                    saisieValide = true;
                }

            } while (saisieValide == false);

            return choixCréation;
        }

        /// <summary>
        /// Permet de saisir le numéro du pas.
        /// </summary>
        public static int SaisieNuméroPas()
        {
            string saisieUtilisateur = "";
            int numéroPas = 0;
            bool saisieValide = false;

            do
            {
                Console.Write("Numéro du pas : ");
                saisieUtilisateur = Console.ReadLine();

                if (!int.TryParse(saisieUtilisateur, out numéroPas))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVeuillez saisir une valeur correcte (nombre).\n");
                    Console.ResetColor();
                }

                else
                {
                    numéroPas = int.Parse(saisieUtilisateur);
                    saisieValide = true;
                }

            } while (saisieValide == false);

            return numéroPas;
        }
        /// <summary>
        /// Vérifie la saisie si elle contient des lettres.
        /// </summary>
        /// <returns>la saisie de l'utilisateur.</returns>
        public static string VérifierSaisieNom(string messageSaisie)
        {
            string saisiUtilisateur = "";
            float résultatConversionF = 0;
            int résultatConversionI = 0;
            string nomPas = "";
            bool saisieValide = false;

            do
            {
                Console.Write(messageSaisie);
                saisiUtilisateur = Console.ReadLine();

                if (int.TryParse(saisiUtilisateur, out résultatConversionI) || saisiUtilisateur == "" || float.TryParse(saisiUtilisateur, out résultatConversionF))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nVeuillez saisir une valeur correcte (lettre(s)).\n\n");
                    Console.ResetColor();
                }

                else
                {
                    nomPas = saisiUtilisateur;
                    saisieValide = true;
                }

            } while (saisieValide == false);

            return saisiUtilisateur;
        }

        /// <summary>
        /// Permet de définir les valeurs de validité d'une position.
        /// </summary>
        /// <returns>la position du pas.</returns>
        public static int SaisiePositionPas()
        {
            int positionPas = 0;
            string saisiUtilisateur = "";
            bool saisieValide = false;

            do
            {
                Console.Write("Position du pas (1 à 5) : ");
                saisiUtilisateur = Console.ReadLine();

                if (!int.TryParse(saisiUtilisateur, out positionPas) || positionPas < 1 || positionPas > 5)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVeuillez saisir une valeur entre 1 et 5.\n");
                    Console.ResetColor();
                }

                else
                {
                    positionPas = int.Parse(saisiUtilisateur);
                    saisieValide = true;
                }

            } while (saisieValide == false);

            return positionPas;
        }

        /// <summary>
        /// Permet de saisir la durée d'un pas.
        /// </summary>
        /// <returns> le temps du pas.</returns>
        public static int SaisieDuréePas()
        {
            int tempsPas = 0;
            string saisieUtilisateur = "";
            bool saisieValide = false;

            do
            {
                Console.Write("Durée du pas (secondes) : ");
                saisieUtilisateur = Console.ReadLine();

                if (!int.TryParse(saisieUtilisateur, out tempsPas) || tempsPas < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVeuillez saisir une valeur de mimimum 0.\n");
                    Console.ResetColor();
                }

                else
                {
                    tempsPas = int.Parse(saisieUtilisateur);
                    saisieValide = true;
                }

            } while (saisieValide == false);

            return tempsPas;
        }

        /// <summary>
        /// Permet de saisir la quittance du pas.
        /// </summary>
        /// <returns> la quittance booléenne.</returns>
        public static bool SaisieQuittancePas()
        {
            bool quittanceBooléen = false;
            string quittancePas = "";

            do
            {
                Console.Write("Quittancer le pas ? 0(non) ou 1(oui) : ");
                quittancePas = Console.ReadLine();
                if (quittancePas == "0")
                {
                    quittanceBooléen = false;
                }
                else if (quittancePas == "1")
                {
                    quittanceBooléen = true;
                }
                else if (quittancePas != "0" && quittancePas != "1")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nLa valeur saisie doit être 0 ou 1.\n");
                    Console.ResetColor();
                }

            } while (quittancePas != "0" && quittancePas != "1");

            return quittanceBooléen;
        }

        /// <summary>
        /// Sélectionne l'ID le plus grand des recettes, ensuite, permet de saisir l'ID de la recette à associer.
        /// </summary>
        /// <returns> l'ID de la recette</returns>
        public static string SaisieIDRecette(ref string IDRecette)
        {
            string saisieUtilisateur = "";
            bool saisieValide = false;
            int résultat = 0;

            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT MAX(REC_ID) AS RECID, REC_Nom, REC_DateCreation FROM recette";
                    Console.Write("ID\t");
                    Console.Write("Nom\t\t".PadLeft(6));
                    Console.Write(" Date de création\n".PadLeft(12));
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine("\n{0}\t {1}\t\t {2}".PadLeft(10), reader["RECID"], reader["REC_Nom"], reader["REC_DateCreation"]);
                            compteur++;
                        }

                        Console.WriteLine("\n{0} recettes affichées.\n", compteur);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                GestionMenuPrincipale.AttenteSaisieUtilisateur();
                Console.Write("\n\n");
            }

            do
            {
                Console.Write("Entrer le n° de l'ID de la recette : ");
                saisieUtilisateur = Console.ReadLine();

                if (saisieUtilisateur != IDRecette || !int.TryParse(saisieUtilisateur, out résultat))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVeuillez saisir l'ID correcte de la recette à insérer.\n");
                    Console.ResetColor();
                }

                else
                {
                    IDRecette = saisieUtilisateur;
                    saisieValide = true;
                }

            } while (saisieValide == false);

            return IDRecette;
        }

        /// <summary>
        /// Affiche les pas existant.
        /// </summary>
        public static void AfficherPas()
        {
            string IDRecette = "";
            MySqlDataReader reader;

            List<string> listeRecettes = new List<string>();

            do
            {
                try
                {
                    Console.Write("\nVeuillez saisir l'ID de la recette pour voir les pas : ");
                    IDRecette = Console.ReadLine();

                    listeRecettes = GestionRecettes.StockerIDRecette();

                    if (listeRecettes.Contains(IDRecette))
                    {
                        using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                        {
                            cmd.CommandText = $"SELECT * FROM pas WHERE REC_ID = {IDRecette}";
                            string colonnes = "\nNuméro {0,-9} Nom {0,-9} Position {0,-9} Temps {0,-9} Quittance {0,-9} ID de la recette";
                            Console.WriteLine(string.Format(colonnes, "", "", "", "", ""));
                            Console.WriteLine("");


                            using (reader = cmd.ExecuteReader())
                            {
                                int compteur = 0;
                                while (reader.Read())
                                {
                                    Console.Write(string.Format("{0,-17}", reader["PAS_Numero"]));
                                    Console.Write(string.Format("{0,-14}", reader["PAS_Nom"]));
                                    Console.Write(string.Format("{0,-19}", reader["PAS_Position"]));
                                    Console.Write(string.Format("{0,-16}", reader["PAS_Temps"]));

                                    if ((bool)reader["PAS_Quittance"] == true)
                                    {
                                        Console.Write(string.Format("{0,-20}", "Avec"));
                                    }

                                    else
                                    {
                                        Console.Write(string.Format("{0,-20}", "Sans"));
                                    }

                                    Console.Write(string.Format("{0,0}", reader["REC_ID"]));
                                    Console.WriteLine("");

                                    compteur++;
                                }

                                Console.WriteLine("\n{0} pas affiché(s).", compteur);
                                GestionMenuPrincipale.AttenteSaisieUtilisateur();
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nL'ID de la recette n'existe pas, veuillez réessayer.");
                        Console.ResetColor();
                    }
                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\n\n");
                }
            } while (!listeRecettes.Contains(IDRecette));
        }

        /// <summary>
        /// Permet d'effacer tous les pas d'une recette.
        /// </summary>
        public static void EffacerTousPasRecette(string ID)
        {
            int nbreEffacés = 0;
            nbreEffacés = 0;

            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM pas WHERE REC_ID = @id;";
                    cmd.Parameters.AddWithValue("@id", ID);
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
        }
    }
}
