Imports System

''' <summary>
''' This is a modern adapation of Acey Ducey from BASIC Computer Games.
''' 
''' The structural changes primarily consist of replacing the many GOTOs with
''' Do/Loop constructs to force the continual execution of the program.
''' 
''' Some modern improvements were added, primarily the inclusion of a multiple 
''' subroutines and functions, which eliminates repeated logic and reduces 
''' then need for nested loops.  
''' 
''' The archaic RND function is greatly simplified with the .NET Framework's Random class.
''' 
''' Elementary comments are provided for non-programmers or novices.
''' </summary>
Module Program
    Sub Main()
        Call New AceyDucey().Play()
    End Sub
End Module
