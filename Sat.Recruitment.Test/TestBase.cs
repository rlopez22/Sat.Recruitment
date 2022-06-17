using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Api;
using Sat.Recruitment.Api.Utils;

namespace Sat.Recruitment.Test
{
    public class TestBase
    {
        protected ApplicationDbContext CreateContext(string nombreDB)
        {
            var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(nombreDB)
                .Options;

            var dbContext = new ApplicationDbContext(opciones);
            return dbContext;
        }

        protected IMapper ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfiles());
            });

            return config.CreateMapper();
        }
    }
}
