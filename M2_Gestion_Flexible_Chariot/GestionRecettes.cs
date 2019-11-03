using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace M2_Gestion_Flexible_Chariot
{
    public class GestionRecettes
    {
        static string nomRecette = "";

        /// <summary>
        /// Permet d'afficher le menu recettes.
        /// </summary>
        public static void AffichageMenuRecettes()
        {
            Console.Clear();
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\t      *** recettes ***                          ");
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\n1. Création de recettes");
            Console.WriteLine("2. Affichage de recettes");
            Console.WriteLine("3. Effacement de recettes");
            Console.WriteLine("4. Edition de recettes");
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\t        *** Pas ***                             ");
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\n5. Afficher tous les pas d'une recette");
            Console.WriteLine("6. Effacement de pas");
            Console.WriteLine("7. Edition de pas");
            Console.WriteLine("\n8. Revenir au menu principale");
            Console.WriteLine("__________________________________________________");
        }

        /// <summary>
        /// Permet de saisir le choix de l'utilisateur en fonction du menu recettes.
        /// </summary>
        public static void ChoixMenuRecettes()
        {
            char choixMenuRecettes = ' ';
            string IDPas = "";
            string IDRecette = "";

            Console.Write("Votre choix : ");
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
                    GestionMenuPrincipale.EntrerSaisieUtilisateur();
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
                    break;
                case '5':
                    GestionRecettes.AffichageRecettes();
                    IDRecette = SaisirIDRecette();
                    GestionPas.AfficherPas(IDRecette);
                    AffichageMenuRecettes();
                    break;
                case '6':
                    GestionPas.EffacerPas();
                    break;

                case '7':
                    //GestionPas.AfficherPas();
                    IDPas = GestionPas.ChoisirPasAModifier();
                    GestionPas.ModifierPas(IDPas);
                    break;

                case '8':
                    GestionMenuPrincipale.AffichageMenuPrincipale();
                    break;
                default:
                    ErreurSaisieMenuRecette();
                    break;
            }
        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        public static void ErreurSaisieMenuRecette()
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
        public static void CréationRecettes()
        {
            char choixAjouterRecette = ' ';
            int nombreRecette = 1;
            int nbreAjout = 0;
            string IDRecette = "";

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
                        nbreAjout += cmd.ExecuteNonQuery();
                        nombreRecette++;
                    }

                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "SELECT MAX(REC_ID) FROM RECETTE";

                        IDRecette = cmd.ExecuteScalar().ToString();
                    }


                    GestionPas.CréationPas(ref IDRecette);

                    Console.Write("\nVoulez-vouz ajouter une autre recette (O/N) ? : ");

                    choixAjouterRecette = char.Parse(Console.ReadLine().ToUpper());
                    Console.Write("\n");

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
            Console.Write("\nVeuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet d'afficher la liste des recettes.
        /// </summary>
        public static void AffichageRecettes()
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM recette ";
                    string colonnes = "\nID {0,-10} Nom {0,-10} Date de création";
                    Console.WriteLine(string.Format(colonnes, "", "", ""));
                    Console.WriteLine("");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.Write(string.Format("{0,-14}", reader["REC_ID"]));
                            Console.Write(string.Format("{0,-15}", reader["REC_Nom"]));
                            Console.Write(string.Format("{0,0}", reader["REC_DateCreation"]));
                            Console.WriteLine("");

                            compteur++;
                        }

                        Console.WriteLine("\n{0} recettes affichées.", compteur);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\n\n");
            }
        }

        /// <summary>
        /// Demande à l'utilisateur l'ID d'une recette
        /// </summary>
        public static string SaisirIDRecette()
        {
            string saisieUtilisateur = "";
            bool saisieValide = false;
            int résultat = 0;

            do
            {
                Console.Write("\nVeuillez saisir l'ID de la recette pour voir les pas : ");
                saisieUtilisateur = Console.ReadLine();

                if (!int.TryParse(saisieUtilisateur, out résultat))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVeuillez saisir une valeur correcte (nombre(s)).");
                    Console.ResetColor();
                }

                else
                {
                    saisieValide = true;
                }

            } while (saisieValide == false);

            return saisieUtilisateur;
        }

        /// <summary>
        /// Permet d'effacer une recette.
        /// </summary>
        public static void EffacerRecettes()
        {
            char choixEffacerRecette = ' ';
            int nbreEffacés = 0;

            do
            {
                Console.Write("Veuillez saisir l'ID de la recette à effacer : ");
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


    }
}
