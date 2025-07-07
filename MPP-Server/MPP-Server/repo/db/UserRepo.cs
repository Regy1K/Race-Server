using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MPP_Server.model;

namespace MPP_Server.repo.db
{
    public class UserRepo : DBRepo<string, User>
    {
        public UserRepo(string url) : base(url) { }

        public override bool Add(string username, User entity)
        {
            using var connection = new SQLiteConnection(url);
            int rows = 0;
            connection.Open();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "insert into Users values(@username, @password)";
                cmd.Parameters.AddWithValue("@username", entity.Username);
                cmd.Parameters.AddWithValue("@password", entity.Password);
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error($"Failed to add {entity}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            connection.Close();

            if (rows == 1)
            {
                log.Info("Added " + entity);
                return true;
            }

            log.Info("Failed to add " + entity);
            return false;
        }

        public override bool Update(string username, User newEntity)
        {
            using var connection = new SQLiteConnection(url);
            int rows = 0;
            connection.Open();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "update Users set Password=@password where Username=@username";
                cmd.Parameters.AddWithValue("@password", newEntity.Password);
                cmd.Parameters.AddWithValue("@username", username);
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error($"Failed to update {newEntity}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            connection.Close();

            if (rows == 1)
            {
                log.Info("Updated " + newEntity);
                return true;
            }

            log.Info("Failed to update " + newEntity);
            return false;
        }

        public override bool Remove(string username)
        {
            using var connection = new SQLiteConnection(url);
            int rows = 0;
            connection.Open();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "delete from Users where Username=@username";
                cmd.Parameters.AddWithValue("@username", username);
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error($"Failed to delete User with username {username}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            connection.Close();

            if (rows == 1)
            {
                log.Info("Deleted User with username " + username);
                return true;
            }

            log.Info("Failed to delete User with username " + username);
            return false;
        }

        public override User? Find(string username)
        {
            using var connection = new SQLiteConnection(url);
            User? user = null;
            connection.Open();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "select * from Users where Username=@username";
                cmd.Parameters.AddWithValue("@username", username);
                using var result = cmd.ExecuteReader();

                if (result.Read())
                {
                    string foundUsername = (string)result["Username"];
                    string password = (string)result["Password"];
                    user = new User(foundUsername, password);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error searching for User {username}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            connection.Close();

            if (user == null)
            {
                log.Info("Could not find User " + username);
            }
            else
            {
                log.Info("Found " + user);
            }
            return user;
        }

        public override ICollection<User> GetAll()
        {
            using var connection = new SQLiteConnection(url);
            var users = new List<User>();
            connection.Open();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "select * from Users";
                using var result = cmd.ExecuteReader();

                while (result.Read())
                {
                    string username = (string)result["Username"];
                    string password = (string)result["Password"];
                    users.Add(new User(username, password));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                log.Error("Error returning all Users: " + ex.Message);
            }
            connection.Close();

            log.Info($"Returned {users.Count} Users");
            return users;
        }
    }
}
