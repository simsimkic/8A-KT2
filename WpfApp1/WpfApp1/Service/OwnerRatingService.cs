﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Observer;
using WpfApp1.Domain.RepositoryInterfaces;
using WpfApp1.Domain.ServiceInterfaces;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class OwnerRatingService : IOwnerRatingService
    {
        private readonly IOwnerRatingRepository _ownerRatingRepository;
        private readonly IReservationRepository _reservationRepository;
        public OwnerRatingService()
        {
            _ownerRatingRepository = InjectorRepository.CreateInstance<IOwnerRatingRepository>();
            _reservationRepository = InjectorRepository.CreateInstance<IReservationRepository>();
            BindReservation();
        }
        private void BindReservation()
        {
            foreach (OwnerRating r in GetAll())
            {
                r.Reservation = _reservationRepository.Get(r.IdReservation);
            }
        }
        public void Save()
        {
            _ownerRatingRepository.Save();
        }
        public OwnerRating Get(int id)
        {
            return _ownerRatingRepository.Get(id);
        }

        public List<OwnerRating> GetAll()
        {
            return _ownerRatingRepository.GetAll();
        }

        public void Create(OwnerRating ownerRating)
        {
            _ownerRatingRepository.Create(ownerRating);
        }

        public void Delete(OwnerRating ownerRating)
        {
            _ownerRatingRepository.Delete(ownerRating);
        }

        public void Update(OwnerRating ownerRating)
        {
            _ownerRatingRepository.Update(ownerRating);
        }
        
        public void Subscribe(IObserver observer)
        {
            _ownerRatingRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _ownerRatingRepository.Unsubscribe(observer);
        }

        public OwnerRating GetByIdReservation(int idReservation)
        {
            return GetAll().Find(r => r.IdReservation == idReservation);
        }

        public List<OwnerRating> GetAllOwnerRewies(int idOwner)
        {
            return GetAll().FindAll(r => r.Reservation.Accommodation.OwnerId == idOwner && r.Reservation.Status == Model.Enums.GuestRatingStatus.Rated);
        }

    }
}
