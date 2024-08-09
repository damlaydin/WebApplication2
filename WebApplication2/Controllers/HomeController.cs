using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication2.Models;
using DevExpress.XtraReports.Web;
using DevExpress.XtraReports.UI;
using WebApplication2.Reports;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> _logger, ApplicationDbContext _context)
        {
            this._logger = _logger;
            this._context = _context;
        }

        public IActionResult Index()
        {
            return View(new ReportViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SaveReport(ReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (user == null)
                {
                    // Create a new user if it doesn't exist
                    user = new User
                    {
                        UserName = model.UserName,
                        ReportName = model.ReportName
                    };
                    _context.Users.Add(user);
                }
                else
                {
                    // Update the existing user's report information
                    user.ReportName = model.ReportName;
                }

                // Create an instance of your specific report
                var report = new TestReport();
                using (MemoryStream ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    user.ReportData = ms.ToArray();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadReport(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user != null && user.ReportData != null)
            {
                // Create an instance of your specific report
                var report = new TestReport();
                using (MemoryStream ms = new MemoryStream(user.ReportData))
                {
                    report.LoadLayoutFromXml(ms);
                }

                // Pass the report to the view
                ViewBag.Report = report;
                return View("Index");
            }

            return RedirectToAction("Index");
        }

    }
}
