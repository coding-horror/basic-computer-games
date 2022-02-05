import random

#Stock_Market
class Stock_Market():

  def __init__(self):

    #Hard Coded Names
    short_names = ['IBM', 'RCA', 'LBJ', 'ABC', 'CBS']
    full_names = ['INT. BALLISTIC MISSLES', 'RED CROSS OF AMERICA', 
                  'LICHTENSTEIN, BUMRAP & JOKE', 'AMERICAN BANKRUPT CO.',
                  'CENSURED BOOKS STORE']

    #Initializing Dictionary to hold all the information systematically
    self.data = {}
    for sn, fn in zip(short_names, full_names):
      #A dictionary for each stock
      temp = {'Name' : fn, 'Price' : None, 'Holdings' : 0}
      #Nested outer dictionary for all stocks
      self.data[sn] = temp

    #Initializing Randomly generated initial prices
    for stock in self.data.values():
      stock['Price'] = round(random.uniform(80,120),2) #Price b/w 60 and 120

    #Initialize Assets
    self.cash_assets = 10000
    self.stock_assets = 0
    
  def total_assets(self):
    
    return self.cash_assets + self.stock_assets

  def _generate_day_change(self):

    self.changes = []
    for _ in range(len(self.data)):
      self.changes.append(round(random.uniform(-5,5),2)) #Random % Change b/w -5 and 5

  def update_prices(self):

    self._generate_day_change()
    for stock, change in zip(self.data.values(), self.changes):
      stock['Price'] = round(stock['Price'] + (change/100)*stock['Price'], 2)

  def print_exchange_average(self):
    
    sum = 0
    for stock in self.data.values():
      sum += stock['Price']

    print('\nNEW YORK STOCK EXCHANGE AVERAGE: ${:.2f}'.format(sum/5))

  def get_average_change(self):

    sum = 0
    for change in self.changes:
      sum += change
    
    return round(sum/5,2)

  def print_first_day(self):

    print('\nSTOCK\t\t\t\t\tINITIALS\tPRICE/SHARE($)')
    for stock, data in self.data.items():
      if stock != 'LBJ':
        print('{}\t\t\t{}\t\t{}'.format(data['Name'], stock, data['Price']))
      else:
        print('{}\t\t{}\t\t{}'.format(data['Name'], stock, data['Price']))

    self.print_exchange_average()
    self.print_assets()

  def take_inputs(self):

    print('\nWHAT IS YOUR TRANSACTION IN')
    flag = False
    while flag != True:
      new_holdings = []
      for stock in self.data.keys():
        try:
          new_holdings.append(int(input('{}? '.format(stock))))
        except:
          print('\nINVALID ENTRY, TRY AGAIN\n')
          break
      if len(new_holdings) == 5:
        flag = self._check_transaction(new_holdings)
      
    return new_holdings

  def print_trading_day(self):

    print("STOCK\tPRICE/SHARE\tHOLDINGS\tNET. Value\tPRICE CHANGE")
    for stock, data, change in zip(self.data.keys(), self.data.values(),self.changes):
      value = data['Price'] * data['Holdings']
      print('{}\t{}\t\t{}\t\t{:.2f}\t\t{}'.format(stock, data['Price'], data['Holdings'], value, change))

  def update_cash_assets(self, new_holdings):

    sell=0
    buy=0
    for stock, holding in zip(self.data.values(), new_holdings):
      if holding > 0:
        buy += stock['Price']*holding
      
      elif holding < 0:
        sell += stock['Price']*abs(holding)

    self.cash_assets = self.cash_assets + sell - buy

  def update_stock_assets(self):

    sum=0
    for data in self.data.values():
      sum += data['Price']*data['Holdings']

    self.stock_assets = round(sum,2)

  def print_assets(self):

    print('\nTOTAL STOCK ASSETS ARE: ${:.2f}'.format(self.stock_assets))
    print('TOTAL CASH ASSETS ARE: ${:.2f}'.format(self.cash_assets))
    print('TOTAL ASSETS ARE: ${:.2f}'.format(self.total_assets()))

  def _check_transaction(self, new_holdings):

    sum = 0
    for stock, holding in zip(self.data.values(), new_holdings):
      if holding > 0:
        sum += stock['Price']*holding
      
      elif holding < 0:
        if abs(holding) > stock['Holdings']:
          print('\nYOU HAVE OVERSOLD SOME STOCKS, TRY AGAIN\n')
          return False

    if sum > self.cash_assets:
      print('\nYOU HAVE USED ${:.2f} MORE THAN YOU HAVE, TRY AGAIN\n'.format(sum - self.cash_assets))
      return False

    return True

  def update_holdings(self, new_holdings):

    for stock, new_holding in zip(self.data.values(), new_holdings):
      stock['Holdings'] += new_holding

def print_instruction():

  print('''
THIS PROGRAM PLAYS THE STOCK MARKET.  YOU WILL BE GIVEN
$10,000 AND MAY BUY OR SELL STOCKS.  THE STOCK PRICES WILL
BE GENERATED RANDOMLY AND THEREFORE THIS MODEL DOES NOT
REPRESENT EXACTLY WHAT HAPPENS ON THE EXCHANGE.  A TABLE
OF AVAILABLE STOCKS, THEIR PRICES, AND THE NUMBER OF SHARES
IN YOUR PORTFOLIO WILL BE PRINTED.  FOLLOWING THIS, THE
INITIALS OF EACH STOCK WILL BE PRINTED WITH A QUESTION
MARK.  HERE YOU INDICATE A TRANSACTION.  TO BUY A STOCK
TYPE +NNN, TO SELL A STOCK TYPE -NNN, WHERE NNN IS THE
NUMBER OF SHARES.  A BROKERAGE FEE OF 1% WILL BE CHARGED
ON ALL TRANSACTIONS.  NOTE THAT IF A STOCK'S VALUE DROPS
TO ZERO IT MAY REBOUND TO A POSITIVE VALUE AGAIN.  YOU
HAVE $10,000 TO INVEST.  USE INTEGERS FOR ALL YOUR INPUTS.
(NOTE:  TO GET A 'FEEL' FOR THE MARKET RUN FOR AT LEAST
10 DAYS)
          ------------GOOD LUCK!------------\n
    ''')
  
if __name__ == "__main__":
  
  print('\t\t      STOCK MARKET')
  help = input('\nDO YOU WANT INSTRUCTIONS(YES OR NO)? ')

  #Printing Instruction
  if help == 'YES' or help == 'yes' or help == 'Yes':
    print_instruction()
  
  #Initialize Game
  Game = Stock_Market()

  #Do first day
  Game.print_first_day()
  new_holdings = Game.take_inputs()
  Game.update_holdings(new_holdings)
  Game.update_cash_assets(new_holdings)
  print('\n------------END OF TRADING DAY--------------\n')

  response = 1
  while response == 1:

    #Simulate a DAY
    Game.update_prices()
    Game.print_trading_day()
    Game.print_exchange_average()
    Game.update_stock_assets()
    Game.print_assets()

    response = int(input('\nDO YOU WISH TO CONTINUE (YES-TYPE 1, NO-TYPE 0)? '))
    if response == 0:
      break
    
    new_holdings = Game.take_inputs()
    Game.update_holdings(new_holdings)
    Game.update_cash_assets(new_holdings)
    print('\n------------END OF TRADING DAY--------------\n')

  print('\nHOPE YOU HAD FUN!!!!')
  input('')
