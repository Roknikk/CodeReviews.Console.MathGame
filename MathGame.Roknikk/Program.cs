using System.Diagnostics;

Random random = new Random();
Stopwatch timer = new();

// Banner Logo.
void BannerLogo()
{
  Console.WriteLine(""" 
 ________                                 _            __       __                      ______   __   __                  ______    ______  
|        \                              |  \          |  \     /  \                    |      \ |  \ |  \                /      \  /      \ 
 \$$$$$$$$______   __    __   _______  _| $$_         | $$\   /  $$  ______             \$$$$$$_| $$_| $$ _______       |  $$$$$$\|  $$$$$$\
   | $$  /      \ |  \  |  \ /       \|   $$ \        | $$$\ /  $$$ /      \             | $$ |   $$ \\$ /       \       \$$__| $$ \$$__| $$
   | $$ |  $$$$$$\| $$  | $$|  $$$$$$$ \$$$$$$        | $$$$\  $$$$|  $$$$$$\            | $$  \$$$$$$  |  $$$$$$$        |     $$ /      $$
   | $$ | $$   \$$| $$  | $$ \$$    \   | $$ __       | $$\$$ $$ $$| $$    $$            | $$   | $$ __  \$$    \        __\$$$$$\|  $$$$$$ 
   | $$ | $$      | $$__/ $$ _\$$$$$$\  | $$|  \      | $$ \$$$| $$| $$$$$$$$ __        _| $$_  | $$|  \ _\$$$$$$\      |  \__| $$| $$_____ 
   | $$ | $$       \$$    $$|       $$   \$$  $$      | $$  \$ | $$ \$$     \|  \      |   $$ \  \$$  $$|       $$       \$$    $$| $$     \
    \$$  \$$        \$$$$$$  \$$$$$$$     \$$$$        \$$      \$$  \$$$$$$$| $$       \$$$$$$   \$$$$  \$$$$$$$         \$$$$$$  \$$$$$$$$
                                                                              \$                                                              
""");
}

// Introductory message.
void WelcomeMessage()
{
  Console.WriteLine("""

  Welcome to Trust Me, It is 32! 
  You will have to choose one arithmetic operation, and the game will ask you 5 questions related to it.
  For each correct answer, you will gain 1 point ! \(^-^)/. But if your answer is wrong, you will lose 1 point (;_;).

  """);
}

// Show the menu of all the posible options in the game .
void ShowOperationMenu()
{
  string[] options = {"Addition", "Subtraction", "Multiplication", "Division", "Random Question", "Show previous games"};

  int optionNumber = 0;

  Console.WriteLine("This is the list of the available options in the game.");
  Console.WriteLine("");

  foreach(string option in options)
  {
    Console.WriteLine($"{++optionNumber}. {option}");;
  }

  Console.WriteLine("");
}

// Get the user's choice.
char AskOptionUser()
{
  Console.Write("Enter the number corresponding to the option shown in the menu: ");

  while (true)
  {
   string? userChosenOption = Console.ReadLine();

   if(userChosenOption?.Length == 1 && 
   userChosenOption[0] > '0' && 
   userChosenOption[0] <= '6')
    {
        return userChosenOption[0];
    }
    else
    {
      Console.Write("Invalid input. Please try again: ");
      Console.WriteLine("");
    }
  }
}


// Generate the math question.
(int correctAnswer, string question) GenerateMathQuestion(char option)
{

  short value1 = (short)random.Next(0, 101);
  short value2 = (short)random.Next(0, 101);

  switch (option)
  {
    case '1':
      return (value1+value2, $"{value1} + {value2} = ");

    case '2':
      return (value1-value2, $"{value1} - {value2} = ");

    case '3':
      return (value1*value2, $"{value1} * {value2} = ");
    
    case '4':
      value2 = (short)random.Next(1, 101);
      while(value1%value2 != 0)
      {
        value1 = (short)random.Next(0, 101);
        value2 = (short)random.Next(1, 101);
      }
      return (value1/value2, $"{value1} / {value2} = ");
    
    default:
      return (0, "empty");
  }
}

