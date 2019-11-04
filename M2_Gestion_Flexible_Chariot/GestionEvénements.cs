using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace M2_Gestion_Flexible_Chariot
{
    public class GestionEvénements
    {
        /// <summary>
        /// Affiche les événements en fonction du lot saisi.
        /// </summary>
        public static void AfficherHistoriqueLot()
        {
            string IDHistoriqueLot = "";
            List<string> listeLots = new List<string>();

            do
            {
                Console.Write("\n\nVeuillez saisir l'ID du lot pour afficher ses événements : ");
                IDHistoriqueLot = Console.ReadLine();
                Console.Write("\n");

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "SELECT LOT_ID FROM lot ";

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listeLots.Add(reader.GetString(reader.GetOrdinal("LOT_ID")));
                            }
                        }

                        if (listeLots.Contains(IDHistoriqueLot))
                        {
                            cmd.CommandText = $"SELECT EVE_Libelle, EVE_Date, LOT_ID FROM evenement WHERE LOT_ID = {IDHistoriqueLot} ";
                            string colonnes = "Libellé {0,-25} Date de création {0,-25} ID du lot\n";
                            Console.Write(string.Format(colonnes, "", "", ""));

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                int compteur = 0;
                                Console.Write("\n");
                                while (reader.Read())
                                {
                                    Console.Write(string.Format("{0,-34}", reader["EVE_Libelle"]));
                                    Console.Write(string.Format("{0,-43}", reader["EVE_Date"]));
                                    Console.Write(string.Format("{0,0}", reader["LOT_ID"]));
                                    Console.WriteLine("");

                                    compteur++;
                                }
                                Console.Write("\n");
                                Console.WriteLine("{0} événement(s) affiché(s).", compteur);
                                GestionMenuPrincipale.EntrerSaisieUtilisateur();
                            }
                        }

                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("L'ID du lot n'existe pas, veuillez réessayer.");
                            Console.ResetColor();
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
            } while (!listeLots.Contains(IDHistoriqueLot));
        }
    }
}