import "listUtil"
print " "*28 + "Amazing Program"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print; print
while true
	inp = input("What and your width and length? ")
	inp = inp.replace(",", " ")
	fields = inp.split
	h = fields[0].val; v = fields[-1].val
	if h > 1 and v > 1 then break
	print "Meaningless dimensions.  Try again."
end while

// order: keeps track of the order in which each cell was
// visited as we built the maze.  0 means not explored yet.  Indexed in [column][row] order.
// (This is W in the original BASIC program.)
order = list.init2d(h,v, 0)

// walls: keeps track of the walls below and to the right of each cell:
//		0: walls below and to the right
//		1: wall to the right
//		2: wall below
//		3: neither wall
// (This is V in the original BASIC program.)
// Note that a wall to the right can be removed from a
// valid entry by adding 2; a wall below can be removed
// by adding 1.
walls = list.init2d(h,v, 0)
print
print
print
print

// pick an exit at the top of the maze,
// and print the maze top
x = floor(rnd * h)
for i in range(0, h-1)
	if i == x then print ".  ","" else print ".--",""
end for
print "."

// walk from our starting position (by the exit) around
// the maze, clearing a wall on each step
c = 1		// current step number
order[x][0] = c; c += 1
r = x; s = 0		// [r][s] is our current position in the maze
while true
	// collect the set of directions we can move in
	dirs = []
	if r > 0 and order[r-1][s] == 0 then dirs.push "left"
	if s > 0 and order[r][s-1] == 0 then dirs.push "up"
	if r+1 < h and order[r+1][s] == 0 then dirs.push "right"
	if s+1 < v and order[r][s+1] == 0 then dirs.push "down"
	if not dirs then
		//print "Uh-oh, I'm stuck at " + r + "," + s
		// couldn't find any directions for this cell; 
		// find the next already-explored cell
		while true
			r += 1
			if r >= h then
				r = 0
				s += 1			
				if s >= v then s = 0
			end if
			if order[r][s] != 0 then break
		end while
		continue
	end if
	
	// pick a random available direction; move there,
	// clearing the wall in between and updating order
	d = dirs.any
	if d == "left" then
		walls[r-1][s] += 2
		r = r-1
	else if d == "up" then
		walls[r][s-1] += 1
		s = s-1
	else if d == "right" then
		walls[r][s] += 2
		r = r+1
	else if d == "down" then
		walls[r][s] += 1
		s = s+1
	end if
	
	//print "At step " + c + ", at " + r + "," + s
	order[r][s] = c
	c += 1
	if c > h*v then break
end while

// pick an exit at the bottom of the maze
x = floor(rnd * h)
walls[x][v-1] += 1

// print the (rest of the) maze
for j in range(0, v-1)
	print "I", ""
	for i in range(0, h-1)
	if walls[i][j] < 2 then print "  I", "" else print "   ", ""
	end for
	print
	for i in range(0, h-1)
		if walls[i][j] % 2 == 0 then print ":--", "" else print ":  ", ""
	end for
	print "."
end for
