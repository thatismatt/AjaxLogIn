using System.ComponentModel.DataAnnotations;

namespace AjaxLogIn.Models
{
    public class LogInModel
    {
        [Required]
        [Display(Name = "Email address")]
        public string Email { get { return m_Email; } set { m_Email = value.Trim(); } }
        private string m_Email;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool? RememberMe { get; set; }
    }
}