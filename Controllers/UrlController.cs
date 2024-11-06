using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using urlshorter.Services;
using urlshorter.Models;
using urlshorter.Helpers;

namespace urlshorter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly UrlService _urlService;

        public UrlController(UrlService urlService)
        {
            _urlService = urlService;
        }

        [Authorize]
        [HttpPost("encurtar")]
        public async Task<IActionResult> ShortenUrl([FromBody] ShortenUrlRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.OriginalUrl))
                return BadRequest("A URL original é necessária.");

            var url = await _urlService.CreateShortUrlAsync(request.OriginalUrl);

            var deviceInfo = DeviceHelper.GetDeviceInfo(Request);
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            await _urlService.LogAccessAsync(url.Id, deviceInfo.DeviceType, deviceInfo.OS, deviceInfo.Browser, ipAddress);

            return Ok(new { ShortUrl = $"https://yourdomain.com/{url.ShortCode}" });
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToUrl(string shortCode)
        {
            var url = await _urlService.GetUrlByShortCodeAsync(shortCode);
            if (url == null)
            {
                return NotFound("URL encurtada não encontrada.");
            }

            var deviceInfo = DeviceHelper.GetDeviceInfo(Request);
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            var lastAccess = await _urlService.GetLastAccessLogAsync(url.Id);
            if (lastAccess == null || (DateTime.UtcNow - lastAccess.AccessedAt).TotalSeconds > 10)
            {
                await _urlService.LogAccessAsync(url.Id, deviceInfo.DeviceType, deviceInfo.OS, deviceInfo.Browser, ipAddress);
            }

            return Redirect(url.OriginalUrl);
        }
    }
}
