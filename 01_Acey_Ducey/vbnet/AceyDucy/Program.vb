Imports System

''' <summary>
''' This is a modern adapation of Acey Ducey from BASIC Computer Games.
''' 
''' The structural changes primarily consist of replacing the many GOTOs with
''' Do/Loop constructs to force the continual execution of the program.
''' 
''' Because modern Basic allows multi-line If/Then blocks, many GOTO jumps were
''' able to be eliminated and the logic was able to be moved to more relevant areas,
''' For example, the increment/decrement of the player's balance could be in the same
''' area as the notification of win/loss.
''' 
''' Some modern improvements were added, primarily the inclusion of a function, which
''' eliminated a thrice-repeated block of logic to display the card value.  The archaic
''' RND function is greatly simplified with the .NET Framework's Random class.
''' 
''' Elementary comments are provided for non-programmers or novices.
''' </summary>
Module Program
    Sub Main(args As String())
        ' These are the variables that will hold values during the program's execution
        Dim input As String
        Dim rnd As New Random ' You can create a new instance of an object during declaration
        Dim currentBalance As Integer = 100 ' You can set a initial value at declaration
        Dim currentWager As Integer
        Dim cardA, cardB, cardC As Integer ' You can specify multiple variables of the same type in one declaration statement

        ' Display the opening title and instructions
        ' Use a preceding $ to insert calculated values within the string using {}
        Console.WriteLine($"{Space((Console.WindowWidth \ 2) - 10)}ACEY DUCEY CARD GAME")
        Console.WriteLine($"{Space((Console.WindowWidth \ 2) - 21)}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER")
        Console.WriteLine("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP")
        Console.WriteLine("YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING")
        Console.WriteLine("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE")
        Console.WriteLine("A VALUE BETWEEN THE FIRST TWO.")
        Console.WriteLine("IF YOU DO NOT WANT TO BET, INPUT A 0")

        Do ' This loop continues as long as the player wants to keep playing

            Do ' This loop continues as long as the player has money to play

                Console.WriteLine("")
                Console.WriteLine($"YOU NOW HAVE {currentBalance} DOLLARS.")
                Console.WriteLine("")

                Console.WriteLine("HERE ARE YOUR NEXT TWO CARDS:")

                ' We need to ensure that card B is a higher value for our later comparison,
                ' so we will loop until we have two cards that meet this criteria
                Do
                    cardA = rnd.Next(2, 14)
                    cardB = rnd.Next(2, 14)

                Loop While cardA > cardB

                ' We use a function to display the text value of the numeric card value
                ' because we do this 3 times and a function reduces repetition of code
                Console.WriteLine(DisplayCard(cardA))
                Console.WriteLine(DisplayCard(cardB))

                Do ' This loop continues until the player provides a valid wager value
                    Console.WriteLine("")
                    Console.WriteLine("WHAT IS YOUR BET")

                    currentWager = 0
                    input = Console.ReadLine

                    ' Any input from the console is a string, but we require a number. 
                    ' Test the input to make sure it is a numeric value.
                    If Integer.TryParse(input, currentWager) Then
                        ' Test to ensure the player has not wagered more than their balance
                        If currentWager > currentBalance Then
                            Console.WriteLine("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.")
                            Console.WriteLine($"YOU HAVE ONLY {currentBalance} DOLLARS TO BET.")

                        Else
                            ' The player has provided a numeric value that is less/equal to their balance, 
                            ' exit the loop and continue play
                            Exit Do

                        End If ' check player balance

                    End If ' check numeric input

                Loop ' wager loop

                ' If the player is wagering, draw the third card, otherwise, mock them.
                If currentWager > 0 Then
                    cardC = rnd.Next(2, 14)

                    Console.WriteLine(DisplayCard(cardC))

                    ' The effort we made to have two cards in numeric order earlier makes this check easier,
                    ' otherwise we would have to have a second check in the opposite direction
                    If cardC < cardA OrElse cardC >= cardB Then
                        Console.WriteLine("SORRY, YOU LOSE")
                        currentBalance -= currentWager ' Shorthand code to decrement a number (currentBalance=currentBalance - currentWager)

                    Else
                        Console.WriteLine("YOU WIN!!!")
                        currentBalance += currentWager ' Shorthand code to increment a number (currentBalance=currentBalance + currentWager)

                    End If

                Else
                    Console.WriteLine("CHICKEN!!")
                    Console.WriteLine("")

                End If

            Loop While currentBalance > 0 ' loop as long as the player has money

            ' At this point, the player has no money (currentBalance=0).  Inform them of such.
            Console.WriteLine("")
            Console.WriteLine("SORRY, FRIEND, BUT YOU BLEW YOUR WAD.")
            Console.WriteLine("")
            Console.WriteLine("")

            ' We will loop to ensure the player provides some answer.
            Do
                Console.WriteLine("TRY AGAIN (YES OR NO)")
                Console.WriteLine("")

                input = Console.ReadLine

            Loop While String.IsNullOrWhiteSpace(input)

            ' We will assume that the player wants to play again only if they answer yes.
            ' (yeah and ya are valid as well, because we only check the first letter)
            If input.Substring(0, 1).Equals("y", StringComparison.CurrentCultureIgnoreCase) Then ' This allows upper and lower case to be entered.
                currentBalance = 100 ' Reset the players balance before restarting

            Else
                ' Exit the outer loop which will end the game.
                Exit Do

            End If

        Loop ' The full game loop

        Console.WriteLine("O.K., HOPE YOU HAD FUN!")

    End Sub

    ' This function is called for each of the 3 cards used in the game.
    ' The input and the output are both consistent, making it a good candidate for a function.
    Private Function DisplayCard(value As Integer) As String
        ' We check the value of the input and run a block of code for whichever
        ' evaluation matches
        Select Case value
            Case 2 To 10 ' Case statements can be ranges of values, also multiple values (Case 2,3,4,5,6,7,8,9,10)
                Return value.ToString

            Case 11
                Return "JACK"

            Case 12
                Return "QUEEN"

            Case 13
                Return "KING"

            Case 14
                Return "ACE"

        End Select

        ' Although we have full knowledge of the program and never plan to send an invalid
        ' card value, it's important to provide a message for the next developer who won't
        Throw New ArgumentOutOfRangeException("Card value must be between 2 and 14")

    End Function

End Module
