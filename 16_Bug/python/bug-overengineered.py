"""
BUG (overengineered)

Overengineered version of bug game
Demonstrates function-based Model View Controller pattern

Ported by Peter Sharp
"""

from random import randint
from collections import namedtuple

PAGE_WIDTH = 64

def main(states, data):
    """
    Starts the game loop using given states and data
    
    Uses a modified version of the MVC (Model View Controller) pattern that uses functions instead of objects
    
    each state in the game has one of each of the following:
    View, displays data
    Control, converts raw command from user into something the model understands
    Model, updates game data based on action received from controller
    """
    
    while True:
        if 'exit' == data['state']: break
        view, control, model = states[data['state']]
        cmd = view(data)
        action = control(cmd)
        data = model(data, action)
        
Bodypart = namedtuple('Bodypart', ['name', 'count', 'depends'])

def print_start(_):
    """
    Prints start message
    """
    print_centered("BUG")
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()
    print("THE GAME BUG")
    print("I HOPE YOU ENJOY THIS GAME.")
    print()
    return input("DO YOU WANT INSTRUCTIONS? ")

def control_start(cmd):
    """
    Controls the start state
    """
    if cmd.lower() in ('y', 'yes'):
        action = 'instructions'
    else:
        action = 'game'
    return action
    
def print_instructions(data):
    """
    Prints game instructions 
    """
    print("THE OBJECT OF BUG IS TO FINISH YOUR BUG BEFORE I FINISH")
    print("MINE. EACH NUMBER STANDS FOR A PART OF THE BUG BODY.")
    print("I WILL ROLL THE DIE FOR YOU, TELL YOU WHAT I ROLLED FOR YOU")
    print("WHAT THE NUMBER STANDS FOR, AND IF YOU CAN GET THE PART.")
    print("IF YOU CAN GET THE PART I WILL GIVE IT TO YOU.")
    print("THE SAME WILL HAPPEN ON MY TURN.")
    print("IF THERE IS A CHANGE IN EITHER BUG I WILL GIVE YOU THE")
    print("OPTION OF SEEING THE PICTURES OF THE BUGS.")
    print("THE NUMBERS STAND FOR PARTS AS FOLLOWS:")
    
    print_table([
        ("NUMBER","PART","NUMBER OF PART NEEDED"),
        *[(i + 1, part.name, part.count) for i, part in enumerate(data['partTypes'])]
    ])
    print()
    print()
    return ''

def goto_game(_):
    """
    Returns game 
    """
    return 'game'

def update_state(data, action):
    """
    sets game state to given player value
    """
    return {**data, 'state': action}

def update_game(data, action):
    """
    Updates game data for player turns until one player successfully gets a body part 
    """
    # stores logs of what happened during a particular round
    logs = []
    
    if 'pictures' == action:
        data['state'] = 'pictures'
    else:
        partAdded = False
        while partAdded == False:
            for player, parts in data['players'].items():
                # rolls the dice for a part
                newPartIdx = randint(1,6) - 1
                
                # gets information about the picked part
                partType = data['partTypes'][newPartIdx]
                
                # gets the number of existing parts of that type the player has
                partCount = parts[newPartIdx]
                
                logs.append(('rolled', newPartIdx, player))
                
                # a new part can only be added if the player has the parts 
                # the new part depends on and doesn't have enough of the part already
                overMaxParts = partType.count < partCount + 1
                missingPartDep = partType.depends != None and parts[partType.depends] == 0
                
                if not overMaxParts and not missingPartDep:
                    # adds a new part 
                    partCount += 1
                    logs.append(('added', newPartIdx, player))
                    partAdded = True
                elif missingPartDep:
                    logs.append(('missingDep', newPartIdx, player, partType.depends))
                if overMaxParts:
                    logs.append(('overMax', newPartIdx, player, partCount))
                    
                data['players'][player][newPartIdx] = partCount
    data['logs'] = logs
    
    # checks if any players have finished their bug
    finished = get_finished(data)
    if len(finished) > 0:
        # and sets the state to 'won' if that's the case
        data['finished'] = finished
        data['state'] = 'won'
    return data

def get_finished(data):
    """
    Gets players who have finished their bugs
    """
    totalParts = sum([partType.count for partType in data['partTypes']])
    finished = []
    for player, parts in data['players'].items():
        if(sum(parts) == totalParts): finished.append(player)
    return finished

