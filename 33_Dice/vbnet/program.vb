Imports System

Module Program
    Sub Main(args As String())
        Const header As String =
"                   DICE
CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



THIS PROGRAM SIMULATES THE ROLLING OF A
PAIR OF DICE.
YOU ENTER THE NUMBER OF TIMES YOU WANT THE COMPUTER TO
/ROLL/ THE DICE.  WATCH OUT, VERY LARGE NUMBERS TAKE
A LONG TIME.  IN PARTICULAR, NUMBERS OVER 5000."

        Console.WriteLine(header)

        Dim D6 As New Random()
        Dim continuePrompt As String = "YES"
        While continuePrompt = "YES"
            Console.Write($"{vbCrLf}HOW MANY ROLLS? ")
            Dim x As Integer = Convert.ToInt32(Console.ReadLine())
            Dim F = Enumerable.Repeat(0, 11).ToList()
            For s As Integer = 0 To x - 1
                F(D6.Next(6) + D6.Next(6)) += 1
            Next

            Console.WriteLine($"{vbCrLf}TOTAL SPOTS   NUMBER OF TIMES")
            For V As Integer = 0 To 10
                Console.WriteLine($" {V + 2}{vbTab,-8}{F(V)}")
            Next

            Console.Write($"{vbCrLf}TRY AGAIN ")
            continuePrompt = Console.ReadLine().ToUpper()
        End While
    End Sub
End Module
