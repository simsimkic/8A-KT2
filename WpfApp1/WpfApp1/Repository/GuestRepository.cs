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
    public class GuestRepository : IGuestRepository
    {
        private const string _filePath = "../../../Resources/Data/guests.csv";

        private readonly Serializer<Guest> _serializer;
        private List<IObserver> _observers;
        private List<Guest> _guests;
        private static IGuestRepository _instance = null;

        public static IGuestRepository GetInsatnce()
        {
            if(_instance == null)
            {
                _instance = new GuestRepository();
            }
            return _instance;
        }

        private GuestRepository()
        {
            _serializer = new Serializer<Guest>();
            _guests = new List<Guest>();
            _guests = _serializer.FromCSV(_filePath);
            _observers = new List<IObserver>();
        }

        public Guest Get(int id)
        {
            return _guests.Find(o => o.Id == id);
        }

        public List<Guest> GetAll()
        {
            return _guests;
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
            foreach(var o in _observers)
            {
                o.Update();
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Guest Update(Guest entity)
        {
            throw new NotImplementedException();
        }
    }
}
