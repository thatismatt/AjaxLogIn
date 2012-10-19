using System.Web.Security;

namespace AjaxLogIn.Infrastructure
{
    public interface IAuthService
    {
        void SetAuthCookie(string userName, bool rememberMe);
        void SignOut();
    }

    public class AuthService : IAuthService
    {
        public void SetAuthCookie(string userName, bool rememberMe)
        {
            FormsAuthentication.SetAuthCookie(userName, rememberMe);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}