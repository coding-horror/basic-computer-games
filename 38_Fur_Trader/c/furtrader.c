
/*
 * Ported from furtrader.bas to ANSI C (C99) by krt@krt.com.au
 *
 * compile with:
 *    gcc -g -Wall -Werror furtrader.c -o furtrader
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>


/* Constants */
#define FUR_TYPE_COUNT    4
#define FUR_MINK          0
#define FUR_BEAVER        1
#define FUR_ERMINE        2
#define FUR_FOX           3
#define MAX_FURS        190
const char *FUR_NAMES[FUR_TYPE_COUNT] = { "MINK", "BEAVER", "ERMINE", "FOX" };

#define FORT_TYPE_COUNT 3
#define FORT_MONTREAL   1
#define FORT_QUEBEC     2
#define FORT_NEWYORK    3
const char *FORT_NAMES[FORT_TYPE_COUNT] = { "HOCHELAGA (MONTREAL)", "STADACONA (QUEBEC)", "NEW YORK" };



/* Print the words at the specified column */
void printAtColumn( int column, const char *words )
{
    int i;
    for ( i=0; i<column; i++ )
        printf( " " );
    printf( "%s\n", words );
}

/* trivial function to output a line with a \n */
void print( const char *words )
{
    printf( "%s\n", words );
}

/* Show the player the introductory message */
void showIntroduction()
{
    print( "YOU ARE THE LEADER OF A FRENCH FUR TRADING EXPEDITION IN " );
    print( "1776 LEAVING THE LAKE ONTARIO AREA TO SELL FURS AND GET" );
    print( "SUPPLIES FOR THE NEXT YEAR.  YOU HAVE A CHOICE OF THREE" );
    print( "FORTS AT WHICH YOU MAY TRADE.  THE COST OF SUPPLIES" );
    print( "AND THE AMOUNT YOU RECEIVE FOR YOUR FURS WILL DEPEND" );
    print( "ON THE FORT THAT YOU CHOOSE." );
    print( "" );
}


/*
 * Prompt the user for input.
 * When input is given, try to conver it to an integer
 * return the integer converted value or 0 on error
 */
int getNumericInput()
{
    int  result = -1;
    char buffer[64];   /* somewhere to store user input */
    char *endstr;

    while ( result == -1 )
    {
        printf( ">> " );                                 /* prompt the user */
        fgets( buffer, sizeof( buffer ), stdin );        /* read from the console into the buffer */
        result = (int)strtol( buffer, &endstr, 10 );     /* only simple error checking */

        if ( endstr == buffer )                          /* was the string -> integer ok? */
            result = -1;
    }

    return result;
}


/*
 * Prompt the user for YES/NO input.
 * When input is given, try to work out if it's YES, Yes, yes, Y, etc.
 * And convert to a single upper-case letter
 * Returns a character of 'Y' or 'N'.
 */
char getYesOrNo()
{
    char result = '!';
    char buffer[64];   /* somewhere to store user input */

    while ( !( result == 'Y' || result == 'N' ) )       /* While the answer was not Yes or No */
    {
        print( "ANSWER YES OR NO" );
        printf( ">> " );

        fgets( buffer, sizeof( buffer ), stdin );            /* read from the console into the buffer */
        if ( buffer[0] == 'Y' || buffer[0] == 'y' )
            result = 'Y';
        else if ( buffer[0] == 'N' || buffer[0] == 'n' )
            result = 'N';
    }

    return result;
}



/* 
 * Show the player the choices of Fort, get their input, if the
 * input is a valid choice (1,2,3) return it, otherwise keep
 * prompting the user. 
 */
int getFortChoice()
{
    int result = 0;

    while ( result == 0 )
    {
        print( "" );
        print( "YOU MAY TRADE YOUR FURS AT FORT 1, FORT 2," );
        print( "OR FORT 3.  FORT 1 IS FORT HOCHELAGA (MONTREAL)" );
        print( "AND IS UNDER THE PROTECTION OF THE FRENCH ARMY." );
        print( "FORT 2 IS FORT STADACONA (QUEBEC) AND IS UNDER THE" );
        print( "PROTECTION OF THE FRENCH ARMY.  HOWEVER, YOU MUST" );
        print( "MAKE A PORTAGE AND CROSS THE LACHINE RAPIDS." );
        print( "FORT 3 IS FORT NEW YORK AND IS UNDER DUTCH CONTROL." );
        print( "YOU MUST CROSS THROUGH IROQUOIS LAND." );
        print( "ANSWER 1, 2, OR 3." );

        result = getNumericInput();   /* get input from the player */
    }

    return result;
}


