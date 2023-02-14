namespace MagicVilla_VillaAPI.Logging
{
    public class Logg : ILoggerCustom
    {
        public void Info(string message, string type)
        {
            if(type=="Error")
            {
                Console.Error.WriteLine("Error - "+message);
            }
            else
            {
                Console.Error.WriteLine(message);
            }
        }
    }
}
