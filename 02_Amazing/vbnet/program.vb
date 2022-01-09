Imports System

Module Program
    Dim H As Integer = 0, V As Integer = 0, Q As Integer = 0, Z As Integer = 0, C As Integer = 2, R As Integer, S As Integer = 1
    Dim rnd As New Random()
    Sub Main(args As String())
        Const header As String =
"             AMAZING PROGRAM
 CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



"
        Console.WriteLine(header)


        While H <= 1 AndAlso V <= 1
            Console.Write("WHAT ARE YOUR WIDTH AND LENGTH? ")

            'We no longer have the old convenient INPUT command, so need to parse out the inputs
            Dim parts = Console.ReadLine().Split(","c).Select(Function(s) Convert.ToInt32(s.Trim())).ToList()
            H = parts(0)
            V = parts(1)

            If H <= 1 OrElse V <= 1 Then Console.WriteLine("MEANINGLESS DIMENSIONS.  TRY AGAIN.")
        End While

        Dim W(H, V) As Integer, VV(H, V) As Integer

        Console.WriteLine("

")

        'TOP LINE
        Dim X As Integer = rnd.Next(1, H + 1)
        For i As Integer = 0 To H - 1
            If i = X Then Console.Write(".  ") Else Console.Write(".--")
        Next
        Console.WriteLine(".")
        W(X, 1) = 1
        R = X

        LoopPrep(W)

        Dim keepGoing As Boolean = True
        While keepGoing
            '260
            If r - 1 = 0 OrElse W(r - 1, S) <> 0 Then
                keepGoing = SelectRandomPath(W, VV) '530
            ElseIf S - 1 = 0 OrElse W(r, S - 1) <> 0 Then
                keepGoing = SelectRandomPathAlt(W, VV) '390 --- these may be named backwards
            ElseIf R = H OrElse W(R + 1, S) <> 0 Then
                keepGoing = SelectRandomPathAlt2(W, VV)
            Else
                keepGoing = SelectRandomMethod(W, VV, 1, 2, 3)
            End If
        End While

        PrintFinalResults(VV)
    End Sub

    Public Sub LoopPrep(W(,) As Integer)
        Do
            If r <> H Then
                r += 1
            ElseIf S <> V Then
                r = 1
                S += 1
            Else
                r = 1
                S = 1
            End If
        Loop While W(r, S) = 0
    End Sub

    Dim methods() As Func(Of Integer(,), Integer(,), Boolean) = {AddressOf Random1, AddressOf Random2, AddressOf Random3, AddressOf Random4}
    Public Function SelectRandomMethod(W(,) As Integer, VV(,) As Integer, ParamArray possibles() As Integer) As Boolean
        Dim x As Integer = rnd.Next(0, possibles.Length)
        Return methods(possibles(x) - 1)(W, VV)
    End Function

    'This method may opt to loop back to 210 rather than select a random function
    Public Function SelectRandomPath(W(,) As Integer, VV(,) As Integer) As Boolean '530
        If S - 1 = 0 OrElse W(R, S - 1) <> 0 Then '670
            If R = H OrElse W(R + 1, S) <> 0 Then '740
                If S <> V Then '760
                    If W(R, S + 1) <> 0 Then '780
                        LoopPrep(W)
                        Return True ' Goto 210
                    Else
                        Return Random4(W, VV)
                    End If
                ElseIf Z = 1 Then '780
                    LoopPrep(W)
                    Return True
                Else
                    Q = 1
                    Return Random4(W, VV)
                End If
            ElseIf S <> V Then '700
                If W(R, S + 1) <> 0 Then '730
                    Return Random3(W, VV)
                Else
                    Return SelectRandomMethod(W, VV, 3, 4)
                End If
            ElseIf Z = 1 Then '730
                Return Random3(W, VV)
            Else
                Q = 1
                Return SelectRandomMethod(W, VV, 3, 4)
            End If
        ElseIf R = H OrElse W(R + 1, S) <> 0 Then '610
            If S <> V Then '630
                If W(R, S + 1) <> 0 Then '660
                    Return Random2(W, VV)
                Else
                    Return SelectRandomMethod(W, VV, 2, 4)
                End If
            ElseIf Z = 1 Then '660
                Return Random2(W, VV)
            Else
                Q = 1
                Return SelectRandomMethod(W, VV, 2, 4)
            End If
        ElseIf S <> V Then '560
            If W(R, S + 1) <> 0 Then '590
                Return SelectRandomMethod(W, VV, 2, 3)
            Else
                Return SelectRandomMethod(W, VV, 2, 3, 4)
            End If
        ElseIf Z = 1 Then '590
            Return SelectRandomMethod(W, VV, 2, 3)
        Else
            Q = 1
            Return SelectRandomMethod(W, VV, 2, 3, 4)
        End If
    End Function

    Public Function SelectRandomPathAlt(W(,) As Integer, VV(,) As Integer) As Boolean '390
        If R = H OrElse W(R + 1, S) <> 0 Then
            If S <> V Then
                If W(R, S + 1) <> 0 Then
                    Return Random1(W, VV)
                Else
                    Return SelectRandomMethod(W, VV, 1, 4)
                End If
            ElseIf Z = 1 Then
                Return Random1(W, VV)
            Else
                Q = 1
                Return SelectRandomMethod(W, VV, 1, 4)
            End If
        ElseIf S <> V Then
            If w(R, S + 1) <> 0 Then
                Return SelectRandomMethod(W, VV, 1, 3)
            Else
                Return SelectRandomMethod(W, VV, 1, 3, 4)
            End If
        ElseIf Z = 1 Then
            Return SelectRandomMethod(W, VV, 1, 3)
        Else
            Q = 1
            Return SelectRandomMethod(W, VV, 1, 3, 4)
        End If
    End Function

    Public Function SelectRandomPathAlt2(W(,) As Integer, VV(,) As Integer) As Boolean '330
        If R = H OrElse W(R + 1, S) <> 0 Then
            If S <> V Then
                If W(R, S + 1) <> 0 Then
                    Return SelectRandomMethod(W, VV, 1, 2)
                Else
                    Return SelectRandomMethod(W, VV, 1, 2, 4)
                End If
            ElseIf Z = 1 Then
                Return SelectRandomMethod(W, VV, 1, 2)
            Else
                Q = 1
                Return SelectRandomMethod(W, VV, 1, 2, 4)
            End If
        Else
            Return SelectRandomMethod(W, VV, 1, 2, 3)
        End If
    End Function

    Public Sub PrintFinalResults(VV(,) As Integer)  'starts at 1010
        If Z <> 1 Then '1015
            Dim X As Integer = rnd.Next(1, H + 1)
            If VV(X, V) = 0 Then '1014
                VV(X, V) = 1
            Else
                VV(X, V) = 3
            End If
        End If

        For j As Integer = 1 To V
            Console.Write("I")
            For i As Integer = 1 To H
                If VV(i, j) < 2 Then '1030
                    Console.Write("  I")
                Else
                    Console.Write("   ")
                End If
            Next
            Console.WriteLine()

            For i As Integer = 1 To H
                If VV(i, j) = 0 OrElse VV(i, j) = 2 Then
                    Console.Write(":--")
                Else
                    Console.Write(":  ")
                End If
            Next

            Console.WriteLine(".")
        Next
    End Sub

    Public Function Random1(W(,) As Integer, VV(,) As Integer) As Boolean '790
        W(R - 1, S) = C
        C = C + 1
        VV(R - 1, S) = 2
        R = R - 1
        If C = H * V + 1 Then Return False 'Goto 1010 (Finish the program)
        Q = 0
        Return True 'Goto 260
    End Function

    Public Function Random2(W(,) As Integer, VV(,) As Integer) As Boolean '820
        W(R, S - 1) = C
        C = C + 1
        VV(R, S - 1) = 1
        S = S - 1
        If C = H * V + 1 Then Return False
        Q = 0
        Return True
    End Function

    Public Function Random3(W(,) As Integer, VV(,) As Integer) As Boolean '860
        W(R + 1, S) = C
        C = C + 1
        If VV(R, S) = 0 Then VV(R, S) = 2 Else VV(R, S) = 3
        R = R + 1
        If C = H * V + 1 Then Return False 'Goto 1010 (Finish the program)
        Return SelectRandomPath(W, VV)
    End Function

    Public Function Random4(W(,) As Integer, VV(,) As Integer) As Boolean '910
        If Q = 1 Then
            Z = 1
            If VV(R, S) = 0 Then
                VV(R, S) = 1
                Q = 0
                R = 1
                S = 1

                If W(R, S) = 0 Then LoopPrep(W)
                Return True
            Else
                VV(R, S) = 3
                Q = 0
                LoopPrep(W)
                Return True
            End If
        End If
        W(R, S + 1) = C
        C += 1
        If VV(R, S) = 0 Then VV(R, S) = 1 Else VV(R, S) = 3
        S += 1
        If C = H * V + 1 Then Return False
        Return True
    End Function
End Module
