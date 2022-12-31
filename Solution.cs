//Eleanor Forrest
//One-Armed Bandit Simulation
//(but this time in C#)

using System;

class Reel 
{
    bool hold = false;
    
    public bool Hold
    {
        get {return hold;}
        set {hold = value;}
    }
}

class Player
{
    double balance = 0;

    public double Balance
    {
        get {return balance;}
        set {balance = value;}
    }

    public void AddBalance(FileStream tfs)
    {
        //Ask the user to input money
        Console.WriteLine("Enter up to £1 into the machine. It is 10p per spin.");
        bool validInput = false;
        double inputBalance = 0;
        while(!validInput)
        {
            Console.WriteLine("Enter the amount of money you will place into the machine. Enter in the form '0.50' for 50p etc.");
            Console.Write(">>>");
            string inputBalanceString = Console.ReadLine();
            
            //check if this input is valid
            if (double.TryParse(inputBalanceString, out inputBalance) && 0<inputBalance && inputBalance<=1)
            {
                inputBalance = Math.Round(inputBalance, 2, MidpointRounding.ToZero);
                validInput = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Enter a value between 0 and 1.");
            }
        }
        balance = inputBalance;
        //update Transactions file
        StreamWriter sw = new StreamWriter(tfs);
        sw.WriteLine($"Money input : {inputBalance}");
        sw.Close();


    }
}

class Game
{
    FileStream tfs = new FileStream("Transactions.txt", FileMode.Create, FileAccess.ReadWrite);
    FileStream srfs = new FileStream("SpinResults.txt", FileMode.Create, FileAccess.ReadWrite);
    Player player = new Player();
    Reel reel1 = new Reel();
    Reel reel2 = new Reel();
    Reel reel3 = new Reel();

    void initialiseTextFiles()
    {
        StreamWriter sw = new StreamWriter(tfs);
        sw.WriteLine("A record of transactions in the One-Armed Bandit simulation.");
        sw.Close();
        StreamWriter sw2 = new StreamWriter(srfs);
        sw2.WriteLine("A record of the spin results in the One-Armed Bandit simulation.");
        sw2.Close();
    }
   

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
            Play();
        }
        //else - close files before quitting.

        return true;

    }

    void Play()
    {
        //Check that the user has enough money to spin
        if (player.Balance <0.1)
        {
            player.AddBalance(tfs);
        }
        //Ask if the user would like to hold any reels
        Console.WriteLine("How many reels would you like to hold?");
        bool validInput = false;
        int inputHold = 0;
        while (!validInput)
        {
            Console.WriteLine("Enter 0, 1 or 2.");
            Console.Write(">>>");
            string inputHoldString = Console.ReadLine();
            if (int.TryParse(inputHoldString, out inputHold) && 0<= inputHold && inputHold<=2)
            {
                validInput = true;
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
        //Set the appropriate reel attributes 
        if (inputHold == 2) 
        {
            reel2.Hold = true;
            reel3.Hold = true;
        }
        else if (inputHold == 1)
        {
            reel3.Hold = true;
        }
        //Output the winning combinations
        Console.WriteLine("The winning combinations are as follows:");
        string winCombos = @"
        7	7	7	= £20.00
	BAR	BAR	BAR	= £5.00
	bell	bell	bell	= £3.00
	cherry	cherry	cherry	= £1.00";
        Console.WriteLine(winCombos);
        //Output number of spins remaining
        int spinsRemaining = Convert.ToInt32(Math.Round(player.Balance *10, 0, MidpointRounding.ToZero));
        Console.WriteLine($"You have {spinsRemaining} spins remaining.");
        //Ask the user to run the machine
        Console.WriteLine("Hit Enter to run the machine.");
        Console.ReadLine();
        //Take 10p away from the user's balance
        player.Balance -= 0.1;
        //Spin each reel
        


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