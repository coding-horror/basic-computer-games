#include <iostream>
#include <time.h>
#include "Aceyducey.h"


int main()
{
    srand((unsigned int)time(NULL));
    bool isPlaying(true);
    Money = 100;
    WelcomeMessage();
    while (isPlaying)
    {
        Play(isPlaying);
    }
    printf("O.K., HOPE YOU HAD FUN!\n");
}

void WelcomeMessage()
{
    for (int i = 0; i < 25; i++)
    {
        printf(" ");
    }
    printf("ACEY DUCEY CARD GAME\n");
    for (int i = 0; i < 14; i++)
    {
        printf(" ");
    }
    printf("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\nACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER \n");
    printf("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP\nYOU HAVE AN OPTION TO BET OR NOT BET DEPENDING\n");
    printf("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE\nA VALUE BETWEEN THE FIRST TWO.\n");
    printf("IF YOU DO NOT WANT TO BET, INPUT A 0\n");
}

void Play(bool& isPlaying)
{
    short int DealerCards[2];
    int Bet;
    short int CurrentCard;
    printf("YOU NOW HAVE %d DOLLARS.\n\n", Money);
    printf("HERE ARE YOUR NEXT TWO CARDS: \n");
    DrawCard(DealerCards[0]);
    printf("\n");
    DrawCard(DealerCards[1]);
    printf("\n\n\n");
    do {
        printf("WHAT IS YOUR BET: ");
        std::cin >> Bet;
        if (Bet == 0)
        {
            printf("CHICKEN!!\n\n");
        }
    } while (Bet > Money || Bet < 0);
    DrawCard(CurrentCard);
    printf("\n");
    if (CurrentCard > DealerCards[0] && CurrentCard < DealerCards[1])
    {
        printf("YOU WIN!!!\n");
        Money += Bet;
        return;
    }
    else
    {
        printf("SORRY, YOU LOSE\n");
        Money -= Bet;
    }
    if (isGameOver())
    {
        printf("TRY AGAIN (YES OR NO)\n\n");
        std::string response;
        std::cin >> response;
        if (response != "YES")
        {
            isPlaying = false;
        }
        Money = 100;
    }
}

bool isGameOver()
{
    if (Money <= 0)
    {
        printf("\n\n");
        printf("SORRY, FRIEND, BUT YOU BLEW YOUR WAD.\n\n");
        return true;
    }
    return false;
}

void DrawCard(short int& Card)
{
    short int RandomNum1 = (rand() % 10) + 2;
    short int RandomNum2 = rand() % 3;
    Card = RandomNum1 + RandomNum2;
    switch (Card)
    {
    case 11:
        printf("JACK");
        break;
    case 12:
        printf("QUEEN");
        break;
    case 13:
        printf("KING");
        break;
    case 14:
        printf("ACE");
        break;
    default:
        printf("%d", Card);
    }
}