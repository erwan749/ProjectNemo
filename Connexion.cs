using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Windows;

namespace NemoApp
{
    internal class Connexion
    {
        private static MySqlConnection connection;
        private static string server;
        private static string database;
        private static string uid;
        private static string password;

        public static void Initialize()
        {
            server = "mysql-nemoproject.alwaysdata.net";
            database = "nemoproject_database";
            uid = "390982";
            password = "projetnemo";

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password}";
            connection = new MySqlConnection(connectionString);

            // Test de connexion avec un MessageBox
            if (OpenConnection())
            {
                MessageBox.Show("Connexion à la base de données réussie !", "Connexion", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseConnection();
            }
            else
            {
                MessageBox.Show("Échec de la connexion à la base de données.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static bool OpenConnection()
        {
            if (connection == null)
            {
                Console.WriteLine("La connexion n'a pas été initialisée. Veuillez appeler Initialize().");
                return false;
            }
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur de connexion : " + ex.Message);
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server. Contact administrator.");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password. Please try again.");
                        break;
                }
                return false;
            }
        }

        private static bool CloseConnection()
        {
            if (connection == null)
            {
                Console.WriteLine("La connexion n'est pas initialisée.");
                return false;
            }
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur lors de la fermeture de la connexion : " + ex.Message);
                return false;
            }
        }

        public static void InsertMateriel(int idTypeMat, double prix, int qteDispo)
        {
            string query = $"INSERT INTO Materiel (idTypeMat, prixLoc, qteDisp) VALUES({idTypeMat}, {prix}, {qteDispo})";
            Console.WriteLine(query);

            if (OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
            }
        }

        public static void InsertLocation(int idCli, double idPlong, int idMat, int qte)
        {
            string query = $"INSERT INTO LocationMateriel (idCli, idPlong, idMat, qte) VALUES({idCli}, {idPlong}, {idMat}, {qte})";
            Console.WriteLine(query);

            if (OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
            }
        }

        public static void UpdateMateriel(int idMat, int idTypeMat, double prix, int qteDispo)
        {
            string query = $"UPDATE Materiel SET idTypeMat = {idTypeMat}, prixLoc = {prix}, qteDisp = {qteDispo} WHERE idMat = {idMat}";
            Console.WriteLine(query);

            if (OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
            }
        }

        public static void UpdateLocation(int idLoc, int idCli, double idPlong, int idMat, int qte)
        {
            string query = $"UPDATE LocationMateriel SET idCli = {idCli}, idPlong = {idPlong}, idMat = {idMat}, qte = {qte} WHERE idLoc = {idLoc}";
            Console.WriteLine(query);

            if (OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
            }
        }

        public static void DeleteMateriel(int idMat)
        {
            string query = $"DELETE FROM Materiel WHERE idMat = {idMat}";

            if (OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
            }
        }

        public static void DeleteLocation(int idLoc)
        {
            string query = $"DELETE FROM LocationMateriel WHERE idLoc = {idLoc}";

            if (OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
            }
        }

        public static List<Materiel> SelectMateriel()
        {
            string query = "SELECT idMat, nomTypeMat, prixLoc, qteDisp FROM Materiel INNER JOIN TypeMateriel ON Materiel.idTypeMat = TypeMateriel.idTypeMat";
            List<Materiel> dbMateriel = new List<Materiel>();

            if (OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                using (MySqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Materiel leMateriel = new Materiel(
                            Convert.ToInt16(dataReader["idMat"]),
                            Convert.ToString(dataReader["nomTypeMat"]),
                            Convert.ToDouble(dataReader["prixLoc"]),
                            Convert.ToInt16(dataReader["qteDisp"])
                        );
                        dbMateriel.Add(leMateriel);
                    }
                }
                CloseConnection();
            }

            return dbMateriel;
        }

        public static List<Location> SelectLocation()
        {
            string query = "SELECT idLoc, nomCli, preCli, idPlong, idMat, qte FROM LocationMateriel INNER JOIN Clients ON LocationMateriel.idCli = Clients.idCli";
            List<Location> dbLocation = new List<Location>();

            if (OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                using (MySqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Location leLocation = new Location(
                            Convert.ToInt16(dataReader["idLoc"]),
                            Convert.ToString(dataReader["nomCli"] + " " + dataReader["preCli"]),
                            Convert.ToInt16(dataReader["idPlong"]),
                            Convert.ToInt16(dataReader["idMat"]),
                            Convert.ToInt16(dataReader["qte"])
                        );
                        dbLocation.Add(leLocation);
                    }
                }
                CloseConnection();
            }

            return dbLocation;
        }
    }
}
