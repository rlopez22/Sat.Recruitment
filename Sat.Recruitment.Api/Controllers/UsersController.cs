using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Api.DTOs;
using Sat.Recruitment.Api.Entities;
using Sat.Recruitment.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet] // api/users
        public async Task<List<UserDTO>> Get()
        {
            List<User> users = await userService.GetAllUsers();
            return mapper.Map<List<UserDTO>>(users);
        }

        [HttpGet("{id}", Name = "getUser")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            object user = await userService.GetUserByID(id);

            if (user == null)
            {
                return NotFound();
            }

            return mapper.Map<UserDTO>(user);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserCreationDTO userCreation)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(userCreation, new ValidationContext(userCreation), errors, true))
            {
                return BadRequest(string.Join("\r\n", errors.Select(x => x?.ErrorMessage)));
            }
            else
            {
                //Normalize email
                userCreation.Email = userService.GetEmailNormalized(userCreation.Email);

                bool userIsDuplicated = await userService.CheckUserIsDuplicated(userCreation);

                if (userIsDuplicated)
                {
                    return BadRequest("The user is duplicated.");
                }

                User user = mapper.Map<User>(userCreation);

                await userService.AddUser(user);

                UserDTO userDTO = mapper.Map<UserDTO>(user);
                return new CreatedAtRouteResult("getUser", new { id = user.Id }, userDTO);
            }
        }
    }
}
