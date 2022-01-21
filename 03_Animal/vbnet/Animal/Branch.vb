Public Class Branch
    Public Property Text As String

    Public ReadOnly Property IsEnd As Boolean
        Get
            Return Yes Is Nothing AndAlso No Is Nothing
        End Get
    End Property

    Public Property Yes As Branch
    Public Property No As Branch

    ' Allows walking all the descendants recursively
    Public Iterator Function DescendantTexts() As IEnumerable(Of String)
        If Yes IsNot Nothing Then
            Yield Yes.Text
            For Each childText In Yes.DescendantTexts
                Yield childText
            Next
        End If

        If No IsNot Nothing Then
            Yield No.Text
            For Each childText In No.DescendantTexts
                Yield childText
            Next
        End If
    End Function
End Class
