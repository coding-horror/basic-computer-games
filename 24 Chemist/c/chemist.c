#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

void center(const char *s) {
	size_t indent = (72 - strlen(s)) / 2;
	for (size_t i = 0; i < indent; i++) {
		putchar(' ');
	}
	printf("%s\n", s);
}

int main(int argc, char **argv) {
	center("CHEMIST");
	center("Creative Computing  Morristown, New Jersey");
	printf("\n\n\n");

	printf("The fictitious checmical kryptocyanic acid can only be\n");
	printf("diluted by the ratio of 7 parts water to 3 parts acid.\n");
	printf("If any other ratio is attempted, the acid becomes unstable\n");
	printf("and soon explodes.  Given the amount of acid, you must\n");
	printf("decide who much water to add for dilution.  If you miss\n");
	printf("you face the consequences.\n\n");

	size_t t = 0;
	while (1) {
		int a = rand() % 50;
		double w = 7.0 * a / 3.0;
		printf("%d liters of kryptocyanic acid.  How much water? ", a);

		char s[2000];
		if (fgets(s, 2000, stdin) == NULL) {
			break;
		}

		double r = atof(s);
		double d = fabs(w - r);
		if (d > w / 20.0) {
			printf(" Sizzle!  You have just been desalinated into a blob\n");
			printf(" of quivering protoplasm!\n");

			t++;
			if (t < 9) {
				printf(" However, you may try again with another life.\n\n");
			} else {
				printf(" Your 9 lives are used, but you will be long remembered for\n");
				printf(" your contributions to the field of comic book chemistry.\n");
				exit(EXIT_FAILURE);
			}
		} else {
			printf(" Good job! You may breathe now, but don't inhale the fumes!\n\n");
		}
	}

	return EXIT_SUCCESS;
}
