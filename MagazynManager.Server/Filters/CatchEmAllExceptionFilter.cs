using MagazynManager.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MagazynManager.Server.Filters
{
    public class CatchEmAllExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                if (context.Exception is SqlException)
                {
                    var ex = context.Exception as SqlException;
                    if (ex.Number == 2601 || ex.Number == 2627)
                    {
                        context.Result = new BadRequestObjectResult("Nie można wstawić wartości (duplikat)");
                    }
                }

                if (context.Exception is BussinessException)
                {
                    context.Result = new BadRequestObjectResult(context.Exception.Message);
                }
                else
                {
                    context.Result = new BadRequestResult();
                }
                context.ExceptionHandled = true;
            }

            return Task.CompletedTask;
        }
    }
}