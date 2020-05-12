using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MagazynManager.Domain.Entities.Uzytkownicy
{
    public class User : BaseEntity<Guid>
    {
        public string Email { get; private set; }
        public int Age { get; private set; }
        public string PasswordHash { get; }
        public string Salt { get; private set; }
        public IEnumerable<Claim> Claims { get; set; }
        public Guid PrzedsiebiorstwoId { get; }

        private User()
        {
        }

        public User(Guid id, string email, string passwordHash, int age, string salt, IEnumerable<Claim> userClaims, Guid przedsiebiorstwoId)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            Age = age;
            Salt = salt;
            Claims = userClaims;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }

        public static User RegisterUser(string email, int age)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                Age = age,
                Salt = Guid.NewGuid().ToString()
            };
        }

        public string GetPasswordHash(string password)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password + Salt));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public bool ValidatePassword(string password)
        {
            return GetPasswordHash(password) == PasswordHash;
        }
    }
}