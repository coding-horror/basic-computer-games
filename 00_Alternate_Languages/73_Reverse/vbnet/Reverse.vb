Imports System

Module Reverse


    '  VB.NET Port of Reverse from book BASIC COMPUTER GAMES.
    '  Console app for .NET Core 3.1
    '  Some simplification and error checking was added.
    '  David Widel

    Const NumberOfDigits As Integer = 9

    Private Numbers As New Generic.List(Of Integer)



    Sub Main(args As String())

        DisplayIntro()

        Console.WriteLine("DO YOU WANT THE RULES? (Y/N)")
        If Console.ReadKey(True).Key = ConsoleKey.Y Then
            DisplayRules()
        End If



        Dim playing As Boolean = True

        Do While (playing)  ' New game loop.

            InitializeRandomNumberList()

            Console.WriteLine("HERE WE GO ... THE LIST IS:")
            DisplayNumbers()

            Dim Counter As Integer = 0

            While True ' Turn loop.

                Dim ReverseNumber As Integer = GetHowManyToReverse()
                If ReverseNumber = 0 Then
                    If Not TryAgain() Then
                        playing = False
                    End If
                    Continue Do
                End If

                PerformReverse(ReverseNumber)
                Counter += 1
                DisplayNumbers()

                If DidPlayerWin() Then
                    Console.WriteLine("YOU WON IN " & Counter.ToString & " MOVES")
                    If Not TryAgain() Then
                        Console.WriteLine("O.K. HOPE YOU HAD FUN!!")
                        playing = False
                    End If
                    Continue Do
                End If

            End While

        Loop




    End Sub

    Sub DisplayIntro()
        Console.WriteLine("REVERSE")
        Console.WriteLine("CREATIVE COMPUTING    MORRISTOWN, NEW JERSEY")
        Console.WriteLine("REVERSE -- A GAME OF SKILL")


    End Sub

    Sub DisplayRules()

        Console.WriteLine("THIS IS THE GAME OF REVERSE. TO WIN, ALL YOU HAVE")
        Console.WriteLine("TO DO IS ARRANGE A LIST OF NUMBERS (1 THROUGH " & NumberOfDigits.ToString & ")")
        Console.WriteLine("IN NUMERICAL ORDER FROM LEFT TO RIGHT. TO MOVE, YOU")
        Console.WriteLine("TELL ME HOW MANY NUMBERS (COUNTING FROM THE LEFT) TO")
        Console.WriteLine("REVERSE. FOR EXAMPLE, IF THE CURRENT LIST IS:")
        Console.WriteLine()
        Console.WriteLine("2 3 4 5 1 6 7 8 9")
        Console.WriteLine()
        Console.WriteLine("AND YOU REVERSE 4, THE RESULT WILL BE:")
        Console.WriteLine()
        Console.WriteLine("5 4 3 2 1 6 7 8 9")
        Console.WriteLine()
        Console.WriteLine("NOW IF YOU REVERSE 5, YOU WIN!")
        Console.WriteLine()
        Console.WriteLine("1 2 3 4 5 6 7 8 9")
        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine("NO DOUBT YOU WILL LIKE THIS GAME, BUT")
        Console.WriteLine("IF YOU WANT TO QUIT, REVERSE O (ZERO).")
        Console.WriteLine()

    End Sub

    Sub InitializeRandomNumberList()

        Dim R As New Random
        Numbers.Clear()

        Do Until Numbers.Count = NumberOfDigits

            Dim NewNumber = R.Next(1, NumberOfDigits + 1) 'Lower bound is inclusive, Upper bound is exclusive

            If Not Numbers.Contains(NewNumber) Then
                Numbers.Add(NewNumber)
            End If

        Loop


        If DidPlayerWin() Then
            Numbers.Reverse()
        End If

    End Sub

    Private Sub DisplayNumbers()
        For i As Integer = 0 To Numbers.Count - 1
            Console.Write(Numbers(i).ToString(" #"))
        Next
        Console.WriteLine()
    End Sub

    Function GetHowManyToReverse() As Integer
        Console.WriteLine("HOW MANY SHALL I REVERSE?")

        Do

            Dim K = Console.ReadLine
            Dim ReverseNumber As Integer

            If Integer.TryParse(K, ReverseNumber) Then
                If ReverseNumber <= NumberOfDigits Then
                    Return ReverseNumber
                Else
                    Console.WriteLine("OOPS! TOO MANY! I CAN REVERSE AT MOST " & NumberOfDigits.ToString)
                End If
            Else
                'Added check.
                Console.WriteLine("OOPS! NUMBERS PLEASE!")
            End If

        Loop

    End Function

    Sub PerformReverse(ReverseNumber As Integer)

        ' We will make pointers to the 2 digits to swap, swap them, and converge the pointers.
        Dim LowerPointer As Integer = 1
        Dim UpperPointer As Integer = ReverseNumber

        Do
            'Since our list begins at 0 we must always subtract one to access a digit.
            Dim temp As Integer = Numbers(LowerPointer - 1)
            Numbers(LowerPointer - 1) = Numbers(UpperPointer - 1)
            Numbers(UpperPointer - 1) = temp
            LowerPointer += 1
            UpperPointer -= 1

        Loop Until UpperPointer <= LowerPointer

    End Sub

    Function DidPlayerWin() As Boolean
        For i As Integer = 0 To Numbers.Count - 2
            If Numbers(i) > Numbers(i + 1) Then Return False
        Next
        Return True
    End Function


    Private Function TryAgain() As Boolean
        Console.WriteLine("TRY AGAIN? (Y OR N)")
        If Console.ReadKey(True).Key = ConsoleKey.Y Then
            Return True
        Else
            Return False
        End If
    End Function




End Module
