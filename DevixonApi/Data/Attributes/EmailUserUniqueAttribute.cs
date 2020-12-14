using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevixonApi.Data.Attributes
{
    public class EmailUserUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (AppDbContext) validationContext.GetService(typeof(AppDbContext));
            var entity = context.Users.SingleOrDefault(e => e.Email == value.ToString());

            if (entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string email)
        {
            return $"Email {email} is already in use.";
        }
    }
}