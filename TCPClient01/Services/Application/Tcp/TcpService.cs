using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using TCPClient01.Enums;
using TCPClient01.Interfaces.Application.Form;
using TCPClient01.Interfaces.Application.Tcp;

namespace TCPClient01.Services.Application.Tcp
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A TCP service. </summary>
    ///
    /// <remarks>   Justin, 7/11/2015. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class TcpService : ITcpService
    {
        private byte[] _mrx;
        private TcpClient _tcpc;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the message. </summary>
        ///
        /// <value> The message. </value>
        ///-------------------------------------------------------------------------------------------------
        public string Message { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The state. </value>
        ///-------------------------------------------------------------------------------------------------
        public TcpState State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the TCP client. </summary>
        ///
        /// <value> The m TCP client. </value>
        ///-------------------------------------------------------------------------------------------------
        public TcpClient MTcpClient { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the length of the byt array. </summary>
        ///
        /// <value> The length of the byt array. </value>
        ///-------------------------------------------------------------------------------------------------
        public long BytArrayLength { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Justin, 7/11/2015. </remarks>
        ///
        /// <param name="byteArrayLength">  Length of the byte array. </param>
        ///-------------------------------------------------------------------------------------------------
        public TcpService(int byteArrayLength)
        {
            BytArrayLength = byteArrayLength;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates TCP client. </summary>
        ///
        /// <remarks>   Justin, 7/11/2015. </remarks>
        ///
        /// <param name="ipAddress">    The IP address. </param>
        /// <param name="port">         The port. </param>
        /// <param name="form1">        The first form. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        public bool BeginConnect(string ipAddress, string port, IForm form1)
        {
            IPAddress ipaddr;

            int nPort;

            Message = string.Empty;

            if (!int.TryParse(port, out nPort))
                Message = "invalid port supplied";

            if (!IPAddress.TryParse(ipAddress, out ipaddr))
                Message += " invalid ip address supplied"; //this is the default if the try parse fails

            //if either of these are not set, return back
            if (nPort == 0 || ipaddr == null) return false;

            //instantiate tcp client
            MTcpClient = new TcpClient();

            MTcpClient.BeginConnect(ipaddr, nPort, ar =>
            {
                //a callback method to be called when an a connection attempt has finished

                try
                {
                    //get the tcp client from the asnc result
                    _tcpc = ar.AsyncState as TcpClient;

                    //end the connection attempt
                    if (_tcpc != null)
                        _tcpc.EndConnect(ar);

                    _mrx = new byte[BytArrayLength];

                    GetTcpStream(form1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("There was an error: {0}", ex.Message));
                }

            }, MTcpClient);

            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to server. </summary>
        ///
        /// <remarks>   Justin, 7/11/2015. </remarks>
        ///
        /// <param name="text"> The text. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        public bool SendDataToServer(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            //convert the client text to a byte array
            var tx = Encoding.ASCII.GetBytes(text);

            if (MTcpClient == null || !MTcpClient.Client.Connected) return false;

            //begin write, define a callback to be called when a write has finished
            MTcpClient.GetStream().BeginWrite(tx, 0, tx.Length, ar =>
            {
                try
                {
                    var tcpc = ar.AsyncState as TcpClient;

                    //end write stream
                    if (tcpc != null)
                        tcpc.GetStream().EndWrite(ar);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("There was an error: {0}", ex.Message));
                }

            }, MTcpClient);

            return true;
        }


        private void GetTcpStream(IForm mainForm)
        {
            _tcpc.GetStream().BeginRead(_mrx, 0, _mrx.Length, ar =>
            {
                try
                {
                    _tcpc = ar.AsyncState as TcpClient;

                    if (_tcpc == null) { MessageBox.Show(@"The TCP client was not instantiated."); return; }

                    var countBytes = _tcpc.GetStream().EndRead(ar);

                    if (countBytes == 0)
                    {
                        MessageBox.Show(@"Connection was broken");
                        return;
                    }

                    var received = Encoding.ASCII.GetString(_mrx, 0, countBytes);

                    mainForm.SetOutput(received);
                    GetTcpStream(mainForm);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("There was an error: {0}", ex.Message));
                }
            }, _tcpc);
        }

    }
}