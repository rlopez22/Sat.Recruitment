using Sat.Recruitment.Api.Entities;

namespace Sat.Recruitment.Api.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public UserTypes Type { get; set; }

        public decimal Money { get; set; }
    }
}
