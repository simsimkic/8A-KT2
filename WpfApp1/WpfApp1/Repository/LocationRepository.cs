﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp.Observer;
using WpfApp1.Domain.RepositoryInterfaces;
using WpfApp1.Model;
using WpfApp1.Serializer;

namespace WpfApp1.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private const string _filePath = "../../../Resources/Data/locations.csv";
        private readonly List<IObserver> _observers;

        private readonly Serializer<Location> _serializer;

        private List<Location> _locations;

        private static ILocationRepository _instance = null;

        public static ILocationRepository GetInstance()
        {
            if(_instance == null)
            {
                _instance = new LocationRepository();
            }
            return _instance;
        }

        private LocationRepository()
        {
            _locations = new List<Location>();
            _serializer = new Serializer<Location>();
            _locations = _serializer.FromCSV(_filePath);
            _observers = new List<IObserver>();
        }
        public Location Create(Location entity)
        {
            entity.Id = NextId();
            _locations.Add(entity);
            Save();
            NotifyObservers();
            return entity;
        }

        public Location Delete(Location entity)
        {
            _locations.Remove(entity);
            Save();
            NotifyObservers();
            return entity;
        }

        public Location Get(int id)
        {
            return _locations.Find(l => l.Id == id);
        }

        public List<Location> GetAll()
        {
            return _locations;
        }

        public int NextId()
        {
            if(_locations.Count == 0)
                return 0;
            int nextId = _locations[_locations.Count - 1].Id + 1;
            foreach(Location l in _locations) 
            {
                if(nextId == l.Id)
                {
                    nextId++;
                }
            }
            return nextId;
        }
       
        public Location Update(Location entity)
        {
            var oldEntity = Get(entity.Id);
            if(oldEntity == null)
            {
                return null;
            }
            oldEntity = entity;
            Save();
            NotifyObservers();
            return oldEntity;
        }


        public void Save()
        {
            _serializer.ToCSV(_filePath, _locations);
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
            foreach(var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
