import random
            
# Python translation by Frank Palazzolo - 2/2021
    
print(' '*28+'AMAZING PROGRAM')
print(' '*15+'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY')
print()
print()
print()

while True:
    width, length = input('What are your width and length?').split(',')
    width = int(width)
    length = int(length)
    if width != 1 and length != 1:
        break
    print('Meaningless dimensions. Try again.')

# Build two 2D arrays
#
# used:
#   Initially set to zero, unprocessed cells
#   Filled in with consecutive non-zero numbers as cells are processed
#
# walls:
#   Initially set to zero, (all paths blocked)
#   Remains 0 if there is no exit down or right
#   Set to 1 if there is an exit down
#   Set to 2 if there is an exit right
#   Set to 3 if there are exits down and right

used = []
walls = []
for i in range(length):
    used.append([0]*width)
    walls.append([0]*width)

# Use direction variables with nice names
GO_LEFT,GO_UP,GO_RIGHT,GO_DOWN=[0,1,2,3]
# Give Exit directions nice names
EXIT_DOWN = 1
EXIT_RIGHT = 2

# Pick a random entrance, mark as used
enter_col=random.randint(0,width-1)
row,col=0,enter_col
count=1
used[row][col]=count
count=count+1

while count!=width*length+1:
    # remove possible directions that are blocked or
    # hit cells that we have already processed
    possible_dirs = [GO_LEFT,GO_UP,GO_RIGHT,GO_DOWN]
    if col==0 or used[row][col-1]!=0:
        possible_dirs.remove(GO_LEFT)
    if row==0 or used[row-1][col]!=0:
        possible_dirs.remove(GO_UP)
    if col==width-1 or used[row][col+1]!=0: 
        possible_dirs.remove(GO_RIGHT)
    if row==length-1 or used[row+1][col]!=0:
        possible_dirs.remove(GO_DOWN)   

    # If we can move in a direction, move and make opening
    if len(possible_dirs)!=0:
        direction=random.choice(possible_dirs) 
        if direction==GO_LEFT:
            col=col-1
            walls[row][col]=EXIT_RIGHT
        elif direction==GO_UP:
            row=row-1 
            walls[row][col]=EXIT_DOWN
        elif direction==GO_RIGHT:
            walls[row][col]=walls[row][col]+EXIT_RIGHT    
            col=col+1
        elif direction==GO_DOWN:
            walls[row][col]=walls[row][col]+EXIT_DOWN
            row=row+1
        used[row][col]=count
        count=count+1
    # otherwise, move to the next used cell, and try again
    else:
        while True:
            if col!=width-1:
                col=col+1
            elif row!=length-1:
                row,col=row+1,0
            else:
                row,col=0,0
            if used[row][col]!=0:
                break

# Add a random exit
col=random.randint(0,width-1)
row=length-1
walls[row][col]=walls[row][col]+1
    
# Print the maze
for col in range(width):
    if col==enter_col:
        print('.  ',end='')
    else:
        print('.--',end='')
print('.')
for row in range(length):
    print('I',end='')
    for col in range(width):
        if walls[row][col]<2:
            print('  I',end='')
        else:
            print('   ',end='')
    print()
    for col in range(width):
        if walls[row][col]==0 or walls[row][col]==2:
            print(':--',end='')
        else:
            print(':  ',end='')    
    print('.')

        
        