/*
 * Print the description for the fort 
 */
void showFortComment( int which_fort )
{
    print( "" );
    if ( which_fort == FORT_MONTREAL )
    {
        print( "YOU HAVE CHOSEN THE EASIEST ROUTE.  HOWEVER, THE FORT" );
        print( "IS FAR FROM ANY SEAPORT.  THE VALUE" );
        print( "YOU RECEIVE FOR YOUR FURS WILL BE LOW AND THE COST" );
        print( "OF SUPPLIES HIGHER THAN AT FORTS STADACONA OR NEW YORK." );
    }
    else if ( which_fort == FORT_QUEBEC )
    {
        print( "YOU HAVE CHOSEN A HARD ROUTE.  IT IS, IN COMPARSION," );
        print( "HARDER THAN THE ROUTE TO HOCHELAGA BUT EASIER THAN" );
        print( "THE ROUTE TO NEW YORK.  YOU WILL RECEIVE AN AVERAGE VALUE" );
        print( "FOR YOUR FURS AND THE COST OF YOUR SUPPLIES WILL BE AVERAGE." );
    }
    else if ( which_fort == FORT_NEWYORK )
    {
        print( "YOU HAVE CHOSEN THE MOST DIFFICULT ROUTE.  AT" );
        print( "FORT NEW YORK YOU WILL RECEIVE THE HIGHEST VALUE" );
        print( "FOR YOUR FURS.  THE COST OF YOUR SUPPLIES" );
        print( "WILL BE LOWER THAN AT ALL THE OTHER FORTS." );
    }
    else
    {
        printf( "Internal error #1, fort %d does not exist\n", which_fort );
        exit( 1 );  /* you have a bug */
    }
    print( "" );
}        


/*    
 * Prompt the player for how many of each fur type they want.
 * Accept numeric inputs, re-prompting on incorrect input values
 */
void getFursPurchase( int *furs )
{
    int i;

    printf( "YOUR %d FURS ARE DISTRIBUTED AMONG THE FOLLOWING\n", FUR_TYPE_COUNT );
    print( "KINDS OF PELTS: MINK, BEAVER, ERMINE AND FOX." );
    print( "" );

    for ( i=0; i<FUR_TYPE_COUNT; i++ )
    {
        printf( "HOW MANY %s DO YOU HAVE\n", FUR_NAMES[i] );
        furs[i] = getNumericInput();
    }
}
        

/*
 * (Re)Set the player's inventory to zero
 */
void zeroInventory( int *player_fur_count )
{
    int i;
    for ( i=0; i<FUR_TYPE_COUNT; i++ )
    {
        player_fur_count[i] = 0;
    }
}


/*
 * Tally the player's inventory 
 */
int sumInventory( int *player_fur_count )
{
    int result = 0;
    int i;
    for ( i=0; i<FUR_TYPE_COUNT; i++ )
    {
        result += player_fur_count[i];
    }

    return result;
}


/*
 * Return a random number between a & b 
 * Ref: https://stackoverflow.com/a/686376/1730895
 */
float randomAB(float a, float b)
{
    return ((b - a) * ((float)rand() / (float)RAND_MAX)) + a;
}
/* Random floating point number between 0 and 1 */
float randFloat()
{
    return randomAB( 0, 1 );
}


/* States to allow switching in main game-loop */
#define STATE_STARTING      1
#define STATE_CHOOSING_FORT 2
#define STATE_TRAVELLING    3
#define STATE_TRADING       4

