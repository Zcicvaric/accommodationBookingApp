using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IUser
    {
        Task<IdentityResult> CreateNewUserAsync(ApplicationUser applicationUser, string password, bool registerAsHost);
        Task<bool> SignInUserAsync(ApplicationUser user, string password, bool permanentCookie);
        Task<bool> SignOutUserAsync();
        Task<List<ApplicationUser>> GetAllUserAccountsAsync();
        Task<List<ApplicationUser>> GetAllHostAccountsAsync();
    }
}
