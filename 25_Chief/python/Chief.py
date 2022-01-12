def print_lightning_bolt():

  print('*'*36)
  n = 24
  while n > 16:
    print(' '*n + 'x x')
    n -=1
  print(' '*16 + 'x xxx')
  print(' '*15 + 'x   x')
  print(' '*14+ 'xxx x')
  n -=1
  while n > 8:
    print(' '*n + 'x x')
    n -=1
  print(' '*8 + 'xx')
  print(' '*7 +'x')
  print('*'*36)


def print_solution(n):

  print('\n{} plus 3 gives {}. This Divided by 5 equals {}'.format(n, n+3, (n+3)/5))
  print('This times 8 gives {}. If we divide 5 and add 5.'.format(( (n+3)/5 )*8 ))
  print('We get {}, which, minus 1 equals {}'.format(( ((n+3)/5)*8)/5+5, ((((n+3)/5)*8)/5+5)-1 ))

def Game():
  print('\nTake a Number and ADD 3. Now, Divide this number by 5 and')
  print('multiply by 8. Now, Divide by 5 and add the same. Subtract 1')

  resp = float(input('\nWhat do you have? '))
  comp_guess = (((resp - 4)*5)/8)*5 -3
  resp2 = input('\nI bet your number was {} was i right(Yes or No)? '.format(comp_guess))
  
  if resp2 == 'Yes' or resp2 == 'YES' or resp2 == 'yes':
    print('\nHuh, I Knew I was unbeatable')
    print('And here is how i did it')
    print_solution(comp_guess)
    input('')
  
  else:
    resp3 = float(input('\nHUH!! what was you original number? '))
    
    if resp3 == comp_guess:
      print('\nThat was my guess, AHA i was right')
      print("Shamed to accept defeat i guess, don't worry you can master mathematics too")
      print('Here is how i did it')
      print_solution(comp_guess)
      input('')
    
    else:
      print('\nSo you think you\'re so smart, EH?')
      print('Now, Watch')
      print_solution(resp3)

      resp4 = input('\nNow do you believe me? ')

      if resp4 == 'Yes' or resp4 == 'YES' or resp4 == 'yes':
        print('\nOk, Lets play again sometime bye!!!!')
        input('')

      else:
        print('\nYOU HAVE MADE ME VERY MAD!!!!!')
        print("BY THE WRATH OF THE MATHEMATICS AND THE RAGE OF THE GODS")
        print("THERE SHALL BE LIGHTNING!!!!!!!")
        print_lightning_bolt()
        print('\nI Hope you believe me now, for your own sake')
        input('')

if __name__ == '__main__':

  print('I am CHIEF NUMBERS FREEK, The GREAT INDIAN MATH GOD.')
  play = input('\nAre you ready to take the test you called me out for(Yes or No)? ')
  if play == 'Yes' or play == 'YES' or play == 'yes':
    Game()
  else:
    print('Ok, Nevermind. Let me go back to my great slumber, Bye')
    input('')