int main( void )
{
    /* variables for storing player's status */
    float player_funds = 0;                              /* no money */
    int   player_furs[FUR_TYPE_COUNT]  = { 0, 0, 0, 0 }; /* no furs */

    /* player input holders */
    char  yes_or_no;
    int   event_picker;
    int   which_fort;
    
    /* what part of the game is in play */
    int   game_state = STATE_STARTING;

    /* commodity prices */
    float mink_price   = -1;
    float beaver_price = -1;
    float ermine_price = -1;
    float fox_price    = -1;  /* sometimes this takes the "last" price (probably this was a bug) */

    float mink_value;
    float beaver_value;
    float ermine_value;
    float fox_value;      /* for calculating sales results */


    srand( time( NULL ) );  /* seed the random number generator */

    printAtColumn( 31, "FUR TRADER" );
    printAtColumn( 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY" );
    printAtColumn( 15, "(Ported to ANSI-C Oct 2012 krt@krt.com.au)" );
    print( "\n\n\n" );

    /* Loop forever until the player asks to quit */
    while ( 1 )
    {
        if ( game_state == STATE_STARTING )
        {
            showIntroduction();

            player_funds = 600;            /* Initial player start money */
            zeroInventory( player_furs );  /* Player fur inventory */

            print( "DO YOU WISH TO TRADE FURS?" );
            yes_or_no = getYesOrNo();
            if ( yes_or_no == 'N' )
                exit( 0 );                 /* STOP */
            game_state = STATE_TRADING;
        }
        
        else if ( game_state == STATE_TRADING )
        {
            print( "" );
            printf( "YOU HAVE $ %1.2f IN SAVINGS\n", player_funds );
            printf( "AND %d FURS TO BEGIN THE EXPEDITION\n", MAX_FURS ); 
            getFursPurchase( player_furs );

            if ( sumInventory( player_furs ) > MAX_FURS )
            {
                print( "" );
                print( "YOU MAY NOT HAVE THAT MANY FURS." );
                print( "DO NOT TRY TO CHEAT.  I CAN ADD." );
                print( "YOU MUST START AGAIN." );
                print( "" );
                game_state = STATE_STARTING;   /* T/N: Wow, harsh. */
            }
            else
            {
                game_state = STATE_CHOOSING_FORT;
            }
        }

        else if ( game_state == STATE_CHOOSING_FORT )
        {
            which_fort = getFortChoice();
            showFortComment( which_fort );
            print( "DO YOU WANT TO TRADE AT ANOTHER FORT?" );
            yes_or_no = getYesOrNo();
            if ( yes_or_no == 'N' )
                game_state = STATE_TRAVELLING;
        }

        else if ( game_state == STATE_TRAVELLING )
        {
            print( "" );
            if ( which_fort == FORT_MONTREAL )
            {
                mink_price   = ( ( 0.2 * randFloat() + 0.70 ) * 100 + 0.5 ) / 100;
                ermine_price = ( ( 0.2 * randFloat() + 0.65 ) * 100 + 0.5 ) / 100;
                beaver_price = ( ( 0.2 * randFloat() + 0.75 ) * 100 + 0.5 ) / 100;
                fox_price    = ( ( 0.2 * randFloat() + 0.80 ) * 100 + 0.5 ) / 100;

                print( "SUPPLIES AT FORT HOCHELAGA COST $150.00." );
                print( "YOUR TRAVEL EXPENSES TO HOCHELAGA WERE $10.00." );
                player_funds -= 160;
            }

            else if ( which_fort == FORT_QUEBEC )
            {
                mink_price   = ( ( 0.30 * randFloat() + 0.85 ) * 100 + 0.5 ) / 100;
                ermine_price = ( ( 0.15 * randFloat() + 0.80 ) * 100 + 0.5 ) / 100;
                beaver_price = ( ( 0.20 * randFloat() + 0.90 ) * 100 + 0.5 ) / 100;
                fox_price    = ( ( 0.25 * randFloat() + 1.10 ) * 100 + 0.5 ) / 100;
                event_picker = ( 10 * randFloat() ) + 1;

                if ( event_picker <= 2 )
                {
                    print( "YOUR BEAVER WERE TOO HEAVY TO CARRY ACROSS" );
                    print( "THE PORTAGE.  YOU HAD TO LEAVE THE PELTS, BUT FOUND" );
                    print( "THEM STOLEN WHEN YOU RETURNED." );
                    player_furs[ FUR_BEAVER ] = 0;
                }
                else if ( event_picker <= 6 )
                {
                    print( "YOU ARRIVED SAFELY AT FORT STADACONA." );
                }
                else if ( event_picker <= 8 )
                {
                    print( "YOUR CANOE UPSET IN THE LACHINE RAPIDS.  YOU" );
                    print( "LOST ALL YOUR FURS." );
                    zeroInventory( player_furs );
                }
                else if ( event_picker <= 10 )
                {
                    print( "YOUR FOX PELTS WERE NOT CURED PROPERLY." );
                    print( "NO ONE WILL BUY THEM." );
                    player_furs[ FUR_FOX ] = 0;
                }
                else
                {
                    printf( "Internal Error #3, Out-of-bounds event_picker %d\n", event_picker );
                    exit( 1 );  /* you have a bug */
                }

                print( "" );
                print( "SUPPLIES AT FORT STADACONA COST $125.00." );
                print( "YOUR TRAVEL EXPENSES TO STADACONA WERE $15.00." );
                player_funds -= 140;
            }

            else if ( which_fort == FORT_NEWYORK )
            {
                mink_price   = ( ( 0.15 * randFloat() + 1.05 ) * 100 + 0.5 ) / 100;
                ermine_price = ( ( 0.15 * randFloat() + 0.95 ) * 100 + 0.5 ) / 100;
                beaver_price = ( ( 0.25 * randFloat() + 1.00 ) * 100 + 0.5 ) / 100;
                if ( fox_price < 0 )
                {
                    /* Original Bug?  There is no Fox price generated for New York, 
                       it will use any previous "D1" price.
                       So if there was no previous value, make one up */
                    fox_price = ( ( 0.25 * randFloat() + 1.05 ) * 100 + 0.5 ) / 100; /* not in orginal code */
                }
                event_picker = ( 10 * randFloat() ) + 1;

                if ( event_picker <= 2 )
                {
                    print( "YOU WERE ATTACKED BY A PARTY OF IROQUOIS." );
                    print( "ALL PEOPLE IN YOUR TRADING GROUP WERE" );
                    print( "KILLED.  THIS ENDS THE GAME." );
                    exit( 0 );
                }
                else if ( event_picker <= 6 )
                {
                    print( "YOU WERE LUCKY.  YOU ARRIVED SAFELY" );
                    print( "AT FORT NEW YORK." );
                }
                else if ( event_picker <= 8 )
                {
                    print( "YOU NARROWLY ESCAPED AN IROQUOIS RAIDING PARTY." );
                    print( "HOWEVER, YOU HAD TO LEAVE ALL YOUR FURS BEHIND." );
                    zeroInventory( player_furs );
                }
                else if ( event_picker <= 10 )
                {
                    mink_price /= 2;
                    fox_price  /= 2;
                    print( "YOUR MINK AND BEAVER WERE DAMAGED ON YOUR TRIP." );
                    print( "YOU RECEIVE ONLY HALF THE CURRENT PRICE FOR THESE FURS." );
                }
                else
                {
                    print( "Internal Error #4, Out-of-bounds event_picker %d\n" );
                    exit( 1 );  /* you have a bug */
                }

                print( "" );
                print( "SUPPLIES AT NEW YORK COST $85.00." );
                print( "YOUR TRAVEL EXPENSES TO NEW YORK WERE $25.00." );
                player_funds -= 105;
            }

            else
            {
                printf( "Internal error #2, fort %d does not exist\n", which_fort );
                exit( 1 );  /* you have a bug */
            }

            /* Calculate sales */
            beaver_value = beaver_price * player_furs[ FUR_BEAVER ];
            fox_value    = fox_price    * player_furs[ FUR_FOX ];
            ermine_value = ermine_price * player_furs[ FUR_ERMINE ];
            mink_value   = mink_price   * player_furs[ FUR_MINK ];
         
            print( "" );
            printf( "YOUR BEAVER SOLD FOR $%6.2f\n", beaver_value );
            printf( "YOUR FOX SOLD FOR    $%6.2f\n", fox_value );
            printf( "YOUR ERMINE SOLD FOR $%6.2f\n", ermine_value );
            printf( "YOUR MINK SOLD FOR   $%6.2f\n", mink_value );

            player_funds += beaver_value + fox_value + ermine_value + mink_value;

            print( "" );
            printf( "YOU NOW HAVE $ %1.2f INCLUDING YOUR PREVIOUS SAVINGS\n", player_funds );

            print( "" );
            print( "DO YOU WANT TO TRADE FURS NEXT YEAR?" );
            yes_or_no = getYesOrNo();
            if ( yes_or_no == 'N' )
                exit( 0 );             /* STOP */
            else
                game_state = STATE_TRADING;

        }
    }

    return 0; /* exit OK */
}

