using System.Net.Sockets;
using TCPClient01.Interfaces.Application.Form;

namespace TCPClient01.Interfaces.Application.Tcp
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for TCP service. </summary>
    ///
    /// <remarks>   Justin, 7/11/2015. </remarks>
    ///-------------------------------------------------------------------------------------------------
    internal interface ITcpService
    {
        /// <summary>   The TCP client. </summary>
        TcpClient MTcpClient { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the length of the byt array. </summary>
        ///
        /// <value> The length of the byt array. </value>
        ///-------------------------------------------------------------------------------------------------
        long BytArrayLength { get; set; }

        string Message { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates TCP client. </summary>
        ///
        /// <param name="ipAddress">    The IP address. </param>
        /// <param name="port">         The port. </param>
        /// <param name="form1">        The first form. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        bool BeginConnect(string ipAddress, string port, IForm form1);

        bool SendDataToServer(string text);
    }
}