using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMgr.Domain;
using UserMgr.Infrastracture;

namespace UserMgr.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserDomainService userDomainService;

        public LoginController(UserDomainService userDomainService)
        {
            this.userDomainService = userDomainService;
        }
        [UnitOfWork(typeof(UserDbContext))]
        [HttpPost]
        public async Task<IActionResult> LoginByPhoneAndPassword(LoginByPhoneAndPasswordRequest req)
        {
            if(req.Password.Length<=3) { return BadRequest("密码长度需要大于3"); }
            var result = await userDomainService.CheckPassword(req.PhoneNumber,req.Password);
            switch (result)
            {
                case UserAccessResult.OK:
                    return BadRequest("登陆成功");
                case UserAccessResult.PhoneNumberNotFound:
                    return BadRequest("登陆失败");
                case UserAccessResult.LockOut:
                    return BadRequest("帐号锁定");
                case UserAccessResult.NoPassword:
                case UserAccessResult.PasswordError:
                    return BadRequest("登陆失败");
                default:
                    throw new ApplicationException("未知");
            }
        }
    }
}
