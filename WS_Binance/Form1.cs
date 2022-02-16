using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Binance.Client.Websocket;
using System.Net.WebSockets;

namespace SocketAll
{
    public partial class Form1 : Form
    {
        public readonly string WsEndpoint = "wss://stream.binance.com:9443/stream?streams=ltcbtc@aggTrade/ethbtc@aggTrade";
        public Form1()
        {
            InitializeComponent();
        }

        private async Task Form1_Load(object sender, EventArgs e)
        {

        }

        private async Task StartWebSocket()
        {
            var socket = new ClientWebSocket();
            Uri serverUri = new Uri(WsEndpoint);

            await socket.ConnectAsync(serverUri, CancellationToken.None);

            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);

                WebSocketReceiveResult result = await socket.ReceiveAsync(bytesReceived, CancellationToken.None);
                var line = Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);
                MessageBox.Show("Line: " + line);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartWebSocket();
        }
    }
}
