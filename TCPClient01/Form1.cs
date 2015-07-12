using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using TCPClient01.Interfaces.Application.Form;
using TCPClient01.Interfaces.Application.Tcp;
using TCPClient01.Services.Application.Tcp;

namespace TCPClient01
{
    public partial class Form1 : Form, IForm
    {
        private ITcpService _mTcpService;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Callback, called when the set text. </summary>
        ///
        /// <remarks>   Justin, 7/11/2015. </remarks>
        ///
        /// <param name="text"> The text. </param>
        ///-------------------------------------------------------------------------------------------------
        delegate void SetTextCallback(string text);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Callback, called when the get text. </summary>
        ///
        /// <remarks>   Justin, 7/11/2015. </remarks>
        ///
        /// <returns>   A string. </returns>
        ///-------------------------------------------------------------------------------------------------
        delegate string GetTextCallback();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Justin, 7/11/2015. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public Form1()
        {
            InitializeComponent();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets an output. </summary>
        ///
        /// <remarks>   Justin, 7/11/2015. </remarks>
        ///
        /// <param name="text"> The text. </param>
        ///-------------------------------------------------------------------------------------------------
        public void SetOutput(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (tbConsole.InvokeRequired)
            {
                SetTextCallback d = SetOutput;
                Invoke(d, text);
            }
            else
            {
                tbConsole.Text = (tbConsole.Text.Length > 100 &&
                                        tbConsole.Text[tbConsole.Text.Length - 1] == ' ')

                    ? tbConsole.Text += text
                    : tbConsole.Text += text + Environment.NewLine;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the output. </summary>
        ///
        /// <remarks>   Justin, 7/11/2015. </remarks>
        ///
        /// <returns>   The output. </returns>
        ///-------------------------------------------------------------------------------------------------
        public string GetOutput()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (tbConsole.InvokeRequired)
            {
                GetTextCallback d = GetOutput;
                Invoke(d);
            }
            else
            {
                return tbConsole.Text;
            }

            return string.Empty;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            _mTcpService.SendDataToServer(tbPayload.Text);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _mTcpService = new TcpService(655568);

            //pass Ip address, port number and form pointer to create a tcp listner and client (tcp server and client)
            if (!_mTcpService.BeginConnect(tbServerIp.Text, tbServerPort.Text, this))
            {
                //display the error message if not successful
                MessageBox.Show(_mTcpService.Message);
                return;
            }

            MessageBox.Show(string.Format("Now connected to end point: {0}, port {1}", tbServerIp.Text, tbServerPort.Text));
        }
    }
}
