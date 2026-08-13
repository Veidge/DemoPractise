using System;
using System.Collections.Generic;
using System.Text;

namespace DemoTraining
{
    internal class User
    {
        private int id;
        private string username;
        private string password;

        private User() { }

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string GetUsername() 
        {
            return username!;
        }

        public string GetPassword()
        {
            return password!;
        }
    }
}
