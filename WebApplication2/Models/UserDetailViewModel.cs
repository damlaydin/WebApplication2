using DevExpress.XtraReports.UI;
using WebApplication2.Reports;

namespace WebApplication2.Models
{
    public class UserDetailViewModel
    {
        public User User { get; set; }
        public UserInfo UserInfo { get; set; }
        public XtraReport Report { get; set; }
    }
}
