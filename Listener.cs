using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_3cX_CDR_Listerer_Form
{
    public class Listener : ApplicationContext
    {
        NotifyIcon notifyIcon;

        public Listener()
        {
            MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));


            notifyIcon = new NotifyIcon()
            {
                Icon = Demo_3cX_CDR_Listerer_Form.Properties.Resources.Logo, //create a logo in the resources
                ContextMenu = new ContextMenu(new MenuItem[]
                { exitMenuItem }),
                Visible = true
            };
            Logger.Log("Starting 3cX Listener");

            Task.Delay(1000).Wait();
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 5015;

                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList.Last();//IPAddress.Parse("127.0.0.1");//

                // IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(ipAddress, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Logger.Log($"Waiting for a connection...  on {ipAddress.ToString()} and {port.ToString()}");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Logger.Log("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        Logger.Log($"Received: {data}");
                        SendToWhereever(data);

                        // Process the data sent by the client.
                        // data = data.ToUpper();

                        // byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        // stream.Write(msg, 0, msg.Length);
                        //Logger.Log("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Logger.Log(e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }

        /// <summary>
        /// Send the data received to your endpoint.
        /// </summary>
        /// <param name="data"></param>
        private void SendToWhereever(string data)
        {
            throw new NotImplementedException();
        }

        void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            notifyIcon.Visible = false;
            Logger.Log("Stopping 3cX Listener");

            Application.Exit();
        }
    }

}
