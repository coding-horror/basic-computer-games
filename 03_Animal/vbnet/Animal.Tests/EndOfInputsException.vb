''' <summary>
''' <para>Indicates that there are no more inputs in the MockConsole.</para>
''' We need this while testing, because otherwise the game loop will continue forever, waiting for a nonexistent input.
''' </summary>
Public Class EndOfInputsException
    Inherits Exception
End Class
