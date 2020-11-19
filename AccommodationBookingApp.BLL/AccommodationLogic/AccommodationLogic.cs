using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.AccommodationLogic
{
    public class AccommodationLogic
    {
        private IAccommodation _accommodation = new DataAccess.Functions.AccommodationFunctions();
        private BookingLogic bookingLogic = new BookingLogic();

        public async Task<Boolean> CreateNewAccomodation(Accommodation accommodation,
                                                         int accommodationTypeId,
                                                         string applicationUserId)
        {
            try
            {
                var result = await _accommodation.CreateAccommodation(accommodation,accommodationTypeId, applicationUserId);
                if(result.Id > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<Accommodation>> GetAccommodations()
        {
            List<Accommodation> accommodations = await _accommodation.GetAllAccommodations();

            return accommodations;
        }

        public async Task<List<Accommodation>> GetAccommodationsWithUserId(string userId)
        {
            List<Accommodation> accommodations = await _accommodation.GetAccommodationsWithUserIdAsync(userId);

            return accommodations;
        }

        public async Task<List<Accommodation>> GetFilteredAccommodations(string accommodationCity,
                             int accommodationTypeId, int numberOfGuests, DateTime checkInDate, DateTime checkOutDate)
        {
            List<Accommodation> accommodationsToRemove = new List<Accommodation>();
            List<Accommodation> accommodations = await _accommodation.GetFilteredAccommodations(accommodationCity, accommodationTypeId, numberOfGuests,
                                                                                                checkInDate, checkOutDate);

            //obavezno provjerit rubne slucajeve npr kad gleda zadnju rezervaciju i sl++ (search dobro radi, problem sa check-in i check-out datumima na details stranici)
            foreach (Accommodation accommodation in accommodations)
            {
                var bookingsForAccommodation = await bookingLogic.GetAllBookingsForAccommodation(accommodation.Id);
                if (bookingsForAccommodation.Count != 0)
                {
                    foreach (Booking booking in bookingsForAccommodation)
                    {
                        if (checkInDate < booking.CheckOutDate && checkOutDate > booking.CheckInDate
                            && booking.ApprovalStatus != ApprovalStatus.Cancelled
                            && booking.ApprovalStatus != ApprovalStatus.CancelledByUser)
                        {
                            accommodationsToRemove.Add(accommodation);
                        }
                    }
                }
            }

            foreach (Accommodation accommodationToRemove in accommodationsToRemove)
            {
                accommodations.Remove(accommodationToRemove);
            }

            return accommodations;
        }
        
        public async Task<Accommodation> GetAccommodationById(int accommodationId)
        {
            return await _accommodation.GetAccommodationById(accommodationId);
        }

        public async Task<List<String>> GetDatesOccupiedForAccommodation(int accommodationId)
        {
            List<string> listOFDatesOccupied = new List<string>();

            var bookings = await bookingLogic.GetAllBookingsForAccommodation(accommodationId);
            var bookingsSorted = bookings.OrderBy(booking => booking.CheckInDate);

            foreach(Booking booking in bookingsSorted)
            {
                if(booking.ApprovalStatus == ApprovalStatus.Cancelled || booking.ApprovalStatus == ApprovalStatus.Declined
                   || booking.ApprovalStatus == ApprovalStatus.CancelledByUser)
                {
                    continue;
                }
                for (var currentDate = booking.CheckInDate; currentDate < booking.CheckOutDate; currentDate = currentDate.AddDays(1))
                {
                    listOFDatesOccupied.Add(currentDate.ToShortDateString());
                }
            }

            return listOFDatesOccupied;
        }
    }
}
