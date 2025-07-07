using MPP_Server.model;
using System.Collections.Generic;

namespace MPP_Server.service
{
    public interface IObserver
    {
        void update(ICollection<Participant> participants, ICollection<Race> races, ICollection<Participation> participations);
    }
}