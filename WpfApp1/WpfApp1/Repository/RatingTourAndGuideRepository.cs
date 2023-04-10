﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Observer;
using WpfApp1.Model;
using WpfApp1.Serializer;

namespace WpfApp1.Repository
{
    public class RatingTourAndGuideRepository : IRepository<RatingTourAndGuide>, ISubject
    {
        private const string _filePath = "../../../Resources/Data/ratingTourAndGuide.csv";
        private readonly List<IObserver> _observers;

        private readonly Serializer<RatingTourAndGuide> _serializer;

        private List<RatingTourAndGuide> _ratingTourAndGuides;
        public TourBookingRepository TourBookingRepository { get; set; }

        private static RatingTourAndGuideRepository _instance = null;

        public static RatingTourAndGuideRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RatingTourAndGuideRepository();
            }
            return _instance;
        }
        private RatingTourAndGuideRepository()
        {
            _serializer = new Serializer<RatingTourAndGuide>();
            _observers = new List<IObserver>();
            _ratingTourAndGuides = new List<RatingTourAndGuide>();
            _ratingTourAndGuides = _serializer.FromCSV(_filePath);
            TourBookingRepository = TourBookingRepository.GetInstance();
        }
        public RatingTourAndGuide Create(RatingTourAndGuide entity)
        {
            entity.Id = NextId();
            _ratingTourAndGuides.Add(entity);
            Save();
            NotifyObservers();
            return entity;
        }
        public RatingTourAndGuide Delete(RatingTourAndGuide entity)
        {
            _ratingTourAndGuides.Remove(entity);
            Save();
            NotifyObservers();
            return entity;
        }
        public RatingTourAndGuide Get(int id)
        {
            return _ratingTourAndGuides.Find(r => r.Id == id);
        }

        public List<RatingTourAndGuide> GetAll()
        {
            return _ratingTourAndGuides;
        }
        public int NextId()
        {
            if (_ratingTourAndGuides.Count == 0)
                return 0;
            int nextId = _ratingTourAndGuides[_ratingTourAndGuides.Count - 1].Id + 1;
            foreach (RatingTourAndGuide r in _ratingTourAndGuides)
            {
                if (nextId == r.Id)
                {
                    nextId++;
                }
            }
            return nextId;
        }
        public void Save()
        {
            _serializer.ToCSV(_filePath, _ratingTourAndGuides);
        }
        public RatingTourAndGuide Update(RatingTourAndGuide entity)
        {
            var oldEntity = Get(entity.Id);
            if (oldEntity == null)
            {
                return null;
            }
            oldEntity = entity;
            Save();
            NotifyObservers();
            return oldEntity;
        }
        public void BindTourBooking()
        {
            foreach (RatingTourAndGuide r in _ratingTourAndGuides)
            {
                r.TourBooking = TourBookingRepository.Get(r.IdTourBooking);
            }
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
