using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MPP_Server.model;
using MPP_Server.repo.db;

namespace MPP_Server.service
{
    public class Service : IService
    {
        private readonly IDbContextFactory<RaceContext> _contextFactory;
        private readonly ParticipantRepo participantRepo;
        private readonly ParticipationRepo participationRepo;
        private readonly UserRepo userRepo;
        private readonly Dictionary<string, IObserver> loggedUsers = new();

        public Service(IDbContextFactory<RaceContext> contextFactory)
        {
            _contextFactory = contextFactory;
            string connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
            participantRepo = new ParticipantRepo(connectionString);
            participationRepo = new ParticipationRepo(connectionString);
            userRepo = new UserRepo(connectionString);
        }

        public bool AddParticipant(int id, string name)
        {
            if (participantRepo.Add(id, new Participant(id, name)))
            {
                UpdateAll();
                return true;
            }
            return false;
        }

        public int? AddRace(Race race)
        {
            using var context = _contextFactory.CreateDbContext();
            var raceRepo = new RaceRepo(context);
            if (raceRepo.Add(race))
            {
                UpdateAll();
                return race.ID;
            }
            return null;
        }

        public bool AddParticipation(int id, int participant, int race, int points)
        {
            if (participantRepo.Find(participant) == null)
                return false;

            using var context = _contextFactory.CreateDbContext();
            var raceRepo = new RaceRepo(context);
            if (raceRepo.Find(race) == null)
                return false;

            var result = participationRepo.Add(id, new Participation(id, participant, race, points));
            if (result)
            {
                UpdateAll();
            }
            return result;
        }

        public bool RemoveParticipant(int id)
        {
            bool result = participantRepo.Remove(id);
            if (result)
            {
                var ids = participationRepo.GetAllByParticipant(id).Select(p => p.ID).ToList();
                foreach (var participationId in ids)
                    participationRepo.Remove(participationId);

                UpdateAll();
            }
            return result;
        }

        public bool RemoveRace(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var raceRepo = new RaceRepo(context);
            bool result = raceRepo.Remove(id);
            if (result)
            {
                var ids = participationRepo.GetAll().Where(p => p.Race == id).Select(p => p.ID).ToList();
                foreach (var participationId in ids)
                    participationRepo.Remove(participationId);

                UpdateAll();
            }
            return result;
        }

        public bool RemoveParticipation(int id)
        {
            var p = participationRepo.Find(id);
            if (p == null) return false;

            var result = participationRepo.Remove(id);
            if (result)
            {
                UpdateAll();
            }
            return result;
        }

        public Participant? FindParticipant(int id)
        {
            var p = participantRepo.Find(id);
            if (p == null) return null;

            int pts = 0;
            foreach (var pa in participationRepo.GetAllByParticipant(p.ID))
                pts += pa.Points;
            p.Points = pts;
            return p;
        }

        public Race? FindRace(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var raceRepo = new RaceRepo(context);
            return raceRepo.Find(id);
        }

        public Participation? FindParticipation(int id)
        {
            return participationRepo.Find(id);
        }

        public ICollection<Participant> GetParticipants()
        {
            var result = new List<Participant>();
            foreach (var p in participantRepo.GetAll())
            {
                int pts = 0;
                foreach (var pa in participationRepo.GetAllByParticipant(p.ID))
                    pts += pa.Points;
                p.Points = pts;
                result.Add(p);
            }
            return result.OrderBy(p => p.Name).ToList();
        }

        public ICollection<Race> GetRaces()
        {
            using var context = _contextFactory.CreateDbContext();
            var raceRepo = new RaceRepo(context);
            return raceRepo.GetAll();
        }

        public ICollection<Participation> GetParticipations()
        {
            return participationRepo.GetAll();
        }

        public bool UpdateParticipant(int id, string newName)
        {
            if (participantRepo.Update(id, new Participant(id, newName)))
            {
                UpdateAll();
                return true;
            }
            return false;
        }

        public bool UpdateRace(Race race)
        {
            using var context = _contextFactory.CreateDbContext();
            var raceRepo = new RaceRepo(context);
            if (raceRepo.Update(race))
            {
                UpdateAll();
                return true;
            }
            return false;
        }

        public bool UpdateParticipation(int id, int newParticipant, int newRace, int newPoints)
        {
            if (participantRepo.Find(newParticipant) == null)
                return false;

            using var context = _contextFactory.CreateDbContext();
            var raceRepo = new RaceRepo(context);
            if (raceRepo.Find(newRace) == null)
                return false;

            var result = participationRepo.Update(id, new Participation(id, newParticipant, newRace, newPoints));
            if (result)
            {
                UpdateAll();
            }
            return result;
        }

        protected void UpdateAll()
        {
            var participants = participantRepo.GetAll().OrderBy(p => p.Name).ToArray();
            for (int i = 0; i < participants.Length; i++)
            {
                int pts = 0;
                foreach (var pa in participationRepo.GetAllByParticipant(participants[i].ID))
                    pts += pa.Points;
                participants[i].Points = pts;
            }

            ICollection<Race> races;
            using (var context = _contextFactory.CreateDbContext())
            {
                var raceRepo = new RaceRepo(context);
                races = raceRepo.GetAll();
            }

            foreach (var observer in loggedUsers.Values)
            {
                observer.update(participants, races.ToArray(), participationRepo.GetAll().ToArray());
            }
        }

        public void UpdateObserver(User user)
        {
            if (loggedUsers.TryGetValue(user.Username, out var observer))
            {
                var participants = participantRepo.GetAll().OrderBy(p => p.Name).ToList();
                for (int i = 0; i < participants.Count; i++)
                {
                    int pts = 0;
                    foreach (var pa in participationRepo.GetAllByParticipant(participants[i].ID))
                        pts += pa.Points;
                    participants[i].Points = pts;
                }
                observer.update(participants, GetRaces(), participationRepo.GetAll());
            }
        }

        public bool Login(User user, IObserver observer)
        {
            if (loggedUsers.ContainsKey(user.Username))
                return false;

            var u = userRepo.Find(user.Username);
            if (u != null && u.Password == user.Password)
            {
                loggedUsers.Add(user.Username, observer);
                return true;
            }
            return false;
        }

        public void Logout(User user)
        {
            loggedUsers.Remove(user.Username);
        }
    }
}
