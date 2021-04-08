Imports System

Module Program
  Sub Main(args As String())
    Dim m As Integer
10: Console.WriteLine(Space(26) & "ACEY DUCEY CARD GAME")
20: Console.WriteLine(Space(15) & "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
21: Console.WriteLine()
22: Console.WriteLine()
30: Console.WriteLine("ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER ")
40: Console.WriteLine("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP")
50: Console.WriteLine("YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING")
60: Console.WriteLine("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE")
70: Console.WriteLine("A VALUE BETWEEN THE FIRST TWO.")
80: Console.WriteLine("IF YOU DO NOT WANT TO BET, INPUT A 0")
100: Dim n = 100 ' This variable is never used!
110: Dim q = 100
120: Console.WriteLine($"YOU NOW HAVE {q} DOLLARS.")
130: Console.WriteLine()
140: GoTo 260
210: q += m
220: GoTo 120
240: q -= m
250: GoTo 120
260: Console.WriteLine("HERE ARE YOUR NEXT TWO CARDS: ")
270: Dim a = Int(14 * Rnd(1)) + 2
280: If a < 2 Then GoTo 270
290: If a > 14 Then GoTo 270
300: Dim b = Int(14 * Rnd(1)) + 2
310: If b < 2 Then GoTo 300
320: If b > 14 Then GoTo 300
330: If a >= b Then GoTo 270
350: If a < 11 Then GoTo 400
360: If a = 11 Then GoTo 420
370: If a = 12 Then GoTo 440
380: If a = 13 Then GoTo 460
390: If a = 14 Then GoTo 480
400: Console.WriteLine(a)
410: GoTo 500
420: Console.WriteLine("JACK")
430: GoTo 500
440: Console.WriteLine("QUEEN")
450: GoTo 500
460: Console.WriteLine("KING")
470: GoTo 500
480: Console.WriteLine("ACE")
500: If b < 11 Then GoTo 550
510: If b = 11 Then GoTo 570
520: If b = 12 Then GoTo 590
530: If b = 13 Then GoTo 610
540: If b = 14 Then GoTo 630
550: Console.WriteLine(b)
560: GoTo 650
570: Console.WriteLine("JACK")
580: GoTo 650
590: Console.WriteLine("QUEEN")
600: GoTo 650
610: Console.WriteLine("KING")
620: GoTo 650
630: Console.WriteLine("ACE")
640: Console.WriteLine()
650: Console.WriteLine()
660: Console.WriteLine("WHAT IS YOUR BET") : Dim bet = Console.ReadLine()
    If Not Integer.TryParse(bet, m) Then GoTo 660 'Old INPUT command could prompt for a number; ReadLine() always returns a String
    If m < 0 Then GoTo 660 'Original code allowed player to bet negative amounts; remove this line to allow hax
670: If m <> 0 Then GoTo 680
675: Console.WriteLine("CHICKEN!!")
676: Console.WriteLine()
677: GoTo 260
680: If m <= q Then GoTo 730
690: Console.WriteLine("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.")
700: Console.WriteLine($"YOU HAVE ONLY {q} DOLLARS TO BET.")
710: GoTo 650
730: Dim c = Int(14 * Rnd(1)) + 2
740: If c < 2 Then GoTo 730
750: If c > 14 Then GoTo 730
760: If c < 11 Then GoTo 810
770: If c = 11 Then GoTo 830
780: If c = 12 Then GoTo 850
790: If c = 13 Then GoTo 870
800: If c = 14 Then GoTo 890
810: Console.WriteLine(c)
820: GoTo 910
830: Console.WriteLine("JACK")
840: GoTo 910
850: Console.WriteLine("QUEEN")
860: GoTo 910
870: Console.WriteLine("KING")
880: GoTo 910
890: Console.WriteLine("ACE")
900: Console.WriteLine()
910: If c > a Then GoTo 930
920: GoTo 970
930: If c >= b Then GoTo 970
950: Console.WriteLine("YOU WIN!!!")
960: GoTo 210
970: Console.WriteLine("SORRY, YOU LOSE")
980: If m < q Then GoTo 240
990: Console.WriteLine()
1000: Console.WriteLine()
1010: Console.WriteLine("SORRY, FRIEND, BUT YOU BLEW YOUR WAD.")
1015: Console.WriteLine() : Console.WriteLine()
1020: Console.WriteLine("TRY AGAIN (YES OR NO)") : Dim aa$ = Console.ReadLine() 'needed to introduce a new variable name
1025: Console.WriteLine() : Console.WriteLine()
1030: If aa$.Trim().ToUpper() = "YES" Then GoTo 110
1040: Console.WriteLine("O.K., HOPE YOU HAD FUN!")
1050: End
  End Sub

End Module
