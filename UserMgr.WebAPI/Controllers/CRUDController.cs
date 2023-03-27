using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMgr.Domain;
using UserMgr.Domain.Entities;
using UserMgr.Infrastracture;

namespace UserMgr.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly UserDbContext userDbContext;

        public CRUDController(IUserRepository userRepository,UserDbContext userDbContext)
        {
            this.userRepository = userRepository;
            this.userDbContext = userDbContext;
        }
        [UnitOfWork(typeof(UserDbContext))]
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserRequest req)
        {
            if(await userRepository.FindOneAsync(req.PhoneNumber)!=null) 
            {
                return BadRequest("手机号己经存在");
            }
            var user = new User(req.PhoneNumber);
            user.ChangePassword(req.Password);
            userDbContext.Users.Add(user);
            return Ok("完成");
        }
    }
}
