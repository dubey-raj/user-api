using System.ComponentModel.DataAnnotations;

namespace UserService.Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string[] Roles { get; set; } = [];
    }

    public class RegisterCustomerRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string AddressFlat { get; set; }

        [Required]
        public string AddressStreet { get; set; }

        [Required]
        public string AddressCity { get; set; }

        [Required, RegularExpression(@"^\d{6}?$", ErrorMessage = "Invalid Zip Code.")]
        public string AddressZipCode { get; set; }

        [Required]
        public string AddressState { get; set; }
    }

    public class UpdateCustomerRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string AddressFlat { get; set; }

        [Required]
        public string AddressStreet { get; set; }

        [Required]
        public string AddressCity { get; set; }

        [Required, RegularExpression(@"^\d{6}?$", ErrorMessage = "Invalid Zip Code.")]
        public string AddressZipCode { get; set; }

        [Required]
        public string AddressState { get; set; }
    }
}
