using MediatR;
using UserMgr.Domain;
using UserMgr.Domain.Events;
using UserMgr.Infrastracture;

namespace UserMgr.WebAPI
{
    public class UserAccessResultEventHandler : INotificationHandler<UserAccessResultEvent>
    {
        /*
        //也可以
        private readonly IUserRepository userRepository;
        private readonly UserDbContext userDbContext;

        public UserAccessResultEventHandler(IUserRepository userRepository, UserDbContext userDbContext)
        {
            this.userRepository = userRepository;
            this.userDbContext = userDbContext;
        }
        */
        private readonly IServiceScopeFactory serviceScopeFactory;

        public UserAccessResultEventHandler(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Handle(UserAccessResultEvent notification, CancellationToken cancellationToken)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var userDbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            await userRepository.AddNewLoginHistory(notification.PhoneNumber, $"登陆结果是{notification.Result}");
            await userDbContext.SaveChangesAsync();
        }
    }
}
