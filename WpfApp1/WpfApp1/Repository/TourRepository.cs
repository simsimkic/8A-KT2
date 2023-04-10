﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Observer;
using WpfApp1.Domain.RepositoryInterfaces;
using WpfApp1.Model;
using WpfApp1.Serializer;

namespace WpfApp1.Repository
{
    public class TourRepository : IRepository<Tour>, ISubject
    {
        private const string _filePath = "../../../Resources/Data/tours.csv";
        private readonly List<IObserver> _observers;
        private readonly Serializer<Tour> _serializer;

        private List<Tour> _tours;
        private static TourRepository instance = null;
        public ILocationRepository ILocationRepository { get; set; }

        private TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = new List<Tour>();
            _tours = _serializer.FromCSV(_filePath);
            _observers = new List<IObserver>();
            ILocationRepository = LocationRepository.GetInstance();
        }
        public void BindLocation()
        {
            foreach (Tour tour in _tours)
            {
                tour.Location = ILocationRepository.Get(tour.IdLocation);
            }
        }
        public Tour Get(int id)
        {
            return _tours.Find(t => t.Id == id);
        }
        public Tour Create(Tour entity)
        {
            entity.Id = NextId();
            _tours.Add(entity);
            Save();
            return entity;
        }
        public Tour Update(Tour entity)
        {
            var oldEntity = Get(entity.Id);
            if(oldEntity == null)
            {
                return null;
            }
            oldEntity = entity;
            Save();
            return oldEntity;
        }
        public void Save()
        {
            _serializer.ToCSV(_filePath, _tours);
        }
        public Tour Delete(Tour entity)
        {
            _tours.Remove(entity);
            Save();
            return entity;
        }
        public int NextId()
        {
            if (_tours.Count == 0) return 0;
            int newId = _tours[_tours.Count() - 1].Id + 1;
            foreach(Tour a in _tours)
            {
                if (newId == a.Id)
                {
                    newId++;
                }
            }
            return newId;
        }
        public List<Tour> GetAll()
        {
            return _tours;
        }
        public static TourRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new TourRepository();
            }
            return instance;
        }
        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }
        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
