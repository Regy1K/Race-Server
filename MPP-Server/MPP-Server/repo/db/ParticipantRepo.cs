using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MPP_Server.model;
using NLog; 

namespace MPP_Server.repo.db
{
    public class ParticipantRepo : DBRepo<int, Participant>
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public ParticipantRepo(string url) : base(url) { }

        public override bool Add(int id, Participant entity)
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            int rows = 0;
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Participants (ID, Name) VALUES (@id, @name)";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", entity.Name);
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error($"Failed to add {entity}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            if (rows == 1)
            {
                log.Info($"Added {entity}");
                return true;
            }
            log.Info($"Failed to add {entity}");
            return false;
        }

        public override bool Update(int id, Participant newEntity)
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            int rows = 0;
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE Participants SET Name = @name WHERE ID = @id";
                cmd.Parameters.AddWithValue("@name", newEntity.Name);
                cmd.Parameters.AddWithValue("@id", id);
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error($"Failed to update {newEntity}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            if (rows == 1)
            {
                log.Info($"Updated {newEntity}");
                return true;
            }
            log.Info($"Failed to update {newEntity}");
            return false;
        }

        public override bool Remove(int id)
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            int rows = 0;
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Participants WHERE ID = @id";
                cmd.Parameters.AddWithValue("@id", id);
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error($"Failed to delete Participant with id {id}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            if (rows == 1)
            {
                log.Info($"Deleted Participant with id {id}");
                return true;
            }
            log.Info($"Failed to delete Participant with id {id}");
            return false;
        }

        public override Participant? Find(int id)
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            Participant? participant = null;
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Participants WHERE ID = @id";
                cmd.Parameters.AddWithValue("@id", id);
                using var result = cmd.ExecuteReader();
                if (result.Read())
                {
                    int Id = Convert.ToInt32(result["ID"]);
                    string name = (string)result["Name"];
                    participant = new Participant(Id, name);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error finding Participant with id {id}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            if (participant == null)
                log.Info($"Could not find Participant with id {id}");
            else
                log.Info($"Found {participant}");
            return participant;
        }

        public override ICollection<Participant> GetAll()
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            var participants = new List<Participant>();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Participants";
                using var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    int Id = Convert.ToInt32(result["ID"]);
                    string name = (string)result["Name"];
                    participants.Add(new Participant(Id, name));
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error returning all Participants: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            log.Info($"Returned {participants.Count} Participants");
            return participants;
        }
    }
}
