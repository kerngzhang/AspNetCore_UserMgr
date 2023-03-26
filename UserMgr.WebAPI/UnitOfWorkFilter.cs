using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace UserMgr.WebAPI
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //执行Action方法
            var result = await next();
            if(result.Exception!= null)
            {//只有Action执行成功，才自动调用SaveChanges
                return;
            }
            var actionDesc = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDesc == null) return;
            var uowAttr = actionDesc.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            if (uowAttr == null) return;
            foreach (var dbContext in uowAttr.DbContextTypes)
            {
                //向DI拿DbContext实例
                var dbCtx = context.HttpContext.RequestServices.GetService(dbContext) as DbContext;
                if(dbCtx!=null)
                {
                    await dbCtx.SaveChangesAsync();
                }
            }
        }
    }
}
