using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace M2_Gestion_Flexible_Chariot
{
    static class GestionBaseDeDonnée
    {
        static public MySqlConnection connexion;

        /// <summary>
        /// Etablit une connexion à la base de données.
        /// </summary>
        /// <param name="databaseName">Nom de la base de données</param>
        /// <param name="userName">Nom d'utilisateur</param>
        /// <param name="password">Mot de passe</param>
        static public void ConnectToDB(string databaseName, string userName, string password)
        {
            string connectionString = $"server=localhost;database={databaseName};Uid={userName};Pwd={password};";
            //string connectionString = $"server=172.16.100.9;database={databaseName};Uid={userName};Pwd={password};";

            connexion = new MySqlConnection(connectionString);

            try
            {
                connexion.Open();
            }
            catch (MySqlException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Problème lors de la connexion à la base de donnée : ");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                Console.Write("\nVeuillez appuyer sur une touche pour continuer... ");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Donne un accès à l'objet de connexion MySql.
        /// </summary>
        /// <returns>la connexion MySQL</returns>
        static public MySqlConnection GetMySqlConnection()
        {
            return connexion;
        }

        /// <summary>
        /// Ferme la connexion à la base de données.
        /// </summary>
        static public void DisconnectFromDB()
        {
            if (connexion != null)
                connexion.Close();
        }
    }
}
