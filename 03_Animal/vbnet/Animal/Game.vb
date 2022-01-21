Option Compare Text

Public Class Game
    ' This Dictionary holds the corresponding value for each of the variants of "YES" and "NO" we accept
    ' Note that the Dictionary is case-insensitive, meaning it maps "YES", "yes" and even "yEs" to True
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

    ' The pre-initialized root branch
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
        ' Print the program heading
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
                ' List all the stored animals
                console.WriteLine(
"
ANIMALS I ALREADY KNOW ARE:")

                ' We're using a ForEach extension method instead of the regular For Each loop to provide the index alongside the text
                root.DescendantTexts.ForEach(Sub(text, index)
                                                 ' We want to move to the next line after every four animals
                                                 ' But for the first animal, where the index is 0, 0 Mod 4 will also return 0
                                                 ' So we have to explicitly exclude the first animal
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
                ' Branches can either be questions, or end branches
                ' We have to walk the questions, prompting each time for "yes" or "no"
                console.Write($"{currentBranch.Text} ")
                Do
                    ynResponse = GetYesNo()
                Loop While ynResponse Is Nothing

                ' Depending on the answer, we'll follow either the branch at "Yes" or "No"
                currentBranch = If(
                    ynResponse,
                    currentBranch.Yes,
                    currentBranch.No
                )
            Loop

            ' Now we're at an end branch
            console.Write($"IS IT A {currentBranch.Text}? ")
            ynResponse = GetYesNo()
            If ynResponse Then ' Only if ynResponse = True will we go into this If Then
                console.WriteLine("WHY NOT TRY ANOTHER ANIMAL?")
                Continue Do
            End If

            ' Get the new animal
            console.Write("THE ANIMAL YOU WERE THINKING OF WAS A ? ")
            Dim newAnimal = console.ReadLine

            ' Get the question used to distinguish the new animal from the current end branch
            console.WriteLine(
$"PLEASE TYPE IN A QUESTION THAT WOULD DISTINGUISH A 
{newAnimal} FROM A {currentBranch.Text}")
            Dim newQuestion = console.ReadLine

            ' Get the answer to that question, for the new animal
            ' for the old animal, the answer would be the opposite
            console.Write(
$"FOR A {newAnimal} THE ANSWER WOULD BE ? ")
            Do
                ynResponse = GetYesNo()
            Loop While ynResponse Is Nothing

            ' Create the new end branch for the new animal
            Dim newBranch = New Branch With {.Text = newAnimal}

            ' Copy over the current animal to another new end branch
            Dim currentBranchCopy = New Branch With {.Text = currentBranch.Text}

            ' Make the current branch into the distinguishing question
            currentBranch.Text = newQuestion

            ' Set the Yes and No branches of the current branch according to the answer
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
