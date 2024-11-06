using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using urlshorter.Data;

namespace urlshorter.Controllers
{
    public class AccessLogController : Controller
    {
        private readonly AppDbContext _context;

        public AccessLogController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? urlId, string sortOrder)
        {
            var logs = _context.AccessLogs
                .Include(log => log.Url)
                .AsQueryable();

            if (urlId.HasValue)
            {
                logs = logs.Where(log => log.UrlId == urlId.Value);
            }

            logs = sortOrder switch
            {
                "date_desc" => logs.OrderByDescending(log => log.AccessedAt),
                "device" => logs.OrderBy(log => log.DeviceType),
                "device_desc" => logs.OrderByDescending(log => log.DeviceType),
                "os" => logs.OrderBy(log => log.OS),
                "os_desc" => logs.OrderByDescending(log => log.OS),
                _ => logs.OrderBy(log => log.AccessedAt),
            };

            var accessLogs = await logs.ToListAsync();
            return View(accessLogs);
        }
    }
}
