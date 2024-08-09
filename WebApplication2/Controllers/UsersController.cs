using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using DevExpress.XtraReports.UI;
using System.IO;
using WebApplication2.Reports;

namespace WebApplication2.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List users
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Details(int id)
        {
            // Fetch the user with the corresponding id and include related UserInfo
            var user = await _context.Users
                .Include(u => u.UserInfos)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Fetch the associated UserInfo
            var userInfo = await _context.UserInfos.FirstOrDefaultAsync(ui => ui.UserId == id);

            // Create an instance of the report
            var report = new TestReport();
            if (user.ReportData != null)
            {
                // Load the report layout from the stored binary data
                using (MemoryStream ms = new MemoryStream(user.ReportData))
                {
                    report.LoadLayoutFromXml(ms);
                }
            }

            // Prepare the ViewModel
            var viewModel = new UserDetailViewModel
            {
                User = user,
                UserInfo = userInfo,
                Report = report
            };

            // Pass the ViewModel to the view
            return View(viewModel);
        }


    }
}
