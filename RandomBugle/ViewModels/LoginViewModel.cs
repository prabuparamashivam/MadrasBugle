using System.ComponentModel.DataAnnotations;

namespace RandomBugle.ViewModels
{
    public class LoginViewModel
    { 
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
