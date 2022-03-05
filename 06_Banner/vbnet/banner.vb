Imports System

Module Banner
    Private Horizontal As Integer
    Private Vertical As Integer
    Private Centered As Boolean
    Private Character As String
    Private Statement As String

    ' This provides a bit-ended representation of each symbol
    ' that can be output.  Each symbol Is defined by 7 parts -
    ' where each part Is an integer value that, when converted to
    ' the binary representation, shows which section Is filled in
    ' with values And which are spaces.  i.e., the 'filled in'
    ' parts represent the actual symbol on the paper.
    Private letters As Dictionary(Of Char, Integer()) = New Dictionary(Of Char, Integer()) From
        {
            {" ", {0, 0, 0, 0, 0, 0, 0}},
            {"A", {505, 37, 35, 34, 35, 37, 505}},
            {"B", {512, 274, 274, 274, 274, 274, 239}},
            {"C", {125, 131, 258, 258, 258, 131, 69}},
            {"D", {512, 258, 258, 258, 258, 131, 125}},
            {"E", {512, 274, 274, 274, 274, 258, 258}},
            {"F", {512, 18, 18, 18, 18, 2, 2}},
            {"G", {125, 131, 258, 258, 290, 163, 101}},
            {"H", {512, 17, 17, 17, 17, 17, 512}},
            {"I", {258, 258, 258, 512, 258, 258, 258}},
            {"J", {65, 129, 257, 257, 257, 129, 128}},
            {"K", {512, 17, 17, 41, 69, 131, 258}},
            {"L", {512, 257, 257, 257, 257, 257, 257}},
            {"M", {512, 7, 13, 25, 13, 7, 512}},
            {"N", {512, 7, 9, 17, 33, 193, 512}},
            {"O", {125, 131, 258, 258, 258, 131, 125}},
            {"P", {512, 18, 18, 18, 18, 18, 15}},
            {"Q", {125, 131, 258, 258, 322, 131, 381}},
            {"R", {512, 18, 18, 50, 82, 146, 271}},
            {"S", {69, 139, 274, 274, 274, 163, 69}},
            {"T", {2, 2, 2, 512, 2, 2, 2}},
            {"U", {128, 129, 257, 257, 257, 129, 128}},
            {"V", {64, 65, 129, 257, 129, 65, 64}},
            {"W", {256, 257, 129, 65, 129, 257, 256}},
            {"X", {388, 69, 41, 17, 41, 69, 388}},
            {"Y", {8, 9, 17, 481, 17, 9, 8}},
            {"Z", {386, 322, 290, 274, 266, 262, 260}},
            {"0", {57, 69, 131, 258, 131, 69, 57}},
            {"1", {0, 0, 261, 259, 512, 257, 257}},
            {"2", {261, 387, 322, 290, 274, 267, 261}},
            {"3", {66, 130, 258, 274, 266, 150, 100}},
            {"4", {33, 49, 41, 37, 35, 512, 33}},
            {"5", {160, 274, 274, 274, 274, 274, 226}},
            {"6", {194, 291, 293, 297, 305, 289, 193}},
            {"7", {258, 130, 66, 34, 18, 10, 8}},
            {"8", {69, 171, 274, 274, 274, 171, 69}},
            {"9", {263, 138, 74, 42, 26, 10, 7}},
            {"?", {5, 3, 2, 354, 18, 11, 5}},
            {"*", {69, 41, 17, 512, 17, 41, 69}},
            {"=", {41, 41, 41, 41, 41, 41, 41}},
            {"!", {1, 1, 1, 384, 1, 1, 1}},
            {".", {1, 1, 129, 449, 129, 1, 1}}
        }

    ' <summary>
    ' This displays the provided text on the screen And then waits for the user
    ' to enter a integer value greater than 0.
    ' </summary>
    ' <param name="DisplayText">Text to display on the screen asking for the input</param>
    ' <returns>The integer value entered by the user</returns>
    Private Function GetNumber(DisplayText As String) As Integer
        Console.Write(DisplayText)
        Dim TempStr As String = Console.ReadLine()
        Dim TempInt As Integer

        Int32.TryParse(TempStr, TempInt)

        If (TempInt <= 0) Then
            Throw New ArgumentException($"{DisplayText} must be greater than zero")
        End If

        Return TempInt
    End Function

    ' <summary>
    ' This displays the provided text on the screen And then waits for the user
    ' to enter a Y Or N.  It cheats by just looking for a 'y' and returning that
    ' as true.  Anything else that the user enters Is returned as false.
    ' </summary>
    ' <param name="DisplayText">Text to display on the screen asking for the input</param>
    ' <returns>Returns true Or false</returns>
    Private Function GetBool(DisplayText As String) As Boolean
        Console.Write(DisplayText)
        Return Console.ReadLine().StartsWith("y", StringComparison.InvariantCultureIgnoreCase)
    End Function

    ' <summary>
    ' This displays the provided text on the screen And then waits for the user
    ' to enter an arbitrary string.  That string Is then returned 'as-is'.
    ' </summary>
    ' <param name="DisplayText">Text to display on the screen asking for the input</param>
    ' <returns>The string entered by the user.</returns>
    Private Function GetString(DisplayText As String) As String
        Console.Write(DisplayText)
        Return (Console.ReadLine().ToUpper())
    End Function

    ' <summary>
    ' This queries the user for the various inputs needed by the program.
    ' </summary>
    Private Sub GetInput()
        Horizontal = GetNumber("Horizontal ")
        Vertical = GetNumber("Vertical ")
        Centered = GetBool("Centered ")
        Character = GetString("Character (type 'ALL' if you want character being printed) ")
        Statement = GetString("Statement ")
        ' We don't care about what the user enters here.  This is just telling them
        ' to set the page in the printer.
        GetString("Set page ")
    End Sub

    ' <summary>
    ' This prints out a single character of the banner - adding
    ' a few blanks lines as a spacer between characters.
    ' </summary>
    Private Sub PrintChar(ch As Char)
        ' In the trivial case (a space character), just print out the spaces
        If ch.Equals(" ") Then
            Console.WriteLine(New String("\n", 7 * Horizontal))
            Return
        End If

        ' If a specific character to be printed was provided by the user,
        ' then user that as our ouput character - otherwise take the
        ' current character
        Dim outCh As Char = IIf(Character.Equals("ALL"), ch, Character.Substring(0, 1))
        Dim letter(7) As Integer
        Try
            letters(outCh).CopyTo(letter, 0)
        Catch ex As KeyNotFoundException
            Throw New KeyNotFoundException($"The provided letter {outCh} was not found in the letters list")
        End Try

        ' This iterates through each of the parts that make up
        ' each letter.  Each part represents 1 * Horizontal lines
        ' of actual output.
        For idx As Integer = 0 To 7
            ' New int array declarations default to zeros
            ' numSections decides how many 'sections' need to be printed
            ' for a given line of each character
            Dim numSections(7) As Integer
            ' fillInSection decides whether each 'section' of the
            ' character gets filled in with the character Or with blanks
            Dim fillInSection(9) As Integer

            ' This uses the value in each part to decide which
            ' sections are empty spaces in the letter Or filled in
            ' spaces.  For each section marked with 1 in fillInSection,
            ' that will correspond to 1 * Vertical characters actually
            ' being output.
            For exp As Integer = 8 To 0 Step -1
                If (Math.Pow(2, exp) < letter(idx)) Then
                    fillInSection(8 - exp) = 1
                    letter(idx) -= Math.Pow(2, exp)
                    If (letter(idx) = 1) Then
                        ' Once we've exhausted all of the sections
                        ' defined in this part of the letter, then
                        ' we marked that number And break out of this
                        ' for loop.
                        numSections(idx) = 8 - exp
                        Exit For
                    End If
                End If
            Next exp

            ' Now that we know which sections of this part of the letter
            ' are filled in Or spaces, we can actually create the string
            ' to print out.
            Dim lineStr As String = ""

            If (Centered) Then
                lineStr += New String(" ", (63 - 4.5 * Vertical) * 1 / 1 + 1)
            End If

            For idx2 As Integer = 0 To numSections(idx)
                lineStr = lineStr + New String(IIf(fillInSection(idx2) = 0, " ", outCh), Vertical)
            Next idx2

            ' Then we print that string out 1 * Horizontal number of times
            For lineIdx As Integer = 1 To Horizontal
                Console.WriteLine(lineStr)
            Next lineIdx
        Next idx


        ' Finally, add a little spacer after each character for readability.
        Console.WriteLine(New String(Environment.NewLine, 2 * Horizontal - 1))
    End Sub

    ' <summary>
    ' This prints the entire banner based in the parameters
    ' the user provided.
    ' </summary>
    Private Sub PrintBanner()
        ' Iterate through each character in the statement
        For Each ch As Char In Statement
            PrintChar(ch)
        Next ch

        ' In the original version, it would print an additional 75 blank
        ' lines in order to feed the printer paper...don't really need this
        ' since we're not actually printing.
        Console.WriteLine(New String(Environment.NewLine, 75))
    End Sub

    ' <summary>
    ' Main entry point into the banner class And handles the main loop.
    ' </summary>
    Public Sub Play()
        GetInput()
        PrintBanner()
    End Sub
End Module

Module Program
    Sub Main(args As String())
        Banner.Play()
    End Sub
End Module
