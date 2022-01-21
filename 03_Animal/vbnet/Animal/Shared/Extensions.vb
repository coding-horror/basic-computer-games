Imports System.Runtime.CompilerServices

Public Module Extensions
    <Extension> Public Sub ForEach(Of T)(src As IEnumerable(Of T), action As Action(Of T))
        For Each x In src
            action(x)
        Next
    End Sub
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

    <Extension> Public Function ToTitleCase(s As String) As String
        If s Is Nothing Then Return Nothing
        Return Char.ToUpperInvariant(s(0)) + s.Substring(1).ToUpperInvariant
    End Function

    ' https://stackoverflow.com/a/3681580/111794
    <Extension> Public Function ToReverseCase(s As String) As String
        If s Is Nothing Then Return Nothing
        Return New String(s.Select(Function(c) If(
            Not Char.IsLetter(c),
            c,
            If(
                Char.IsUpper(c), Char.ToLowerInvariant(c), Char.ToUpperInvariant(c)
            )
            )).ToArray)
    End Function

    ' https://stackoverflow.com/a/58132204/111794
    <Extension> Public Function Slice(Of T)(lst As IList(Of T), start As Integer, [end] As Integer) As T()
        start = If(start >= 0, start, lst.Count + start)
        [end] = If([end] > 0, [end], lst.Count + [end])
        Return lst.Skip(start).Take([end] - start).ToArray
    End Function
End Module
