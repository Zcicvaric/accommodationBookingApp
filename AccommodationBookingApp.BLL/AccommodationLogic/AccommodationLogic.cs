using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
                                                         string applicationUserId,
                                                         string accommodationImagesFolder,
                                                         IFormFile AccommodationHeaderPhoto,
                                                         List<IFormFile> AcommodationPhotos)
        {
            //convert accommodation city names to have first character in every word uppercase kastel stari => Kastel Stari
            accommodation.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(accommodation.City);
            try
            {
                var headerPhotoFileName = Guid.NewGuid().ToString() + "_" + AccommodationHeaderPhoto.FileName;
                accommodation.HeaderPhotoFileName = headerPhotoFileName;
                var result = await _accommodation.CreateAccommodation(accommodation, applicationUserId);
                if(result.Id > 0)
                {
                    string directoryPath = Path.Combine(accommodationImagesFolder, accommodation.Name);
                    string headerFolderPath = Path.Combine(directoryPath, "Header");
                    Directory.CreateDirectory(directoryPath);
                    Directory.CreateDirectory(headerFolderPath);

                    string headerPhotoFilePath = Path.Combine(headerFolderPath, headerPhotoFileName);
                    await AccommodationHeaderPhoto.CopyToAsync(new FileStream(headerPhotoFilePath, FileMode.Create));

                    foreach (IFormFile formFile in AcommodationPhotos)
                    {
                        var photoFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                        string photoFilePath = Path.Combine(accommodationImagesFolder, accommodation.Name, photoFileName);
                        await formFile.CopyToAsync(new FileStream(photoFilePath, FileMode.Create));
                    }

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

        public async Task<List<Accommodation>> GetFilteredAccommodations(string accommodationCity, int numberOfGuests,
                                                                         DateTime checkInDate, DateTime checkOutDate,
                                                                         int accommodationTypeId, string latestCheckInTime,
                                                                         string earliestCheckOutTime,
                                                                         bool showOnlyAccommodationsWithInstantBooking,
                                                                         bool showOnlyAccommodationsWhereUserCanCancelBooking)
        {
            //convert accommodation city names to have every word's first character uppercase kastel stari => Kastel Stari
            accommodationCity = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(accommodationCity);
            List<Accommodation> accommodationsToRemove = new List<Accommodation>();

            List<Accommodation> accommodations = await _accommodation.GetFilteredAccommodations(accommodationCity, numberOfGuests);

            if (accommodationTypeId != 0)
            {
                accommodations = accommodations.Where(accommodation => accommodation.AccommodationType.Id == accommodationTypeId).ToList();
            }
            if (showOnlyAccommodationsWithInstantBooking)
            {
                accommodations = accommodations.Where(accommodation => accommodation.RequireApproval == false).ToList();
            }
            if (showOnlyAccommodationsWhereUserCanCancelBooking)
            {
                accommodations = accommodations.Where(accommodation => accommodation.UserCanCancelBooking == true).ToList();
            }

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
                            continue;
                        }
                        if (latestCheckInTime != null)
                        {
                            int accommodationCheckInTimeInt = Int32.Parse(accommodation.CheckInTime.Split(":")[0]);
                            if (accommodationCheckInTimeInt > Int32.Parse(latestCheckInTime.Split(":")[0]))
                            {
                                accommodationsToRemove.Add(accommodation);
                                continue;
                            }
                        }
                        if (earliestCheckOutTime != null)
                        {
                            int AccommodationCheckOutTimeInt = Int32.Parse(accommodation.CheckOutTime.Split(":")[0]);
                            if (AccommodationCheckOutTimeInt < Int32.Parse(earliestCheckOutTime.Split(":")[0]))
                            {
                                accommodationsToRemove.Add(accommodation);
                                continue;
                            }
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
            string dateFormat = "dd.MM.yyyy.";

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

                    listOFDatesOccupied.Add(currentDate.ToString(dateFormat));
                }
            }

            return listOFDatesOccupied;
        }
    }
}
