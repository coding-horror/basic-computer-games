"""
LUNAR

Lunar landing simulation

Ported by Dave LeCompte
"""

import collections
import math


PAGE_WIDTH = 64

COLUMN_WIDTH = 2
SECONDS_WIDTH = 4
MPH_WIDTH = 6
ALT_MI_WIDTH = 6
ALT_FT_WIDTH = 4
MPH_WIDTH = 6
FUEL_WIDTH = 8
BURN_WIDTH = 10

SECONDS_LEFT = 0
SECONDS_RIGHT = SECONDS_LEFT + SECONDS_WIDTH
ALT_LEFT = SECONDS_RIGHT + COLUMN_WIDTH
ALT_MI_RIGHT = ALT_LEFT + ALT_MI_WIDTH
ALT_FT_RIGHT = ALT_MI_RIGHT + COLUMN_WIDTH + ALT_FT_WIDTH
MPH_LEFT = ALT_FT_RIGHT + COLUMN_WIDTH
MPH_RIGHT = MPH_LEFT + MPH_WIDTH
FUEL_LEFT = MPH_RIGHT + COLUMN_WIDTH
FUEL_RIGHT = FUEL_LEFT + FUEL_WIDTH
BURN_LEFT = FUEL_RIGHT + COLUMN_WIDTH
BURN_RIGHT = BURN_LEFT + BURN_WIDTH

PhysicalState = collections.namedtuple('PhysicalState', ['velocity', 'altitude'])

def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title):
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()

def add_rjust(line, s, pos):
    # adds a new field to a line right justified to end at pos
    
    s = str(s)
    slen = len(s)
    if len(line) + slen > pos:
        new_len = pos - slen
        line = line[:new_len]
    if len(line) + slen < pos:
        spaces = " " * (pos - slen - len(line))
        line = line + spaces
    return line + s

def add_ljust(line, s, pos):
    # adds a new field to a line left justified starting at pos
    
    s = str(s)
    slen = len(s)
    if len(line) > pos:
        line = line[:pos]
    if len(line) < pos:
        spaces = " " * (pos - len(line))
        line = line + spaces
    return line + s


def print_instructions():
    # Somebody had a bad experience with Xerox.
    
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

def show_landing(sim_clock, capsule):
    w = 3600 * capsule.v
    print(f"ON MOON AT {sim_clock.elapsed_time} SECONDS - IMPACT VELOCITY {w} MPH")
    if w < 1.2:
        print("PERFECT LANDING!")
    elif w < 10:
        print("GOOD LANDING (COULD BE BETTER)")
    elif w <= 60:
        print("CRAFT DAMAGE... YOU'RE STRANDED HERE UNTIL A RESCUE")
        print("PARTY ARRIVES. HOPE YOU HAVE ENOUGH OXYGEN!")
    else:
        print("SORRY THERE WERE NO SURVIVORS. YOU BLEW IT!")
        print(f"IN FACT, YOU BLASTED A NEW LUNAR CRATER {w*.227:.4f} FEET DEEP!")
    end_sim()


def show_out_of_fuel(sim_clock, capsule):
    # line 240
    print(f"FUEL OUT AT {sim_clock.elapsed_time} SECONDS")
    delta_t = (-capsule.v + math.sqrt(capsule.v ** 2 + 2 *
                                      capsule.a * capsule.g)) / capsule.g
    capsule.v += capsule.g * delta_t
    sim_clock.advance(delta_t)
    show_landing(sim_clock, capsule)

def format_line_for_report(t, miles, feet, velocity, fuel, burn_rate, is_header):
    line = add_rjust("", t, SECONDS_RIGHT)
    line = add_rjust(line, miles, ALT_MI_RIGHT)
    line = add_rjust(line, feet, ALT_FT_RIGHT)
    line = add_rjust(line, velocity, MPH_RIGHT)
    line = add_rjust(line, fuel, FUEL_RIGHT)
    if is_header:
        line = add_rjust(line, burn_rate, BURN_RIGHT)
    else:
        line = add_ljust(line, burn_rate, BURN_LEFT)
    return line

