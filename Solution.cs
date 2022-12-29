//Eleanor Forrest
//One-Armed Bandit Simulation
//(but this time in C#)

using System;

class Reel 
{

}

class Player
{
    double balance = 0;

    public double GetBalance()
    {
        return balance;
    }

    public void AddBalance()
    {
        //Ask the user to input money
        Console.WriteLine("Enter up to £1 into the machine. It is 10p per spin.");
        Console.WriteLine("Enter the amount of money you will place into the machine. Enter in the form '0.50' for 50p etc.");
        Console.Write(">>>");
        string inputBalance = Console.ReadLine();
        //check if float between 0.01 and 1.00, special error for 0.00, no more than 2 dp


    }
}

class Game
{
    public bool Menu()
    {
        //Output the main menu
        Console.WriteLine(@"
What would you like to do?
    '1' - Play Game
    '2' - View Statistics
    '3' - Quit
");
        string[] options = {"1", "2", "3"};
        string selected = Simulation.UserInput(options);

        //Call the specified function
        if (selected == "1")
        {
            //call Play, at the sart of Play check if the user has enough money, if not call getMoney
        }

        return true;

    }
}

class Simulation
{
    public static string UserInput(string [] options)
    //Check the user has entered a valid input to select from a menu
    {
        Console.Write(">>>");
        string selected = Console.ReadLine();
        while (!options.Contains(selected))
        {
            Console.WriteLine("Invalid input. Enter one of the available options only (without quotation marks).");
            Console.Write(">>>");
            selected = Console.ReadLine();
        }
        return selected;
    }
    
    static void Main()
    {
        Game game = new Game();
        Player player = new Player();
        Reel reel1 = new Reel();
        Reel reel2 = new Reel();
        Reel reel3 = new Reel();
        //welcome the player
        Console.WriteLine("Welcome to the One-Armed Bandit Simulation.");
        //run the simulation until the user chooses to quit
        bool quit = false;
        while (!quit)
        {
            quit = game.Menu();
        }
    }
}

//https://blog.mwpreston.net/2018/09/24/how-to-run-c-sharp-in-visual-studio-code/