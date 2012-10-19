using System;
using System.Collections.Generic;
using AjaxLogIn.Models;

namespace AjaxLogIn.Infrastructure
{
    public interface UserService
    {
        bool ValidateUser(LogInModel model);
        void CreateUser(SignUpModel model);
        IEnumerable<string> ListUsers();
    }

    public class InMemoryUserService : UserService
    {
        private static readonly IDictionary<string, string> s_UsersToPasswords = new Dictionary<string, string>();

        public bool ValidateUser(LogInModel model)
        {
            return s_UsersToPasswords.ContainsKey(model.Email)
                && s_UsersToPasswords[model.Email] == model.Password;
        }

        public void CreateUser(SignUpModel model)
        {
            if (s_UsersToPasswords.ContainsKey(model.Email))
            {
                throw new Exception("User already exists.");
            }
            s_UsersToPasswords[model.Email] = model.Password;
        }

        public IEnumerable<string> ListUsers()
        {
            return s_UsersToPasswords.Keys;
        }
    }
}