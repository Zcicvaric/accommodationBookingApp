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
        private readonly IAccommodation accommodationFunctions = new DataAccess.Functions.AccommodationFunctions();
        private readonly BookingLogic bookingLogic = new BookingLogic();

        public async Task<bool> CreateNewAccomodation(string name, string city, string address, int numberOfBeds, int pricePerNight, int currencyId,
                                                         bool requireApproval, int accommodationTypeId, string checkInTime, string checkOutTime, 
                                                         string accommodationOwnerUsername, bool userCanCancelBooking, string accommodationImagesFolder, IFormFile AccommodationHeaderPhoto,
                                                         List<IFormFile> AcommodationPhotos)
        {

            var newAccommodation = new Accommodation {
                Name = name,
                City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(city),
                Address = address,
                NumberOfBeds = numberOfBeds,
                PricePerNight = pricePerNight,
                RequireApproval = requireApproval,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime,
                UserCanCancelBooking = userCanCancelBooking
            };

            try
            {
                var headerPhotoFileName = Guid.NewGuid().ToString() + "_" + AccommodationHeaderPhoto.FileName;
                newAccommodation.HeaderPhotoFileName = headerPhotoFileName;

                var result = await accommodationFunctions.CreateAccommodation(newAccommodation, accommodationTypeId, currencyId, accommodationOwnerUsername);

                if(result.Id > 0)
                {
                    var directoryPath = Path.Combine(accommodationImagesFolder, (name + "_" + result.Id.ToString()));
                    var headerFolderPath = Path.Combine(directoryPath, "Header");
                    Directory.CreateDirectory(directoryPath);
                    Directory.CreateDirectory(headerFolderPath);

                    var headerPhotoFilePath = Path.Combine(headerFolderPath, headerPhotoFileName);
                    await AccommodationHeaderPhoto.CopyToAsync(new FileStream(headerPhotoFilePath, FileMode.Create));

                    foreach (var formFile in AcommodationPhotos)
                    {
                        var photoFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                        var photoFilePath = Path.Combine(accommodationImagesFolder, (name + "_" + result.Id.ToString()), photoFileName);
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

        public async Task<bool> DeleteAccommodationAsync(Accommodation accommodationToDelete, string accommodationPhotosFolder)
        {
            try
            {
                var deleteResult = await accommodationFunctions.DeleteAccommodationAsync(accommodationToDelete);
                
                if (deleteResult)
                {
                    var accommodationPhotosDirectoryPath = Path.Combine(accommodationPhotosFolder, accommodationToDelete.Name
                                                                           + "_" + accommodationToDelete.Id);
                    Directory.Delete(accommodationPhotosDirectoryPath, true);
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Accommodation>> GetAccommodations()
        {
            var accommodations = await accommodationFunctions.GetAllAccommodations();

            return accommodations;
        }

        public async Task<List<Accommodation>> GetAccommodationsWithUserId(string userId)
        {
            var accommodations = await accommodationFunctions.GetAccommodationsWithUserIdAsync(userId);

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
            var accommodationsToRemove = new List<Accommodation>();

            var accommodations = await accommodationFunctions.GetFilteredAccommodations(accommodationCity, numberOfGuests);

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
            foreach (var accommodation in accommodations)
            {
                var bookingsForAccommodation = await bookingLogic.GetAllBookingsForAccommodation(accommodation.Id);
                
                if (bookingsForAccommodation.Count != 0)
                {
                    foreach (var booking in bookingsForAccommodation)
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
                            var accommodationCheckInTimeInt = int.Parse(accommodation.CheckInTime.Split(":")[0]);
                            if (accommodationCheckInTimeInt > int.Parse(latestCheckInTime.Split(":")[0]))
                            {
                                accommodationsToRemove.Add(accommodation);
                                continue;
                            }
                        }
                        if (earliestCheckOutTime != null)
                        {
                            var accommodationCheckOutTimeInt = int.Parse(accommodation.CheckOutTime.Split(":")[0]);
                            if (accommodationCheckOutTimeInt < int.Parse(earliestCheckOutTime.Split(":")[0]))
                            {
                                accommodationsToRemove.Add(accommodation);
                                continue;
                            }
                        }
                    }
                }
            }

            foreach (var accommodationToRemove in accommodationsToRemove)
            {
                accommodations.Remove(accommodationToRemove);
            }

            return accommodations;
        }
        
        public async Task<Accommodation> GetAccommodationById(int accommodationId)
        {
            return await accommodationFunctions.GetAccommodationById(accommodationId);
        }

        public async Task<List<string>> GetDatesOccupiedForAccommodation(int accommodationId)
        {
            var listOFDatesOccupied = new List<string>();
            var dateFormat = "dd.MM.yyyy.";

            var bookings = await bookingLogic.GetAllBookingsForAccommodation(accommodationId);
            var bookingsSorted = bookings.OrderBy(booking => booking.CheckInDate);

            foreach(var booking in bookingsSorted)
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
