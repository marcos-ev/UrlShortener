using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using urlshorter.Data;
using urlshorter.Models;

namespace urlshorter.Services
{
    public class UrlService
    {
        private readonly AppDbContext _context;

        public UrlService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Url> CreateShortUrlAsync(string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
                throw new ArgumentException("A URL original não pode ser nula ou vazia.");

            var existingUrl = await _context.Urls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);
            if (existingUrl != null)
                return existingUrl;

            var shortCode = GenerateShortCode();
            var url = new Url { OriginalUrl = originalUrl, ShortCode = shortCode };
            _context.Urls.Add(url);
            await _context.SaveChangesAsync();
            return url;
        }

        public async Task<Url?> GetUrlByShortCodeAsync(string shortCode)
        {
            return await _context.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        }

        public async Task<AccessLog?> GetLastAccessLogAsync(int urlId)
        {
            return await _context.AccessLogs
                .Where(log => log.UrlId == urlId)
                .OrderByDescending(log => log.AccessedAt)
                .FirstOrDefaultAsync();
        }

        public async Task LogAccessAsync(int urlId, string deviceType, string os, string browser, string ipAddress)
        {
            var accessLog = new AccessLog
            {
                UrlId = urlId,
                AccessedAt = DateTime.UtcNow,
                DeviceType = deviceType,
                OS = os,
                Browser = browser,
                IpAddress = ipAddress
            };
            _context.AccessLogs.Add(accessLog);
            await _context.SaveChangesAsync();
        }

        private string GenerateShortCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
