Console.WriteLine("Welcome to SnakeEyes! Use your points to roll two or more dice, and the sum of the numbers\n" +
"you get is added to your score. To use any dice, you have to use a number of points equal to\n" +
"half the amount of faces it has. If you get any consecutive even number, you get a bonus. \n" +
"If you get any consecutive odd number, you get a penalty. But if you get snake eyes or don't\n" +
"have enough points, it's Game Over! You can use a d4, a d6, a d8, a d12, or a d20.");
Console.WriteLine("You begin with 5 points. Good luck!");
Console.WriteLine("If you enter 0, the game will quit. If you enter -1, you will go back to the main menu.\n" +
"To cancel your last action, enter -2.");
int points = 5;
List<int> availableSides = new List<int> { 4, 6, 8, 12, 20 };
D4 d4 = new D4();
D6 d6 = new D6();
D8 d8 = new D8();
D12 d12 = new D12();
D20 d20 = new D20();
List<RNG.Dice> availableDice = new List<RNG.Dice> { d4, d6, d8, d12, d20 };

int getNumDice()
{
    while (true)
    {
        Console.Write("Enter the number of dice you want to roll - ");
        try
        {
            int numDice = Convert.ToInt32(Console.ReadLine());
            if (numDice == -1)
            {
                saveHighScore();
                mainMenu();
            }
            if (numDice == 0)
            {
                saveHighScore();
                Environment.Exit(1);
            }
            if (numDice < 2)
            {
                Console.WriteLine("You must roll at least two dice.");
                continue;
            }
            return numDice;

        }
        catch (FormatException)
        {
            Console.WriteLine("Please enter an integer.");
            continue;
        }
    }
}

