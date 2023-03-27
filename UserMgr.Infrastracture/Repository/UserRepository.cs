using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain;
using UserMgr.Domain.Entities;
using UserMgr.Domain.Entities.ValueObjects;
using UserMgr.Domain.Events;

namespace UserMgr.Infrastracture.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext dbContext;
        private readonly IDistributedCache distributedCache;
        private readonly IMediator mediator;

        public UserRepository(UserDbContext dbContext,IDistributedCache distributedCache,IMediator mediator)
        {
            this.dbContext = dbContext;
            this.distributedCache = distributedCache;
            this.mediator = mediator;
        }

        public async Task AddNewLoginHistory(PhoneNumber phoneNumber, string message)
        {
            var user = await FindOneAsync(phoneNumber);
            Guid? userId = null;
            if(user!=null)userId=user.Id;
            dbContext.UserLoginHistories.Add(new UserLoginHistory(userId,phoneNumber,message));
        }

        public async Task<User?> FindOneAsync(PhoneNumber phoneNumber)
        {
            return await dbContext.Users.Include(c=>c.UserAccessFail).SingleOrDefaultAsync(c => 
            c.PhoneNumber.Number == phoneNumber.Number &&
            c.PhoneNumber.RegionNumber == phoneNumber.RegionNumber);
        }

        public async Task<User?> FindOneAsync(Guid userId)
        {
            return await dbContext.Users.Include(c => c.UserAccessFail).SingleOrDefaultAsync(c =>c.Id==userId);
        }

        public async Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber)
        {
            //使用缓存
            var key = $"PhoneNumberCode_{phoneNumber.RegionNumber}_{phoneNumber.Number}";
            var code = await distributedCache.GetStringAsync(key);
            distributedCache.Remove(key);
            return code;
        }

        public Task PublishEventAsync(UserAccessResultEvent _event)
        {
            return mediator.Publish(_event);
        }

        public Task SavePhoneNumberCodeAsync(PhoneNumber phoneNumber, string code)
        {
            //使用缓存
            var key = $"PhoneNumberCode_{phoneNumber.RegionNumber}_{phoneNumber.Number}";
            //int code = Random.Shared.Next(1000,9999)
            return distributedCache.SetStringAsync(key, code, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            }) ;
        }
    }
}
