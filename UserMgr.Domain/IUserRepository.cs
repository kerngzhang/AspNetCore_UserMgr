using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain.Entities;
using UserMgr.Domain.Entities.ValueObjects;
using UserMgr.Domain.Events;

namespace UserMgr.Domain
{
    public interface IUserRepository
    {
        public Task<User?> FindOneAsync(PhoneNumber phoneNumber);
        public Task<User?> FindOneAsync(Guid userId);
        public Task AddNewLoginHistory(PhoneNumber phoneNumber, string message);
        public Task SavePhoneNumberCodeAsync(PhoneNumber phoneNumber,string code);//验证码
        public Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber);
        public Task PublishEventAsync(UserAccessResultEvent _event);
    }
}
