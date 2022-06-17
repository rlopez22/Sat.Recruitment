using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sat.Recruitment.Api.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(maximumLength: 20, ErrorMessage = "{0}  - Incorrect maximum length ({1})")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "{0} is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string Address { get; set; }

        [Phone]
        [Required(ErrorMessage = "{0} is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DefaultValue(UserTypes.Normal)]
        public UserTypes Type { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Money { get; set; }
    }
}
