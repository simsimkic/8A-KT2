﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Observer;
using WpfApp1.Model;

namespace WpfApp1.Domain.ServiceInterfaces
{
    public interface IAccommodationService : IService<Accommodation>
    {
        void Create(Accommodation entity);
        void Delete(Accommodation entity);
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        List<Accommodation> SearchAccommodation(string name, string city, string state, string type, int guestsNumber, int reservationDays);
        List<Accommodation> GetSortedListBySuperOwner();
    }
}