using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace M2_Gestion_Flexible_Chariot
{
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
                Console.WriteLine("\n\n\t      Pas n° " + nombrePas + "\n");

                numéroPas = SaisieNuméroPas();
                nomPas = SaisieNomPas();
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

            Console.WriteLine("\nNombre de pas ajoutés : {0}\n", nbreAjout);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Répète l'erreur de saisie
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
                Console.Write("Quel est le numéro du pas ? : ");
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
        /// Permet de saisir le nom du pas.
        /// </summary>
        /// <returns></returns>
        public static string SaisieNomPas()
        {
            string saisiUtilisateur = "";
            float résultatConversionF = 0;
            int résultatConversionI = 0;
            string nomPas = "";
            bool saisieValide = false;

            do
            {
                Console.Write("Quel est le nom du pas ? : ");
                saisiUtilisateur = Console.ReadLine();

                if (int.TryParse(saisiUtilisateur, out résultatConversionI) || saisiUtilisateur == "" || float.TryParse(saisiUtilisateur, out résultatConversionF))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVeuillez saisir une valeur correcte (lettre(s)).\n");
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
        /// Permet de définir les valeurs de valadité d'une position.
        /// </summary>
        public static int SaisiePositionPas()
        {
            int positionPas = 0;
            string saisiUtilisateur = "";
            bool saisieValide = false;

            do
            {
                Console.Write("Quel est la position du pas (1 à 5) ? : ");
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
        public static int SaisieDuréePas()
        {
            int tempsPas = 0;
            string saisieUtilisateur = "";
            bool saisieValide = false;

            do
            {
                Console.Write("Quel est la durée du pas (secondes) ? : ");
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
        /// <returns> La quittance booléenne.</returns>
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
        /// Permet de saisir l'ID de la recette à associer.
        /// </summary>
        /// <returns></returns>
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
                GestionMenuPrincipale.EntrerSaisieUtilisateur();
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
        public static void AfficherPas(string IDRecette)
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = $"SELECT * FROM pas WHERE REC_ID = {IDRecette}";
                    string colonnes = "\nID {0,-9} Numéro {0,-9} Nom {0,-9} Position {0,-9} Temps {0,-9} Quittance {0,-9} ID de la recette";
                    Console.WriteLine(string.Format(colonnes, "", "", "", "", "", ""));
                    Console.WriteLine("");
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.Write(string.Format("{0,-13}", reader["PAS_ID"]));
                            Console.Write(string.Format("{0,-17}", reader["PAS_Numero"]));
                            Console.Write(string.Format("{0,-14}", reader["PAS_Nom"]));
                            Console.Write(string.Format("{0,-19}", reader["PAS_Position"]));
                            Console.Write(string.Format("{0,-16}", reader["PAS_Temps"]));
                            Console.Write(string.Format("{0,-20}", reader["PAS_Quittance"]));
                            Console.Write(string.Format("{0,0}", reader["REC_ID"]));
                            Console.WriteLine("");
                            
                            compteur++;
                        }

                        Console.WriteLine("\n{0} pas affichées.", compteur);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\n\n");
            }

            GestionMenuPrincipale.EntrerSaisieUtilisateur();
        }

        /// <summary>
        /// Permet d'effacer un pas.
        /// </summary>
        public static void EffacerPas()
        {
            int nbreEffacés = 0;
            char choixEffacerPas = ' ';

            do
            {
                nbreEffacés = 0;
                Console.Write("\n\nVeuillez saisir l'ID du pas à effacer : ");
                int id = int.Parse(Console.ReadLine());

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM pas WHERE PAS_ID = @id;";
                        cmd.Parameters.AddWithValue("@id", id);
                        nbreEffacés += cmd.ExecuteNonQuery();

                        if (nbreEffacés == 0)
                        {
                            Console.WriteLine("L'ID saisi ne corresspond à aucun PAS_ID, veuillez recommencer la saisie.");
                        }

                        else
                        {
                            Console.Write("\nVoulez-vouz effacer un autre pas (O/N) ? : ");
                            choixEffacerPas = char.Parse(Console.ReadLine().ToUpper());
                            if (choixEffacerPas == 'O')
                            {
                                //AffichagePas();
                            }
                            Console.Write("\n");
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
            } while (choixEffacerPas != 'N' || nbreEffacés == 0);

            Console.WriteLine("Nombre d'enregistrements effacés : {0}\n", nbreEffacés);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche le menu permettant la modification d'un pas.
        /// </summary>
        public static void AfficherMenuModiferPas()
        {
            Console.WriteLine("1. Modification du numéro");
            Console.WriteLine("2. Modification du nom");
            Console.WriteLine("3. Modification de la postion");
            Console.WriteLine("4. Modification du temps");
            Console.WriteLine("5. Modification de la quittance");
            Console.WriteLine("6. Modification de l'ID de la recette");
        }

        /// <summary>
        /// Choisis le pas à modifier
        /// </summary>
        public static string ChoisirPasAModifier()
        {
            string saisieIDPas = "";

            Console.Write("Choix du pas à modifier : ");
            saisieIDPas = Console.ReadLine();
            Console.WriteLine("");

            return saisieIDPas;
        }

        /// <summary>
        /// Saisie un choix pour le menu de modification du pas.
        /// </summary>
        public static void ModifierPas(string IDPas)
        {
            int numéroPas = 0;
            string saisieUtilisateur = " ";

            AfficherMenuModiferPas();

            Console.Write("\nChoix de la modification d'un pas : ");
            saisieUtilisateur = Console.ReadLine();

            switch (saisieUtilisateur)
            {
                case "1":
                    numéroPas = SaisieNuméroPas();
                    Console.WriteLine("Modification effectuée");
                    GestionMenuPrincipale.EntrerSaisieUtilisateur();
                    AfficherMenuModiferPas();
                    ModifierPas(IDPas);
                    break;
                case "2":
                    numéroPas = SaisieNuméroPas();
                    Console.WriteLine("Modification effectuée");
                    GestionMenuPrincipale.EntrerSaisieUtilisateur();
                    AfficherMenuModiferPas();
                    ModifierPas(IDPas);
                    break;
                case "3":
                    numéroPas = SaisieNuméroPas();
                    Console.WriteLine("Modification effectuée");
                    GestionMenuPrincipale.EntrerSaisieUtilisateur();
                    AfficherMenuModiferPas();
                    ModifierPas(IDPas);
                    break;
                case "4":
                    numéroPas = SaisieNuméroPas();
                    Console.WriteLine("Modification effectuée");
                    GestionMenuPrincipale.EntrerSaisieUtilisateur();
                    AfficherMenuModiferPas();
                    ModifierPas(IDPas);
                    break;
                case "5":
                    numéroPas = SaisieNuméroPas();
                    Console.WriteLine("Modification effectuée");
                    GestionMenuPrincipale.EntrerSaisieUtilisateur();
                    AfficherMenuModiferPas();
                    ModifierPas(IDPas);
                    break;
                case "6":
                    numéroPas = SaisieNuméroPas();
                    Console.WriteLine("Modification effectuée");
                    GestionMenuPrincipale.EntrerSaisieUtilisateur();
                    AfficherMenuModiferPas();
                    ModifierPas(IDPas);
                    break;
                default:
                    break;
            }
        }

        /*
        /// <summary>
        /// Met à jour les informations d'un pas.
        /// </summary>
        public static void MettreAJourPas()
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "UPDATE pas (PAS_numéro) VALUES (@numéroPas) ;";

                    cmd.Parameters.AddWithValue("@numéroPas", );
                    

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
            */

    }
}
