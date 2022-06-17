using AutoMapper;
using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Api.DTOs;
using Sat.Recruitment.Api.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public class LoadFileService : ILoadFileService
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public LoadFileService(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task LoadUsersFromFileAsync(string FullPath)
        {
            if (!string.IsNullOrEmpty(FullPath) && File.Exists(FullPath))
            {
                using (StreamReader sr = new StreamReader(FullPath))
                {
                    while (sr.Peek() >= 0)
                    {
                        var line = sr.ReadLineAsync().Result;
                        await ProcessLineAndSaveAsync(line);
                    }
                }
            }
        }

        private async Task ProcessLineAndSaveAsync(string line)
        {
            string[] values = line.Split(',');

            var userCreation = new UserCreationDTO
            {
                Name = values[0].ToString(),
                Email = values[1].ToString(),
                Phone = values[2].ToString(),
                Address = values[3].ToString(),
                Type = Enum.TryParse(values[4].ToString(), out UserTypes userType) ? userType : UserTypes.Normal,
                Money = decimal.Parse(values[5].ToString()),
            };

            List<ValidationResult> errors = new List<ValidationResult>();
            if (Validator.TryValidateObject(userCreation, new ValidationContext(userCreation), errors, true))
            {
                //Normalize email
                userCreation.Email = userService.GetEmailNormalized(userCreation.Email);

                bool userIsDuplicated = await userService.CheckUserIsDuplicated(userCreation);

                if (!userIsDuplicated)
                {
                    User user = mapper.Map<User>(userCreation);

                    await userService.AddUser(user);
                }
            }
        }
    }
}
