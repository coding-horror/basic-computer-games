Imports System.Runtime.CompilerServices

Public Module Extensions
    <Extension> Public Sub ForEach(Of T)(src As IEnumerable(Of T), action As Action(Of T, Integer))
        Dim index As Integer
        For Each x In src
            action(x, index)
            index += 1
        Next
    End Sub

    <Extension> Public Function MaxLength(s As String, value As Integer) As String
        If s Is Nothing Then Return Nothing
        Return s.Substring(0, Math.Min(s.Length, value))
    End Function

    <Extension> Public Function ForceEndsWith(s As String, toAppend As String) As String
        If Not s.EndsWith(toAppend, StringComparison.OrdinalIgnoreCase) Then s += toAppend
        Return s
    End Function
End Module
