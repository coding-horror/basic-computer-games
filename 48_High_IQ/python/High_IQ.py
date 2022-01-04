
def print_instructions():
    print("This is where you will find instructions")
    
def play_game():
    print("Lets play a game")
  
def main():
    if input("Do you want instrunctions?").lower().startswith("y"):
        print_instructions()
    
if __name__ == "__main__":
    main()
