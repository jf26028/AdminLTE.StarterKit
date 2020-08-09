using System;
using System.Threading.Tasks;
using AdminLTE.StarterKit.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace AdminLTE.StarterKit.Infrastructure
{
    public class DbContextTransactionPageFilter : IAsyncPageFilter
    {
        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<ApplicationDbContext>();

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                var actionExecuted = await next();
                if (actionExecuted.Exception != null && !actionExecuted.ExceptionHandled)
                {
                    dbContext.Database.RollbackTransaction();
                }
                else
                {
                    // await _dbContext.Database.CommitTransactionAsync();
                    dbContext.Database.CommitTransaction(); // no async here?
                }
            }
            catch (Exception)
            {
                dbContext.Database.RollbackTransaction();
                throw;
            }
        }
    }
}