def print_game(data):
    """
    Displays the results of the game turn
    """
    for log in data['logs']:
        code, partIdx, player, *logdata = log
        partType = data['partTypes'][partIdx]
        
        if('rolled' == code):
            print()
            print(f"{player} ROLLED A {partIdx + 1}")
            print(f"{partIdx + 1}={partType.name}")
            
        elif('added' == code):
            if 'YOU' == player:
                if partType.name in ['FEELERS', 'LEGS', 'TAIL']:
                    print(f"I NOW GIVE YOU A {partType.name.replace('s', '')}.")
                else:
                    print(f"YOU NOW HAVE A {partType.name}.")
            elif 'I' == player:
                if partType.name in ['BODY', 'NECK', 'TAIL']:
                    print(f"I NOW HAVE A {partType.name}.")
                elif partType.name == 'FEELERS':
                    print("I GET A FEELER.")
                
            if partType.count > 2:
                print(f"{player} NOW HAVE {data['players'][player][partIdx]} {partType.name}")
                
        elif 'missingDep' == code:
            depIdx, = logdata
            dep = data['partTypes'][depIdx]
            print(f"YOU DO NOT HAVE A {dep.name}" if 'YOU' == player else f"I NEEDED A {dep.name}")
            
        elif 'overMax' == code:
            partCount, = logdata
            if(partCount > 1):
                num = 'TWO' if 2 == partCount else partCount
                maxMsg = f"HAVE {num} {partType.name}S ALREADY"
            else:
                maxMsg = f"ALREADY HAVE A {partType.name}"
            print(f"{player} {maxMsg}")

    return input("DO YOU WANT THE PICTURES? ") if len(data['logs']) else 'n'

def print_pictures(data):
    """
    Displays what the bugs look like for each player
    """
    typeIxs = { partType.name: idx for idx, partType in enumerate(data['partTypes']) }
    PIC_WIDTH = 22
    for player, parts in data['players'].items():
        print(f"*****{'YOUR' if 'YOU' == player else 'MY'} BUG*****")
        print()
        print()
        if(parts[typeIxs['BODY']] > 0):
            if(parts[typeIxs['FEELERS']] > 0):
                F = ' '.join(['F'] * parts[typeIxs['FEELERS']])
                for _ in range(4):
                    print(' ' * 9 + F)
            if(parts[typeIxs['HEAD']] > 0):
                print_centered("HHHHHHH", PIC_WIDTH)
                print_centered("H     H", PIC_WIDTH)
                print_centered("H O O H", PIC_WIDTH)
                print_centered("H     H", PIC_WIDTH)
                print_centered("H  V  H", PIC_WIDTH)
                print_centered("HHHHHHH", PIC_WIDTH)
            if(parts[typeIxs['NECK']] > 0):
                for _ in range(2):
                    print_centered("N N", PIC_WIDTH)
            print_centered("BBBBBBBBBBBB", PIC_WIDTH)
            for _ in range(2):
                print_centered("B          B", PIC_WIDTH)
       
            if(parts[typeIxs['TAIL']] > 0):
                print("TTTTTB          B")
            print_centered("BBBBBBBBBBBB", PIC_WIDTH)
            if(parts[typeIxs['LEGS']] > 0):
                L = 'L' * parts[typeIxs['LEGS']]
                for _ in range(2):
                    print(' '*5+L)
        print()

def control_game(cmd):
    """
    returns state based on command
    """
    if cmd.lower() in ('y', 'yes'):
        action = 'pictures'
    else:
        action = 'game'
    return action

def print_winner(data):
    """
    Displays the winning message
    """
    for player in data['finished']:
        print(f"{'YOUR' if 'YOU' == player else 'MY'} BUG IS FINISHED.")
    print("I HOPE YOU ENJOYED THE GAME, PLAY IT AGAIN SOON!!")
    
def exit_game(_):
    """
    Exists the game regardless of input
    """
    return 'exit'
    
def print_centered(msg, width=PAGE_WIDTH):
    """
    Prints given message centered to given width
    """
    spaces = " " * ((width - len(msg)) // 2)
    print(spaces + msg)
    
def print_table(rows):
    for row in rows:
        print(*row, sep="\t")

if __name__ == '__main__':
    
    # The main states in the game
    states = {
        # Initial state of the game
        'start': (print_start, control_start, update_state),
        
        # displays game instructions
        'instructions': (print_instructions, goto_game, update_state),
        
        # the main game state 
        'game': (print_game, control_game, update_game),
        
        # displays pictures before returning to game
        'pictures': (print_pictures, goto_game, update_state),
        
        # Displays the winning players and message
        'won': (print_winner, exit_game, update_state)
    }

    # body part types used by the game to work out whether a player's body part can be added
    partTypes = (
        Bodypart(name="BODY", count=1 , depends=None),
        Bodypart(name="NECK", count=1 , depends=0),
        Bodypart(name="HEAD", count=1 , depends=1),
        Bodypart(name="FEELERS", count=2 , depends=2),
        Bodypart(name="TAIL", count=1 , depends=0),
        Bodypart(name="LEGS", count=6 , depends=0)
    )

    # all the data used by the game
    data = {
        'state': 'start',
        'partNo': None,
        'players': {
            'YOU': [0] * len(partTypes),
            'I':   [0] * len(partTypes)
        },
        'partTypes': partTypes,
        'finished': [],
        'logs': []
    }
    main(states, data)