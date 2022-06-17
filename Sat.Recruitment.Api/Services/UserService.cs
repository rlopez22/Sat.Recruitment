using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Api.DTOs;
using Sat.Recruitment.Api.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddUser(User user)
        {
            //Get User gift
            user.Money = this.GetMoneyGift(user.Type, user.Money);

            context.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task<bool> CheckUserIsDuplicated(UserCreationDTO userCreation)
        {
            return await context.Users.AnyAsync(ExistingUser => ExistingUser.Email.Equals(userCreation.Email) ||
                                                                ExistingUser.Phone.Equals(userCreation.Phone) ||
                                                                (ExistingUser.Name.ToLower().Equals(userCreation.Name.ToLower()) &&
                                                                 ExistingUser.Address.ToLower().Equals(userCreation.Address.ToLower())));
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await context.Users.ToListAsync();
        }
        public string GetEmailNormalized(string email)
        {
            //I don't undestand the purpose of the original code but it is refactored to avoid exceptions...
            if (email != null && !string.IsNullOrEmpty(email) && email.Contains("@"))
            {
                var aux = email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

                var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

                aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

                email = string.Join("@", new string[] { aux[0], aux[1] });
            }
            else
            {
                return string.Empty;
            }

            return email;
        }

        public decimal GetMoneyGift(UserTypes UserType, decimal Money)
        {
            double giftPercentage = 0.0;

            switch (UserType)
            {
                case UserTypes.Normal:
                    giftPercentage = Money > 10 && Money < 100 ?
                                     0.8 :
                                     Money > 100 ? 0.12 : 0;
                    break;
                case UserTypes.SuperUser:
                    giftPercentage = Money > 100 ? 0.20 : 0;
                    break;
                case UserTypes.Premium:
                    giftPercentage = Money > 100 ? 2 : 0;
                    break;
                default:
                    break;
            }

            decimal percentage = Convert.ToDecimal(giftPercentage);
            decimal gif = Money * percentage;
            return Money += gif;
        }

        public async Task<User> GetUserByID(int id)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
