using Sat.Recruitment.Api.DTOs;
using Sat.Recruitment.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public interface IUserService
    {
        Task AddUser(User user);
        Task<bool> CheckUserIsDuplicated(UserCreationDTO userCreation);
        Task<List<User>> GetAllUsers();
        string GetEmailNormalized(string email);
        decimal GetMoneyGift(UserTypes UserType, decimal Money);
        Task<User> GetUserByID(int id);
    }
}