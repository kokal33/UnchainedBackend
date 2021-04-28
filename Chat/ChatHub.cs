using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace UnchainedBackend.Chat
{
    public class ChatHub : Hub
    {
        private static short cardNum;
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ReceiveCard", cardNum);
            await base.OnConnectedAsync();
        }

        //Basic chat
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        //For card changing command
        public async Task BroadcastCard(string card)
        {
            cardNum = (short)int.Parse(card);
            await Clients.All.SendAsync("ReceiveCard", cardNum);
        }
        //For special admin messages
        public async Task AdminMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveAdminMessage", user, message);
        }
    }
}
