namespace EFCoreATM_Interface
{
    public class AtmStarter
    {
        public static void AtmDisplay()
        {
            Utility.AppName();

            Menu menu = new();
            menu.Options();
        }
    }
}
