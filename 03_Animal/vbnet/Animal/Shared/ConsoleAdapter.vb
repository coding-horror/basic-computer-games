Public Class ConsoleAdapter
    Inherits ConsoleAdapterBase

    Public Overrides Sub Write(value As Object)
        Console.Write(value)
    End Sub

    Public Overrides Sub WriteLine()
        Console.WriteLine()
    End Sub

    Public Overrides Sub WriteCenteredLines(value As Object)
        If Console.CursorLeft <> 0 Then WriteLine()
        Dim toWrite = If(value?.ToString, "")
        Write($"{Space((Console.WindowWidth - toWrite.Length) \ 2)}{toWrite}")
        WriteLine()
    End Sub

    Public Overrides Function ReadLine() As String
        Dim response As String
        Do
            response = Console.ReadLine
        Loop While response Is Nothing
        Return response.Trim
    End Function
End Class
