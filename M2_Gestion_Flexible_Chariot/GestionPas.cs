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
       public static void CréationPas()
        {
            int nbreAjout = 0;
            char choixCréationPas = ' ';
            int nombrePas = 1;
            int numéroPas = 0;
            string nomPas = "";
            int positionPas = 0;
            int tempsPas = 0;
            bool quittancePas = false;
            int IDRecette = 0;

            do
            {
                numéroPas = SaisieNuméroPas(ref nombrePas);
                nomPas = SaisieNomPas();
                positionPas = SaisiePositionPas();
                tempsPas = SaisieDuréePas();
                quittancePas = SaisieQuittancePas();
                GestionRecettes.AffichageRecettes();
                IDRecette = SaisieIDRecette();

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

            } while (choixCréationPas != 'N');

            Console.WriteLine("Nombre de pas ajoutés : {0}\n", nbreAjout);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet de saisir le numéro du pas.
        /// </summary>
        public static int SaisieNuméroPas(ref int nombrePas)
        {
            string saisieUtilisateur = "";
            int numéroPas = 0;
            bool saisieValide = false;

            do
            {
                Console.WriteLine("\n\n\t      Pas n° " + nombrePas + "\n");

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
        public static int SaisieIDRecette()
        {
            int IDRecette = 0;
            string saisieUtilisateur = "";
            bool saisieValide = false;

            do
            {
                Console.Write("\n\nQuelle est la recette qu'il faut associer pour le pas ? : ");
                saisieUtilisateur = Console.ReadLine();

                if (!int.TryParse(saisieUtilisateur, out IDRecette))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVeuillez saisir une valeur de mimimum 0.\n");
                    Console.ResetColor();
                }

                else
                {
                    IDRecette = int.Parse(saisieUtilisateur);
                    saisieValide = true;
                }

            } while (saisieValide == false);

            return IDRecette;
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



    }
}
