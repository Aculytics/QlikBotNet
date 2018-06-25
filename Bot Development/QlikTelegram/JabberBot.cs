using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agsXMPP;
using agsXMPP.protocol.client;

namespace QlikTelegram {
    public class JabberBot {
        public JabberBot() {
            try {
                Console.WriteLine("Thanks for trying the Jabber bot!");
                XmppClientConnection xmppCon = new XmppClientConnection();
                xmppCon.Username = AppSettings.JabberUser;
                xmppCon.Password = AppSettings.JabberPassword;
                xmppCon.Server = AppSettings.JabberServer;
                xmppCon.ConnectServer = AppSettings.JabberConnectServer;
                xmppCon.AutoAgents = false;
                xmppCon.AutoPresence = true;
                xmppCon.AutoRoster = true;
                xmppCon.AutoResolveConnectServer = false;
            }
            catch (Exception e){
                Console.WriteLine(e.Message);
            }
        }

        private void OnMessageReceived(object sender, Message message) {
            Console.WriteLine("Message received from {0}: {1}", message.From.ToString(), message.Body);
        }
    }
}
