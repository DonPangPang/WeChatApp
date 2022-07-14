using Microsoft.AspNetCore.Authorization;

#pragma warning disable 1591

namespace WeChatApp.WebApp.Auth
{
    /// <summary>
    /// Token
    /// </summary>
    public class TokenParameter : IAuthorizationRequirement
    {
        public string Name { get; set; } = null!;
        public string Secret { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;

        public int AccessExpiration { get; set; }
        public int RefreshExpiration { get; set; }
    }
}

#pragma warning restore 1591