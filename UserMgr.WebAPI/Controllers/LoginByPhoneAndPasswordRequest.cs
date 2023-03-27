using UserMgr.Domain.Entities.ValueObjects;

namespace UserMgr.WebAPI.Controllers
{
    public record LoginByPhoneAndPasswordRequest(PhoneNumber PhoneNumber,string Password);
    public record AddUserRequest(PhoneNumber PhoneNumber, string Password);
}
