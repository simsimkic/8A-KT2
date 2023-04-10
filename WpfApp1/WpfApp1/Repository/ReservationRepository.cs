﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Observer;
using WpfApp1.Model;
using WpfApp1.Serializer;
using WpfApp1.Model.Enums;
using WpfApp1.Domain.RepositoryInterfaces;
using WpfApp1.Domain.Models.Enums;

namespace WpfApp1.Repository
{
    public class ReservationRepository : IReservationRepository
    {

        private const string _filePath = "../../../Resources/Data/reservations.csv";
        private readonly List<IObserver> _observers;

        private readonly Serializer<Reservation> _serializer;

        private List<Reservation> _reservations;
        private static IReservationRepository _instance = null;
        public static IReservationRepository GetInstance()
        {
            if(_instance == null)
            {
                _instance = new ReservationRepository();
            }
            return _instance;
        }
        private ReservationRepository()
        {
            _reservations = new List<Reservation>();
            _serializer= new Serializer<Reservation>();
            _reservations = _serializer.FromCSV(_filePath);
            _observers = new List<IObserver>();
            SetStatus();                                //Status trenutne rezervacije (da li je u toku, prosla, ocenja ili neocenjena
            SetRatingStatus();
        }

        //INPROGRES RESERVED RATED UNRATED EXPIRED
        public void SetStatus()
        {
            foreach (Reservation reservation in _reservations)
            {
                if (reservation.Status == GuestRatingStatus.Reserved && reservation.StartDate <= DateTime.Now)
                {
                    reservation.Status = GuestRatingStatus.Inprogres;
                }
                else if (reservation.Status == GuestRatingStatus.Rated || reservation.Status == GuestRatingStatus.Expired)
                {
                    continue;
                }
                else if (reservation.Status == GuestRatingStatus.Inprogres && reservation.EndDate < DateTime.Now)
                {
                    reservation.Status = GuestRatingStatus.Unrated;
                }
                else if (reservation.Status == GuestRatingStatus.Unrated && reservation.EndDate < DateTime.Now.AddDays(-5))
                {
                    reservation.Status = GuestRatingStatus.Expired;
                }
                
            }
            Save();
        }

        public void SetRatingStatus()
        {
            foreach (Reservation reservation in _reservations)
            {
                if (reservation.GuestReservationStatus == AccommodationAndOwnerRatingStatus.Disabled && reservation.EndDate < DateTime.Now)
                {
                    reservation.GuestReservationStatus = AccommodationAndOwnerRatingStatus.Unrated;
                }
                else if (reservation.GuestReservationStatus == AccommodationAndOwnerRatingStatus.Rated || reservation.GuestReservationStatus == AccommodationAndOwnerRatingStatus.Expired)
                {
                    continue;
                }
                else if (reservation.GuestReservationStatus == AccommodationAndOwnerRatingStatus.Unrated && reservation.EndDate < DateTime.Now.AddDays(-5))
                {
                    reservation.GuestReservationStatus = AccommodationAndOwnerRatingStatus.Expired;
                }
            }
            Save();
        }

        public Reservation Create(Reservation entity)
        {
            entity.Id = NextId();
            _reservations.Add(entity);
            Save();
            NotifyObservers();
            return entity;
        }

        public Reservation Delete(Reservation entity)
        {
            //_reservations.Remove(entity);
            entity.Deleted = true;
            Save();
            NotifyObservers();
            return entity;
        }

        public Reservation Get(int id)
        {
            return _reservations.Find(r => r.Id == id && r.Deleted == false);
        }

        public List<Reservation> GetAll()
        {
            return _reservations.FindAll(r => r.Deleted == false);
        }

        public int NextId()
        {
            if(_reservations.Count == 0)
                return 0;
            int nextId = _reservations[_reservations.Count - 1].Id + 1;
            foreach(Reservation r in _reservations)
            {
                if(nextId == r.Id)
                {
                    nextId++;
                }
            }
            return nextId;
        }

        public void NotifyObservers()
        {
            foreach(var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Save()
        {
            _serializer.ToCSV(_filePath, _reservations);
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }


        public Reservation Update(Reservation entity)
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
    }
}
