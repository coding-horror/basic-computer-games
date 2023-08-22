os = ["Dartmouth", ""]
sa = [0, 1]
ls = ["", "Kick","receive"," yard ","run back for ","ball on ",
         "yard line"," simple run"," tricky run"," short pass",
         " long pass","punt"," quick kick "," place kick"," loss ",
         " no gain","gain "," touchdown "," touchback ","safety***",
         "junk"]
p = 0
x = 0
x1 = 0

fnf = function(x); return 1 - 2 * p; end function
fng = function(z); return p * (x1 - x) + (1 - p) * (x - x1); end function

show_score = function
   print
   print "SCORE:  " + sa[0] + " TO " + sa[1]
   print
   print
end function

show_position = function
   if x <= 50 then
       print ls[5] + os[0] + " " + x + " " + ls[6]
   else
       print ls[5] + os[1] + " " + (100 - x) + " " + ls[6]
   end if
end function

offensive_td = function
   print ls[17] + "***"
   if rnd <= 0.8 then
       sa[p] += 7
       print "Kick is good."
   else
       print "Kick is off to the side"
       sa[p] += 6
   end if
   show_score
   print os[p] + " kicks off"
   globals.p = 1 - p
end function

// Main program
main = function
   print " "*33 + "FTBALL"
   print " "*15 + "Creative Computing  Morristown, New Jersey"
   print
   print
   print "This is Dartmouth championship football."
   print
   print "You will quarterback Dartmouth. Call plays as follows:"
   print "1= simple run; 2= tricky run; 3= short pass;"
   print "4= long pass; 5= punt; 6= quick kick; 7= place kick."
   print
   os[1] = input("Choose your opponent: ").upper
   os[0] = "DARMOUTH"
   print
   sa[0] = 0
   sa[1] = 0   
   globals.p = floor(rnd * 2)
   print os[p] + " won the toss"
   if p != 0 then
       print os[1] + " Elects to receive."
       print
   else
       print "Do you elect to kick or receive? "
       while true
           str = input.lower
           if str and (str[0] == "k" or str[0] == "r") then break
           print "Incorrect answer.  Please type 'kick' or 'receive'"
       end while
       if str[0] == "k" then e = 1 else e = 2
       if e == 1 then globals.p = 1
   end if
   globals.t = 0
   start = 1
   while true
       if start <= 1 then
           x = 40 + (1 - p) * 20
       end if
       if start <= 2 then
           y = floor(200 * ((rnd - 0.5))^3 + 55)
           print " " + y + " " + ls[3] + " kickoff"
           x = x - fnf(1) * y
           if abs(x - 50) >= 50 then
               print "Touchback for " + os[p] + "."
               x = 20 + p * 60
               start = 4
           else
               start = 3
           end if
       end if
       if start <= 3 then
           y = floor(50 * (rnd)^2) + (1 - p) * floor(50 * (rnd)^4)
           x = x + fnf(1) * y
           if abs(x - 50) < 50 then
               print " " + y + " " + ls[3] + " runback"
           else
               print ls[4]
               offensive_td
               start = 1
               continue
           end if
       end if
       if start <= 4 then
           // First down
           show_position
       end if
       if start <= 5 then
           x1 = x
           d = 1
           print
           print "First down " + os[p] + "***"
           print
           print
       end if
       // New play
       globals.t += 1
       if t == 30 then
           if rnd <= 1.3 then
               print "Game delayed.  Dog on field."
               print
           end if
       end if
       if t >= 50 and rnd <= 0.2 then break
       if p != 1 then
           // Opponent's play
           if d <= 1 then
               if rnd > 1/3 then z = 1 else z = 3
           else if d != 4 then
               if 10 + x - x1 < 5 or x < 5 then
                   if rnd > 1/3 then z = 1 else z = 3
               else if x <= 10 then
                   a = floor(2 * rnd)
                   z = 2 + a
               else if x <= x1 or d < 3 or x < 45 then
                   a = floor(2 * rnd)
                   z = 2 + a * 2
               else
                   if (rnd > 1 / 4) then
                       z = 4
                   else
                       z = 6
                   end if
               end if
           else
               if x <= 30 then
                   z = 5
               else if 10 + x - x1 < 3 or x < 3 then
                   if rnd > 1/3 then z = 1 else z = 3
               else
                   z = 7
               end if
           end if
       else
			while true
				z = input("Next play? ").val
                if 1 <= z <= 7 then break
				print "Illegal play number, retype"
            end while
       end if
       f = 0
       print ls[z + 6] + ".  "
       r = rnd * (0.98 + fnf(1) * 0.02)
       r1 = rnd
       if z == 1 or z == 2 then		// Simple Run or Tricky Run
       		   done = false
               if z == 1 then
                   y = floor(24 * (r - 0.5)^3 + 3)
                   if rnd >= 0.05 then
                       routine = 1                       
                       done = true
                   end if
               else
                   y = floor(20 * r - 5)
                   if rnd > 0.1 then
                   		routine = 1
                   		done = true
                   end if
               end if
               if not done then
				   f = -1
				   x3 = x
				   x = x + fnf(1) * y
				   if abs(x - 50) < 50 then
					   print "***  Fumble after "
					   routine = 2
				   else
					   print "***  Fumble."
					   routine = 4
				   end if
               end if
     	   else if z == 3 or z == 4 then	// Short Pass or Long Pass
               if z == 3 then
                   y = floor(60 * (r1 - 0.5)^3 + 10)
               else
                   y = floor(160 * ((r1 - 0.5))^3 + 30)
               end if
               if z == 3 and r < 0.05 or z == 4 and r < 0.1 then
                   if d != 4 then
                       print "Intercepted."
                       f = -1
                       x = x + fnf(1) * y
                       if abs(x - 50) >= 50 then
                           routine = 4
                       else
	                       routine = 3
                       end if
                   else
                       y = 0
                       if rnd < 0.3 then
                           print "Batted down.  ", ""
                       else
                           print "Incomplete.  ", ""
                       end if
                       routine = 1
                   end if
               else if z == 4 and r < 0.3 then
                   print "Passer tackled.  ", ""
                   y = -floor(15 * r1 + 3)
                   routine = 1
               else if z == 3 and r < 0.15 then
                   print "Passer taclked.  ", ""
                   y = -floor(10 * r1)
                   routine = 1
               else if z == 3 and r < 0.55 or z == 4 and r < 0.75 then
                   y = 0
                   if rnd < 0.3 then
                       print "Batted down.  ", ""
                   else
                       print "Incomplete.  ", ""
                   end if
                   routine = 1
               else
                   print "Complete.  ", ""
                   routine = 1
               end if
     	   else if z == 5 or z == 6 then		// Punt or Quick Kick
               y = floor(100 * ((r - 0.5))^3 + 35)
               if (d != 4) then y = floor(y * 1.3)
               print " " + y + " " + ls[3] + " punt"
               if abs(x + y * fnf(1) - 50) < 50 and d >= 4 then
                   y1 = floor((r1)^2 * 20)
                   print " " + y1 + " " + ls[3] + " Run back"
                   y = y - y1
               end if
               f = -1
               x = x + fnf(1) * y
               if abs(x - 50) >= 50 then routine = 4 else routine = 3
     	   else if z == 7 then 		// Place kick
               y = floor(100 * ((r - 0.5))^3 + 35)
               if r1 <= 0.15 then
                   print "Kick is blocked  ***"
                   x = x - 5 * fnf(1)
                   globals.p = 1 - p
                   start = 4
                   continue
               end if
               x = x + fnf(1) * y
               if abs(x - 50) >= 60 then
                   if r1 <= 0.5 then
                       print "Kick is off to the side."
                       print ls[18]
                       globals.p = 1 - p
                       x = 20 + p * 60
                       start = 4
                       continue
                   else
                       print "Field goal ***"
                       sa[p] = sa[p] + 3
                       show_score
                       print os[p] + " kicks off"
                       globals.p = 1 - p
                       start = 1
                       continue
                   end if
               else
                   print "Kick is short."
                   if abs(x - 50) >= 50 then
                       // Touchback
                       print ls[18]
                       globals.p = 1 - p
                       x = 20 + p * 60
                       start = 4
                       continue
                   end if
                   globals.p = 1 - p
                   start = 3
                   continue
               end if            
	   end if
       // Gain or loss
       if routine <= 1 then
           x3 = x
           x = x + fnf(1) * y
           if abs(x - 50) >= 50 then
               routine = 4
           end if
       end if
       if routine <= 2 then
           if y != 0 then
               print " " + abs(y) + " " + ls[3]
               if (y < 0) then
                   yt = -1
               else if y > 0 then
                   yt = 1
               else
                   yt = 0
               end if
               print ls[15 + yt]
               if abs(x3 - 50) <= 40 and rnd < 0.1 then
                   // Penalty
                   p3 = floor(2 * rnd)
                   print os[p3] + " offsides -- penalty of 5 yards."
                   print
                   print
                   if p3 != 0 then
                       print "Do you accept the penalty?"
                       while true
                           str = input.lower
                           if str and (str[0] == "y" or str[0] == "n") then break
                           print "Yype 'yes' or 'no'"
                       end while
                       if str[0] == "y" then
                           f = 0
                           d = d - 1
                           if (p != p3) then
                               x = x3 + fnf(1) * 5
                           else
                               x = x3 - fnf(1) * 5
                           end if
                       end if
                   else
                       // opponent's strategy on penalty
                       if ((p != 1 and (y <= 0 or f < 0 or fng(1) < 3 * d - 2)) or
                           (p == 1 and ((y > 5 and f >= 0) or d < 4 or fng(1) >= 10))) then
                           print "penalty refused."
                       else
                           print "penalty accepted."
                           f = 0
                           d = d - 1
                           if (p != p3) then
                               x = x3 + fnf(1) * 5
                           else
                               x = x3 - fnf(1) * 5
                           end if
                       end if
                   end if
                   routine = 3
               end if
           end if
       end if
       if routine <= 3 then
           show_position
           if f != 0 then
               globals.p = 1 - p
               start = 5
               continue
           else if fng(1) >= 10 then
               start = 5
               continue
           else if d == 4 then
               globals.p = 1 - p
               start = 5
               continue
           else
               d += 1
               print "DOWN: " + d + "     "
               if (x1 - 50) * fnf(1) >= 40 then
                   print "Goal to go"
               else
                   print "Yards to go: " + (10 - fng(1))
               end if
               print
               print
               start = 6
               continue
           end if
       end if
       if routine <= 4 then
           // Ball in end-zone
           e = (x >= 100)
           case = 1 + e - f * 2 + p * 4
           if case == 1 or case == 5 then
                   // Safety
                   sa[1 - p] = sa[1 - p] + 2
                   print ls[19]
                   show_score
                   print os[p] + " kicks off from its 20 yard line."
                   x = 20 + p * 60
                   globals.p = 1 - p
                   start = 2
                   continue
           end if
           if case == 3 or case == 6 then
                   // defensive td
                   print ls[17] + "for " + os[1 - p] + "***"
                   globals.p = 1 - p
           end if
           if case == 3 or case == 6 or case == 2 or case == 8 then
                   // offensive td
                   print ls[17] + "***"
                   if rnd <= 0.8 then
                       sa[p] = sa[p] + 7
                       print "kick is good."
                   else
                       print "kick is off to the side"
                       sa[p] = sa[p] + 6
                   end if
                   show_score
                   print os[p] + " kicks off"
                   globals.p = 1 - p
                   start = 1
                   continue
            end if
            if case == 4 or case == 7 then
                   // Touchback
                   print ls[18]
                   globals.p = 1 - p
                   x = 20 + p * 60
                   start = 4
                   continue
           end if
       end if
   end while
   print "END OF GAME  ***"
   print "FINAL SCORE:  " + os[0] + ": " + sa[0] + "  " + os[1] + ": " + sa[1]
end function

main
