using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain.Entities.ValueObjects;

namespace UserMgr.Domain
{
    public interface ISmsCodeSender
    {
        Task SendAsync(PhoneNumber phoneNumber,string code);
    }
}
