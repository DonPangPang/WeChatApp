using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatApp.AdminClient.Auth
{
    public interface ILoginService
    {
        Task Login(string token);
        Task Logout();
    }
}