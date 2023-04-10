﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Domain.ServiceInterfaces;
using WpfApp1.Model;
using WpfApp1.Repository;
using WpfApp1.Service;
using WpfApp1.View;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IOwnerService _ownerService;
        private readonly IGuestService _guestService;
        public readonly TouristService _touristService;
        public User LogInUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            _ownerService = InjectorService.CreateInstance<IOwnerService>();
            _guestService = InjectorService.CreateInstance<IGuestService>();
            _touristService = new TouristService();  

        }
        private void TourSearchAndOverview(object sender, RoutedEventArgs e)
        {
            TourSearchAndOverview a = new TourSearchAndOverview();
            a.Show();

        }
        private void AccommodationView(object sender, RoutedEventArgs e)
        {
            Guest guest = GuestRepository.GetInsatnce().Get(0);
            AccommodationView accommodationView = new AccommodationView(guest);
            accommodationView.Show();
        }
        private void LogIn(object sender, RoutedEventArgs e)
        {
            Password = passwordBox.Password;

            LogInUser = _ownerService.GetByUsernameAndPassword(Username, Password);   
            if(LogInUser != null)
            {
                OwnerAccount ownerAccount = new OwnerAccount(LogInUser);
                ownerAccount.Show();
                this.Close();
            }
            LogInUser = _touristService.GetByUsernameAndPassword(Username, Password);
            if(LogInUser != null)
            {
                TourSearchAndOverview tourSearchAndOverview = new TourSearchAndOverview();
                tourSearchAndOverview.Show();
                this.Close();
            }
            LogInUser = _guestService.GetByUsernameAndPassword(Username, Password);
            if(LogInUser != null)
            {
                GuestAccount guestAccount = new GuestAccount(LogInUser);
                guestAccount.Show();
                this.Close();
            }
        }
    }
}
