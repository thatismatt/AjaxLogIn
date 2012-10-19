using System;
using System.Web.Mvc;
using AjaxLogIn.Infrastructure;
using AjaxLogIn.Models;

namespace AjaxLogIn.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserService m_UserService;
        private readonly IAuthService m_AuthService;

        public AccountController(UserService userService, IAuthService authService)
        {
            m_UserService = userService;
            m_AuthService = authService;
        }

        [HttpPost]
        public JsonResult LogIn(LogInModel model)
        {
            if (ModelState.IsValid && m_UserService.ValidateUser(model))
            {
                var rememberMe = model.RememberMe.HasValue && model.RememberMe.Value;
                m_AuthService.SetAuthCookie(model.Email, rememberMe);
                return JsonSuccess(new { IsLoggedIn = true });
            }

            return JsonSuccess(new { IsLoggedIn = false, Error = "The user name and password provided are incorrect." });
        }

        public JsonResult LogOut()
        {
            m_AuthService.SignOut();
            return JsonSuccess();
        }

        [HttpPost]
        public JsonResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    m_UserService.CreateUser(model);
                }
                catch (Exception e)
                {
                    return JsonSuccess(new { IsRegistered = false, Error = e.Message });
                }

                const bool rememberMe = false;
                m_AuthService.SetAuthCookie(model.Email, rememberMe);
                return JsonSuccess(new { IsRegistered = true });
            }

            // If we got this far, something failed, redisplay form
            return JsonSuccess(new { IsRegistered = false, Error = "Invalid signup details." });
        }

        public JsonResult Details()
        {
            return JsonSuccess(new { Email = User.Identity.Name,  Request.IsAuthenticated });
        }

        public JsonResult Debug()
        {
            return JsonSuccess(m_UserService.ListUsers());
        }
    }
}
