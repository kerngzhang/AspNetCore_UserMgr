using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain.Entities.ValueObjects;
using Zack.Commons;

namespace UserMgr.Domain.Entities
{
    public class User:IAggregateRoot
    {
        public Guid Id { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        private string? passwordHash;
        public UserAccessFail UserAccessFail { get; private set; }
        private User() { }
        public User(PhoneNumber phoneNumber)
        {
            this.PhoneNumber = phoneNumber;
            Id=Guid.NewGuid();
            UserAccessFail = new UserAccessFail(this);
        }

        public bool HasPassword()
        {
            return !string.IsNullOrWhiteSpace(passwordHash);
        }

        public void ChangePassword(string password)
        {
            if(password.Length<=3)
            {
                throw new ArgumentOutOfRangeException("密码长度必须大于3");
            }
            passwordHash=HashHelper.ComputeMd5Hash(password);
        }

        public bool CheckPassword(string password)
        {
            return passwordHash == HashHelper.ComputeMd5Hash(password);
        }

        public void ChangePhoneNumber(PhoneNumber phoneNumber)
        {
            this.PhoneNumber=phoneNumber;
        }

    }
}
