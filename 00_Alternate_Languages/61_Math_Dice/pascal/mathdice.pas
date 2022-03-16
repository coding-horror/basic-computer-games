(*
 * Ported from mathdice.bas to Pascal by krt@krt.com.au
 *
 * Compile with Free Pascal (https://www.freepascal.org/) ~
 *    fpc mathdice.pas
 *)

program MathDice;


procedure printDice( face_value: integer );
(* Prints a box with spots representing a die face for the user *)
begin
    writeln( ' ----- ' );

    if ( face_value = 1 ) then
        writeln( 'I     I' )
    else if ( ( face_value = 2 ) or ( face_value = 3 ) ) then
        writeln( 'I *   I' )
    else
        writeln( 'I * * I' );

    if ( ( face_value = 2 ) or ( face_value = 4 ) ) then
        writeln( 'I     I' )
    else if ( face_value = 6 ) then
        writeln( 'I * * I' )
    else
        writeln( 'I  *  I' );

    if ( face_value = 1 ) then
        writeln( 'I     I' )
    else if ( ( face_value = 2 ) or ( face_value = 3 ) ) then
        writeln( 'I   * I' )
    else
        writeln( 'I * * I' );

    writeln( ' ----- ' );
end;


procedure writeAtColumn( width: integer; words: string );
(* Prints <width> worth of spaces before the <words> to justify the text *)
var
    i: integer;
begin
    for i := 1 to width do
        write( ' ' );
    writeln( words );
end;


function inputNumber(): integer;
(* Get a number from the player with error checking.
   If they type a non-number, ask them again *)
var
    player_input:  string;    (* The string entered by the player *)
    player_answer: integer;   (* The converted value of the text *)
    input_error:   integer;   (* The letter's column that caused an error *)
begin

    input_error := 1;
    while ( input_error <> 0 ) do
    begin
        readln( player_input );

        val( player_input, player_answer, input_error );

        if ( input_error <> 0 ) then
            write( 'Please input a number: ' );
    end;

    inputNumber := player_answer;
end;



var
    dice1:         integer;   (* die 1 face value *)
    dice2:         integer;   (* die 2 face value *)
    answer:        integer;   (* the sum of the dice *)
    player_answer: integer;   (* The value entered by the player *)
begin
    writeAtColumn( 31, 'MATH DICE' );
    writeAtColumn( 15, 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY' );
    writeAtColumn( 15, '(Ported to Pascal Oct 2012 krt@krt.com.au)' );
    writeln( '' );
    writeln( '' );
    writeln( '' );

    writeln( 'THIS PROGRAM GENERATES SUCCESSIVE PICTURES OF TWO DICE.' );
    writeln( 'WHEN TWO DICE AND AN EQUAL SIGN FOLLOWED BY A QUESTION' );
    writeln( 'MARK HAVE BEEN PRINTED, TYPE YOUR ANSWER AND THE RETURN KEY.' );
    writeln( 'TO CONCLUDE THE LESSON, TYPE CONTROL-C AS YOUR ANSWER.' );
    writeln( '' );
    writeln( '' );

    while ( true ) do
    begin
        dice1 := Random( 6 ) + 1;   (* Random number between 1 and 6 (including)  *)
        dice2 := Random( 6 ) + 1;   (* Random number between 1 and 6 (including)  *)
        answer := dice1 + dice2;

        (* Show the player two dice faces *)
        printDice( dice1 );
        writeln( '   +' );
        printDice( dice2 );

        write( '      = ' );

        player_answer := inputNumber();

        if ( player_answer <> answer ) then
        begin
            (* Give the player a second chance at the answer... *)
            writeln( 'NO, COUNT THE SPOTS AND GIVE ANOTHER ANSWER.' );
            write( '      = ' );
            player_answer := inputNumber();
        end;

        if ( player_answer <> answer ) then
            writeln( 'NO, THE ANSWER IS ', answer )
        else
            writeln( 'RIGHT!' );

        writeln( '' );
        writeln( 'THE DICE ROLL AGAIN...' );
    end;
end.
