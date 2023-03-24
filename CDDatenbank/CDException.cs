namespace CDDatabase
{
    internal class CDException : Exception
    {
        /// <summary>
        /// In case things have gone horribly wrong
        /// </summary>
        /// <param name="message"></param>
        public CDException(string message):base(message)
        {  }

    }
}
