using EncryptPad.Shared.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using SQLite;

namespace EncryptPad.Actions
{
    public class SqliteCleanupAction : ActionFilterAttribute
    {
        private readonly SQLiteAsyncConnection _dbContext;

        public SqliteCleanupAction(SQLiteAsyncConnection dbContext)
        {
            _dbContext = dbContext;
        }

        public override async void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var db = new SQLiteAsyncConnection(DataSource.databasePath);
            var query = _dbContext.Table<OTPKey>();
            var result = await query.ToListAsync();

            foreach (var key in result)
            {
                if (key.Date.AddHours(2) < DateTime.Now)
                {
                    await _dbContext.DeleteAsync<OTPKey>(key.Id);
                }
            }
        }
    }
}