Public Class AceyDucey
    ''' <summary>
    ''' Create a single instance of the Random class to be used 
    ''' throughout the program. 
    ''' </summary>
    Private ReadOnly Property Rnd As New Random()

    ''' <summary>
    ''' Define a varaible to store the the player balance. <br/>
    ''' Defaults to 0
    ''' </summary>
    ''' <remarks>
    ''' Since <see cref="Integer"/> is a value type, and no value 
    ''' has been explicitly set, the default value of the type is used.
    ''' </remarks>
    Private _balance As Integer

    Public Sub New()
        DisplayIntroduction()
    End Sub

    ''' <summary>
    ''' Play multiple games of Acey Ducey until the player chooses to quit.
    ''' </summary>
    Public Sub Play()
        Do
            PlayGame()
        Loop While TryAgain() 'Loop (play again) based on the Boolean value returned by TryAgain

        Console.WriteLine("O.K., HOPE YOU HAD FUN!")
    End Sub

    ''' <summary>
    ''' Play a game of Acey Ducey, which ends when the player balance reaches 0
    ''' </summary>
    Private Sub PlayGame()
        _balance = 100 'At the start of the game, set the player balance to 100

        Console.WriteLine()
        Console.WriteLine($"YOU NOW HAVE {_balance} DOLLARS.")

        Do
            PlayTurn()
        Loop While _balance > 0 'Continue playing while the user has a balance

        Console.WriteLine()
        Console.WriteLine("SORRY, FRIEND, BUT YOU BLEW YOUR WAD.")
    End Sub

    ''' <summary>
    ''' Play one turn of Acey Ducey
    ''' </summary>
    ''' <remarks>
    ''' A turn consists of displaying to cards, making a wager 
    ''' and determining the result (win/lose)
    ''' </remarks>
    Private Sub PlayTurn()
        Console.WriteLine()
        Console.WriteLine("HERE ARE YOUR NEXT TWO CARDS: ")

        Dim cards = GetOrderedCards()

        For Each card In cards
            DisplayCard(card)
        Next

        Dim wager As Integer = GetWager()
        Dim finalCard As Integer = GetCard()

        If wager = 0 Then
            Console.WriteLine("CHICKEN!!")
            Return
        End If

        DisplayCard(finalCard)

        Console.WriteLine()

        '''Check if the value of the final card is between the first and second cards.
        '''
        '''The use of AndAlso is used to short-circuit the evaluation of the IF condition.
        '''Short-circuiting means that both sides of the condition do not need to be
        '''evaluated. In this case, if the left criteria returns FALSE, the right criteria 
        '''is ignored and the evaluation result is returned as FALSE.
        '''
        '''This works because AndAlso requires both condition to return TRUE  in order to be 
        '''evaluated as TRUE. If the first condition is FALSE we already know the evaluation result. 
        If finalCard >= cards.First() AndAlso finalCard <= cards.Last() Then
            Console.WriteLine("YOU WIN!!!")
            _balance += wager 'Condensed version of _balance = _balance + wager
        Else
            Console.WriteLine("SORRY, YOU LOSE.")
            _balance -= wager 'Condensed version of _balance = _balance - wager
        End If
    End Sub

    ''' <summary>
    ''' Get two cards in ascending order
    ''' </summary>
    ''' <remarks>
    ''' The original version generates two cards (A and B)
    ''' If A is greater than or equal to B, both cards are regenerated. 
    ''' <br/><br/> 
    ''' This version generates the two cards, but only regenerates A 
    ''' if A is equal to B. The cards are then returned is ascending order,
    ''' ensuring that A is less than B (maintaining the original end result)
    ''' </remarks>
    Private Function GetOrderedCards() As Integer()
        '''When declaring fixed size arrays in VB.NET you declare the MAX INDEX of the array 
        '''and NOT the SIZE (number of elements) of the array.
        '''As such, card(1) gives you and array with index 0 and index 1, which means
        '''the array stores two elements and not one
        Dim cards(1) As Integer

        cards(0) = GetCard()
        cards(1) = GetCard()

        'Perform this action as long as the first card is equal to the second card
        While cards(0) = cards(1)
            cards(0) = GetCard()
        End While

        Array.Sort(cards) 'Sort the values in ascending order

        Return cards
    End Function

    ''' <summary>
    ''' Get a random number (card) ranked 2 to 14
    ''' </summary>
    Private Function GetCard() As Integer
        Return Rnd.Next(2, 15)
    End Function

    ''' <summary>
    ''' Display the face value of the card 
    ''' </summary>
    Private Sub DisplayCard(card As Integer)
        Dim output As String

        Select Case card
            Case 2 To 10
                output = card.ToString()
            Case 11
                output = "JACK"
            Case 12
                output = "QUEEN"
            Case 13
                output = "KING"
            Case 14
                output = "ACE"
            Case Else
                Throw New ArgumentOutOfRangeException(NameOf(card), "Value must be between 2 and 14")
        End Select

        Console.WriteLine(output)
    End Sub

    ''' <summary>
    ''' Prompt the user to make a bet
    ''' </summary>
    ''' <remarks>
    ''' The function will not return until a valid bet is made. <br/>
    ''' <see cref="Int32.TryParse(String, ByRef Integer)"/> is used to validate that the user input is a valid <see cref="Integer"/>
    ''' </remarks>
    Private Function GetWager() As Integer
        Dim wager As Integer
        Do
            Console.WriteLine()
            Console.Write("WHAT IS YOUR BET? ")

            Dim input As String = Console.ReadLine()

            '''Determine if the user input is an Integer
            '''If it is an Integer, store the value in the variable wager
            If Not Integer.TryParse(input, wager) Then
                Console.WriteLine("SORRY, I DID'T QUITE GET THAT.")
                Continue Do 'restart the loop
            End If

            'Prevent the user from betting more than their current balance
            If _balance < wager Then
                Console.WriteLine("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.")
                Console.WriteLine($"YOU HAVE ONLY {_balance} DOLLARS TO BET.")
                Continue Do 'restart the loop
            End If

            'Prevent the user from betting negative values
            If wager < 0 Then
                Console.WriteLine("FUNNY GUY! YOU CANNOT MAKE A NEGATIVE BET.")
                Continue Do 'restart the loop
            End If

            Exit Do 'If we get to this line, exit the loop as all above validations passed
        Loop

        Return wager
    End Function

    ''' <summary>
    ''' Prompt the user to try again
    ''' </summary>
    ''' <remarks>
    ''' This function will not return until a valid reponse is given
    ''' </remarks>
    Private Function TryAgain() As Boolean
        Dim response As String
        Do
            Console.Write("TRY AGAIN (YES OR NO) ")

            response = Console.ReadLine()

            If response.Equals("YES", StringComparison.OrdinalIgnoreCase) Then Return True
            If response.Equals("NO", StringComparison.OrdinalIgnoreCase) Then Return False

            Console.WriteLine("SORRY, I DID'T QUITE GET THAT.")
        Loop
    End Function

    ''' <summary>
    ''' Display the opening title and instructions
    ''' </summary>
    ''' <remarks>
    ''' Refer to 
    ''' <see href="https://docs.microsoft.com/en-us/dotnet/visual-basic/programming-guide/language-features/strings/interpolated-strings">
    '''     Interpolated Strings
    ''' </see> documentation for the use of $ and { } with strings
    ''' </remarks>
    Private Sub DisplayIntroduction()
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
        Console.WriteLine("")
    End Sub
End Class
