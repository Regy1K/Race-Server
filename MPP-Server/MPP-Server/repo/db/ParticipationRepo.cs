using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MPP_Server.model;
using NLog; 

namespace MPP_Server.repo.db
{
    public class ParticipationRepo : DBRepo<int, Participation>
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public ParticipationRepo(string url) : base(url) { }

        public override bool Add(int id, Participation entity)
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            int rows = 0;
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Participations (ID, Participant, Race, Points) VALUES (@id, @participant, @race, @points)";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@participant", entity.Participant);
                cmd.Parameters.AddWithValue("@race", entity.Race);
                cmd.Parameters.AddWithValue("@points", entity.Points);
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

        public override bool Update(int id, Participation newEntity)
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            int rows = 0;
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE Participations SET Participant = @participant, Race = @race, Points = @points WHERE ID = @id";
                cmd.Parameters.AddWithValue("@participant", newEntity.Participant);
                cmd.Parameters.AddWithValue("@race", newEntity.Race);
                cmd.Parameters.AddWithValue("@points", newEntity.Points);
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
                cmd.CommandText = "DELETE FROM Participations WHERE ID = @id";
                cmd.Parameters.AddWithValue("@id", id);
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error($"Failed to delete Participation with id {id}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            if (rows == 1)
            {
                log.Info($"Deleted Participation with id {id}");
                return true;
            }
            log.Info($"Failed to delete Participation with id {id}");
            return false;
        }

        public override Participation? Find(int id)
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            Participation? participation = null;
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Participations WHERE ID = @id";
                cmd.Parameters.AddWithValue("@id", id);
                using var result = cmd.ExecuteReader();
                if (result.Read())
                {
                    int Id = Convert.ToInt32(result["ID"]);
                    int participant = Convert.ToInt32(result["Participant"]);
                    int race = Convert.ToInt32(result["Race"]);
                    int points = Convert.ToInt32(result["Points"]);
                    participation = new Participation(Id, participant, race, points);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error searching for Participation with id {id}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            if (participation == null)
                log.Info($"Could not find Participation with id {id}");
            else
                log.Info($"Found {participation}");
            return participation;
        }

        public override ICollection<Participation> GetAll()
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            var participations = new List<Participation>();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Participations";
                using var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    int Id = Convert.ToInt32(result["ID"]);
                    int participant = Convert.ToInt32(result["Participant"]);
                    int race = Convert.ToInt32(result["Race"]);
                    int points = Convert.ToInt32(result["Points"]);
                    participations.Add(new Participation(Id, participant, race, points));
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error returning all Participations: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            log.Info($"Returned {participations.Count} Participations");
            return participations;
        }

        public ICollection<Participation> GetAllByRace(int raceID)
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            var participations = new List<Participation>();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Participations WHERE Race = @race";
                cmd.Parameters.AddWithValue("@race", raceID);
                using var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    int Id = Convert.ToInt32(result["ID"]);
                    int participant = Convert.ToInt32(result["Participant"]);
                    int race = Convert.ToInt32(result["Race"]);
                    int points = Convert.ToInt32(result["Points"]);
                    participations.Add(new Participation(Id, participant, race, points));
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error returning Participations by race {raceID}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            log.Info($"Returned {participations.Count} Participations for race {raceID}");
            return participations;
        }

        public ICollection<Participation> GetAllByParticipant(int participantID)
        {
            using var connection = new SQLiteConnection(url);
            connection.Open();
            var participations = new List<Participation>();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Participations WHERE Participant = @participant";
                cmd.Parameters.AddWithValue("@participant", participantID);
                using var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    int Id = Convert.ToInt32(result["ID"]);
                    int participant = Convert.ToInt32(result["Participant"]);
                    int race = Convert.ToInt32(result["Race"]);
                    int points = Convert.ToInt32(result["Points"]);
                    participations.Add(new Participation(Id, participant, race, points));
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error returning Participations by participant {participantID}: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            log.Info($"Returned {participations.Count} Participations for participant {participantID}");
            return participations;
        }
    }
}
