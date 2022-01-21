Public Class ConsoleAdapter
    Inherits ConsoleAdapterBase

    Public Overrides Sub Write(value As Object)
        Console.Write(value)
    End Sub

    Public Overrides Sub WriteLine(value As Object)
        Console.WriteLine(value)
    End Sub

    Public Overrides Sub WriteLine()
        Console.WriteLine()
    End Sub

    Public Overrides Sub WriteCenteredLine(value As Object)
        If Console.CursorLeft <> 0 Then Throw New InvalidOperationException("Can only write centered line if cursor is at start of line.")
        Dim toWrite = If(value?.ToString, "")
        Console.WriteLine($"{Space((Console.WindowWidth - toWrite.Length) \ 2)}{toWrite}")
    End Sub

    Public Overrides Function ReadLine() As String
        Dim response As String
        Do
            response = Console.ReadLine
        Loop While response Is Nothing
        Return response.Trim
    End Function
End Class