List<int> getDiceList(int numDice)
{
    List<int> diceList = new List<int>();
    for (int i = 0; i < numDice; i++)
    {
        int sides;
        Console.Write("Enter the number of sides for die " + (i + 1) + " - ");
        try
        {
            sides = Convert.ToInt32(Console.ReadLine());
            if (availableSides.Contains(sides))
            {
                diceList.Add(sides);
            }
            else if (i < 1)
            {
                break;
            }
            else if (sides == -2)
            {
                i -= 2;
                continue;
            }
            else if (sides == -1)
            {
                saveHighScore();
                mainMenu();
            }
            else if (sides == 0)
            {
                saveHighScore();
                Environment.Exit(1);
            }
            else
            {
                Console.WriteLine("Please enter a valid number of sides.");
                i--;
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Please enter a valid number of sides.");
            i--;
        }
    }
    int balance = points - diceList.Sum() / 2;
    if (balance < 0)
    {
        Console.WriteLine("You don't have enough points to roll those dice. You have " + points + " points.");
        diceList.Clear();
        diceList = getDiceList(numDice);
    }
    else
    {
        points -= diceList.Sum() / 2;
    }
    return diceList;
}

List<int> rollDice(List<int> diceList)
{
    List<int> rolls = new List<int>();
    for (int i = 0; i < diceList.Count; i++)
    {
        int sides = diceList[i];
        RNG.Dice die = availableDice[availableSides.IndexOf(sides)];
        int roll = die.roll();
        rolls.Add(roll);
        Console.WriteLine("You rolled a " + roll + " on a d" + sides + ".");
    }
    return rolls;
}

void calculateBonusAndPenalty(List<int> rolls, List<int> diceList)
{
    for (int i = 0; i < rolls.Count - 1; i++)
    {
        if (rolls[i] % 2 == 0 && rolls[i + 1] == rolls[i])
        {
            int bonus = 2;
            for (int j = i; j < rolls.Count - 2; j++)
            {
                if (rolls[j+1] % 2 == 0 && rolls[j + 2] == rolls[j + 1])
                {
                    bonus++;
                }
                else
                {
                    break;
                }
            }
            points += (int)Math.Pow(rolls[i], bonus);
            Console.WriteLine("You got a bonus of " + (int)Math.Pow(rolls[i], bonus) + " points!");
            i += bonus;
        }
        else if (rolls[i] % 2 != 0 && rolls[i + 1] == rolls[i])
        {
            int penalty = 2;
            for (int j = i; j < rolls.Count - 2; j++)
            {
                if (rolls[j + 1] % 2 != 0 && rolls[j + 2] == rolls[j + 1])
                {
                    penalty++;
                }
                else
                {
                    break;
                }
            }
            points -= (int)Math.Pow(rolls[i] - 1, penalty);
            Console.WriteLine("You got a penalty of " + (int)Math.Pow(rolls[i] - 1, penalty) + " points.");
            i += penalty;
        }
    }
}

void checkForSnakeEyes(List<int> rolls)
{
    for (int i = 0; i < rolls.Count - 1; i++)
    {
        if (rolls[i] == 1 && rolls[i + 1] == 1)
        {
            Console.WriteLine("You got snake eyes! Game Over!");
            saveHighScore();
            mainMenu();
        }
    }
}

void saveHighScore()
{
    string path = @"C:\Users\Public\Documents\SnakeEyesHighScores.txt";
    Console.Write("Enter your name (3 characters, no numbers) or press enter to skip - ");
    string name = Console.ReadLine();
    if (name.Length != 3 || !name.All(char.IsLetter))
    {
        Console.WriteLine("Invalid name. Skipping.");
        return;
    }
    if (!File.Exists(path))
    {
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine(name.ToUpper() + "-" + points);
            for (int i = 2; i < 11; i++)
            {
                sw.WriteLine("AAA-0");
            }
        }
    }
    else
    {
        string[] lines = File.ReadAllLines(path);
        Highscore[] highScores = { new Highscore { Name = lines[0].Split('-')[0], Score = Convert.ToInt32(lines[0].Split('-')[1]) },
                                   new Highscore { Name = lines[1].Split('-')[0], Score = Convert.ToInt32(lines[1].Split('-')[1]) },
                                   new Highscore { Name = lines[2].Split('-')[0], Score = Convert.ToInt32(lines[2].Split('-')[1]) },
                                   new Highscore { Name = lines[3].Split('-')[0], Score = Convert.ToInt32(lines[3].Split('-')[1]) },
                                   new Highscore { Name = lines[4].Split('-')[0], Score = Convert.ToInt32(lines[4].Split('-')[1]) },
                                   new Highscore { Name = lines[5].Split('-')[0], Score = Convert.ToInt32(lines[5].Split('-')[1]) },
                                   new Highscore { Name = lines[6].Split('-')[0], Score = Convert.ToInt32(lines[6].Split('-')[1]) },
                                   new Highscore { Name = lines[7].Split('-')[0], Score = Convert.ToInt32(lines[7].Split('-')[1]) },
                                   new Highscore { Name = lines[8].Split('-')[0], Score = Convert.ToInt32(lines[8].Split('-')[1]) },
                                   new Highscore { Name = lines[9].Split('-')[0], Score = Convert.ToInt32(lines[9].Split('-')[1]) } };
        for (int i = 0; i < highScores.Length; i++)
        {
            if (points > highScores[i].Score)
            {
                Highscore temp = highScores[i];
                highScores[i] = new Highscore { Name = name.ToUpper(), Score = points };
                for (int j = i + 1; j < highScores.Length; j++)
                {
                    Highscore temp2 = highScores[j];
                    highScores[j] = temp;
                    temp = temp2;
                }
                break;
            }
        }
        using (StreamWriter sw = File.CreateText(path))
        {
            for (int i = 0; i < 10; i++)
            {
                sw.WriteLine(highScores[i].Name + '-' + highScores[i].Score);
            }
        }
    }
}

void showHighScores()
{
    string path = @"C:\Users\Public\Documents\SnakeEyesHighScores.txt";
    if (!File.Exists(path))
    {
        Console.WriteLine("No high scores yet!");
    }
    else
    {
        string[] lines = File.ReadAllLines(path);
        Console.WriteLine("High Scores:");
        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }
    }
}

void playGame()
{
    points = 5;
    while (points > 3)
    {
        // This is the main loop of the game. It will continue until the player runs out of points or gets snake eyes.
        int numDice = getNumDice();
        List<int> diceList = getDiceList(numDice);
        List<int> rolls = rollDice(diceList);
        Console.WriteLine("You rolled a total of " + rolls.Sum() + ".");
        points += rolls.Sum();
        calculateBonusAndPenalty(rolls, diceList);
        checkForSnakeEyes(rolls);
        Console.WriteLine("You now have " + points + " points.");
    }
    Console.WriteLine("You ran out of points! Game over!");
    saveHighScore();
    mainMenu();
}

void mainMenu()
{
    while (true)
    {
        // This is the main menu loop. It will continue until the player chooses to quit.
        Console.WriteLine("1. Play Game");
        Console.WriteLine("2. Show High Scores");
        Console.WriteLine("3. Quit");
        Console.Write("Enter your choice - ");
        try
        {
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                playGame();
            }
            else if (choice == 2)
            {
                showHighScores();
            }
            else if (choice == 3)
            {
                Environment.Exit(1);
            }
            else
            {
                Console.WriteLine("Please enter a valid choice.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Please enter a valid choice.");
        }
    }
}

mainMenu();

