//* Project Name : Car Rental System
//* Date : 28/05/2014
//* Ross Loftus

//* Main Window Initialized

//* 1.0 Declare db and variable times and objects
//* 1.1 Declare and set default combobox setting and define as cartype enum 
//* 1.2 Display image file and path names
//* 1.3 Set default dates on datepickers and black out past dates
//* 1.4 Hide search button
//* 1.5 Enum And values

//* Search button click event

//* 2.0 Clear previous results
//* 2.1 Ensure a car type is selected
//* 2.2 SQL query for 'all' car types
//* 2.3 Query to return all cars of selected size in Car table
//* 2.4 Declare everyCar list
//* 2.5 Populate everycar with every car in the Car table
//* 2.6 Find cars not available for selected dates 
//* 2.7 Create list for booked cars
//* 2.8 Populate booked car list with booked cars from query2
//* 2.9 Loop through everyCar and remove cars in bookedCars from it
//* 2.10 Display results
//* 2.11 Repeat process for selected car size 
//* 2.12 Message for when no car type is selected

//* Listbox Selection Changed

//* 3.0 Declare car selection from lbxDisplay
//* 3.1 Declare variable start and end times for ToShortDateString method
//* 3.2 Display selectedCar details in SelectedCar textblock

//* Book button click     

//* 4.0 Declare car selection from lbxDisplay
//* 4.1 Create new car rental booking object
//* 4.2 Add and save to booking table
//* 4.3 Confirm booking with details
//* 4.5 Reset controls, listbox and textblock
//* 4.6 Exception message when no car has been selected

// *Date pickers selections changed

//* 5.0 Declare begin date as selected date
//* 5.1 Clear blocked dates
//* 5.2 Set new ones
//* 5.3 Enable end date date picker
//* 5.4 Declare end date as selected date
//* 5.5 Clear blocked dates
//* 5.6 Set dates to be blocked
//* 5.7 Show search button


using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using System.Drawing;
using System.Collections.ObjectModel;

