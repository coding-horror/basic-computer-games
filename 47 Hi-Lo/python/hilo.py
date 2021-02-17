#!/usr/bin/env python3

QUESTION_PROMPT='? '

def play():
    print('HI LO')
    print('CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n')
    print('THIS IS THE GAME OF HI LO.\n')
    print('YOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE')
    print('HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS.  IF YOU')
    print('GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!')
    print('THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,')
    print('IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.\n\n')

    total_winnings = 0
    while True:
        print('PLAY AGAIN (YES OR NO)', end=QUESTION_PROMPT)
        answer = input().upper()
        if answer != 'YES':
            break

    print('\nSO LONG.  HOPE YOU ENJOYED YOURSELF!!!')

if __name__ == '__main__':
    play()
