using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WeChatApp.AdminClient.Services;
using WeChatApp.Shared.GlobalVars;

namespace WeChatApp.AdminClient.Auth
{
    public class JwtAuthProvider : AuthenticationStateProvider, ILoginService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IHttpFunc _httpFunc;

        private AuthenticationState anonimo => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public JwtAuthProvider(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory, IHttpFunc httpFunc)
        {
            _localStorage = localStorage;
            _httpClientFactory = httpClientFactory;

            _httpFunc = httpFunc;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>(GlobalVars.ClientTokenKey);

            if (string.IsNullOrEmpty(savedToken))
            {
                return anonimo!;
            }

            return BuildAuthenticationState(savedToken);
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            _httpFunc.SetToken(token);
            //_httpClientFactory.CreateClient(ApiVars.ApiBase).DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        public async Task Login(string token)
        {
            await _localStorage.SetItemAsync(GlobalVars.ClientTokenKey, token);

            var authState = BuildAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            _httpClientFactory.CreateClient(ApiVars.ApiBase).DefaultRequestHeaders.Remove("Authorization");

            await _localStorage.RemoveItemAsync(GlobalVars.ClientTokenKey);

            NotifyAuthenticationStateChanged(Task.FromResult(anonimo!));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs!.TryGetValue(ClaimTypes.Role, out object? roles);

            if (roles != null)
            {
                if (roles.ToString()!.Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);

                    foreach (var parsedRole in parsedRoles!)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}