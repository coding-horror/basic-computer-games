from random import randint
from collections import namedtuple

PAGE_WIDTH = 64

def main():
    states = {
        'start': (print_start, control_start, update_state),
        'instructions': (print_instructions, goto_game, update_state),
        'game': (print_game, control_game, update_game),
        'pictures': (print_pictures, goto_game, update_state),
        'won': (print_winner, exit_game, update_state)
    }

    partTypes = (
        Bodypart(name="BODY", count=1 , depends=None),
        Bodypart(name="NECK", count=1 , depends=0),
        Bodypart(name="HEAD", count=1 , depends=1),
        Bodypart(name="FEELERS", count=2 , depends=2),
        Bodypart(name="TAIL", count=1 , depends=0),
        Bodypart(name="LEGS", count=6 , depends=0)
    )

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
    
    while True:
        if 'exit' == data['state']: break
        view, control, model = states[data['state']]
        cmd = view(data)
        action = control(cmd)
        data = model(data, action)
        
Bodypart = namedtuple('Bodypart', ['name', 'count', 'depends'])

def print_start(_):
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
    if cmd.lower() in ('y', 'yes'):
        action = 'instructions'
    else:
        action = 'game'
    return action
    
def print_instructions(data):
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
    return 'game'

def update_state(data, action):
    return {**data, 'state': action}

def update_game(data, action):
    logs = []
    
    if 'pictures' == action:
        data['state'] = 'pictures'
    else:
        partAdded = False
        while partAdded == False:
            for player, parts in data['players'].items():
                newPartIdx = randint(1,6) - 1
                partType = data['partTypes'][newPartIdx]
                partCount = parts[newPartIdx]
                
                logs.append(('rolled', newPartIdx, player))
                
                overMaxParts = partType.count < partCount + 1
                missingPartDep = partType.depends != None and parts[partType.depends] == 0
                
                if not overMaxParts and not missingPartDep:
                    partCount += 1
                    logs.append(('added', newPartIdx, player))
                    partAdded = True
                elif missingPartDep:
                    logs.append(('missingDep', newPartIdx, player, partType.depends))
                if overMaxParts:
                    logs.append(('overMax', newPartIdx, player, partCount))
                    
                data['players'][player][newPartIdx] = partCount
    data['logs'] = logs
    
    finished = get_finished(data)
    if len(finished) > 0:
        data['finished'] = finished
        data['state'] = 'won'
    return data

def get_finished(data):
    totalParts = sum([partType.count for partType in data['partTypes']])
    finished = []
    for player, parts in data['players'].items():
        if(sum(parts) == totalParts): finished.append(player)
    return finished

def print_game(data):
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
    if cmd.lower() in ('y', 'yes'):
        action = 'pictures'
    else:
        action = 'game'
    return action

def print_winner(data):
    for player in data['finished']:
        print(f"{'YOUR' if 'YOU' == player else 'MY'} BUG IS FINISHED.")
    print("I HOPE YOU ENJOYED THE GAME, PLAY IT AGAIN SOON!!")
    
def exit_game(_):
    return 'exit'
    
def print_centered(msg, width=PAGE_WIDTH):
    spaces = " " * ((width - len(msg)) // 2)
    print(spaces + msg)
    
def print_table(rows):
    for row in rows:
        print(*row, sep="\t")

if __name__ == '__main__':
    main()