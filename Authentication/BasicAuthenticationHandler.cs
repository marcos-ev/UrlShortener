using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace urlshorter.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly TimeProvider _timeProvider;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            TimeProvider timeProvider)
            : base(options, logger, encoder)
        {
            _timeProvider = timeProvider;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader))
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header");
                }

                var parsedAuthHeader = AuthenticationHeaderValue.Parse(authHeader);

                if (parsedAuthHeader.Parameter == null)
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header");
                }

                var credentialBytes = Convert.FromBase64String(parsedAuthHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                if (credentials.Length != 2)
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header Format");
                }

                var username = credentials[0];
                var password = credentials[1];

                if (username == "admin" && password == "password")
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, username) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return await Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    return await Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
                }
            }
            catch
            {
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }
    }
}
