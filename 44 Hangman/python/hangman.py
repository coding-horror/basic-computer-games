#!/usr/bin/env python3
import random

class Canvas:
    ''' For drawing text-based figures '''

    def __init__(self, width=12, height=12, fill=' '):
        self._buffer = []
        for _ in range(height):
            line = []
            for _ in range(width):
                line.append('')
            self._buffer.append(line)

        self.clear()

    def clear(self, fill=' '):
        for row in self._buffer:
            for x in range(len(row)):
                row[x] = fill

    def draw(self):
        for line  in self._buffer:
            # Joining by the empty string ('') smooshes all of the
            # individual characters together as one line.
            print(''.join(line))

    def put(self, s, x, y):
        # In an effort to avoid distorting the drawn image, only write the
        # first character of the given string to the buffer.
        self._buffer[y][x] = s[0]


def draw_head(canvas):
    canvas.put('-', 5, 2)
    canvas.put('-', 6, 2)
    canvas.put('-', 7, 2)
    canvas.put('(', 4, 3)
    canvas.put('.', 5, 3)
    canvas.put('.', 7, 3)
    canvas.put(')', 8, 3)
    canvas.put('-', 5, 4)
    canvas.put('-', 6, 4)
    canvas.put('-', 7, 4)


def draw_gallows(canvas):
    for i in range(12):
        canvas.put('X', 0, i)
    for i in range(7):
        canvas.put('X', i, 0)
    canvas.put('X', 6, 1)


def draw_body(canvas):
    for i in range(5, 9, 1):
        canvas.put('X', 6, i)


def draw_right_arm(canvas):
    for i in range(3, 7):
        canvas.put('\\', i - 1, i)


def draw_left_arm(canvas):
    canvas.put('/', 10, 3)
    canvas.put('/', 9, 4)
    canvas.put('/', 8, 5)
    canvas.put('/', 7, 6)


def draw_right_leg(canvas):
    canvas.put('/', 5, 9)
    canvas.put('/', 4, 10)


def draw_left_leg(canvas):
    canvas.put('\\', 7, 9)
    canvas.put('\\', 8, 10)


def draw_left_hand(canvas):
    canvas.put('\\', 10, 2)


def draw_right_hand(canvas):
    canvas.put('/', 2, 2)


def draw_left_foot(canvas):
    canvas.put('\\', 9, 11)
    canvas.put('-', 10, 11)


def draw_right_foot(canvas):
    canvas.put('-', 2, 11)
    canvas.put('/', 3, 11)


PHASES = (
    ("FIRST, WE DRAW A HEAD", draw_head),
    ("NOW WE DRAW A BODY.", draw_body),
    ("NEXT WE DRAW AN ARM.", draw_right_arm),
    ("THIS TIME IT'S THE OTHER ARM.", draw_left_arm),
    ("NOW, LET'S DRAW THE RIGHT LEG.", draw_right_leg),
    ("THIS TIME WE DRAW THE LEFT LEG.", draw_left_leg),
    ("NOW WE PUT UP A HAND.", draw_left_hand),
    ("NEXT THE OTHER HAND.", draw_right_hand),
    ("NOW WE DRAW ONE FOOT", draw_left_foot),
    ("HERE'S THE OTHER FOOT -- YOU'RE HUNG!!", draw_right_foot)
)

WORDS = ('GUM','SIN','FOR','CRY','LUG','BYE','FLY',
         'UGLY','EACH','FROM','WORK','TALK','WITH','SELF',
         'PIZZA','THING','FEIGN','FIEND','ELBOW','FAULT','DIRTY',
         'BUDGET','SPIRIT','QUAINT','MAIDEN','ESCORT','PICKAX',
         'EXAMPLE','TENSION','QUININE','KIDNEY','REPLICA','SLEEPER',
         'TRIANGLE','KANGAROO','MAHOGANY','SERGEANT','SEQUENCE',
         'MOUSTACHE','DANGEROUS','SCIENTIST','DIFFERENT','QUIESCENT',
         'MAGISTRATE','ERRONEOUSLY','LOUDSPEAKER','PHYTOTOXIC',
         'MATRIMONIAL','PARASYMPATHOMIMETIC','THIGMOTROPISM')

QUESTION_PROMPT = '? '


def revealed_word(word, letters_used):
    return ''.join(letter if letter in letters_used else '-'
                  for letter in word)


def play():

    print('HANGMAN')
    print('CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n')

    canvas = Canvas()
    draw_gallows(canvas)

    word = random.choice(WORDS)
    letters_used = set()
    guesses_count = 0

    while True:
        print('HERE ARE THE LETTERS YOU USED:')
        print(', '.join(sorted(letters_used)))
        print('\n')

        print(revealed_word(word, letters_used))

        print('WHAT IS YOUR GUESS', end=QUESTION_PROMPT)
        guess = input().upper()

        if guess in letters_used:
            print('YOU GUESSED THAT LETTER BEFORE!')
            continue

        if guess not in word:
            comment, draw_function = PHASES[guesses_count]
            print('\n\nSORRY, THAT LETTER ISN\'T IN THE WORD.')
            print(comment)
            draw_function(canvas)
            canvas.draw()

            guesses_count += 1
            if guesses_count == len(PHASES):
               print('SORRY, YOU LOSE.  THE WORD WAS', word)
               break

        letters_used.add(guess)
        print('\n' + revealed_word(word, letters_used))

        print('\nWHAT IS YOUR GUESS FOR THE WORD', end=QUESTION_PROMPT)
        guessed_word = input().upper()

        if guessed_word != word:
            print('WRONG.  TRY ANOTHER LETTER.')
            continue


if __name__ == '__main__':
    play()
