#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

//check if windows or linux for the clear screen
#ifdef _WIN32
#define CLEAR "cls"
#else
#define CLEAR "clear"
#endif

void show_solution(float guess);
float guess_number(float number);
void game();

float guess_number(float number){
    float guess;
    guess = ((((number - 4) * 5) / 8) * 5 - 3);
    return guess;
}

void game(){
    float number,guess;
    char answer[4];
    printf("Think a number\n");
    printf("Then add to it 3 and divide it by 5\n");
    printf("Now multiply by 8, divide by 5 and then add 5\n");
    printf("Finally substract 1\n");
    printf("What is the number you got?(if you got decimals put them ex: 23.6): ");
    scanf("%f",&number);
    guess = guess_number(number);
    printf("The number you thought was %f am I right(Yes or No)?\n",guess);
    scanf("%s",answer);
    for(int i = 0; i < strlen(answer); i++){
        answer[i] = tolower(answer[i]);
    }
    if(strcmp(answer,"yes") == 0){
        printf("\nHuh, I Knew I was unbeatable");
        printf("And here is how i did it:\n");
        show_solution(guess);
    }
    else if (strcmp(answer,"no") == 0){
        printf("HUH!! what was you original number?: ");
        scanf("%f",&number);
        if(number == guess){
            printf("Huh, I Knew I was unbeatable");
            printf("And here is how i did it:\n");
            show_solution(guess);
        }
        else{
            printf("If I got it wrong I guess you are smarter than me");
        }
    }
    else{
        system(CLEAR);
        printf("I don't understand what you said\n");
        printf("Please answer with Yes or No\n");
        game();
    }

}

void show_solution(float guess){
    printf("%f plus 3 is %f\n",guess,guess + 3);
    printf("%f divided by 5 is %f\n",guess + 3,(guess + 3) / 5);
    printf("%f multiplied by 8 is %f\n",(guess + 3) / 5,(guess + 3) / 5 * 8);
    printf("%f divided by 5 is %f\n",(guess + 3) / 5 * 8,(guess + 3) / 5 * 8 / 5);
    printf("%f plus 5 is %f\n",(guess + 3) / 5 * 8 / 5,(guess + 3) / 5 * 8 / 5 + 5);
    printf("%f minus 1 is %f\n",(guess + 3) / 5 * 8 / 5 + 5,(guess + 3) / 5 * 8 / 5 + 5 - 1);
}

void main(){
    char answer[4];
    printf("I am CHIEF NUMBERS FREEK, The GREAT INDIAN MATH GOD.\n");
    printf("Are you ready to take the test you called me out for(Yes or No)? ");
    scanf("%s",answer);
    for(int i = 0; i < strlen(answer); i++){
        answer[i] = tolower(answer[i]);
    }
    if(strcmp(answer,"yes") == 0){
        game();
    }else if (strcmp(answer,"no") == 0){
        printf("You are a coward, I will not play with you.%d %s\n",strcmp(answer,"yes"),answer);
    }
    else{
        system(CLEAR);
        printf("I don't understand what you said\n");
        printf("Please answer with Yes or No\n");
        main();
    }
}