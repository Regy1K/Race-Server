using MPP_Server.model;
using System.Collections.Generic;

namespace MPP_Server.service
{
    public interface IService
    {
        bool Login(User user, IObserver observer);
        void Logout(User user);
        bool AddParticipant(int id, string name);
        int? AddRace(Race race);
        bool AddParticipation(int id, int participant, int race, int points);
        bool RemoveParticipant(int id);
        bool RemoveRace(int id);
        bool RemoveParticipation(int id);
        Participant? FindParticipant(int id);
        Race? FindRace(int id);
        Participation? FindParticipation(int id);
        ICollection<Participant> GetParticipants();
        ICollection<Race> GetRaces();
        ICollection<Participation> GetParticipations();
        bool UpdateParticipant(int id, string newName);
        bool UpdateRace(Race race);
        bool UpdateParticipation(int id, int newParticipant, int newRace, int newPoints);
    }
}