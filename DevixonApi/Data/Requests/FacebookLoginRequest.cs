using System.ComponentModel.DataAnnotations;

namespace DevixonApi.Data.Requests
{
    public class FacebookLoginRequest
    {
        [Required]
        [StringLength(255)]
        public string FacebookToken { get; set; }
    }
}