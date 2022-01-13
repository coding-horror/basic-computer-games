/*
 * C "port" of TOWER, a.k.a. 90_Tower.
 *
 * Build using
 * $ cc -lreadline -o tower tower.c
 *
 * NOTE that this code numbers the needles 0-2 internally, though the
 * user interface still calls them 1-3.
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
#include <ctype.h>
#include <readline/readline.h>
#include <readline/history.h>

#define MAX_DISKS 7

#ifdef DEBUG
#define DEBUG_MSG(msg,...) \
    fprintf( stderr, "Debug - " msg " at %s line %d\n", ##__VA_ARGS__, \
        __FILE__, __LINE__ )
#else
#define DEBUG_MSG(msg,...)
#endif


#define _STR(s) #s
#define STR(s) _STR(s)

/*
 * Use the readline package to get a line of input. The returned char *
 * pointer MUST BE FREED AFTER USE. On end of file, we exit. The
 * arguments are:
 * 
 * 1) The prompt
 * 2) A callback to validate the input. This takes the input as its
 *    argument and returns a true value if the input is valid, and a
 *    false value if it is not.
 * 3) The maximum number of retries. If this is exceeded, we exit.
 * 4) The invalid-argument message, which must end in "\n".
 * 5) The retry-exceeded message, which must end in "\n".
 */
char *get_input(
    const char *prompt,
    int (*check) ( char * ),
    int tries,
    const char *warning,
    const char *fatal
    ) {
    while ( 1 ) {
    char *line = readline( prompt );
    if ( ! line ) {
        printf( "\n" );
        exit( 0 );
    }
    if ( check( line ) ) {
        return line;
    }
    free( line );
    if ( ! --tries ) {
        fprintf( stderr, "%s", fatal );
        exit( 1 );
    } else {
        fprintf( stderr, "%s", warning );
    }
    }
}

/*
 * get_* callback function to see if the input is an unsigned integer.
 */
int check_unsigned_integer( char * line ) {
    size_t len = strlen( line );
    if ( ! len ) {
    return 0;
    }
    for ( int inx = 0; inx < len; inx++ ) {
    if ( !  isdigit( line[0] ) ) {
        return 0;
    }
    }
    return 1;
}

/*
 * Wrap the get_input function to get an integer. The arguments are the
 * same as get_input. The callback is expected to do all validation.
 * The return is the integer.
 */
int get_integer(
    const char *prompt,
    int (*check) ( char * ),
    int tries,
    const char *warning,
    const char *fatal
    ) {
    char *line = get_input( prompt, check, tries, warning, fatal );
    int rslt = atoi( line );
    free( line );
    return rslt;
}

/*
 * get_* callback to see if the input begins with 'y' or 'n', either
 * case.
 */
int check_yes_no( char *line ) {
    return line[0] == 'Y' || line[0] == 'y' || line[0] == 'N' || line[0] == 'n';
}

/*
 * Wrap the get_input function to get a yes-or-no answer.
 * The sole argument is the prompt.
 * The return is true for yes and false for no.
 */
int get_yes_no ( const char *prompt ) {
    char *line = get_input(
        prompt,
        check_yes_no,
        INT_MAX,
        "Please respont 'y' or 'n'\n",
        NULL
        );
    int rslt = line[0] == 'Y' || line[0] == 'y';
    free( line );
    return rslt;
}

/*
 * get_* callback to validate the number of disks to use this iteration.
 */
int check_input_disks( char * line ) {
    if ( check_unsigned_integer( line ) ) {
    int val = atoi( line );
    return val > 0 && val <= MAX_DISKS;
    } else {
    return 0;
    }
}

/*
 * Global stuff. Yukk.
 */
int legal_disks[ MAX_DISKS * 2 + 2 ];   /* Legal disk numbers. */
int board[3][MAX_DISKS + 1];        /* The board */
int top_pos[3];             /* Stack pointers for needles */

/*
 * get_* callback to validate a disk number. This does NOT check whether
 * it is actually available to move.
 */
int check_disk_number( char *line ) {
    if ( check_unsigned_integer( line ) ) {
    int val = atoi( line );
    DEBUG_MSG( "disk num is %d; max is %d", val, MAX_DISKS * 2 + 1 );
    return val <= MAX_DISKS * 2 + 1 && legal_disks[val];
    } else {
    return 0;
    }
}

/*
 * get_* callback to validate a needle number.
 */
int check_needle_number( char *line ) {
    if ( check_unsigned_integer( line ) ) {
    int val = atoi( line );
    return val && val <= 3;
    } else {
    return 0;
    }
}

