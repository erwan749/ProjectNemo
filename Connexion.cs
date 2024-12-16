using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Runtime.CompilerServices;

namespace NemoApp
{
    internal class Connexion
    {
        private static MySqlConnection connection;
        private static string server;
        private static string database;
        private static string uid;
        private static string password;

        #region connect

        public static void Initialize()
        {
            server = "mysql-nemoproject.alwaysdata.net";
            database = "nemoproject_database";
            uid = "390982";
            password = "projetnemo";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password;
            connection = new MySqlConnection(connectionString);
        }

        private static bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                Console.WriteLine("Erreur connexion BDD");
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }


        private static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region rôle

        public static Dictionary<int, string> SelectedRole()
        {
            Dictionary<int, string> usersRoles = new Dictionary<int, string>();
            string query = "SELECT * FROM Roles";
            if (OpenConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int userId = reader.GetInt32("idRole");
                        string role = reader.GetString("nomRole");

                        usersRoles.Add(userId, role);
                    }
                    reader.Close();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erreur lors de la récupération des données : " + ex.Message);
                }
                finally
                {
                    CloseConnection();
                }
            }

            return usersRoles;
        }


        #endregion

        #region personels

        public static List<Personnel> SelectedPersonnel()
        {

            string query = "SELECT idPers , nomPers , prePers ,Personnel.idRole,nomRole ,certifPers   FROM Personnel inner join Roles on Personnel.idRole = Roles.idRole";

            List<Personnel> dbPersonnel = new List<Personnel>();

            if (Connexion.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    Personnel lePersonnel = new Personnel(Convert.ToInt16(dataReader["idPers"]), Convert.ToString(dataReader["nomPers"]), Convert.ToString(dataReader["prePers"]), Convert.ToInt16(dataReader["idRole"]), Convert.ToString(dataReader["nomRole"]), Convert.ToString(dataReader["certifPers"]));
                    dbPersonnel.Add(lePersonnel);
                }
                dataReader.Close();
                connection.Close();

            }
            return dbPersonnel;
        }


        public static void InsertPersonnel(string nomP, string preP, int idR, string certifP)
        {
            string query = "INSERT INTO Personnel (nomPers , prePers , idRole , certifPers)  VALUES('" + nomP + "','" + preP + "'," + idR + ",'" + certifP + "')";
            Console.WriteLine(query);
            if (Connexion.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                Connexion.CloseConnection();
            }

        }

        public static void UpdatePersonnel(int idP, string nomP, string preP, int idR, string certifP)
        {
            string query = "UPDATE Personnel SET nomPers ='" + nomP + "', prePers ='" + preP + "', idRole =" + idR + ", certifPers =' " + certifP + "' WHERE idPers=" + idP;
            Console.WriteLine(query);
            //Open connection
            if (Connexion.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                Connexion.CloseConnection();
            }

        }


        public static void DeletePersonnel(int idP)
        {
            string query = "DELETE FROM Personnel WHERE idPers =" + idP;

            if (Connexion.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                Connexion.CloseConnection();
            }
        }

        #endregion

        #region Materiel

        public static List<Materiel> SelectMateriel()
        {
            //Select statement
            string query = "SELECT idMat , nomTypeMat , prixLoc ,qteDisp   FROM Materiel inner join TypeMateriel on Materiel.idTypeMat = TypeMateriel.idTypeMat ";

            //Create a list to store the result
            List<Materiel> dbMateriel = new List<Materiel>();

            //Ouverture connection
            if (Connexion.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    Materiel leMateriel = new Materiel(Convert.ToInt16(dataReader["idMat"]), Convert.ToString(dataReader["nomTypeMat"]), Convert.ToDouble(dataReader["prixLoc"]), Convert.ToInt16(dataReader["qteDisp"]));
                    dbMateriel.Add(leMateriel);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                Connexion.CloseConnection();

                //retour de la collection pour être affichée
                return dbMateriel;
            }
            else
            {
                return dbMateriel;
            }
        }
        public static void InsertMateriel(int idTypeMat, double prix, int qteDispo)
        {
            string query = "INSERT INTO Materiel  (idTypeMat , prixLoc , qteDisp) VALUES(" + idTypeMat + "," + prix + "," + qteDispo + ")";
            Console.WriteLine(query);
            if (Connexion.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                Connexion.CloseConnection();
            }
        }
        public static void UpdateMateriel(int idMat, int idTypeMat, double prix, int qteDispo)
        {
            //Update Magazine
            string query = "UPDATE Materiel SET idTypeMat=" + idTypeMat + ", prixLoc=" + prix + ", qteDisp=" + qteDispo + " WHERE idMat=" + idMat;
            Console.WriteLine(query);
            //Open connection
            if (Connexion.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                Connexion.CloseConnection();
            }
        }
        public static void DeleteMateriel(int idMat)
        {
            //Delete Magazine
            string query = "DELETE FROM Materiel WHERE idMat =" + idMat;

            if (Connexion.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                Connexion.CloseConnection();
            }
        }
        #endregion

        #region Location
        public static void InsertLocation(int idCli, double idPlong, int idMat, int qte)
        {
            string query = "INSERT INTO LocationMateriel  (idCli  , idPlong , idMat ,qte ) VALUES(" + idCli + "," + idPlong + "," + idMat + "," + qte + ")";
            Console.WriteLine(query);
            if (Connexion.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                Connexion.CloseConnection();
            }
        }
        public static void UpdateLocation(int idLoc, int idCli, double idPlong, int idMat, int qte)
        {
            //Update Magazine
            string query = "UPDATE LocationMateriel SET idCli =" + idCli + ", idPlong =" + idPlong + ", idMat =" + idMat + ", qte = " + qte + " WHERE idLoc=" + idLoc;
            Console.WriteLine(query);
            //Open connection
            if (Connexion.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                Connexion.CloseConnection();
            }
        }
        public static void DeleteLocation(int idLoc)
        {
            //Delete Magazine
            string query = "DELETE FROM LocationMateriel WHERE idLoc =" + idLoc;

            if (Connexion.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                Connexion.CloseConnection();
            }
        }
        public static List<Location> SelectLocation()
        {
            //Select statement
            string query = "SELECT idLoc  , nomCli , preCli  , idPlong  ,idMat , qte   FROM LocationMateriel inner join Clients on LocationMateriel.idCli  = Clients.idCli  ";

            //Create a list to store the result
            List<Location> dbLocation = new List<Location>();

            //Ouverture connection
            if (Connexion.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    Location leLocation = new Location(Convert.ToInt16(dataReader["idLoc"]), Convert.ToString(dataReader["nomCli"]) + " " + Convert.ToString(dataReader["preCli"]), Convert.ToInt16(dataReader["idPlong"]), Convert.ToInt16(dataReader["idMat"]), Convert.ToInt16(dataReader["qte"]));
                    dbLocation.Add(leLocation);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                Connexion.CloseConnection();

                //retour de la collection pour être affichée
                return dbLocation;
            }
            else
            {
                return dbLocation;
            }

        }
        #endregion
    }
}