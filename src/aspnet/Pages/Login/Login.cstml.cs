using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace skyblock_bot.Pages;

public class LoginModel : RootModel
{
    public LoginModel(ILogger<LoginModel> logger) : base(logger) { }

    public sealed override async Task CustomPost()
    {
        FilterDefinition<MongoUser> filter = new FilterDefinitionBuilder<MongoUser>()
            .Where(e => e.Username == Credentials.Username && e.Password == Credentials.Password);

        IAsyncCursor<MongoUser>? test = await MongoBot.MongoUser.FindAsync(filter);

        if (test == null)
        {
            Program.Utility.Log(Enums.LogLevel.WARN, "Request for MongoUser returned null!");
            return;
        }
    }
}
