using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using WebApplication_Dragons.DTOs;
using WebApplication_Dragons.Models;

namespace WebApplication_Dragons.DataHandlers
{
    public class MongoDataHandler
    {
        public MongoDataHandler()
        {
            DB.InitAsync("Dragons", "localhost", 32768);
        }

        public async Task<IEnumerable<Tune>> GetAllTunes()
        {
            return await DB.Find<Tune>().ManyAsync(_ => true);
        }

        public async Task InsertNewTune(Tune newTune)
        {
            await DB.InsertAsync<Tune>(newTune);
        }

        public async Task CreateNewUser(Account account)
        {
            await DB.InsertAsync<Account>(account);
        }

        public async Task<Account?> GetUser(string username)
        {
            var matchingUser = await DB.Find<Account>().ManyAsync(q => q.Username.Equals(username));
            if (matchingUser == null) return null;
            if (matchingUser.Count() == 0) return null;

            var account = matchingUser.First();
            return account;
        }

        public async Task<bool> IsUsernameUsed(string username)
        {
            return await DB.Find<Account>().Match(a => a.Username.Equals(username)).ExecuteAnyAsync();
        }
    }
}