/*
 * Find the given disk, and return the needle it is on. If the disk is
 * not at the top of its needle, return -1.
 */
int find_disk( int disk ) {
    for ( int needle = 0; needle < 3; needle++ ) {
    if ( top_pos[needle] && disk == board[needle][top_pos[needle] - 1 ] ) {
        return needle;
    }
    }

    /* Since we assume the disk number is valid but we did not find it,
     * it must not be the topmost disk. */
    printf( "That disk is below another one.  Make another choice.\n" );

    return -1;  /* Needles are 0-2, so -1 is convenient out-of-band value */
}

/*
 * Fill the given buffer. Nothing is returned. The arguments are:
 *
 * 1) A pointer to the buffer
 * 2) The size of the buffer
 * 3) The character to fill.
 */
void fill_buffer( char *buffer, size_t size, char fill ) {
    for ( int inx = 0; inx < size; inx++ ) {
    buffer[inx] = fill;
    }
}
#define FILL_BUFFER(x,y) fill_buffer( x, sizeof x / sizeof x[0], y )
#define CLEAR_BUFFER(x) FILL_BUFFER( x, ' ' )

/*
 * Print the given buffer after changing all trailing spaces to nulls.
 * The arguments are:
 *
 * 1) A pointer to the buffer
 * 2) The size of the buffer
 */
void print_buffer( char *buffer, size_t size ) {
    for ( int inx = size; inx > 0; --inx ) {
    if ( isspace( buffer[inx] ) ) {
        buffer[inx] = '\0';
    } else if ( buffer[inx] ) {
        break;
    }
    }
    printf( "%s\n", buffer );
}
#define PRINT_BUFFER(x) print_buffer( x, sizeof x / sizeof x[0] )

#define DISPLAY_COL_MARGIN ( 6 )
#define DISPLAY_COL_SIZE ( MAX_DISKS * 2 + DISPLAY_COL_MARGIN )
#define DISPLAY_COL_NEEDLE ( MAX_DISKS + DISPLAY_COL_MARGIN )
#define DISPLAY_LINE_SIZE ( DISPLAY_COL_SIZE * 3 + 1 )

/*
 * Display the board.
 */
void display() {
    printf( "\n" );
    char line_buf[ DISPLAY_LINE_SIZE ];
    for ( int line_inx = MAX_DISKS; line_inx >= 0; --line_inx ) {
    CLEAR_BUFFER( line_buf );
    for ( int needle = 0; needle < 3; needle++ ) {
        int col_start = needle * DISPLAY_COL_SIZE;
        line_buf[ col_start + DISPLAY_COL_NEEDLE ] = '|';
        if ( board[needle][line_inx] ) {
        int disk_inx = board[needle][line_inx];
        int half_wid = ( disk_inx - 1 ) / 2;
        int num_loc = col_start + DISPLAY_COL_NEEDLE - 3 - half_wid;
        snprintf( line_buf + num_loc, 3, "%2d", disk_inx );
        line_buf[ num_loc + 2 ] = ' ';  /* Space over trailing null */
        for (
            int inx = col_start + DISPLAY_COL_NEEDLE - half_wid;
            inx < col_start + DISPLAY_COL_NEEDLE;
            inx++) {
            line_buf[inx] = '*';
        }
        for ( int inx = col_start + DISPLAY_COL_NEEDLE + 1;
            inx < col_start + DISPLAY_COL_NEEDLE + half_wid + 1;
            inx++ ) {
            line_buf[inx] = '*';
        }
        }
    }
    PRINT_BUFFER( line_buf );
    }
    CLEAR_BUFFER( line_buf );
    for ( int needle = 0; needle < 3; needle++ ) {
    int loc = needle * DISPLAY_COL_SIZE + DISPLAY_COL_NEEDLE;
    snprintf( line_buf + loc, 2, "%d", needle + 1 );
    line_buf[loc + 1 ] = ' ';
    }
    PRINT_BUFFER( line_buf );
    printf( "\n" );
}

/*
 * The mainline, at last!
 */
