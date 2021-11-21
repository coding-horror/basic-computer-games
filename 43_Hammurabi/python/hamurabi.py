def GenRandom(C):
    C=int(random()*5)+1
def BadInput850():
    print ( "\nHAMURABI:  I CANNOT DO WHAT YOU WISH.")
    print ( "GET YOURSELF ANOTHER STEWARD!!!!!")
    Z=99
def BadInput710(S):
    print ( "HAMURABI:  THINK AGAIN.  YOU HAVE ONLY")
    print ( S,"BUSHELS OF GRAIN.  NOW THEN,")
def BadInput720(A):
    print ( "HAMURABI:  THINK AGAIN.  YOU OWN ONLY",A,"ACRES.  NOW THEN,")
def BadInput710(S):
    print ( "HAMURABI:  THINK AGAIN.  YOU HAVE ONLY")
    print ( S,"BUSHELS OF GRAIN.  NOW THEN,")
def NationalFink():
    print ( "DUE TO THIS EXTREME MISMANAGEMENT YOU HAVE NOT ONLY")
    print ( "BEEN IMPEACHED AND THROWN OUT OF OFFICE BUT YOU HAVE")
    print ( "ALSO BEEN DECLARED NATIONAL FINK!!!!")

def B_input(promptstring): #emulate BASIC input. It rejects non-numeric values
    x=input(promptstring)
    while x.isalpha():
       x=input("?REDO FROM START\n? ")
    return int(x)
       
from random import random
from random import seed
seed()
title = "HAMURABI"
title = title.rjust(32,' ')
print (title)
attribution = "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
attribution = attribution.rjust(15," ")
print (attribution)
print ('\n\n\n')
print ("TRY YOUR HAND AT GOVERNING ANCIENT SUMERIA")
print ("FOR A TEN-YEAR TERM OF OFFICE.\n")
D1=0
P1=0
Z=0 #year
P=95 #population
S=2800 #grain stores
H=3000
E=H-S #rats eaten
Y=3 #yield (amount of production from land). Reused as price per acre
A=H/Y #acres of land
I=5 #immigrants
Q=1 #boolean for plague, also input for buy/sell land
D=0 # people
while (Z<11): #line 270. main loop. while the year is less than 11
    print ("\n\n\nHAMURABI:  I BEG TO REPORT TO YOU")
    Z=Z+1 #year
    print ( "IN YEAR",Z,",",D,"PEOPLE STARVED,",I,"CAME TO THE CITY,")
    P=P+I
    if Q==0: 
      P=int(P/2)
      print ("A HORRIBLE PLAGUE STRUCK!  HALF THE PEOPLE DIED.")
    print ("POPULATION IS NOW",P)
    print ("THE CITY NOW OWNS",A,"ACRES.")
    print ("YOU HARVESTED",Y,"BUSHELS PER ACRE.")
    print ("THE RATS ATE",E,"BUSHELS.")
    print ("YOU NOW HAVE ",S,"BUSHELS IN STORE.\n")
    C=int(10*random()) #random number between 1 and 10
    Y=C+17
    print ("LAND IS TRADING AT",Y,"BUSHELS PER ACRE.")
    Q=-99 #dummy value to track status
    while Q==-99: #always run the loop once
       Q = B_input("HOW MANY ACRES DO YOU WISH TO BUY? ")
       if Q<0:
          Q = -1 #to avoid the corner case of Q=-99
          BadInput850()
          Z=99 #jump out of main loop and exit
       elif Y*Q>S: #can't afford it
           BadInput710(S)
           Q=-99 # give'm a second change to get it right
       elif Y*Q<=S: #normal case, can afford it
           A=A+Q  #increase the number of acres by Q
           S=S-Y*Q #decrease the amount of grain in store to pay for it
           C=0 #WTF is C for?
    if Q ==0 and Z!=99: #maybe you want to sell some land?
        Q = -99
        while Q==-99:
           Q = B_input( "HOW MANY ACRES DO YOU WISH TO SELL? ")
           if Q<0:
              BadInput850()
              Z=99 #jump out of main loop and exit
           elif Q<=A:#normal case
              A=A-Q # reduce the acres
              S=S+Y*Q #add to grain stores
              C=0 #still don't know what C is for
           else: #Q>A error!
              BadInput720()
              Q=-99 #reloop
        print ("\n")
    Q=-99
    while Q==-99 and Z!=99:
       Q = B_input("HOW MANY BUSHELS DO YOU WISH TO FEED YOUR PEOPLE? ")
       if Q<0:
            BadInput850()
           #REM *** TRYING TO USE MORE GRAIN THAN IS IN SILOS?
       elif Q>S:
            BadInput710
            Q=-99 #try again!
       else: #we're good. do the transaction
            S=S-Q #remove the grain from the stores
            C=1 #set the speed of light to 1. jk
    print ("\n")
    D=-99 #dummy value to force at least one loop
    while D == -99 and Z!=99:
        D = B_input("HOW MANY ACRES DO YOU WISH TO PLANT WITH SEED? ")
        if D<0:
           BadInput850()
           Z=99
        elif D>0:
           if D>A: 
               #REM *** TRYING TO PLANT MORE ACRES THAN YOU OWN?
               BadInput720(A)
               D = -99
           elif int(D/2)>S:
                #REM *** ENOUGH GRAIN FOR SEED?
                BadInput710(S)
                D = -99
           elif D>=10*P:
                #REM *** ENOUGH PEOPLE TO TEND THE CROPS?
                print ("BUT YOU HAVE ONLY",P,"PEOPLE TO TEND THE FIELDS!  NOW THEN,")
                D=-99
           else: #we're good. decrement the grain store
                S=S-int(D/2)
    GenRandom(C)
    #REM *** A BOUNTIFUL HARVEST!
    Y=C
    H=D*Y
    E=0
    GenRandom(C)
    if int(C/2)==C/2: #even number. 50/50 chance
        #REM *** RATS ARE RUNNING WILD!!
        E=int(S/C) #calc losses due to rats, based on previous random number
        S=S-E+H #deduct losses from stores
    GenRandom(C)
    #REM *** LET'S HAVE SOME BABIES
    I=int(C*(20*A+S)/P/100+1)
    #REM *** HOW MANY PEOPLE HAD FULL TUMMIES?
    C=int(Q/20)
    #REM *** HORROS, A 15% CHANCE OF PLAGUE
    #yeah, should be HORRORS, but left it
    Q=int(10*(2*random()-.3))
    if P>=C and Z!=99: #if there are some people without full bellies...
        #REM *** STARVE ENOUGH FOR IMPEACHMENT?
        D=P-C
        if D>.45*P:
            print ("\nYOU STARVED",D,"PEOPLE IN ONE YEAR!!!")
            NationalFink()
            Z=99 #exit the loop
        P1=((Z-1)*P1+D*100/P)/Z
        P=C
        D1=D1+D
