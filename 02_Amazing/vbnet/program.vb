Imports System

Module Program

    Enum Directions
        SolveAndReset = 0
        Left = 1
        Up = 2
        Right = 3
        Down = 4
    End Enum

    'Program State
    Dim Width As Integer = 0, Height As Integer = 0, Q As Integer = 0, CellsVisited As Integer = 2, curCol As Integer, curRow As Integer = 1
    Dim SolutionCompleted As Boolean = False
    Dim CellVisitHistory(,) As Integer
    Dim CellState(,) As Integer

    Dim rnd As New Random()

    Public ReadOnly Property BlockedLeft As Boolean
        Get
            Return curCol - 1 = 0 OrElse CellVisitHistory(curCol - 1, curRow) <> 0
        End Get
    End Property
    Public ReadOnly Property BlockedAbove As Boolean
        Get
            Return curRow - 1 = 0 OrElse CellVisitHistory(curCol, curRow - 1) <> 0
        End Get
    End Property
    Public ReadOnly Property BlockedRight As Boolean
        Get
            Return curCol = Width OrElse CellVisitHistory(curCol + 1, curRow) <> 0
        End Get
    End Property
    'Note: "BlockedBelow" does NOT include checking if we have a solution!
    Public ReadOnly Property BlockedBelow As Boolean
        Get
            Return curRow = Height OrElse CellVisitHistory(curCol, curRow + 1) <> 0
        End Get
    End Property
    Public ReadOnly Property OnBottomRow As Boolean
        Get
            Return curRow.Equals(Height)
        End Get
    End Property

    Sub Main(args As String())
        Const header As String =
"             AMAZING PROGRAM
 CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



