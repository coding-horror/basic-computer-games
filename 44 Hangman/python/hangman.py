#!/usr/bin/env python3


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

def play():
    canvas = Canvas()
    draw_gallows(canvas)
    draw_head(canvas)
    draw_body(canvas)
    draw_right_arm(canvas)
    draw_left_arm(canvas)
    draw_right_leg(canvas)
    draw_left_leg(canvas)
    draw_left_hand(canvas)
    draw_right_hand(canvas)
    draw_left_foot(canvas)
    draw_right_foot(canvas)
    canvas.draw()

if __name__ == '__main__':
    play()
