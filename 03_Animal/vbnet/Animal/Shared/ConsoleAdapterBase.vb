Public MustInherit Class ConsoleAdapterBase
    Public MustOverride Sub Write(value As Object)
    Public MustOverride Sub WriteLine(value As Object)
    Public MustOverride Sub WriteLine()
    Public MustOverride Sub WriteCenteredLine(value As Object)

    ''' <summary>Implementations should always return a String without leading or trailing whitespace, never Nothng</summary>
    Public MustOverride Function ReadLine() As String
End Class