"
        Console.WriteLine(header)

        While Width <= 1 OrElse Height <= 1
            Console.Write("WHAT ARE YOUR WIDTH AND LENGTH? ")

            'We no longer have the old convenient INPUT command, so need to parse out the inputs
            Dim parts = Console.ReadLine().Split(","c).Select(Function(s) Convert.ToInt32(s.Trim())).ToList()
            Width = parts(0)
            Height = parts(1)

            If Width <= 1 OrElse Height <= 1 Then Console.WriteLine($"MEANINGLESS DIMENSIONS.  TRY AGAIN.{vbCrLf}")
        End While

        ReDim CellVisitHistory(Width, Height), CellState(Width, Height)

        Console.WriteLine("

")

        curCol = rnd.Next(1, Width + 1) 'Starting X position
        CellVisitHistory(curCol, 1) = 1
        Dim startXPos As Integer = curCol 'we need to know this at the end to print opening line

        Dim keepGoing As Boolean = True
        While keepGoing
            If BlockedLeft Then
                keepGoing = ChoosePath_BlockedToTheLeft()
            ElseIf BlockedAbove Then
                keepGoing = ChoosePath_BlockedAbove()
            ElseIf BlockedRight Then
                keepGoing = ChoosePath_BlockedToTheRight()
            Else
                keepGoing = SelectRandomDirection(Directions.Left, Directions.Up, Directions.Right) 'Go anywhere but down
            End If
        End While

        PrintFinalResults(startXPos)
    End Sub

    Public Sub ResetCurrentPosition()
        Do
            If curCol <> Width Then 'not at the right edge
                curCol += 1
            ElseIf curRow <> Height Then 'not at the bottom
                curCol = 1
                curRow += 1
            Else
                curCol = 1
                curRow = 1
            End If
        Loop While CellVisitHistory(curCol, curRow) = 0
    End Sub

    Dim methods() As Func(Of Boolean) = {AddressOf MarkSolvedAndResetPosition, AddressOf GoLeft, AddressOf GoUp, AddressOf GoRight, AddressOf GoDown}
    Public Function SelectRandomDirection(ParamArray possibles() As Directions) As Boolean
        Dim x As Integer = rnd.Next(0, possibles.Length)
        Return methods(possibles(x))()
    End Function

    Public Function ChoosePath_BlockedToTheLeft() As Boolean
        If BlockedAbove Then
            If BlockedRight Then
                If curRow <> Height Then
                    If CellVisitHistory(curCol, curRow + 1) <> 0 Then ' Can't go down, but not at the edge...blocked. Reset and try again
                        ResetCurrentPosition()
                        Return True
                    Else
                        Return GoDown()
                    End If
                ElseIf SolutionCompleted Then 'Can't go Down (there's already another solution)
                    ResetCurrentPosition()
                    Return True
                Else 'Can't go LEFT, UP, RIGHT, or DOWN, but we're on the bottom and there's no solution yet
                    Return MarkSolvedAndResetPosition()
                End If
            ElseIf BlockedBelow Then
                Return GoRight()
            ElseIf Not OnBottomRow Then
                Return SelectRandomDirection(Directions.Right, Directions.Down)
            ElseIf SolutionCompleted Then 'Can only go right, and we're at the bottom
                Return GoRight()
            Else 'Can only go right, we're at the bottom, and there's not a solution yet
                Return SelectRandomDirection(Directions.Right, Directions.SolveAndReset)
            End If
            '== Definitely can go Up ==
        ElseIf BlockedRight Then
            If BlockedBelow Then
                Return GoUp()
            ElseIf Not OnBottomRow Then
                Return SelectRandomDirection(Directions.Up, Directions.Down)
            ElseIf SolutionCompleted Then 'We're on the bottom row, can only go up
                Return GoUp()
            Else 'We're on the bottom row, can only go up, but there's no solution
                Return SelectRandomDirection(Directions.Up, Directions.SolveAndReset)
            End If
            '== Definitely can go Up and Right ==
        ElseIf BlockedBelow Then
            Return SelectRandomDirection(Directions.Up, Directions.Right)
        ElseIf Not OnBottomRow Then
            Return SelectRandomDirection(Directions.Up, Directions.Right, Directions.Down)
        ElseIf SolutionCompleted Then 'at the bottom, but already have a solution
            Return SelectRandomDirection(Directions.Up, Directions.Right)
        Else
            Return SelectRandomDirection(Directions.Up, Directions.Right, Directions.SolveAndReset)
        End If
    End Function

    Public Function ChoosePath_BlockedAbove() As Boolean
        'No need to check the left side, only called from the "keepGoing" loop where LEFT is already cleared
        If BlockedRight Then
            If BlockedBelow Then
                Return GoLeft()
            ElseIf Not OnBottomRow Then
                Return SelectRandomDirection(Directions.Left, Directions.Down)
            ElseIf SolutionCompleted Then 'Can't go down because there's already a solution
                Return GoLeft()
            Else 'At the bottom, no solution yet...
                Return SelectRandomDirection(Directions.Left, Directions.SolveAndReset)
            End If
        ElseIf BlockedBelow Then
            Return SelectRandomDirection(Directions.Left, Directions.Right)
        ElseIf Not OnBottomRow Then
            Return SelectRandomDirection(Directions.Left, Directions.Right, Directions.Down)
        ElseIf SolutionCompleted Then
            Return SelectRandomDirection(Directions.Left, Directions.Right)
        Else
            Return SelectRandomDirection(Directions.Left, Directions.Right, Directions.SolveAndReset)
        End If
    End Function

    Public Function ChoosePath_BlockedToTheRight() As Boolean
        'No need to check Left or Up, only called from the "keepGoing" loop where LEFT and UP are already cleared
        If BlockedRight Then 'Can't go Right -- why? we knew this when calling the function
            If BlockedBelow Then
                Return SelectRandomDirection(Directions.Left, Directions.Up)
            ElseIf Not OnBottomRow Then
                Return SelectRandomDirection(Directions.Left, Directions.Up, Directions.Down)
            ElseIf SolutionCompleted Then
                Return SelectRandomDirection(Directions.Left, Directions.Up)
            Else
                Return SelectRandomDirection(Directions.Left, Directions.Up, Directions.SolveAndReset)
            End If
        Else 'Should never get here
            Return SelectRandomDirection(Directions.Left, Directions.Up, Directions.Right) 'Go Left, Up, or Right (but path is blocked?)
        End If
    End Function

    Public Sub PrintFinalResults(startPos As Integer)
        For i As Integer = 0 To Width - 1
            If i = startPos Then Console.Write(".  ") Else Console.Write(".--")
        Next
        Console.WriteLine(".")

        If Not SolutionCompleted Then 'Pick a random exit
            Dim X As Integer = rnd.Next(1, Width + 1)
            If CellState(X, Height) = 0 Then
                CellState(X, Height) = 1
            Else
                CellState(X, Height) = 3
            End If
        End If

        For j As Integer = 1 To Height
            Console.Write("I")
            For i As Integer = 1 To Width
                If CellState(i, j) < 2 Then
                    Console.Write("  I")
                Else
                    Console.Write("   ")
                End If
            Next
            Console.WriteLine()

            For i As Integer = 1 To Width
                If CellState(i, j) = 0 OrElse CellState(i, j) = 2 Then
                    Console.Write(":--")
                Else
                    Console.Write(":  ")
                End If
            Next
            Console.WriteLine(".")
        Next
    End Sub

    Public Function GoLeft() As Boolean
        curCol -= 1
        CellVisitHistory(curCol, curRow) = CellsVisited
        CellsVisited += 1
        CellState(curCol, curRow) = 2
        If CellsVisited > Width * Height Then Return False
        Q = 0
        Return True
    End Function

    Public Function GoUp() As Boolean
        curRow -= 1
        CellVisitHistory(curCol, curRow) = CellsVisited
        CellsVisited += 1
        CellState(curCol, curRow) = 1
        If CellsVisited > Width * Height Then Return False
        Q = 0
        Return True
    End Function

    Public Function GoRight() As Boolean
        CellVisitHistory(curCol + 1, curRow) = CellsVisited
        CellsVisited += 1
        If CellState(curCol, curRow) = 0 Then CellState(curCol, curRow) = 2 Else CellState(curCol, curRow) = 3
        curCol += 1
        If CellsVisited > Width * Height Then Return False
        Return ChoosePath_BlockedToTheLeft()
    End Function

    Public Function GoDown() As Boolean
        If Q = 1 Then Return MarkSolvedAndResetPosition()

        CellVisitHistory(curCol, curRow + 1) = CellsVisited
        CellsVisited += 1
        If CellState(curCol, curRow) = 0 Then CellState(curCol, curRow) = 1 Else CellState(curCol, curRow) = 3
        curRow += 1
        If CellsVisited > Width * Height Then Return False
        Return True
    End Function

    Public Function MarkSolvedAndResetPosition() As Boolean
        ' AlWAYS returns true
        SolutionCompleted = True
        Q = 1
        If CellState(curCol, curRow) = 0 Then
            CellState(curCol, curRow) = 1
            curCol = 1
            curRow = 1
            If CellVisitHistory(curCol, curRow) = 0 Then ResetCurrentPosition()
        Else
            CellState(curCol, curRow) = 3
            ResetCurrentPosition()
        End If
        Return True
    End Function
End Module
