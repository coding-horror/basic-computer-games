print " "*33 + "Lunar"
print " "*15 + "Creative Computing Morristown, New Jersey"
print; print; print
print "This is a computer simulation of an Apollo lunar"
print "landing capsule."; print; print
print "The on-board computer has failed (it was made by"
print "Xerox) so you have to land the capsule manually."

printCols = function(fields, delimiter=null)
	if delimiter == null then delimiter = text.delimiter
	line = ""
	for s in fields
		line += (s + " "*12)[:12]
	end for
	print line, delimiter
end function

doOneGame = function
	print; print "Set burn rate of retro rockets to any value between"
	print "0 (free fall) and 200 (maximum burn) pounds per second."
	print "Set new burn rate every 10 seconds."; print
	print "Capsule weight 32,500 lbs; fuel weight 16,500 lbs."
	print; print; print; print "Good luck"
	l = 0

	print
	printCols ["sec","mi + ft","mph","lb fuel","burn rate"]
	print
	a=120; v = 1; m=33000; n=16500; g=1e-03; z = 1.8

	formulaSet1 = function		// (subroutine 330)
		outer.l += s
		outer.t -= s
		outer.m -= s * k
		outer.a = i
		outer.v = j
	end function
	
	formulaSet2 = function		// (subroutine 420)
		outer.q = s * k / m
		outer.j = v + g*s + z*(-q-q*q/2-q^3/3-q^4/4-q^5/5)
		outer.i = a - g*s*s/2 - v*s+z*s*(q/2+q^2/6+q^3/12+q^4/20+q^5/30)
	end function

	formulaSet3 = function		// (loop 340-360)
		while s >= 5e-3
			outer.d = v + sqrt(v * v + 2 * a * (g - z * k / m))
			outer.s = 2 * a / d
			formulaSet2
			formulaSet1
		end while
	end function

	while true
		printCols [l, floor(a) + " " + floor(5280*(a-floor(a))), 3600*v, m-n], ""
		k = input.val
		t=10
		shouldExit = false
		
		while true
			if m-n < 1e-03 then break
			if t < 1e-03 then break
			s = t; if m < n+s*k then s = (m-n)/k
			formulaSet2
			if i <= 0 then
				formulaSet3
				shouldExit = true
				break
			end if
			if v > 0 and j < 0 then
				while v > 0 and j <= 0
					w = (1 - m*g/(z*k))/2
					s = m*v / (z*k*(w + sqrt(w*w + v/z))) + 0.05
					formulaSet2
					if i <= 0 then
						formulaSet3
						shouldExit = true
						break
					end if
					formulaSet1
				end while
				if shouldExit then break
				continue
			end if
			formulaSet1
		end while
		if shouldExit then break
		if m-n < 1e-03 then
			print "Fuel out at " + round(l) + " seconds"
			s = (-v+sqrt(v*v+2*a*g))/g
			v = v+g*s; l = l+s
			break
		end if
	end while
		
	w = 3600*v
	print "On moon at " + l + " seconds - impact velocity " + round(w,1) + " mph"
	if w <= 1.2 then
		print "Perfect landing!"
	else if w <= 10 then
		print "Good landing (could be better)"
	else if w <= 60 then
		print "Craft damage... you're stranded here until a rescue"
		print "party arrives. Hope you have enough oxygen!"
	else
		print "Sorry there were no survivors. You blew it!"
		print "In fact, you blasted a new lunar crater " + round(w*.227,1) + " feet deep!"
	end if
end function

while true
	doOneGame
	print; print; print; print "Try again??"
end while
