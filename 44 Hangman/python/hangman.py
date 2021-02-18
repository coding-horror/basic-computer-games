#!/usr/bin/env python3


class Canvas:
    ''' For drawing text-based figures '''

    def __init__(self, width=12, height=12, fill=' '):
        self._buffer = []
        for _ in range(height):
            line = []
            for _ in range(width):
                line.append(fill[0])
            self._buffer.append(line)

    def draw(self):
        for line  in self._buffer:
            # Joining by the empty string ('') smooshes all of the
            # individual characters together as one line.
            print(''.join(line))

    def put(self, character, y, x):
        # In an effort to avoid distorting the drawn image, only write the
        # first character of the given string to the buffer.
        self._buffer[y][x] = character[0]


def play():
    canvas = Canvas()
    canvas.put('-', 2, 5)
    canvas.put('-', 2, 6)
    canvas.put('-', 2, 7)
    canvas.put('(', 3, 4)
    canvas.put('.', 3, 5)
    canvas.put('.', 3, 7)
    canvas.put(')', 3, 8)
    canvas.put('-', 4, 5)
    canvas.put('-', 4, 6)
    canvas.put('-', 4, 7)
    canvas.draw()

if __name__ == '__main__':
    play()
