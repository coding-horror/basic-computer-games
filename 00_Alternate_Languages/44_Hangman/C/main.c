#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#define MAX_WORDS 100

//check if windows or linux for the clear screen
#ifdef _WIN32
#define CLEAR "cls"
#else
#define CLEAR "clear"
#endif

/**
 * @brief Prints the stage of the hangman based on the number of wrong guesses.
 * 
 * @param stage Hangman stage.
 */
void print_hangman(int stage){
    switch (stage){
        case 0:
            printf("----------\n");
            printf("|        |\n");
            printf("|\n");
            printf("|\n");
            printf("|\n");
            printf("|\n");
            break;
        case 1:
            printf("----------\n");
            printf("|        |\n");
            printf("|        O\n");
            printf("|        |\n");
            printf("|\n");
            printf("|\n");
            break;
        case 2:
            printf("----------\n");
            printf("|        |\n");
            printf("|        o\n");
            printf("|       /|\n");
            printf("|\n");
            printf("|\n");
            break;
        case 3:
            printf("----------\n");
            printf("|        |\n");
            printf("|        o\n");
            printf("|       /|\\\n");
            printf("|\n");
            printf("|\n");
            break; 
        case 4:
            printf("----------\n");
            printf("|        |\n");
            printf("|        o\n");
            printf("|       /|\\\n");
            printf("|       /\n");
            printf("|\n");
            break; 
        case 5:
            printf("----------\n");
            printf("|        |\n");
            printf("|        o\n");
            printf("|       /|\\\n");
            printf("|       / \\\n");
            printf("|\n");
            break;         
        default:
            break;
    }
}

/**
 * @brief Picks and return a random word from the dictionary.
 * 
 * @return Random word 
 */
char* random_word_picker(){
    //generate a random english word
    char* word = malloc(sizeof(char) * 100);
    FILE* fp = fopen("dictionary.txt", "r");
    srand(time(NULL));
    if (fp == NULL){
        printf("Error opening dictionary.txt\n");
        exit(1);
    }
    int random_number = rand() % MAX_WORDS;
    for (int j = 0; j < random_number; j++){
        fscanf(fp, "%s", word);
    }
    fclose(fp);
    return word;
}




void main(void){
    char* word = malloc(sizeof(char) * 100);
    word = random_word_picker();
    char* hidden_word = malloc(sizeof(char) * 100);
    for (int i = 0; i < strlen(word); i++){
        hidden_word[i] = '_';
    }
    hidden_word[strlen(word)] = '\0';
    int stage = 0;
    int wrong_guesses = 0;
    int correct_guesses = 0;
    char* guess = malloc(sizeof(char) * 100);
    while (wrong_guesses < 6 && correct_guesses < strlen(word)){
        CLEAR;
        print_hangman(stage);
        printf("%s\n", hidden_word);
        printf("Enter a guess: ");
        scanf("%s", guess);
        for (int i = 0; i < strlen(word); i++){
            if (strcmp(guess,word) == 0){
                correct_guesses = strlen(word);
            }
            else if (guess[0] == word[i]){
                hidden_word[i] = guess[0];
                correct_guesses++;
            }
        }
        if (strchr(word, guess[0]) == NULL){
            wrong_guesses++;
        }
        stage = wrong_guesses;
    }
    if (wrong_guesses == 6){
        printf("You lose! The word was %s\n", word);
    }
    else {
        printf("You win!\n");
    }
}