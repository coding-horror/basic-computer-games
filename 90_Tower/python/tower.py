import sys

class Disk:
    def __init__(self, size):
        self.__size = size

    def size(self):
        return self.__size

    def print(self):
        print("[ %s ]" % self.size())

class Tower:
    def __init__(self):
        self.__disks = []

    def empty(self):
        return len(self.__disks) == 0

    def top(self):
        if self.empty():
            return None
        else:
            return self.__disks[-1]

    def add(self, disk):
        if not self.empty():
            t = self.top()
            if disk.size() > t.size():
                raise Exception("YOU CAN'T PLACE A LARGER DISK ON TOP OF A SMALLER ONE, IT MIGHT CRUSH IT!")
        self.__disks.append(disk)

    def pop(self):
        if self.empty():
            raise Exception("empty pop")
        return self.__disks.pop()

    def print(self):
        r = "Needle: [%s]" % (", ".join([str(x.size()) for x in self.__disks]))
        print(r)



print("""
IN THIS PROGRAM, WE SHALL REFER TO DISKS BY NUMERICAL CODE.
3 WILL REPRESENT THE SMALLEST DISK, 5 THE NEXT SIZE,
7 THE NEXT, AND SO ON, UP TO 15.  IF YOU DO THE PUZZLE WITH
2 DISKS, THEIR CODE NAMES WOULD BE 13 AND 15.  WITH 3 DISKS
THE CODE NAMES WOULD BE 11, 13 AND 15, ETC.  THE NEEDLES
ARE NUMBERED FROM LEFT TO RIGHT, 1 TO 3.  WE WILL
START WITH THE DISKS ON NEEDLE 1, AND ATTEMPT TO MOVE THEM
TO NEEDLE 3.

GOOD LUCK!

""")




class Game:
    def __init__(self):
        # use fewer sizes to make debugging easier
        # self.__sizes = [3, 5, 7]  # ,9,11,13,15]
        self.__sizes = [3, 5, 7, 9, 11, 13, 15]
        
        self.__sizes.sort()

        self.__towers = []
        self.__moves = 0
        self.__towers = [Tower(), Tower(), Tower()]
        self.__sizes.reverse()
        for size in self.__sizes:
            disk = Disk(size)
            self.__towers[0].add(disk)

    def winner(self):
        return self.__towers[0].empty() and self.__towers[1].empty()

    def print(self):
        for t in self.__towers:
            t.print()

    def moves(self):
        return self.__moves

    def which_disk(self):
        w = int(input("WHICH DISK WOULD YOU LIKE TO MOVE\n"))
        if w in self.__sizes:
            return w
        else:
            raise Exception()

    def pick_disk(self):
        which = None
        while which is None:
            try:
                which = self.which_disk()
            except:
                print("ILLEGAL ENTRY... YOU MAY ONLY TYPE 3,5,7,9,11,13, OR 15.\n")

        valids = [t for t in self.__towers if t.top() and t.top().size() == which]
        assert len(valids) in (0, 1)
        if not valids:
            print("THAT DISK IS BELOW ANOTHER ONE.  MAKE ANOTHER CHOICE.\n")
            return None
        else:
            assert valids[0].top().size() == which
            return valids[0]

    def which_tower(self):
        try:
            needle = int(input("PLACE DISK ON WHICH NEEDLE\n"))
            tower = self.__towers[needle - 1]
        except:
            print("I'LL ASSUME YOU HIT THE WRONG KEY THIS TIME.  BUT WATCH IT,\nI ONLY ALLOW ONE MISTAKE.\n")
            return None
        else:
            return tower

    def take_turn(self):
        from_tower = None
        while from_tower is None:
            from_tower = self.pick_disk()

        to_tower = self.which_tower()
        if not to_tower:
            to_tower = self.which_tower()

        if not to_tower:
            print("I TRIED TO WARN YOU, BUT YOU WOULDN'T LISTEN.\nBYE BYE, BIG SHOT.\n")
            sys.exit(0)

        disk = from_tower.pop()
        try:
            to_tower.add( disk )
            self.__moves += 1
        except Exception as err:
            print(err)
            from_tower.add(disk)

game = Game()
while True:
    game.print()

    game.take_turn()

    if game.winner():
        print("CONGRATULATIONS!!\nYOU HAVE PERFORMED THE TASK IN %s MOVES.\n" % game.moves())
        while True:
            yesno = input("TRY AGAIN (YES OR NO)\n")
            if yesno.upper() == "YES":
                game = Game()
                break
            elif yesno.upper() == "NO":
                print("THANKS FOR THE GAME!\n")
                sys.exit(0)
            else:
                print("'YES' OR 'NO' PLEASE\n")
    elif game.moves() > 128:
        print("SORRY, BUT I HAVE ORDERS TO STOP IF YOU MAKE MORE THAN 128 MOVES.")
        sys.exit(0)
