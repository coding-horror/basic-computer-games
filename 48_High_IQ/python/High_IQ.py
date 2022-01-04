
def print_instructions():
    print("\n" * 3)
    print("HERE IS THE BOARD:\n")
    print("\n")
    print("          !    !    !\n")
    print("         13   14   15\n")
    print("\n")
    print("          !    !    !\n")
    print("         22   23   24\n")
    print("\n")
    print("!    !    !    !    !    !    !\n")
    print("29   30   31   32   33   34   35\n")
    print("\n")
    print("!    !    !    !    !    !    !\n")
    print("38   39   40   41   42   43   44\n")
    print("\n")
    print("!    !    !    !    !    !    !\n")
    print("47   48   49   50   51   52   53\n")
    print("\n")
    print("          !    !    !\n")
    print("         58   59   60\n")
    print("\n")
    print("          !    !    !\n")
    print("         67   68   69\n")
    print("\n")
    print("TO SAVE TYPING TIME, A COMPRESSED VERSION OF THE GAME BOARD\n")
    print("WILL BE USED DURING PLAY.  REFER TO THE ABOVE ONE FOR PEG\n")
    print("NUMBERS.  OK, LET'S BEGIN.\n")


def play_game():
    print("Lets play a game")
  
def main():
    if input("Do you want instrunctions?\n").lower().startswith("y"):
        print_instructions()
    
if __name__ == "__main__":
    main()
