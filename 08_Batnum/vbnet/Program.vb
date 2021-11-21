Imports System

Module BatNum
    Enum WinOptions
        Undefined = 0
        TakeLast = 1
        AvoidLast = 2
    End Enum

    Enum StartOptions
        Undefined = 0
        ComputerFirst = 1
        PlayerFirst = 2
    End Enum

    Dim pileSize As Integer = 0
    Dim minSelect As Integer = 0
    Dim maxSelect As Integer = 0
    Dim startOption As StartOptions = StartOptions.Undefined
    Dim winOption As WinOptions = WinOptions.Undefined

    ' <summary>
    ' Prints the intro and rules of the game.
    ' </summary>
    Private Sub PrintIntro()
        Console.WriteLine("BATNUM".PadLeft(33, " "))
        Console.WriteLine("CREATIVE COMPUTING  MORRISSTOWN, NEW JERSEY".PadLeft(15, " "))
        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine("THIS PROGRAM IS A 'BATTLE OF NUMBERS' GAME, WHERE THE")
        Console.WriteLine("COMPUTER IS YOUR OPPONENT.")
        Console.WriteLine()
        Console.WriteLine("THE GAME STARTS WITH AN ASSUMED PILE OF OBJECTS. YOU")
        Console.WriteLine("AND YOUR OPPONENT ALTERNATELY REMOVE OBJECTS FROM THE PILE.")
        Console.WriteLine("WINNING IS DEFINED IN ADVANCE AS TAKING THE LAST OBJECT OR")
        Console.WriteLine("NOT. YOU CAN ALSO SPECIFY SOME OTHER BEGINNING CONDITIONS.")
        Console.WriteLine("DON'T USE ZERO, HOWEVER, IN PLAYING THE GAME.")
        Console.WriteLine("ENTER A NEGATIVE NUMBER FOR NEW PILE SIZE TO STOP PLAYING.")
        Console.WriteLine()
    End Sub

    ' <summary>
    ' Asks the user for the various parameters necessary
    ' to play the game.
    ' </summary>
    Private Sub GetParams()
        ' Reset the game parameters
        pileSize = 0
        minSelect = 0
        maxSelect = 0
        startOption = StartOptions.Undefined
        winOption = WinOptions.Undefined

        While pileSize < 1
            Console.Write("ENTER PILE SIZE ")
            pileSize = Convert.ToInt32(Console.ReadLine())
        End While
        While winOption = WinOptions.Undefined
            Console.Write("ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST: ")
            winOption = Convert.ToInt32(Console.ReadLine())
        End While
        While minSelect < 1 Or maxSelect < 1 Or minSelect > maxSelect
            Console.Write("ENTER MIN AND MAX ")
            Dim vals = Console.ReadLine().ToString().Split(" ").[Select](Function(n) Integer.Parse(n)).ToList()
            If vals.Count() <> 2 Then
                Continue While
            End If
            minSelect = vals(0)
            maxSelect = vals(1)
        End While
        While startOption = StartOptions.Undefined
            Console.Write("ENTER START OPTION - 1 COMPUTER FIRST, 2 YOU FIRST ")
            startOption = Convert.ToInt32(Console.ReadLine())
        End While
    End Sub

    '<summary>
    'This handles the player's turn - asking the player how many objects
    'to take And doing some basic validation around that input.  Then it
    'checks for any win conditions.
    '</summary>
    '<returns>Returns a Boolean indicating whether the game Is over And the New pileSize.</returns>'
    Private Function PlayerMove() As Boolean
        Dim playerDone As Boolean = False

        While Not playerDone
            Console.WriteLine("YOUR MOVE ")
            Dim playerNum As Integer = Convert.ToInt32(Console.ReadLine())
            If playerNum = 0 Then
                Console.WriteLine("I TOLD YOU NOT TO USE ZERO!  COMPUTER WINS BY FORFEIT.")
                Return True
            End If
            If playerNum > maxSelect Or playerNum < minSelect Then
                Console.WriteLine("ILLEGAL MOVE, REENTER IT")
                Continue While
            End If

            pileSize = pileSize - playerNum
            playerDone = True
            If pileSize <= 0 Then
                If winOption = WinOptions.AvoidLast Then
                    Console.WriteLine("TOUGH LUCK, YOU LOSE.")
                Else
                    Console.WriteLine("CONGRATULATIONS, YOU WIN.")
                End If
                Return True
            End If
        End While

        Return False
    End Function

    '<summary>
    'This handles the logic to determine how many objects the computer
    'will select on its turn.
    '</summary>
    Private Function ComputerPick() As Integer
        Dim q As Integer = IIf(winOption = WinOptions.AvoidLast, pileSize - 1, pileSize)
        Dim c As Integer = minSelect + maxSelect
        Dim computerNum As Integer = q - (c * Int(q / c))
        If computerNum < minSelect Then
            computerNum = minSelect
        End If
        If computerNum > maxSelect Then
            ComputerPick = maxSelect
        End If

        Return computerNum
    End Function

    '<summary>
    'This handles the computer's turn - first checking for the various
    'win/lose conditions And then calculating how many objects
    'the computer will take.
    '</summary>
    '<returns>Returns a boolean indicating whether the game is over.</returns>'
    Private Function ComputerMove() As Boolean
        ' First, check For win conditions On this move
        ' In this Case, we win by taking the last Object And
        ' the remaining pile Is less than max Select
        ' so the computer can grab them all And win
        If winOption = WinOptions.TakeLast And pileSize <= maxSelect Then
            Console.WriteLine($"COMPUTER TAKES {pileSize} AND WINS.")
            Return True
        End If
        ' In this Case, we lose by taking the last Object And
        ' the remaining pile Is less than minsize And the computer
        ' has To take all Of them.
        If winOption = WinOptions.AvoidLast And pileSize <= minSelect Then
            Console.WriteLine($"COMPUTER TAKES {minSelect} AND LOSES.")
            Return True
        End If

        ' Otherwise, we determine how many the computer selects
        Dim currSel As Integer = ComputerPick()
        pileSize = pileSize - currSel
        Console.WriteLine($"COMPUTER TAKES {currSel} AND LEAVES {pileSize}")
        Return False
    End Function

    '<summary>
    'This is the main game loop - repeating each turn until one
    'of the win/lose conditions Is met.
    '</summary>
    Private Sub PlayGame()
        Dim gameOver As Boolean = False
        ' playersTurn Is a Boolean keeping track Of whether it's the
        ' player's or computer's turn
        Dim playersTurn As Boolean = (startOption = StartOptions.PlayerFirst)

        While Not gameOver
            If playersTurn Then
                gameOver = PlayerMove()
                playersTurn = False
                If gameOver Then Return
            End If

            If Not playersTurn Then
                gameOver = ComputerMove()
                playersTurn = True
            End If
        End While
    End Sub

    Public Sub Play()
        While True
            PrintIntro()
            GetParams()
            PlayGame()
        End While
    End Sub
End Module

Module Program
    Sub Main(args As String())
        BatNum.Play()
    End Sub
End Module
