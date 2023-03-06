namespace EFCoreATM_Interface
{
    public class Utility
    {
        private static bool isRunning;
        public static bool ContinueOperation()
        {
            Console.Write("\n\t Do you want to carry out another operation? (y/n)\n\t ");
            string choiceOption = Console.ReadLine().ToLower();

            if (choiceOption == "y")
            {
                isRunning = true;
            }
            else if (choiceOption == "n")
            {
                isRunning = false;
                Console.WriteLine("\n\t Thanks for using our Console AtM Service..");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("\n\t Invalid character entered, try again: ");
                ContinueOperation();
            }

            return isRunning;
        }


        public static void AppName()
        {
            Console.WriteLine("\n\t ============================ \n\t | Welcome to EF_ConsoleATM | \n\t ============================");
        }


    }
}
