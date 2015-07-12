namespace TCPClient01.Interfaces.Application.Form
{
    public interface IForm
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets an output. </summary>
        ///
        /// <param name="text"> The text. </param>
        ///-------------------------------------------------------------------------------------------------
        void SetOutput(string text);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the output. </summary>
        ///
        /// <returns>   The output. </returns>
        ///-------------------------------------------------------------------------------------------------
        string GetOutput();
    }
}