// Show the previous games.
void ShowPreviousGames(List<List<string>> previousGames)
{
  if (previousGames.Count == 0)
  {
    Console.WriteLine("Sorry, there are no previous games to show.");
  }
  else
  {
    Console.WriteLine("These are the available options in the game.");
    for (int gameNumber = 1; gameNumber <= previousGames.Count; gameNumber++)
    {
      Console.WriteLine($"Game {gameNumber}");
    }

    while (true)
    {
      Console.Write("Which previous game do you want to see? (Enter the number of the game): ");
      string? userInput = Console.ReadLine();
      Console.WriteLine("");

      if(userInput?.Length == 1)
      {
        if(int.TryParse(userInput, out int showGameNumber) && showGameNumber <= previousGames.Count)
        {
          foreach (string game in previousGames[showGameNumber-1])
          {
            Console.WriteLine(game);
          }
          Console.WriteLine("");
          break;
        }
        Console.WriteLine("That game doesn't exist.");
      }
    }
  }
}


// Get the user answer. 
int GetUserAnswer()
{
  while (true)
  {
    bool userInput = int.TryParse(Console.ReadLine(), out int userAnswer);

    if (userInput)
    {
      return userAnswer;
    }
    else
    {
      Console.Write("Invalid input. Try again: ");
    }
  }
}

// Add or remove a point from the user's score.
int UpdateUserScore (bool answerRight, int userScore)
{
  if (answerRight)
  {
    ++userScore;
    Console.WriteLine($"Excellent! You now have {userScore} points");
    Console.WriteLine("");
    return userScore;
  }
  else
  {
    userScore = userScore <= 0? 0 : --userScore;
    Console.WriteLine($"Oops! You now have {userScore} points");
    Console.WriteLine("");
    return userScore;
  }
}

// Main application logic.
void RunConsoleApp(List<List<string>> previousGames)
{
  Console.Clear();
  BannerLogo();
  WelcomeMessage();

  while (true){

    int userScore = 0;
    int correctAnswer;
    string question;
    List <string> currentGameHistory = new();

    ShowOperationMenu();

    char userChoice = AskOptionUser();

    // The user wants to see the previous games.
    if (userChoice == '6')
    {
      Console.Clear();
      ShowPreviousGames(previousGames);

      Console.Write("Press any key to go back to the menu");
      Console.ReadKey();

      Console.Clear();

      BannerLogo();

      WelcomeMessage();
      continue;
    }

    // Generate the questions based on the selected option.
    timer.Start(); //Start the time tracker.

    for (short i = 1; i < 6; i++)
    {
      // Generate random math questions.
      if (userChoice == '5')
      {
        (correctAnswer, question) = GenerateMathQuestion(random.Next(1, 5).ToString()[0]);
      }
      else // Questions of just one math operation.
      {
        (correctAnswer, question) = GenerateMathQuestion(userChoice);
      }


      Console.Write($"{question}");

      int userAnswer = GetUserAnswer();

      currentGameHistory.Add($"{question}{userAnswer}"); // Save each question in the game list.

      if (correctAnswer == userAnswer)
      {
        userScore = UpdateUserScore(true, userScore);
        currentGameHistory[i-1] += "  ->  Correct!";
      }
      else
      {
        userScore = UpdateUserScore(false, userScore);
        currentGameHistory[i-1] += "  ->  Incorrect!";
      } 
    }
    timer.Stop();

    Console.WriteLine($"Game time: {timer.Elapsed:mm\\:ss}");

    currentGameHistory.AddRange([$"Score: {userScore}", $"Game time: {timer.Elapsed:mm\\:ss}"]); // Add the score and game time to the game record.

    previousGames.Add(currentGameHistory); // Save all games in a list.

    Console.Write("Do you want to choose another option? Y for yes, any other key for no: ");
    string? closeTheGame = Console.ReadLine(); 
    if (closeTheGame != null && closeTheGame[0] != 'y') // Exit the game.
    {
      break;
    }

    Console.Clear();
    Console.Write("\u001bc\x1b[3J"); // I don't understand how this help to clean the console. But I found the solution in StackOverflow.
    BannerLogo();
  }
  
}

// Launch the MathGame.
List<List<string>> previousGames = new List<List<string>>();

Console.Clear();

RunConsoleApp(previousGames);