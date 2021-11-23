using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreAuthMultiLang
{
    public class SessionToken
    {
        public string _id;
        public long userId;
        public string userName;
        public DateTime creationDT;
        public DateTime lastActionDT;

        public static string GenerateToken(IConfiguration Config, string username, string guid, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, guid),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            // ClaimsIdentity.DefaultRoleClaimType
            var bytes = System.Text.Encoding.UTF8.GetBytes(Config["jwt:Secret"]);
            var key = new SymmetricSecurityKey(bytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            var token = new JwtSecurityToken(
                //Config.GetValue<string>("jwt:Issuer"),
                //Config.GetValue<string>("jwt:Issuer") + "/ressources",
                claims: claims,
                expires: DateTime.Now.AddMinutes(Config.GetValue<int>("jwt:ExpireMinute")),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static async Task<Tuple<string, SessionToken>> CreateAsync(DBAbstraction db, IConfiguration config, string login, string role, long id)
        {
            string session = Guid.NewGuid().ToString();
            SessionToken newUser = new SessionToken()
            {
                _id = id + "_" + session,
                userId = id,
                userName = login,
                creationDT = DateTime.Now,
                //lastIp = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString()
            };
            newUser.lastActionDT = newUser.creationDT;
            // purge old tokens
            // find if user is already in the database and update is connection info
            return new Tuple<string, SessionToken>(GenerateToken(config, login, newUser._id, role), newUser);
        }

        //public static async Task AddConnectionIdAsync(MongoDB.Driver.MongoClient client, ClaimsPrincipal user, string connexionId)
        //{
        //    string _id = user?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        //    if (!string.IsNullOrEmpty(_id))
        //    {
        //        var task = GetCollection(client).FindAsync(Builders<SessionToken>.Filter.Eq("_id", _id));
        //        SessionToken t = await task.Result.FirstOrDefaultAsync();
        //        if (t != null)
        //        {
        //            if (t.signalRIds == null || t.signalRIds != null && !t.signalRIds.Contains(connexionId))
        //                await GetCollection(client).UpdateOneAsync(Builders<SessionToken>.Filter.Eq(a => a._id, t._id), Builders<SessionToken>.Update.AddToSet(aa => aa.signalRIds, connexionId).CurrentDate(aa => aa.lastActionDT));
        //        }
        //    }
        //}

        //public static async Task RemoveConnectionIdAsync(MongoDB.Driver.MongoClient client, ClaimsPrincipal user, string connexionId)
        //{
        //    string _id = user?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        //    if (!string.IsNullOrEmpty(_id))
        //    {
        //        var task = GetCollection(client).FindAsync(Builders<SessionToken>.Filter.Eq("_id", _id));
        //        SessionToken t = await task.Result.FirstOrDefaultAsync();
        //        if (t != null)
        //        {
        //            if (t.signalRIds == null || t.signalRIds != null && t.signalRIds.Contains(connexionId))
        //                await GetCollection(client).UpdateOneAsync(Builders<SessionToken>.Filter.Eq(a => a._id, t._id), Builders<SessionToken>.Update.Pull(aa => aa.signalRIds, connexionId).CurrentDate(aa => aa.lastActionDT));
        //        }
        //    }
        //}

        //public static async Task<SessionToken> GetUserTokenAsync(MongoDB.Driver.MongoClient client, ClaimsPrincipal user)
        //{
        //    string _id = user?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        //    if (!string.IsNullOrEmpty(_id))
        //    {
        //        var task = GetCollection(client).FindAsync(Builders<SessionToken>.Filter.Eq("_id", _id));
        //        SessionToken t = await task.Result.FirstOrDefaultAsync();
        //        if (t != null)
        //        {
        //            var tt = await GetCollection(client).UpdateOneAsync(Builders<SessionToken>.Filter.Eq("_id", t._id), Builders<SessionToken>.Update.CurrentDate(aa => aa.lastActionDT));
        //            t.lastActionDT = DateTime.Now.ToUniversalTime();
        //        }
        //        return t;
        //    }
        //    else
        //        return null;
        //}

        public static string GetService(ClaimsPrincipal user)
        {
            return user?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        }

        public static long CurrentUser(ClaimsPrincipal user)
        {
            var stringId = user?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            string id = "0";
            if (!string.IsNullOrEmpty(stringId))
            {
                string[] items = stringId.Split('_');
                if (items.Length > 0)
                    id = items[0];
            }
            long.TryParse(id ?? "0", out long userId);
            return userId;
        }
    }
}
