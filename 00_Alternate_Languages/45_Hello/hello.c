#include <stdio.h>
#include <string.h>

#define TRUE 1
#define FALSE 0
#define MAX_INPUT_LENGTH 80

void tab(int number_of_spaces);
void get_input(char *input_buffer);
int strings_match(char *string1, char *string2);

int main() {
   int done = FALSE;
   int paid = FALSE;
   int maybe_more, sure;
   
   char name[MAX_INPUT_LENGTH];
   char reply[MAX_INPUT_LENGTH];
   
   tab(33);
   printf("HELLO\n");
   tab(15);
   printf("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
   printf("\n\n\n");
   printf("HELLO.  MY NAME IS CREATIVE COMPUTER.\n");
   printf("\n\nWHAT'S YOUR NAME ");
   get_input(name);
   printf("\nHI THERE, %s, ARE YOU ENJOYING YOURSELF HERE ", name);

   get_input(reply);
   while (!strings_match(reply, "YES") && !strings_match(reply, "NO")) {
      printf("%s, I DON'T UNDERSTAND YOUR ANSWER OF '%s'.\n", name, reply);
      printf("PLEASE ANSWER 'YES' OR 'NO'. DO YOU LIKE IT HERE ");
      get_input(reply);  
   }
   
   if (strings_match(reply, "YES")) {
      printf("I'M GLAD TO HEAR THAT, %s.\n", name);
   }
   else {
      printf("OH, I'M SORRY TO HEAR THAT, %s. MAYBE WE CAN "
         "BRIGHTEN UP YOUR VISIT A BIT.\n", name);
   }

   printf("\nSAY, %s, I CAN SOLVE ALL KINDS OF PROBLEMS EXCEPT "
      "THOSE DEALING WITH GREECE.  WHAT KIND OF PROBLEMS DO "
      "YOU HAVE (ANSWER SEX, HEALTH, MONEY, OR JOB) ", name);

   while (!done) {
      get_input(reply);
      
      if (strings_match(reply, "JOB")) {
         printf("I CAN SYMPATHIZE WITH YOU %s.  I HAVE TO WORK "
            "VERY LONG HOURS FOR NO PAY -- AND SOME OF MY BOSSES "
            "REALLY BEAT ON MY KEYBOARD.  MY ADVICE TO YOU, %s, IS TO "
            "OPEN A RETAIL COMPUTER STORE.  IT'S GREAT FUN.\n\n", name, name);
      }

      else if (strings_match(reply, "MONEY")) {
         printf("SORRY, %s, I'M BROKE TOO.  WHY DON'T YOU SELL "
            "ENCYCLOPEADIAS OR MARRY SOMEONE RICH OR STOP EATING "
            "SO YOU WON'T NEED SO MUCH MONEY?\n\n", name);
      }
      
      else if (strings_match(reply, "HEALTH")) {
         printf("MY ADVICE TO YOU %s IS:\n", name);
         printf("     1.  TAKE TWO ASPRIN\n");
         printf("     2.  DRINK PLENTY OF FLUIDS (ORANGE JUICE, NOT BEER!)\n");
         printf("     3.  GO TO BED (ALONE)\n\n");
      }
      
      else if (strings_match(reply, "SEX")) {
         printf("IS YOUR PROBLEM TOO MUCH OR TOO LITTLE ");
         
         sure = FALSE;
         while (!sure) {
			 get_input(reply);
			 if (strings_match(reply, "TOO MUCH")) {
				printf("YOU CALL THAT A PROBLEM?!!  I SHOULD HAVE SUCH PROBLEMS!\n");
				printf("IF IT BOTHERS YOU, %s, TAKE A COLD SHOWER.\n\n", name);
				sure = TRUE;
			 }
			 else if (strings_match(reply, "TOO LITTLE")) {
				printf("WHY ARE YOU HERE IN SUFFERN, %s? YOU SHOULD BE "
				   "IN TOKYO OR NEW YORK OR AMSTERDAM OR SOMEPLACE WITH SOME "
				   "REAL ACTION.\n\n", name);
				sure = TRUE;
			 }
			 else {
				printf("DON'T GET ALL SHOOK, %s, JUST ANSWER THE QUESTION "
				"WITH 'TOO MUCH' OR 'TOO LITTLE'. WHICH IS IT ", name);
			 }
		  }
      }
      
      else {  // not one of the prescribed categories
         printf("OH, %s, YOUR ANSWER OF '%s' IS GREEK TO ME.\n\n", name, reply);
      }
      
      printf("ANY MORE PROBLEMS YOU WANT SOLVED, %s ", name);
      
      maybe_more = TRUE;
      while (maybe_more) {
         get_input(reply);
         if (strings_match(reply, "NO")) {
            done = TRUE;
            maybe_more = FALSE;
         }
         else if (strings_match(reply, "YES")) {
            printf("WHAT KIND (SEX, MONEY, HEALTH, JOB) ");
            maybe_more = FALSE;
         }
         else {
            printf("JUST A SIMPLE 'YES' OR 'NO' PLEASE, %s. ", name);
         }
      } // no further questions
   } // end of 'not done' loop
   
   printf("\nTHAT WILL BE $5.00 FOR THE ADVICE, %s.\n", name);
   printf("PLEASE LEAVE THE MONEY ON THE TERMINAL.\n");
   // pause a few seconds
   printf("\n\n\nDID YOU LEAVE THE MONEY ");
   get_input(reply);
   while (!paid) {
      if (strings_match(reply, "YES")) {
         printf("HEY, %s??? YOU LEFT NO MONEY AT ALL!\n", name);
         printf("YOU ARE CHEATING ME OUT OF MY HARD-EARNED LIVING.\n");
         printf("\nWHAT A RIP OFF, %s!!!\n", name);
         printf("TAKE A WALK, %s.\n\n", name);
         paid = TRUE;
      }
      else if (strings_match(reply, "NO")) {
         printf("THAT'S HONEST, %s, BUT HOW DO YOU EXPECT "
            "ME TO GO ON WITH MY PSYCHOLOGY STUDIES IF MY PATIENTS "
            "DON'T PAY THEIR BILLS?\n\n", name);
         printf("NICE MEETING YOU, %s, HAVE A NICE DAY.\n", name);
            paid = TRUE;
      }
      else {
         printf("YOUR ANSWER OF '%s' CONFUSES ME, %s.\n", reply, name);
         printf("PLEASE RESPOND WITH 'YES' OR 'NO'.\n");
      }
   }
}


void tab(int number_of_spaces) {
   for (int i=0; i < number_of_spaces; i++)
      putchar(' ');
}


void get_input(char *input_buffer) {
   fgets(input_buffer, MAX_INPUT_LENGTH - 1, stdin);
   input_buffer[strcspn(input_buffer, "\n")] = '\0';    // trim the trailing line break
}


int strings_match(char *string1, char *string2) {
	if (strncmp(string1, string2, MAX_INPUT_LENGTH - 1) != 0)
	   return FALSE;
	else // strings do not match within maximum input line length
	   return TRUE;
}
