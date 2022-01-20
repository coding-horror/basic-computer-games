Option Compare Text

Public Class Game
    Private Shared ReadOnly YesNoResponses As New Dictionary(Of String, Boolean)(StringComparer.InvariantCultureIgnoreCase) From {
        {"yes", True},
        {"y", True},
        {"true", True},
        {"t", True},
        {"1", True},
        {"no", False},
        {"n", False},
        {"false", False},
        {"f", False},
        {"0", False}
    }

    ReadOnly console As ConsoleAdapterBase
    ReadOnly root As New Branch With {
        .Text = "DOES IT SWIM?",
        .Yes = New Branch With {.Text = "FISH"},
        .No = New Branch With {.Text = "BIRD"}
    }

    ''' <summary>Reduces a string or console input to True, False or Nothing. Case-insensitive.</summary>
    ''' <param name="s">Optional String to reduce via the same logic. If not passed in, will use console.ReadLine</param>
    ''' <returns>
    ''' Returns True for a "yes" response (yes, y, true, t, 1) and False for a "no" response (no, n, false, f, 0).<br/>
    ''' Returns Nothing if the response doesn't match any of these.
    ''' </returns>
    Private Function GetYesNo(Optional s As String = Nothing) As Boolean?
        s = If(s, console.ReadLine)
        Dim ret As Boolean
        If YesNoResponses.TryGetValue(s, ret) Then Return ret
        Return Nothing
    End Function

    Sub New(console As ConsoleAdapterBase)
        If console Is Nothing Then Throw New ArgumentNullException(NameOf(console))
        Me.console = console
    End Sub


    Sub BeginLoop()
        console.WriteCenteredLine("ANIMAL")
        console.WriteCenteredLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
        console.Write(
"


PLAY 'GUESS THE ANIMAL'

THINK OF AN ANIMAL AND THE COMPUTER WILL TRY TO GUESS IT.

")

        Do
            console.Write("ARE YOU THINKING OF AN ANIMAL? ")

            Dim response = console.ReadLine
            If response = "list" Then
                console.WriteLine(
"
ANIMALS I ALREADY KNOW ARE:")
                root.DescendantTexts.ForEach(Sub(text, index)
                                                 If index > 0 AndAlso index Mod 4 = 0 Then console.WriteLine()
                                                 console.Write($"{text.MaxLength(15),-15}")
                                             End Sub)
                console.WriteLine(
"
")
                Continue Do
            End If

            Dim ynResponse = GetYesNo(response)
            If ynResponse Is Nothing OrElse Not ynResponse Then Continue Do

            Dim currentBranch = root
            Do While Not currentBranch.IsEnd
                console.Write($"{currentBranch.Text} ")
                Do
                    ynResponse = GetYesNo()
                Loop While ynResponse Is Nothing
                currentBranch = If(
                    ynResponse,
                    currentBranch.Yes,
                    currentBranch.No
                )
            Loop

            'at end
            console.Write($"IS IT A {currentBranch.Text}? ")
            ynResponse = GetYesNo()
            If ynResponse Then ' No difference between False or Nothing
                console.WriteLine("WHY NOT TRY ANOTHER ANIMAL?")
                Continue Do
            End If

            console.Write("THE ANIMAL YOU WERE THINKING OF WAS A ? ")
            Dim newAnimal = console.ReadLine

            console.WriteLine(
$"PLEASE TYPE IN A QUESTION THAT WOULD DISTINGUISH A 
{newAnimal} FROM A {currentBranch.Text}")
            Dim newQuestion = console.ReadLine

            console.Write(
$"FOR A {newAnimal} THE ANSWER WOULD BE ? ")
            Do
                ynResponse = GetYesNo()
            Loop While ynResponse Is Nothing

            Dim newBranch = New Branch With {.Text = newAnimal}
            Dim currentBranchCopy = New Branch With {.Text = currentBranch.Text}
            currentBranch.Text = newQuestion
            If ynResponse Then
                currentBranch.Yes = newBranch
                currentBranch.No = currentBranchCopy
            Else
                currentBranch.No = newBranch
                currentBranch.Yes = currentBranchCopy
            End If

            ' TODO how do we exit?
        Loop
    End Sub
End Class
