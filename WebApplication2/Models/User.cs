using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string ReportName { get; set; }
        public byte[] ReportData { get; set; }
        public string UserName { get; set; }

        public ICollection<UserInfo> UserInfos { get; set; }

    }
}
