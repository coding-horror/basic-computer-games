Imports System
Imports System.Text
Imports System.Text.RegularExpressions

Module Word
    ' Here's the list of potential words that could be selected
    ' as the winning word.
    Dim words As String() = {"DINKY", "SMOKE", "WATER", "GRASS", "TRAIN", "MIGHT", "FIRST",
         "CANDY", "CHAMP", "WOULD", "CLUMP", "DOPEY"}

    ' <summary>
    ' Outputs the instructions of the game.
    ' </summary>
    Private Sub intro()
        Console.WriteLine("WORD".PadLeft(37))
        Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".PadLeft(59))

        Console.WriteLine("I am thinking of a word -- you guess it. I will give you")
        Console.WriteLine("clues to help you get it. Good luck!!")
    End Sub

    ' <summary>
    ' This allows the user to enter a guess - doing some basic validation
    ' on those guesses.
    ' </summary>
    ' <returns>The guess entered by the user</returns>
    Private Function get_guess() As String
        Dim guess As String = ""

        While (guess.Length = 0)
            Console.WriteLine($"{Environment.NewLine}Guess a five letter word. ")
            guess = Console.ReadLine().ToUpper()

            If ((guess.Length <> 5) Or guess.Equals("?") Or Not Regex.IsMatch(guess, "^[A-Z]+$")) Then
                guess = ""
                Console.WriteLine("You must guess a give letter word. Start again.")
            End If
        End While

        Return guess
    End Function

    ' <summary>
    ' This checks the user's guess against the target word - capturing
    ' any letters that match up between the two as well as the specific
    ' letters that are correct.
    ' </summary>
    ' <param name="guess">The user's guess</param>
    ' <param name="target">The 'winning' word</param>
    ' <param name="progress">A string showing which specific letters have already been guessed</param>
    ' <returns>The integer value showing the number of character matches between guess and target</returns>
    Private Function check_guess(guess As String, target As String, progress As StringBuilder) As Integer
        ' Go through each letter of the guess And see which 
        ' letters match up to the target word.
        ' For each position that matches, update the progress
        ' to reflect the guess
        Dim matches As Integer = 0
        Dim common_letters As String = ""

        For ctr As Integer = 0 To 4

            ' First see if this letter appears anywhere in the target
            ' And, if so, add it to the common_letters list.
            If (target.Contains(guess(ctr))) Then
                common_letters.Append(guess(ctr))
            End If
            ' Then see if this specific letter matches the
            ' same position in the target. And, if so, update
            ' the progress tracker
            If (guess(ctr).Equals(target(ctr))) Then
                progress(ctr) = guess(ctr)
                matches += 1
            End If
        Next

        Console.WriteLine($"There were {matches} matches and the common letters were... {common_letters}")
        Console.WriteLine($"From the exact letter matches, you know......... {progress}")
        Return matches
    End Function

    ' <summary>
    ' This plays one full game.
    ' </summary>
    Private Sub play_game()
        Dim guess_word As String, target_word As String
        Dim guess_progress As StringBuilder = New StringBuilder("-----")
        Dim rand As Random = New Random()
        Dim count As Integer = 0

        Console.WriteLine("You are starting a new game...")

        ' Randomly select a word from the list of words
        target_word = words(rand.Next(words.Length))

        ' Just run as an infinite loop until one of the
        ' endgame conditions are met.
        While (True)
            ' Ask the user for their guess
            guess_word = get_guess()
            count += 1

            ' If they enter a question mark, then tell them
            ' the answer and quit the game
            If (guess_word.Equals("?")) Then
                Console.WriteLine($"The secret word is {target_word}")
                Return
            End If

            ' Otherwise, check the guess against the target - noting progress
            If (check_guess(guess_word, target_word, guess_progress) = 0) Then
                Console.WriteLine("If you give up, type '?' for your next guess.")
            End If

            ' Once they've guess the word, end the game.
            If (guess_progress.Equals(guess_word)) Then
                Console.WriteLine($"You have guessed the word.  It took {count} guesses!")
                Return
            End If

        End While
    End Sub

    ' <summary>
    ' The main entry point for the class - just keeps
    ' playing the game until the user decides to quit.
    ' </summary>
    Public Sub play()
        intro()

        Dim keep_playing As Boolean = True

        While (keep_playing)
            play_game()
            Console.WriteLine($"{Environment.NewLine}Want to play again? ")
            keep_playing = Console.ReadLine().StartsWith("y", StringComparison.CurrentCultureIgnoreCase)
        End While

    End Sub
End Module

Module Program
    Sub Main(args As String())
        Word.play()
    End Sub
End Module