class Capsule:
    def __init__(self,
                 altitude = 120,
                 velocity = 1,
                 mass_with_fuel = 33000,
                 mass_without_fuel = 16500,
                 g = 1e-3,
                 z = 1.8):
        self.a = altitude # in miles above the surface
        self.v = velocity # downward
        self.m = mass_with_fuel
        self.n = mass_without_fuel
        self.g = g
        self.z = z
        self.fuel_per_second = 0

    def remaining_fuel(self):
        return self.m - self.n

    def is_out_of_fuel(self):
        return self.remaining_fuel() < 1e-3

    def update_state(self, sim_clock, delta_t, new_state):
        # line 330
        sim_clock.advance(delta_t)
        self.m = self.m - delta_t * self.fuel_per_second
        self.a = new_state.altitude
        self.v = new_state.velocity

    def fuel_time_remaining(self):
        # extrapolates out how many seconds we have at the current fuel burn rate
        assert(self.fuel_per_second > 0)
        return self.remaining_fuel() / self.fuel_per_second

    def predict_motion(self, delta_t):
        # Perform an Euler's Method numerical integration of the equations of motion.

        # line 420
        q = delta_t * self.fuel_per_second / self.m

        # new velocity
        new_velocity = (self.v +
                        self.g * delta_t +
                        self.z * (-q -
                                  q ** 2 / 2 -
                                  q ** 3 / 3 -
                                  q ** 4 / 4 -
                                  q ** 5 / 5))

        # new altitude
        new_altitude = (self.a -
                        self.g * delta_t ** 2 / 2 -
                        self.v * delta_t +
                        self.z * delta_t * (q / 2 +
                                            q ** 2 / 6 +
                                            q ** 3 / 12 +
                                            q ** 4 / 20 +
                                            q ** 5 / 30))

        
        return PhysicalState(altitude = new_altitude,
                             velocity = new_velocity)

    def make_state_display_string(self, sim_clock):
        seconds = sim_clock.elapsed_time
        miles = int(self.a)
        feet = int(5280 * (self.a - miles))
        velocity = int(3600 * self.v)
        fuel = int(self.remaining_fuel())
        burn_rate = " ? "

        return format_line_for_report(seconds, miles, feet, velocity, fuel, burn_rate, False)

    def prompt_for_burn(self, sim_clock):
        # line 150
        PROMPT = True

        msg = self.make_state_display_string(sim_clock)
        if PROMPT:
            self.fuel_per_second = float(input(msg))
        else:
            print(msg)
            self.fuel_per_second = 0.0
        sim_clock.time_until_next_prompt = 10
    

class SimulationClock:
    def __init__(self, elapsed_time, time_until_next_prompt):
        self.elapsed_time = elapsed_time
        self.time_until_next_prompt = time_until_next_prompt

    def time_for_prompt(self):
        return self.time_until_next_prompt < 1e-3

    def advance(self, delta_t):
        self.elapsed_time += delta_t
        self.time_until_next_prompt -= delta_t
        

def process_final_tick(delta_t, sim_clock, capsule):
    # When we extrapolated our position based on our velocity
    # and delta_t, we overshot the surface. For better
    # accuracy, we will back up and do shorter time advances.
            
    # line 340
    while True:
        if delta_t < 5e-3:
            show_landing(sim_clock, capsule)
            return
        #line 35
        average_vel = (capsule.v +
                       math.sqrt(capsule.v **2  +
                                 2 * capsule.a * (capsule.g -
                                                  capsule.z *
                                                  capsule.fuel_per_second / capsule.m))) / 2
        delta_t = capsule.a / average_vel
        new_state = capsule.predict_motion(delta_t)
        capsule.update_state(sim_clock, delta_t, new_state)

def handle_flyaway(capsule):
    """
    The user has started flying away from the moon. Since this is a
    lunar LANDING simulation, we wait until the capsule's velocity is
    positive (downward) before prompting for more input.
    
    Returns True if landed, False if simulation should continue.
    """
    
    while True:
        # line 370
        w = (1 - capsule.m * capsule.g / (capsule.z * capsule.fuel_per_second)) / 2
        delta_t = (capsule.m * capsule.v /
                   (capsule.z * capsule.fuel_per_second *
                    math.sqrt(w**2 + capsule.v / capsule.z))) + 0.05
                
        new_state = capsule.predict_motion(delta_t)

        # line 380
        if new_state.altitude <= 0:
            # have landed
            return True

        # line 390
        capsule.update_state(sim_clock, delta_t, new_state)

        if ((new_state.velocity > 0) or (capsule.v <= 0)):
            # return to normal sim
            return False



    

def end_sim():
    print()
    print()
    print()
    print("TRY AGAIN??")
    print()
    print()
    print()


def run_simulation():
    print()
    print(format_line_for_report("SEC",
                                 "MI",
                                 "FT",
                                 "MPH",
                                 "LB FUEL",
                                 "BURN RATE",
                                 True))

    sim_clock = SimulationClock(0, 10)
    capsule = Capsule()

    capsule.prompt_for_burn(sim_clock)

    while True:
        # line 160
        if capsule.is_out_of_fuel():
            show_out_of_fuel(sim_clock, capsule)
            return

        # line 170
        if sim_clock.time_for_prompt():
            capsule.prompt_for_burn(sim_clock)
            continue

        # line 180
        # clock advance is the shorter of the time to the next prompt,
        # or when we run out of fuel.
        if capsule.fuel_per_second > 0:
            delta_t = min(sim_clock.time_until_next_prompt,
                          capsule.fuel_time_remaining())
        else:
            delta_t = sim_clock.time_until_next_prompt

        # line 200
        new_state = capsule.predict_motion(delta_t)

        if new_state.altitude <= 0:
            process_final_tick(delta_t, sim_clock, capsule)
            return

        # line 210
        if capsule.v > 0 and new_state.velocity < 0:
            # moving away from the moon

            landed = handle_flyaway(capsule)
            if landed:
                process_final_tick(delta_t, sim_clock, capsule)
                return

        else:
            # line 230
            capsule.update_state(sim_clock, delta_t, new_state)



        
        