namespace CarHire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //declare db and variable times and objects
        Car_Hire_Entities db = new Car_Hire_Entities();
        Car selectedCar;
        DateTime begin;
        DateTime end;

        public MainWindow()
        {
            InitializeComponent();

            //cartype enum
            cbxCarType.ItemsSource = Enum.GetValues(typeof(CarType));
            //set default enum value
            cbxCarType.SelectedIndex = 0;

            //declare string for image path name
            string img;

            //image and folder path
            img = "/images/vwbeetle.png";
            img1.Source = new BitmapImage(new Uri(img, UriKind.Relative));

            //disable end date datepicker to ensure user picks a start date first
            dpEnd.IsEnabled = false;

            //set rental startdate to today(DateTime.Now)
            dpStart.DisplayDateStart = DateTime.Today.Subtract(TimeSpan.FromDays(1));

            //set default enddate to 1 day after start date, ensuring minimal rental is one day
            dpEnd.DisplayDateStart = DateTime.Today;
            dpStart.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.Subtract(TimeSpan.FromDays(1))));

            //hide search button until both dates hvae been selected
            btnSearch.Visibility = Visibility.Hidden;

        }

        //enum for car type selection
        public enum CarType
        {
            All, Small, Medium, Large
        }

        //button search click event
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //clear previous search results
            lbxDisplay.ItemsSource = null;
            tblkSelectedCar.Text = null;

            //ensure a car type is selected
            if (cbxCarType.SelectedItem != null)
            {
                //sql query for 'all' car types
                if (cbxCarType.Text == "All")
                {
                    //query to return all cars in Car table
                    var query = from car in db.Cars
                                select car;

                    //everyCar list
                    List<Car> everyCar = new List<Car>();

                    //populate everycar with every car in the Car table
                    foreach (Car car in query)
                    {
                        everyCar.Add(car);
                    }

                    //find cars not avilable for selected dates 
                    var query2 = (from booking in db.Bookings
                                  join car in db.Cars on booking.CarID equals car.ID
                                  where dpStart.SelectedDate <= booking.EndDate && dpEnd.SelectedDate >= booking.StartDate
                                  select car).Distinct();

                    //create list for booked cars
                    List<Car> bookedCars = new List<Car>();

                    //populate booked car list with booked cars from query2
                    foreach (Car booked in query2)
                    {
                        bookedCars.Add(booked);
                    }

                    //loop through everyCar and remove cars in bookedCars from it
                    for (int i = 0; i < everyCar.Count(); i++)
                    {
                        for (int j = 0; j < bookedCars.Count(); j++)
                        {
                            if (everyCar.ElementAt(i).ID == bookedCars.ElementAt(j).ID)
                            {
                                everyCar.Remove(everyCar.ElementAt(i));
                            }
                        }

                    }
                    //if no cars available
                    if (everyCar.Count == 0)
                    {
                        MessageBox.Show("There Are No Cars Available For Those Dates.\n\nPlease Search Again");
                    }
                    else

                        //display list of cars available to rent
                        lbxDisplay.ItemsSource = everyCar.ToList();

                }

                //sql query for other car types
                else
                {
                    //query to return all cars of selected size in Car table
                    var query = from car in db.Cars
                                where car.Size == cbxCarType.Text
                                select car;

                    //everyCar list
                    List<Car> carSize = new List<Car>();

                    //populate everycar with every car in the Car table
                    foreach (Car car in query)
                    {
                        carSize.Add(car);
                    }

                    //find cars not avilable for selected dates 
                    var query2 = (from booking in db.Bookings
                                  join car in db.Cars on booking.CarID equals car.ID
                                  where car.Size == cbxCarType.Text && (dpStart.SelectedDate <= booking.EndDate && dpEnd.SelectedDate >= booking.StartDate)
                                  select car).Distinct();

                    //create list for booked cars
                    List<Car> bookedCars = new List<Car>();

                    //populate booked car list with booked cars from query2
                    foreach (Car booked in query2)
                    {
                        bookedCars.Add(booked);
                    }

                    //loop through carSize and remove cars in bookedCars from it
                    for (int i = 0; i < carSize.Count(); i++)
                    {
                        for (int j = 0; j < bookedCars.Count(); j++)
                        {
                            if (carSize.ElementAt(i).ID == bookedCars.ElementAt(j).ID)
                            {
                                carSize.Remove(carSize.ElementAt(i));
                            }
                        }

                    }
                    //if no cars available
                    if (carSize.Count == 0)
                    {
                        MessageBox.Show("There Are No Cars Available For Those Dates.\n\nPlease Search Again");
                    }
                    else
                        //display list of cars available to rent
                        lbxDisplay.ItemsSource = carSize.ToList();
                }
            }

            //message when no car type is selected
            else
            {
                MessageBox.Show("Please Select A Car Type");
            }

        }

        private void lbxDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //declare car selection from lbxDisplay
            selectedCar = (Car)lbxDisplay.SelectedItem;

            //declare variable start and end times for ToShortDateString method
            DateTime? startDate = dpStart.SelectedDate;
            DateTime? endDate = dpEnd.SelectedDate;

            if (selectedCar != null)
            {
                //display selectedCar details in tblkSelectedCar
                tblkSelectedCar.Text = (selectedCar.GetCarDetail() + "Rental Date : " + startDate.Value.ToShortDateString() + "\n"
                + "Return Date : " + endDate.Value.ToShortDateString());
            }
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            //declare car selection from lbxDisplay
            Car selectedCar = (Car)lbxDisplay.SelectedItem;

            if (selectedCar != null)
            {
                //create new car rental booking object
                Booking b = new Booking

                {
                    CarID = selectedCar.ID,
                    StartDate = (DateTime)dpStart.SelectedDate,
                    EndDate = (DateTime)dpEnd.SelectedDate
                };

                //add and save to booking table
                db.Bookings.Add(b);
                db.SaveChanges();

                //NB After days working on this one issue I could not get the object/row to insert to the table
                //I tried numerous approaches including setting the identity to 'ON' for the primary key and changed the 
                //'copy when newer' settings on both .mdf and .edmx files. The db updated some the bookings while the solution
                //was open, although the changes didn't show when viewing tables. The new bookings, if any, disappeared when 
                //the application was closed. I recreated the database in Visual Studio and everything functions correctly now.

                //confirm booking with details
                MessageBox.Show("Booking Confirmation\n\n" + b.GetBookingDetail());

                //reset controls 
                lbxDisplay.ItemsSource = null;
                tblkSelectedCar.Text = null;
                cbxCarType.SelectedItem = null;

                //disable end date datepicker
                dpEnd.IsEnabled = false;
                //set rental startdate to today(DateTime.Now)
                dpStart.SelectedDate = DateTime.Today;

                //set default enddate to 1 day after start date, ensuring minimum rental is one day
                dpEnd.SelectedDate = DateTime.Today.Add(TimeSpan.FromDays(1));
                dpEnd.BlackoutDates.Add(new CalendarDateRange(DateTime.Now));

                dpStart.IsEnabled = false;
            }
            //message when no car has been selected
            else
            {
                MessageBox.Show("Please Search For And Select A Car To Book");
            }
        }

        //date picker selection change event change
        private void dpStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpStart.SelectedDate != null)
            {
                //declare begin date as selected date
                begin = (DateTime)dpStart.SelectedDate;
                //clear blocked dates
                dpEnd.BlackoutDates.Clear();
                //set new ones
                dpEnd.BlackoutDates.Add(new CalendarDateRange(DateTime.Today, begin));
                //enable end date date picker
                dpEnd.IsEnabled = true;
            }

        }

        private void dpEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpEnd.SelectedDate != null)
            {
                //declare end date as selected date
                end = (DateTime)dpEnd.SelectedDate;
                //clear blocked dates
                dpStart.BlackoutDates.Clear();
                //set new ones
                dpStart.BlackoutDates.Add(new CalendarDateRange(end, DateTime.MaxValue));
                //show search button
                btnSearch.Visibility = Visibility.Visible;

                dpStart.IsEnabled = true;
            }
        }


    }
}