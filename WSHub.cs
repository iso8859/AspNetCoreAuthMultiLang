using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeideonAPIServer
{
    public class WSHub : Hub
    {
        public class AliveResult
        {
            public DateTime dt;
        }

        //IHttpContextAccessor HttpContextAccessor;

        //public WSHub(
        //    IHttpContextAccessor httpContextAccessor)
        //{
        //    HttpContextAccessor = httpContextAccessor;
        //}

        [AllowAnonymous]
        public async Task Alive(string user, string message)
        {
            await Clients.Caller.SendAsync("Alive", new AliveResult() { dt = DateTime.Now });
        }

        [AllowAnonymous]
        public async Task Ping(string machine)
        {
            await Clients.Caller.SendAsync("pong", Environment.MachineName, DateTime.Now);
        }

        /// <summary>
        /// Create the link between this SignalR id and JWT id
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task BindConnectionId()
        {
            //await Structs.SessionToken.AddConnectionIdAsync(Global.Client, Context.GetHttpContext()?.User, Context.ConnectionId);
        }

        [Authorize(Roles = "Service")]
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        [Authorize(Roles = "Service")]
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public override async Task OnConnectedAsync()
        {
            //Structs.SessionToken.AddConnectionIdAsync(Global.Client, Context.GetHttpContext()?.User, Context.ConnectionId).Wait();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //Structs.SessionToken.RemoveConnectionIdAsync(Global.Client, Context.GetHttpContext()?.User, Context.ConnectionId).Wait();
            await base.OnDisconnectedAsync(exception);
        }
    }
}
