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

    public string Spin()
    {
        //Get a random symbol from those available
        string[] symbols = {"7", "BAR", "bell", "cherry", "watermelon", "grapes", "horseshoe"};
        Random rnd = new Random();
        int index = rnd.Next(0, 7);
        return symbols[index]; 
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

    public void AddBalance()
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
        FileStream fs3 = new FileStream("Transactions.txt", FileMode.Append);
        StreamWriter sw = new StreamWriter(fs3);
        sw.WriteLine($"Money input : {inputBalance}");
        sw.WriteLine();
        sw.Close();
        fs3.Close();


    }
}

class Game
{
    Player player = new Player();
    Reel reel1 = new Reel();
    Reel reel2 = new Reel();
    Reel reel3 = new Reel();

    public void InitialiseTextFiles()
    {
        FileStream fs1 = new FileStream("Transactions.txt", FileMode.Create, FileAccess.ReadWrite);
        FileStream fs2 = new FileStream("SpinResults.txt", FileMode.Create, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fs1);
        sw.WriteLine("A record of transactions in the One-Armed Bandit simulation.");
        sw.WriteLine();
        sw.Close();
        fs1.Close();
        StreamWriter sw2 = new StreamWriter(fs2);
        sw2.WriteLine("A record of the spin results in the One-Armed Bandit simulation.");
        sw2.WriteLine();
        sw2.Close();
        fs2.Close();
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
        else if (selected == "2")
        {
            ViewFiles();
        }
        else
        {
            Console.WriteLine("Goodbye.");
            return true;
        }

        return false;

    }

    void Play()
    {
        //Check that the user has enough money to spin
        if (player.Balance <0.1)
        {
            player.AddBalance();
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
        player.Balance = Math.Round(player.Balance, 2);
        //Spin each reel
        Reel[] reels = {reel1, reel2, reel3};
        string[] symbols = {reel1.Spin(), reel2.Spin(), reel3.Spin()};
        
        //Display the results
        for (int i = 1; i <=10; i++)
        {
            Console.Write("* ");
            System.Threading.Thread.Sleep(300);
        }
        Console.WriteLine();
        for (int i = 0; i<=2; i++)
        {
            if (reels[i].Hold == true)
            {
                //Wait a few extra seconds before displaying the symbol
                System.Threading.Thread.Sleep(3000);
            }
            else
            {
                System.Threading.Thread.Sleep(500);
            }
            Console.Write(symbols[i]);
            Console.Write("   ");
        }
        Console.WriteLine();
        //Determine if the player has won
        string[] winPayout = CheckIfWon(symbols);
        bool win = Convert.ToBoolean(winPayout[0]);
        double payout = Convert.ToDouble(winPayout[1]);
        //Add the payout to the user's balance
        if (win == true)
        {
            player.Balance += payout;
            Console.WriteLine("You won. Congratulations!");
        }
        else
        {
            Console.WriteLine("You lost. Better luck next time.");
        }
        //Update the text files
        FileStream fs4 = new FileStream("Transactions.txt", FileMode.Append);
        FileStream fs5 = new FileStream("SpinResults.txt", FileMode.Append);
        StreamWriter sw = new StreamWriter(fs4);
        sw.WriteLine($"Payout: {payout}");
        sw.WriteLine($"Current balance: {player.Balance}");
        sw.WriteLine();
        sw.Close();
        fs4.Close();
        StreamWriter sw2 = new StreamWriter(fs5);
        string symbolsString = $"{symbols[0]}   {symbols[1]}   {symbols[2]}";
        sw2.WriteLine($"Results: {symbolsString}");
        sw2.WriteLine($"Win: {win}");
        sw2.WriteLine($"Payout: {payout}");
        sw2.WriteLine();
        sw2.Close();
        fs5.Close();
    }

    string[] CheckIfWon(string[] symbols)
    {
        if (symbols[0] == symbols[1] && symbols[0]== symbols[2])
        {
            if (symbols[0] == "7")
            {
                string[] returnList = {"true", "20.00"};
                return  returnList;
            }
            else if (symbols[0] == "BAR")
            {
                string[] returnList = {"true", "5.00"};
                return  returnList;
            }
            else if (symbols[0] == "bell")
            {
                string[] returnList = {"true", "3.00"};
                return  returnList;
            }
            else if (symbols[0] == "cherry")
            {
                string[] returnList = {"true", "1.00"};
                return  returnList;
            }
        }
        string[] loseReturnList = {"false", "0.00"};
        return  loseReturnList;
    }

    void ViewFiles()
    {
        //Ask the user to select a file
        Console.WriteLine(@"
Which file do you want to view?
    '1' - Transactions.txt
    '2' - SpinResults.txt
");
        string[] options = {"1", "2"};
        string selected = Simulation.UserInput(options);

        //Output the file
        if (selected == "1")
        {
            FileStream fs6 = new FileStream("Transactions.txt", FileMode.Open);
            StreamReader sw1 = new StreamReader(fs6);
            string line = sw1.ReadLine();
            while (line != null)
            {
                Console.WriteLine(line);
                line = sw1.ReadLine();
            }
            sw1.Close();
            fs6.Close();
        }
        else
        {
            FileStream fs7 = new FileStream("SpinResults.txt", FileMode.Open);
            StreamReader sw2 = new StreamReader(fs7);
            string line = sw2.ReadLine();
            while (line != null)
            {
                Console.WriteLine(line);
                line = sw2.ReadLine();
            }
            sw2.Close();
            fs7.Close();
        }
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
        //initialise the text files
        game.InitialiseTextFiles();
        //run the simulation until the user chooses to quit
        bool quit = false;
        while (quit != true)
        {
            quit = game.Menu();
        }
    }
}

//debug text files!!