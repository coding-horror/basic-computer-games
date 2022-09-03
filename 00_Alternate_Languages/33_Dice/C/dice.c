#include <stdio.h>
#include <stdlib.h>
#include <time.h>

float percent(int number, int total){
    float percent;
    percent = (float)number / (float)total * 100;
    return percent;
}

int main(){
    int dice1,dice2,times,rolls[13] = {0};
    srand(time(NULL));
    printf("This program simulates the rolling of a pair of dice\n");
    printf("How many times do you want to roll the dice?(Higher the number longer the waiting time): ");
    scanf("%d",&times);
    for(int i = 0; i < times; i++){
        dice1 = rand() % 6 + 1;
        dice2 = rand() % 6 + 1;
        rolls[dice1 + dice2]+=1;
    }
    printf("The number of times each sum was rolled is:\n");
    for(int i = 2; i <= 12; i++){
        printf("%d: rolled %d times, or %f%c of the times\n",i,rolls[i],percent(rolls[i],times),(char)37);
    }
}