﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model.Enums;
using WpfApp1.Util;

namespace WpfApp1.Model
{
    public class ReservationPostponement : WpfApp1.Serializer.ISerializable
    {
        public int Id { get; set; }
        public int IdReservation { get; set; }

        public Reservation Reservation { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public string OwnerComment { get; set; }

        public ReservationPostponementStatus Status { get; set; }

        public ReservationPostponement()
        {

        }

        public ReservationPostponement(int id, int idReservation, Reservation reservation, DateTime startDate, DateTime endDate, string ownerComment, ReservationPostponementStatus status)
        {
            Id = id;
            IdReservation = idReservation;
            Reservation = reservation;
            StartDate = startDate;
            EndDate = endDate;
            OwnerComment = ownerComment;
            Status = status;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                IdReservation.ToString(),
                DateHelper.DateToString(StartDate),
                DateHelper.DateToString(EndDate),
                OwnerComment,
                Status.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            IdReservation = Convert.ToInt32(values[1]);
            StartDate = DateHelper.StringToDate(values[2]);
            EndDate = DateHelper.StringToDate(values[3]);
            OwnerComment = values[4];
            Status = Enum.Parse<ReservationPostponementStatus>(values[5]);
        }
    }
}
