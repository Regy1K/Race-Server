using MPP_Server.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPP_Server.repo.db
{
    public class RaceRepo
    {
        private readonly RaceContext _context;

        public RaceRepo(RaceContext context)
        {
            _context = context;
        }

        public ICollection<Race> GetAll()
        {
            // Use AsNoTracking for read-only queries to improve performance
            return _context.Races.AsNoTracking().ToList();
        }

        public bool Add(Race entity)
        {
            _context.Races.Add(entity);
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding Race: " + ex);
                return false;
            }
        }

        public bool Remove(int id)
        {
            var race = _context.Races.Find(id);
            if (race != null)
            {
                _context.Races.Remove(race);
                try
                {
                    return _context.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error removing Race: " + ex);
                    return false;
                }
            }
            return false;
        }

        public Race? Find(int id)
        {
            return _context.Races.AsNoTracking().FirstOrDefault(r => r.ID == id);
        }

        public bool Update(Race newEntity)
        {
            _context.Races.Update(newEntity);
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating Race: " + ex);
                return false;
            }
        }
    }
}