if Z!=99:
   print ( "IN YOUR 10-YEAR TERM OF OFFICE,",P1,"PERCENT OF THE")
   print ( "POPULATION STARVED PER YEAR ON THE AVERAGE, I.E. A TOTAL OF")
   print ( D1,"PEOPLE DIED!!")
   L=A/P
   print ( "YOU STARTED WITH 10 ACRES PER PERSON AND ENDED WITH")
   print ( L,"ACRES PER PERSON.\n")
   if (P1>33 or L<7):
       NationalFink()
   elif (P1>10 or L<9):
       print ( "YOUR HEAVY-HANDED PERFORMANCE SMACKS OF NERO AND IVAN IV.")
       print ( "THE PEOPLE (REMIANING) FIND YOU AN UNPLEASANT RULER, AND,")
       print ( "FRANKLY, HATE YOUR GUTS!!")
   elif (P1>3 or L<10):
       print ( "YOUR PERFORMANCE COULD HAVE BEEN SOMEWHAT BETTER, BUT")
       print ( "REALLY WASN'T TOO BAD AT ALL. ",int(P*.8*random()),"PEOPLE")
       print ( "WOULD DEARLY LIKE TO SEE YOU ASSASSINATED BUT WE ALL HAVE OUR")
       print ( "TRIVIAL PROBLEMS.")
   else:
       print ( "A FANTASTIC PERFORMANCE!!!  CHARLEMANGE, DISRAELI, AND")
       print ( "JEFFERSON COMBINED COULD NOT HAVE DONE BETTER!\n")
   for N in range(1,10):
       print ( '\a')
print("\nSO LONG FOR NOW.\n")



