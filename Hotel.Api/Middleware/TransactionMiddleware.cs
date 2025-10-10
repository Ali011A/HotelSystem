
using Hotel.Infrastructure.Persistence;

namespace Hotel.Api.Middleware
{
    public class TransactionMiddleware : IMiddleware
    {
        ApplicationDbContext _Context;

        public TransactionMiddleware(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task InvokeAsync(HttpContext HttpContext, RequestDelegate next)
        {

            await using var transaction = await _Context.Database.BeginTransactionAsync();
            try
            {
                await next(HttpContext);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                throw;
            }



        }
    }
}
