     # Annotated version of CHECKERS.BAS, modified to improve readability.
     #
     # I've made the following changes:
     #
     #  1. Added many comments and blank lines.
     #  2. Separated each statement into its own line.
     #  3. Indented loops, conditionals and subroutines.
     #  4. Turned *SOME* conditionals and loops into
     #     structured-BASIC-style if/endif and loop/endloop blocks.
     #  5. Switched to using '#' to delimit comments.
     #  6. Subroutines now begin with "Sub_Start"
     #  7. All non-string text has been converted to lower-case
     #  8. All line numbers that are not jump destinations have been removed.
     #
     # This has helped me make sense of the code.  I hope it will also help you.
     #

     # Print the banner
     print tab(32);"CHECKERS"
     print tab(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
     print
     print
     print
     print "THIS IS THE GAME OF CHECKERS.  THE COMPUTER IS X,"
     print "AND YOU ARE O.  THE COMPUTER WILL MOVE FIRST."
     print "SQUARES ARE REFERRED TO BY A COORDINATE SYSTEM."
     print "(0,0) IS THE LOWER LEFT CORNER"
     print "(0,7) IS THE UPPER LEFT CORNER"
     print "(7,0) IS THE LOWER RIGHT CORNER"
     print "(7,7) IS THE UPPER RIGHT CORNER"
     print "THE COMPUTER WILL TYPE '+TO' WHEN YOU HAVE ANOTHER"
     print "JUMP.  TYPE TWO NEGATIVE NUMBERS IF YOU CANNOT JUMP."
     print
     print
     print

     # Declare the "globals":

     # The current move: (rating, current x, current y, new x, new y)
     # 'rating' represents how good the move is; higher is better.
     dim r(4)
     r(0)=-99       # Start with minimum score

     # The board.  Pieces are represented by numeric values:
     #
     #      - 0     = empty square
     #      - -1,-2 = X (-1 for regular piece, -2 for king)
     #      - 1,2   = O (1 for regular piece, 2 for king)
     #
     # This program's player ("me") plays X.
     dim s(7,7)

     g=-1           # constant holding -1

     # Initialize the board.  Data is 2 length-wise strips repeated.
     data 1,0,1,0,0,0,-1,0,0,1,0,0,0,-1,0,-1,15
     for x=0 to 7
       for y=0 to 7
         read j
         if j=15 then 180
         s(x,y)=j
         goto 200
180      restore
         read s(x,y)
200  next y,x

230  # Start of game loop.  First, my turn.

     # For each square on the board, search for one of my pieces
     # and if it can make the best move so far, store that move in 'r'
     for x=0 to 7
       for y=0 to 7

         # Skip if this is empty or an opponent's piece
         if s(x,y) > -1 then 350

         # If this is one of my ordinary pieces, analyze possible
         # forward moves.
         if s(x,y) = -1 then
           for a=-1 to 1 step 2
             b=g
             gosub 650
           next a
         endif

         # If this is one of my kings, analyze possible forward
         # and backward moves.
         if s(x,y) = -2 then
           for a=-1 to 1 step 2
             for b=-1 to 1 step 2
               gosub 650
           next b,a
         endif

350  next y,x
     goto 1140  # Skip the subs


     # Analyze a move from (x,y) to (x+a, y+b) and schedule it if it's
     # the best candidate so far.
650  Sub_Start
       u=x+a
       v=y+b

       # Done if it's off the board
       if u<0 or u>7 or v<0 or v>7 then 870

       # Consider the destination if it's empty
       if s(u,v) = 0 then
         gosub 910
         goto 870
       endif

       # If it's got an opponent's piece, jump it instead
       if s(u,v) > 0

           # Restore u and v, then return if it's off the board
           u=u+a
           v=v+b
           if u<0 or v<0 or u>7 or v>7 then 870

           # Otherwise, consider u,v
           if s(u,v)=0 then gosub 910
       endif           
870  return

     # Evaluate jumping (x,y) to (u,v).
     #
     # Computes a score for the proposed move and if it's higher
     # than the best-so-far move, uses that instead by storing it
     # and its score in array 'r'.
910  Sub_Start

       # q is the score; it starts at 0

       # +2 if it promotes this piece
       if v=0 and s(x,y)=-1 then q=q+2

       # +5 if it takes an opponent's piece
       if abs(y-v)=2 then q=q+5

       # -2 if the piece is moving away from the top boundary
       if y=7 then q=q-2

       # +1 for putting the piece against a vertical boundary
       if u=0 or u=7 then q=q+1

       for c=-1 to 1 step 2
         if u+c < 0 or u+c > 7 or v+g < 0 then 1080

         # +1 for each adjacent friendly piece
         if s(u+c, v+g) < 0 then
           q=q+1
           goto 1080
         endif

         # Prevent out-of-bounds testing
         if u-c < 0 or u-c > 7 or v-g > 7 then 1080

         # -2 for each opponent piece that can now take this piece here
         if s(u+c,v+g) > 0 and(s(u-c,v-g)=0 or(u-c=x and v-g=y))then q=q-2
1080   next c

       # Use this move if it's better than the previous best
       if q>r(0) then
         r(0)=q
         r(1)=x
         r(2)=y
         r(3)=u
         r(4)=v
       endif

       q=0  # reset the score
     return

1140 if r(0)=-99 then 1880  # Game is lost if no move could be found.

     # Print the computer's move.  (Note: chr$(30) is an ASCII RS
     # (record separator) code; probably no longer relevant.)
     print chr$(30)"FROM"r(1);r(2)"TO"r(3);r(4);
     r(0)=-99

     # Make the computer's move.  If the piece finds its way to the
     # end of the board, crown it.
1240 if r(4)=0 then
       s(r(3),r(4))=-2
       goto 1420
     endif
     s(r(3),r(4))=s(r(1),r(2))
     s(r(1),r(2))=0

     # If the piece has jumped 2 squares, it means the computer has
     # taken an opponents' piece.
     if abs(r(1)-r(3)) == 2 then
       s((r(1)+r(3))/2,(r(2)+r(4))/2)=0     # Delete the opponent's piece

       # See if we can jump again.  Evaluate all possible moves.
       x=r(3)
       y=r(4)
       for a=-2 to 2 step 4
         if s(x,y)=-1 then
           b=-2
           gosub 1370
         endif
         if s(x,y)=-2 then
           for b=-2 to 2 step 4
             gosub 1370
           next b
         endif
       next a

       # If we've found a move, go back and make that one as well
       if r(0) <> -99 then
         print "TO" r(3); r(4);
         r(0)=-99
         goto 1240
       endif

       goto 1420   # Skip the sub

       # If (u,v) is in the bounds, evaluate it as a move using
       # the sub at 910
1370   Sub_Start
         u=x+a
         v=y+b
         if u<0 or u>7 or v<0 or v>7 then 1400
         if s(u,v)=0 and s(x+a/2,y+b/2)>0 then gosub 910
1400   return

1420 endif

     # Now, print the board
     print
     print
     print
     for y=7 to 0 step-1
       for x=0 to 7
         i=5*x
         print tab(i);
         if s(x,y)=0 then print".";
         if s(x,y)=1 then print"O";
         if s(x,y)=-1 then print"X";
         if s(x,y)=-2 then print"X*";
         if s(x,y)=2 then print"O*";
       next x
       print" "
       print
     next y
     print

     # Check if either player is out of pieces.  If so, announce the
     # winner.
     for l=0 to 7
       for m=0 to 7
         if s(l,m)=1 or s(l,m)=2 then z=1
         if s(l,m)=-1 or s(l,m)=-2 then t=1
       next m
     next l
     if z<>1 then 1885
     if t<>1 then 1880

     # Prompt the player for their move.
     z=0
     t=0
1590 input "FROM";e,h
     x=e
     y=h
     if s(x,y)<=0 then 1590
1670 input "TO";a,b
     x=a
     y=b
     if s(x,y)=0 and abs(a-e)<=2 and abs(a-e)=abs(b-h)then 1700
     print chr$(7)chr$(11);     # bell, vertical tab; invalid move
     goto 1670

1700 i=46       # Not used; probably a bug
1750 loop
       # Make the move and stop unless it might be a jump.
       s(a,b) = s(e,h)
       s(e,h) = 0
       if abs(e-a) <> 2 then break

       # Remove the piece jumped over
       s((e+a)/2,(h+b)/2) = 0

       # Prompt for another move; -1 means player can't, so I've won.
       # Keep prompting until there's a valid move or the player gives
       # up.
1802   input "+TO";a1,b1
       if a1 < 0 then break
       if s(a1,b1) <> 0 or abs(a1-a) <>2  or abs(b1-b) <> 2 then 1802

       # Update the move variables to correspond to the next jump
       e=a
       h=b
       a=a1
       b=b1

       i=i+15   # Not used; probably a bug
     endloop

     # If the player has reached the end of the board, crown this piece
1810 if b=7 then s(a,b)=2

     # And play the next turn.
     goto 230

     # Endgame:
1880 print
     print "YOU WIN."
     end

1885 print
     print "I WIN."
     end