def main():
    print_header("LUNAR")
    print_instructions()
    while True:
        print_intro()
        run_simulation()

if __name__ == "__main__":
    main()
        

"""
10 PRINT TAB(33);"LUNAR"
20 PRINT TAB(l5);"CREATIVE COMPUTING MORRISTOWN, NEW JERSEY" 
25 PRINT:PRINT:PRINT
30 PRINT "THIS IS A COMPUTER SIMULATION OF AN APOLLO LUNAR" 
40 PRINT "LANDING CAPSULE.": PRINT: PRINT
50 PRINT "THE ON-BOARD COMPUTER HAS FAILED (IT WAS MADE BY" 
60 PRINT "XEROX) SO YOU HAVE TO LAND THE CAPSULE MANUALLY."
70 PRINT: PRINT "SET BURN RATE OF RETRO ROCKETS TO ANY VALUE BETWEEN" 
80 PRINT "0 (FREE FALL) AND 200 (MAXIMUM BURN) POUNDS PER SECOND." 
90 PRINT "SET NEW BURN RATE EVERY 10 SECONDS.": PRINT 
100 PRINT "CAPSULE WEIGHT 32,500 LBS; FUEL WEIGHT 16,500 LBS."
110 PRINT: PRINT: PRINT: PRINT "GOOD LUCK"
120 L=0
130 PRINT: PRINT "SEC","MI + FT","MPH","LB FUEL","BURN RATE":PRINT 
140 A=120:V=1:M=33000:N=16500:G=1E-03:Z=1.8

149 REM DWL capsule.prompt_for_burn
150 PRINT L,INT(A);INT(5280*(A-INT(A))),3600*V,M-N,:INPUT K:T=10 

159 REM main loop
160 IF M-N<1E-03 THEN 240
170 IF T<1E-03 THEN 150
180 S=T: IF M>=N+S*K THEN 200
190 S=(M-N)/K
200 GOSUB 420: IF I<=0 THEN 340
210 IF V<=0 THEN 230
220 IF J<0 THEN 370
230 GOSUB 330: GOTO 160

239 REM DWL show_out_of_fuel
240 PRINT "FUEL OUT AT";L;"SECONDS":S=(-V+SQR(V*V+2*A*G))/G
250 V=V+G*S: L=L+S

259 REM DWL show_landing
260 W=3600*V: PRINT "ON MOON AT";L;"SECONDS - IMPACT VELOCITY";W;"MPH" 
274 IF W<=1.2 THEN PRINT "PERFECT LANDING!": GOTO 440 
280 IF W<=10 THEN PRINT "GOOD LANDING (COULD BE BETTER)":GOTO 440 
282 IF W>60 THEN 300
284 PRINT "CRAFT DAMAGE... YOU'RE STRANDED HERE UNTIL A RESCUE" 
286 PRINT "PARTY ARRIVES. HOPE YOU HAVE ENOUGH OXYGEN!" 
288 GOTO 440
300 PRINT "SORRY THERE WERE NO SURVIVORS. YOU BLEW IT!"
310 PRINT "IN FACT, YOU BLASTED A NEW LUNAR CRATER";W*.227;"FEET DEEP!"
320 GOTO 440

329 REM DWL capsule.update_state
330 L=L+S: T=T-S: M=M-S*K: A=I: V=J: RETURN

339 REM DWL process_final_tick
340 IF S<5E-03 THEN 260
350 D=V+SQR(V*V+2*A*(G-Z*K/M)):S=2*A/D
360 GOSUB 420: GOSUB 330: GOTO 340

369 REM DWL handle_flyaway
370 W=(1-M*G/(Z*K))/2: S=M*V/(Z*K*(W+SQR(W*W+V/Z)))+.05:GOSUB 420
380 IF I<=0 THEN 340
390 GOSUB 330: IF J>0 THEN 160
400 IF V>0 THEN 370
410 GOTO 160

419 REM DWL capsule.predict_motion
420 Q=S*K/M: J=V+G*S+Z*(-Q-Q*Q/2-Q^3/3-Q^4/4-Q^5/5)
430 I=A-G*S*S/2-V*S+Z*S*(Q/2+Q^2/6+Q^3/12+Q^4/20+Q^5/30):RETURN

439 REM DWL end_sim
440 PRINT:PRINT:PRINT:PRINT "TRY AGAIN??": GOTO 70
"""
