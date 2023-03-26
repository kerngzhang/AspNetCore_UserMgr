using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain.Entities.ValueObjects;

namespace UserMgr.Domain.Events
{
    public record UserAccessResultEvent(PhoneNumber PhoneNumber,UserAccessResult Result):INotification
    {
    }
}
