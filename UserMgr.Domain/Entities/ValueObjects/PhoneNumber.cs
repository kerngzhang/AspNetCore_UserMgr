using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgr.Domain.Entities.ValueObjects
{
    public record PhoneNumber(int RegionNumber,string Number);
}
