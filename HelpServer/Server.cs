using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelpServer
{
    public partial class Server : Component {
        WebServer ws = new WebServer(SendResponse, "http://localhost:8081/");
        private static HelpHttp hp = new HelpHttp();

        public Server()
        {
            InitializeComponent();

            ws.Run();
        }

        public Server(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            Process.Start("http://localhost:8081");
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            ws.Stop();
            Application.Exit();
        }

        public static Tuple<byte[], string> SendResponse(HttpListenerRequest request) {
            return hp.handleGETRequest(request);
        }
    }
}