int main ( int argc, char **argv ) {

    printf( "%s",
        "                                 TOWERS\n"
        "               Creative Computing  Morristown, New Jersey\n"
        "\n"
        "\n"
    );

    while ( 1 ) {   /* Iterate indefinitely */
    printf( "%s",
        "Towers of Hanoi Puzzle.\n"
        "\n"
        "You must transfer the disks from the left to the right\n"
        "Tower, one at a time, never putting a larger disk on a\n"
        "smaller disk.\n"
        "\n"
    );

    int size = get_integer(
        "How many disks do you want to move (" STR(MAX_DISKS)
            " is max)? ",
        check_input_disks,
        3,
        "Sorry, but I can't do that job for you.\n",
        "All right, wise guy, if you can't play the game right, I'll\n"
        "just take my puzzle and go home.  So long.\n"
    );

    printf(
        "\n"
        "In this program, we shall refer to disks by numerical code.\n"
        "3 will represent the smallest disk, 5 the next size,\n"
        "7 the next, and so on, up to %d.  If you do the puzzle with\n"
        "2 disks, their code names would be %d and %d.  With 3 disks\n"
        "the code names would be %d, %d and %d, etc.  The needles\n"
        "are numbered from left to right, 1 to 3.  We will\n"
        "start with the disks on needle 1, and attempt to move them\n"
        "to needle 3.\n"
        "\n"
        "\n"
        "Good luck!\n",
        MAX_DISKS * 2 + 1,
        MAX_DISKS * 2 - 1,
        MAX_DISKS * 2 + 1,
        MAX_DISKS * 2 - 3,
        MAX_DISKS * 2 - 1,
        MAX_DISKS * 2 + 1
    );

    /* Initialize the legal disks array. Legal disk numbers get a
     * true value, and illegal ones get a false value. */
    for ( int inx = 0; inx < MAX_DISKS * 2 + 2; inx++ ) {
        legal_disks[inx] = ( ( inx % 2 ) &&
            inx > ( MAX_DISKS - size ) * 2 + 1 ) ? 1 : 0;
        DEBUG_MSG( "legal_diaks[%d] is %d", inx, legal_disks[inx] );
    }

    /* Clear the board. */
    for ( int needle = 0; needle < 3; needle++ ) {
        top_pos[needle] = 0;
        for ( int inx = 0; inx <= MAX_DISKS; inx++ ) {
        board[needle][inx] = 0;
        }
    }

    /* Stick the required number of disks on needle 0 */
    for ( int inx = 0; inx < size; inx++ ) {
        board[0][inx] = ( MAX_DISKS - inx ) * 2 + 1;
    }
    top_pos[0] = size;

    display();

    int moves = 0;

    while ( 1 ) {   /* Iterate indefinitely */
        int disk = get_integer(
            "Which disk would you like to move? ",
            check_disk_number,
            3,
            "Illegal entry.\n",
            "Stop wasting my time.  Go bother someone else.\n"
        );

        int from = find_disk( disk );
        if ( from < 0 ) {
        continue;
        }

        int to = get_integer(
            "Place disk on which needle? ",
            check_needle_number,
            2,
            "I'll assume you hit the wrong key this time.  But watch it,\n"
            "I only allow one mistake.\n",
            "I tried to warn you, but you wouldn't listen.\n"
            "Bye bye, big shot.\n"
        ) - 1;

        /* Validate the needle being moved to */
        int to_slot = top_pos[to];
        if ( to_slot > 0 && board[to][to_slot - 1] < disk ) {
        printf( "%s",
            "You can't place a larger disk on top of a smaller one,\n"
            "It might crush it!\n"
            );
        continue;
        }

        /* Place the disk on its new needle */
        board[to][to_slot] = disk;
        top_pos[to]++;

        /* ... and remove it from its old needle. */
        board[from][--top_pos[from]] = 0;

        moves++;

        display();

        /* Check for game over. */
        if ( top_pos[2] == size ) {
        printf(
            "Congratulations!\n"
            "\n"
            "You have performed the task in %d moves.\n"
            "\n",
            moves
            );
        break;
        } else if ( moves >= 2 << MAX_DISKS ) {
        printf(
            "Sorry, but I have orders to stop if you make more than\n"
            "%d moves.\n",
            moves
            );
        break;
        }

    }

    printf( "\n" );

    if ( ! get_yes_no( "Try again? [y/n]: " ) ) {
        break;
    }
    }


    exit( 0 );
}

/*
 *
 * PORTER
 *
 * Thomas R. Wyant, III F<wyant at cpan dot org>
 *
 * COPYRIGHT AND LICENSE
 *
 * Copyright (C) 2022 by Thomas R. Wyant, III
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the same terms as GNU GPL 3.0, or at the user's option, any
 * later version of the GNU GPL. For more information see
 * https://www.gnu.org/licenses/gpl-3.0.html
 *
 * This program is distributed in the hope that it will be useful, but
 * without any warranty; without even the implied warranty of
 * merchantability or fitness for a particular purpose.
 *
 * ex: set filetype=cpp expandtab tabstop=4 textwidth=72 autoindent :
 */
