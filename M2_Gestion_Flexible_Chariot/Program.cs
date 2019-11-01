using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

/*A FAIRE
 * Modifier la Création de pas, on doit créer le pas au moment de la recette avec une liste, ensuite, on utilie la méthode de création d upas.
 * */

namespace M2_Gestion_Flexible_Chariot
{
    /// <summary>
    /// Classe qui contient le programme principale.
    /// </summary>
    class Program 
    {
        /// <summary>
        /// Point d'entrée du programme.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            GestionBaseDeDonnée.ConnectToDB("gestion_flexible_chariot", "root", "");
            GestionMenuPrincipale.AffichageMenuPrincipale();
            GestionMenuPrincipale.ChoixMenuPrincipale();
            GestionBaseDeDonnée.DisconnectFromDB();
        }






















        /*------------------------------------------------------------------------------------- STATUTS -----------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Permet d'afficher la liste des statuts.
        /// </summary>
        public static void AffichageStatut()
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


