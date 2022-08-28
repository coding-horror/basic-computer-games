#include <stdio.h>
#include <stdlib.h>
#include <time.h>

//check if windows or linux for the clear screen
#ifdef _WIN32
#define CLEAR "cls"
#else
#define CLEAR "clear"
#endif

typedef struct{
    int x;
    int y;
    int z;
}coords;

void instuctions(){
    printf("\nThis is a game in which you will be playing against the\n");
    printf("random decisions of the computer. The field of play is a\n");
    printf("cube of side 3. Any of the 27 locations can be designated\n");
    printf("by inputing three numbers such as 2,3,1. At the start,\n");
    printf("you are automatically at location 1,1,1. The object of\n");
    printf("the game is to get to location 3,3,3. One minor detail:\n");
    printf("the computer will pick, at random, 5 locations at which\n");
    printf("it will plant land mines. If you hit one of these locations\n");
    printf("you lose. One other detail: You may move only one space\n");
    printf("in one direction each move. For example: From 1,1,2 you\n");
    printf("may move to 2,1,2 or 1,1,3. You may not change\n");
    printf("two of the numbers on the same move. If you make an illegal\n");
    printf("move, you lose and the computer takes the money you may\n");
    printf("have bet on that round.\n\n");
    printf("When stating the amount of a wager, printf only the number\n");
    printf("of dollars (example: 250) you are automatically started with\n");
    printf("500 dollars in your account.\n\n");
    printf("Good luck!\n");
}

void game(int money){
    coords player,playerold,mines[5];
    int wager,account = money;
    char choice;
    if(money == 0){
        printf("You have no money left. See ya next time.\n");
        exit(0);
    }
    player.x = 1;
    player.y = 1;
    player.z = 1;
    printf("You have $%d in your account.\n",account);
    printf("How much do you want to wager? ");
    scanf("%d",&wager);
    srand(time(NULL));
    for(int i=0;i<5;i++){
        mines[i].x = rand()%3+1;
        mines[i].y = rand()%3+1;
        mines[i].z = rand()%3+1;
        if(mines[i].x == 3 && mines[i].y == 3 && mines[i].z == 3){
            i--;
        }
    }
    while(player.x != 3 || player.y != 3 || player.z != 3){
        printf("You are at location %d.%d.%d\n",player.x,player.y,player.z);
        if(player.x == 1 && player.y == 1 && player.z == 1)
        printf("Enter new location(use commas like 1,1,2 or else the program will break...): ");
        else printf("Enter new location: ");
        playerold.x = player.x;
        playerold.y = player.y;
        playerold.z = player.z;
        scanf("%d,%d,%d",&player.x,&player.y,&player.z);
        if(((player.x + player.y + player.z) > (playerold.x + playerold.y + playerold.z + 1)) || ((player.x + player.y + player.z) < (playerold.x + playerold.y + playerold.z -1))){
            system(CLEAR);
            printf("Illegal move!\n");
            printf("You lose $%d.\n",wager);
            game(account -= wager);
            break;
        }
        if(player.x < 1 || player.x > 3 || player.y < 1 || player.y > 3 || player.z < 1 || player.z > 3){
            system(CLEAR);
            printf("Illegal move. You lose!\n");
            game(account -= wager);
            break;
        }
        for(int i=0;i<5;i++){
            if(player.x == mines[i].x && player.y == mines[i].y && player.z == mines[i].z){
                system(CLEAR);
                printf("You hit a mine!\n");
                printf("You lose $%d.\n",wager);
                game(account -= wager);
                exit(0);
            }
        }
        if(account == 0){
            system(CLEAR);
            printf("You have no money left!\n");
            printf("Game over!\n");
            exit(0);
        }
    }
    if(player.x == 3 && player.y == 3 && player.z == 3){
        system(CLEAR);
        printf("You made it to the end. You win!\n");
        game(account += wager);
        exit(0);
    }
}

void init(){
    int account = 500;
    char choice;

    printf("Welcome to the game of Cube!\n");
    printf("wanna see the instructions? (y/n): ");
    scanf("%c",&choice);
    if(choice == 'y'){
        system(CLEAR);
        instuctions();
    }
    else if (choice == 'n'){
        system(CLEAR);
        printf("Ok, let's play!\n");
    }
    else{
        system(CLEAR);
        printf("Invalid choice. Try again...\n");
        init();
        exit(0);
    }
    game(account);
    exit(0);
}

void main(){
    init();
}