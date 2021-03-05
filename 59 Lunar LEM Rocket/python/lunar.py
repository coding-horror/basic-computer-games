"""
LUNAR

Lunar landing simulation

Ported by Dave LeCompte
"""

PAGE_WIDTH = 64


def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title):
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def print_instructions():
    print("THIS IS A COMPUTER SIMULATION OF AN APOLLO LUNAR")
    print("LANDING CAPSULE.")
    print()
    print()
    print("THE ON-BOARD COMPUTER HAS FAILED (IT WAS MADE BY")
    print("XEROX) SO YOU HAVE TO LAND THE CAPSULE MANUALLY.")
    print()

def print_intro():
    print("SET BURN RATE OF RETRO ROCKETS TO ANY VALUE BETWEEN")
    print("0 (FREE FALL) AND 200 (MAXIMUM BURN) POUNDS PER SECOND.")
    print("SET NEW BURN RATE EVERY 10 SECONDS.")
    print()
    print("CAPSULE WEIGHT 32,500 LBS; FUEL WEIGHT 16,500 LBS.")
    print()
    print()
    print()
    print("GOOD LUCK")
    print()

def show_landing(l, v):
    w = 3600 * v
    print("ON MOON AT {l} SECONDS - IMPACT VELOCITY {w} MPH")
    if w < 1.2:
        print("PERFECT LANDING!")
    elif w < 10:
        print("GOOD LANDING (COULD BE BETTER)")
    elif w <= 60:
        print("CRAFT DAMAGE... YOU'RE STRANDED HERE UNTIL A RESCUE")
        print("PARTY ARRIVES. HOPE YOU HAVE ENOUGH OXYGEN!")
    else:
        print("SORRY THERE WERE NO SURVIVORS. YOU BLEW IT!")
        print("IN FACT, YOU BLASTED A NEW LUNAR CRATER {w*.227:.4f} FEET DEEP!")
    end_sim()

        
def update_physics():
    # line 330
    elapsed_time += delta_t
    time_to_prompt -= delta_t
    m = m - delta_t * k
    a = i
    v = j
    return

def update_burn():
    # line 420
    q = delta_t * k / m
    j = (v +
         g * delta_t +
         z * (-q - q*q/2 - q**3/3 - q**4/4 - q**5/5))
    i = (a -
         g * delta_t * delta_t/2 -
         v * delta_t +
         z * delta_t * (q / 2 +
                        q ** 2 / 6 +
                        q ** 3 / 12 +
                        q ** 4 / 20 +
                        q ** 5 / 30))

    return q,j,i

def end_sim():
    print()
    print()
    print()
    print("TRY AGAIN??")


def prompt_for_burn():
    # line 150
    print("{l}\t{a}\t{int(5280*(a-int(a)))}\t{3600 * v}\t{m-n}")
    k = input()

def run_simulation():
    print()
    print("SEC\tMI + FT\tMPH\tLB FUEL\tBURN RATE")
    print()
    
    elapsed_time = 0
    a = 120
    v = 1 
    m = 33000 # mass of capsule with fuel
    n = 16500 # mass of capsule without fuel
    g = 1e-3
    z = 1.8

    k = prompt_for_burn()
    time_to_prompt = 10

    while True:
        # line 160
        if M-N < 1e-3:
            # line 240
            print(f"FUEL OUT AT {elapsed_time} SECONDS")
            delta_t = (-v + math.sqrt(v*v + 2*a*g)) / g
            v += g * delta_t
            elapsed_time += delta_t
            show_landing(elapsed_time, v)
            return

        if t < 1e-3:
            k = prompt_for_burn()
            continue

        # line 180
        delta_t = time_to_prompt
        if m < n + delta_t * k:
            delta_t = (m-n) / k
            
        # line 200
        q,j,i = update_burn()

        if i <= 0:
            # line 340
            while True:
                if delta_t < 5e-3:
                    show_landing(elapsed_time, v)
                #line 350
                d = v + math.sqrt(v*v + 2*a*(g-z*k/m))
                delta_t = 2 * a / d
                update_burn()
                update_physics()

        else:
            # line 210
            if ((v > 0) and (j < 0)):
                GOTO 370
            # line 230
            update_physics()



        
        


def main():
    print_header("LUNAR")
    print_instructions()
    while True:
        print_intro()
        run_simulation()
        

"""
120 L=0
130 PRINT: PRINT "SEC","MI + FT","MPH","LB FUEL","BURN RATE":PRINT 
140 A=120:V=1:M=33000:N=16500:G=1E-03:Z=1.8
150 PRINT L,INT(A);INT(5280*(A-INT(A))),3600*V,M-N,:INPUT K:T=10 
160 IF M-N<1E-03 THEN 240
170 IF T<1E-03 THEN 150
180 S=T: IF M>=N+S*K THEN 200
190 S=(M-N)/K
200 GOSUB 420: IF I<=O THEN 340
210 IF V<=0 THEN 230
220 IF J<0 THEN 370
230 GOSUB 330: GOTO 160
240 PRINT "FUEL OUT AT";L;"SECONDS":S=(-V+SQR(V*V+2*A*G))/G
250 V=V+G*S: L=L+S
260 W=3600*V: PRINT "ON MOON AT";L;"SECONDS - IMPACT VELOCITY";W;"MPH" 
274 IF W<=1.2 THEN PRINT "PERFECT LANDING!": GOTO 440 
280 IF W<=10 THEN PRINT "GOOD LANDING (COULD RE BETTER)":GOTO 440 
282 IF W>60 THEN 300
284 PRINT "CRAFT DAMAGE... YOU'RE STRANDED HERE UNTIL A RESCUE" 
286 PRINT "PARTY ARRIVES. HOPE YOU HAVE ENOUGH OXYGEN!" 
288 GOTO 440
300 PRINT "SORRY THERE NERE NO SURVIVORS. YOU BLOW IT!"
310 PRINT "IN FACT, YOU BLASTED A NEW LUNAR CRATER";W*.227;"FEET DEEP!"
320 GOTO 440
330 L=L+S: T=T-S: M=M-S*K: A=I: V=J: RETURN
340 IF S<5E-03 THEN 260
350 D=V+SQR(V*V+2*A*(G-Z*K/M)):S=2*A/D
360 GOSUB 420: GOSUB 330: GOTO 340
370 W=(1-M*G/(Z*K))/2: S=M*V/(Z*K*(W+SQR(W*W+V/Z)))+.05:GOSUB 420
380 IF I<=0 THEN 340
390 GOSUB 330: IF J>0 THEN 160
400 IF V>0 THEN 370
410 GOTO 160

"""
