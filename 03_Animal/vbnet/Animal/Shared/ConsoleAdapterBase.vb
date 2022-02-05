Public MustInherit Class ConsoleAdapterBase
    Public MustOverride Sub Write(value As Object)
    Public MustOverride Sub WriteLine()
    Public MustOverride Sub WriteCenteredLines(value As Object)

    ''' <summary>Implementations should always return a String without leading or trailing whitespace, never Nothng</summary>
    Public MustOverride Function ReadLine() As String

    Public Sub WriteLine(value As Object)
        Write(value)
        WriteLine()
    End Sub
End Class
