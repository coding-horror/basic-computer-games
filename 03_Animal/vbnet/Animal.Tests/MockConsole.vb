Imports System.IO

Public Class MockConsole
    Inherits ConsoleAdapterBase

    Private inputs As Queue(Of String)
    Public ReadOnly Lines As New List(Of (line As String, centered As Boolean)) From {
        ("", False)
    }

    ' TODO it's possible to clear all the lines, and we'd have to check once again in WriteString and WriteCenteredLine if there are any lines

    Sub New(Inputs As IEnumerable(Of String))
        Me.inputs = New Queue(Of String)(Inputs)
    End Sub

    Private Sub CheckLinesInitialized()
        If Lines.Count = 0 Then Lines.Add(("", False))
    End Sub

    Private Sub WriteString(s As String, Optional centered As Boolean = False)
        If s Is Nothing Then Return
        CheckLinesInitialized()
        s.Split(Environment.NewLine).ForEach(Sub(line, index)
                                                 If index = 0 Then
                                                     Dim currentLast = Lines(Lines.Count - 1)
                                                     ' centered should never come from the current last line
                                                     ' if WriteCenteredLine is called, it immediately creates a new line
                                                     Lines(Lines.Count - 1) = (currentLast.line + line, centered)
                                                 Else
                                                     Lines.Add((line, centered))
                                                 End If
                                             End Sub)
    End Sub

    Public Overrides Sub Write(value As Object)
        WriteString(value?.ToString)
    End Sub

    Public Overrides Sub WriteLine(value As Object)
        WriteString(value?.ToString)
        WriteLine()
    End Sub

    Public Overrides Sub WriteLine()
        Lines.Add(("", False))
    End Sub

    Public Overrides Sub WriteCenteredLine(value As Object)
        If Lines.Count = 0 Then Lines.Add(("", False))
        Dim currentLast = Lines(Lines.Count - 1).line
        If currentLast.Length > 0 Then Throw New InvalidOperationException("Can only write centered line if cursor is at start of line.")
        WriteString(value?.ToString, True)
        WriteLine()
    End Sub

    Public Overrides Function ReadLine() As String
        ' Indicates the end of a test run, for programs which loop endlessly
        If inputs.Count = 0 Then Throw New EndOfStreamException("End of inputs")

        Dim nextInput = inputs.Dequeue.Trim
        WriteLine(nextInput)
        Return nextInput
    End Function
End Class
