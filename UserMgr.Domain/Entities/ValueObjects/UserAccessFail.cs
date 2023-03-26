using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserMgr.Domain.Entities.ValueObjects
{
    public record UserAccessFail
    {
        public Guid Id { get; init; }
        public User User { get; init; }
        public Guid UserId { get; init; }
        private bool isLockOut;
        public DateTime? LockEnd { get; private set; }
        public int AccessFailCount { get; private set; }
        private UserAccessFail() { }
        public UserAccessFail(User user)
        {
            Id=Guid.NewGuid();
            User = user;
        }

        public void Reset()
        {
            AccessFailCount= 0;
            LockEnd=null; 
            isLockOut=false;
        }

        public void Fail()
        {
            AccessFailCount++;
            if(this.AccessFailCount>=3)
            {
                LockEnd = DateTime.Now.AddMinutes(5);
                isLockOut=true;
            }
        }

        public bool IsLockOut()
        {
            if(isLockOut)
            {
                if(DateTime.Now>this.LockEnd)//超过锁定时间
                {
                    Reset();
                    return false;
                }
                else 
                    return true;
            }
            else
            {
                return false;
            }
        }
    }
}
