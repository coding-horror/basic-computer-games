import random

#Class of the Game
class NIM:

  def __init__(self):

    self.Piles = {
        1 : 7,
        2 : 5,
        3 : 3,
        4 : 1
        }

  def Remove_pegs(self, command):

    try:

      pile, num = command.split(',')
      num = int(num)
      pile = int(pile)

    except Exception as e:

      if 'not enough values' in str(e):
        print('\nNot a valid command. Your command should be in the form of "1,3", Try Again\n')

      else:
        print('\nError, Try again\n')
      return None

    if self._command_integrity(num, pile) == True:
      self.Piles[pile] -= num
    else:
      print('\nInvalid value of either Peg or Pile\n')

  def get_AI_move(self):
    
    possible_pile = []
    for k,v in self.Piles.items():
      if v != 0:
        possible_pile.append(k)

    pile = random.choice(possible_pile)

    num = random.randint(1,self.Piles[pile])

    return pile, num

  def _command_integrity(self, num, pile):

    if pile <= 4 and pile >= 1:
      if num <= self.Piles[pile]:
        return True
    
    return False

  def print_pegs(self):

    for pile, peg in self.Piles.items():
      print('Pile {} : {}'.format(pile, 'O '*peg))
    
  def Help(self):

    print('-'*10)
    print('\nThe Game is player with a number of Piles of Objects("O" == one peg)')
    print('\nThe Piles are arranged as given below(Tradional NIM)\n')
    self.print_pegs()
    print('\nAny Number of of Objects are removed one pile by "YOU" and the machine alternatively')
    print('\nOn your turn, you may take all the objects that remain in any pile')
    print('but you must take ATLEAST one object')
    print('\nAnd you may take objects from only one pile on a single turn.')
    print('\nThe winner is defined as the one that picks the last remaning object')
    print('-'*10)

  def Checkforwin(self):

    sum = 0
    for k,v in self.Piles.items():
      sum += v
    
    if sum == 0:
      return True
    
    else:
      return False

#main program
if __name__ == "__main__":

  #Game initialization
  game = NIM()

  print('Hello, This is a game of NIM')
  help = input('Do You Need Instruction (YES or NO): ')

  if help == 'yes' or help == 'YES' or help == 'Yes':

    #Printing Game Instruction
    game.Help()

  #Start game loop
  input('\nPress Enter to start the Game:\n')
  End = False
  while True:

    game.print_pegs()

    #Players Move
    command = input('\nYOUR MOVE - Number of PILE, Number of Object? ')
    game.Remove_pegs(command)
    End = game.Checkforwin()
    if End == True: 
      print('\nPlayer Wins the Game, Congratulations!!')
      input('\nPress any key to exit')
      break
    
    #Computers Move
    command = game.get_AI_move()
    print('\nA.I MOVE - A.I Removed {} pegs from Pile {}'.format(command[1],command[0]))
    game.Remove_pegs(str(command[0]) +',' + str(command[1]))
    End = game.Checkforwin()
    if End == True: 
      print('\nComputer Wins the Game, Better Luck Next Time\n')
      input('Press any key to exit')
      break
