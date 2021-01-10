using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.AccommodationLogic
{
    public class AccommodationLogic
    {
        private readonly IAccommodation accommodationFunctions = new DataAccess.Functions.AccommodationFunctions();
        private readonly BookingLogic bookingLogic = new BookingLogic();

        public async Task<bool> CreateNewAccomodationAsync(string name, string city, string address, int numberOfBeds, int pricePerNight, int currencyId,
                                                         bool requireApproval, int accommodationTypeId, string checkInTime, string checkOutTime,
                                                         string accommodationOwnerUsername, bool userCanCancelBooking, string accommodationImagesFolder, IFormFile AccommodationHeaderPhoto,
                                                         List<IFormFile> AcommodationPhotos)
        {

            var newAccommodation = new Accommodation
            {
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
                newAccommodation.HeaderPhotoFileName = AccommodationHeaderPhoto.FileName;

                var result = await accommodationFunctions.CreateAccommodationAsync(newAccommodation, accommodationTypeId, currencyId, accommodationOwnerUsername);

                if (result.Id > 0)
                {
                    var accommodationHeaderPhotoUploadResult = await UploadHeaderPhoto(name, accommodationImagesFolder, result.Id, AccommodationHeaderPhoto, false);

                    if (!accommodationHeaderPhotoUploadResult)
                    {
                        return false;
                    }

                    var accommodationPhotosUploadResult = await UploadAccommodationPhotos(name, accommodationImagesFolder, result.Id, AcommodationPhotos);

                    if (!accommodationPhotosUploadResult)
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UploadHeaderPhoto(string accommodationName, string accommodationsRootPhotosDirectory,
                                                  int accommodationId, IFormFile accommodationHeaderPhoto, bool deleteOldHeaderPhoto)
        {
            try
            {
                var headerPhotoDirectoryPath = Path.Combine(accommodationsRootPhotosDirectory, (accommodationName + "_" + accommodationId.ToString()), "Header");
                var headerPhotoFilePath = Path.Combine(headerPhotoDirectoryPath, accommodationHeaderPhoto.FileName);

                if (deleteOldHeaderPhoto)
                {
                    var updateResult = await accommodationFunctions.UpdateAccommodationHeaderPhoto(accommodationId, accommodationHeaderPhoto.FileName);

                    if (!updateResult)
                    {
                        throw new Exception();
                    }

                    Directory.Delete(headerPhotoDirectoryPath, true);
                }

                Directory.CreateDirectory(headerPhotoDirectoryPath);

                await accommodationHeaderPhoto.CopyToAsync(new FileStream(headerPhotoFilePath, FileMode.Create));

                return true;
            }
            catch
            {

                return false;
            }
        }

        public async Task<bool> UploadAccommodationPhotos(string accommodationName, string accommodationsPhotosRootDirectory,
                                                          int accommodationId, List<IFormFile> accommodationPhotos)
        {
            try
            {
                var photosForAccommodationDirectoryPath = Path.Combine(accommodationsPhotosRootDirectory, (accommodationName + "_" + accommodationId.ToString()));
                Directory.CreateDirectory(photosForAccommodationDirectoryPath);

                foreach (var formFile in accommodationPhotos)
                {
                    var photoFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                    var photoFilePath = Path.Combine(photosForAccommodationDirectoryPath, photoFileName);
                    await formFile.CopyToAsync(new FileStream(photoFilePath, FileMode.Create));
                }

                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool DeleteAccommodationPhoto(string photoFilePath)
        {
            try
            {
                File.Delete(photoFilePath);
            }
            catch 
            {

                return false;
            }

            return true;
        }

        public async Task<bool> UpdateAccommodationAsync(int accommodationId, string name, int numberOfBeds, int pricePerNight,
                                                         int currencyId, bool requireApproval, bool userCanCancelBooking,
                                                         string checkInTime, string checkOutTime, string oldName, string accommodationImagesFolderPath)
        {
            bool updateResult = await accommodationFunctions.UpdateAccommodationAsync(accommodationId, name, numberOfBeds, pricePerNight, currencyId,
                                                                                      requireApproval, userCanCancelBooking, checkInTime, checkOutTime);

            if (updateResult)
            {
                var oldAccommodationPhotosDirectoryPath = Path.Combine(accommodationImagesFolderPath, (oldName + "_" + accommodationId.ToString()));
                var newAccommodationPhotosDirectoryName = name + "_" + accommodationId;

                try
                {
                    FileSystem.RenameDirectory(oldAccommodationPhotosDirectoryPath, newAccommodationPhotosDirectoryName);
                }
                catch (DirectoryNotFoundException)
                {
                    return false;
                }
                // Sometimes the directory can get locked by another process
                catch (IOException)
                {
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return updateResult;
        }

        public async Task<bool> DeleteAccommodationAsync(Accommodation accommodationToDelete, string accommodationPhotosFolder)
        {
            var accommodationPhotosDirectoryPath = Path.Combine(accommodationPhotosFolder, (accommodationToDelete.Name +
                                                                "_" + accommodationToDelete.Id.ToString()));

            var deleteResult = await accommodationFunctions.DeleteAccommodationAsync(accommodationToDelete);

            if (!deleteResult)
            {
                return false;
            }

            try
            {
                Directory.Delete(accommodationPhotosDirectoryPath, true);
            }
            catch (DirectoryNotFoundException)
            {
                return true;
            }
            // Sometimes the directory or a file in the directory get locked by another process and can't be deleted
            // in that case an IOException is raised
            catch (IOException e)
            {
                return true;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<List<Accommodation>> GetAllAccommodationsAsync()
        {
            var accommodations = await accommodationFunctions.GetAllAccommodationsAsync();

            return accommodations;
        }

        public async Task<List<Accommodation>> GetAccommodationsWithUserIdAsync(string userId)
        {
            var accommodations = await accommodationFunctions.GetAccommodationsWithUserIdAsync(userId);

            return accommodations;
        }

        public async Task<List<Accommodation>> GetFilteredAccommodationsAsync(string accommodationCity, int numberOfGuests,
                                                                         DateTime checkInDate, DateTime checkOutDate,
                                                                         int accommodationTypeId, string latestCheckInTime,
                                                                         string earliestCheckOutTime,
                                                                         bool showOnlyAccommodationsWithInstantBooking,
                                                                         bool showOnlyAccommodationsWhereUserCanCancelBooking)
        {
            // Convert accommodation city names to have every word's first character uppercase kastel stari => Kastel Stari
            accommodationCity = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(accommodationCity);
            var accommodationsToRemove = new List<Accommodation>();

            var accommodations = await accommodationFunctions.GetFilteredAccommodationsAsync(accommodationCity, numberOfGuests);

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

            
            foreach (var accommodation in accommodations)
            {
                var bookingsForAccommodation = await bookingLogic.GetAllUpcomingBookingsForAccommodationAsync(accommodation.Id);

                if (bookingsForAccommodation.Count != 0)
                {
                    foreach (var booking in bookingsForAccommodation)
                    {
                        if (checkInDate < booking.CheckOutDate && checkOutDate > booking.CheckInDate)
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

        public async Task<Accommodation> GetAccommodationByIdAsync(int accommodationId)
        {
            return await accommodationFunctions.GetAccommodationByIdAsync(accommodationId);
        }

        public async Task<List<string>> GetDatesOccupiedForAccommodationAsync(int accommodationId)
        {
            var listOFDatesOccupied = new List<string>();
            var dateFormat = "dd.MM.yyyy.";

            var bookings = await bookingLogic.GetAllBookingsForAccommodationAsync(accommodationId);

            foreach (var booking in bookings)
            {
                if (booking.ApprovalStatus == ApprovalStatus.Cancelled || booking.ApprovalStatus == ApprovalStatus.Declined
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

        public async Task<bool> CheckIfBookingDatesAreValid(DateTime checkInDate, DateTime checkOutDate, int accommodationId)
        {
            var bookings = await bookingLogic.GetAllUpcomingBookingsForAccommodationAsync(accommodationId);

            foreach (var booking in bookings)
            {
                if (checkInDate.Date < booking.CheckOutDate.Date && checkOutDate.Date > booking.CheckInDate.Date)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
