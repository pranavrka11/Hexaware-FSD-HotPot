﻿namespace HotPot.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }

        public User()
        {
            
        }

        public User(int id, string name, string email, string password, string phone)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Phone = phone;
        }

        public User(string name, string email, string password, string phone)
        {
            Name = name;
            Email = email;
            Password = password;
            Phone = phone;
        }
    }
}
