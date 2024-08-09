using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("UserInfo")]
    public class UserInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public User User { get; set; }
    }
}
