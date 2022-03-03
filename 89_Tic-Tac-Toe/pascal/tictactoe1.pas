program tictactoe1;
var
    a, b, c, d, e: integer;
	p, q, r, s: integer;
procedure computerMoves(m: integer);
begin
	write('COMPUTER MOVES ');
	writeln(m);
end;

function readYourMove() : integer;
var number: integer;
begin
    write('YOUR MOVE?');
	readln(number);
	readYourMove := number;
end;

function move(number: integer): integer;
begin
	move := number - 8 * trunc((number - 1) / 8);
end;

function padLeft(m: string; n: integer): string;
var tmp: string;
begin
    tmp := '';
    repeat
        tmp := tmp + ' ';
        n := n - 1;
    until n = 0;
    tmp := tmp + m;
    padLeft := tmp;
end;

begin
    writeln(padLeft('TIC TAC TOE', 30));
    writeln(padLeft('CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY', 15));
    writeln('');
    writeln('');
    writeln('');
    writeln('THE GAME BOARD IS NUMBERED:');
    writeln('');
    writeln('1  2  3');
    writeln('8  9  4');
    writeln('7  6  5');
    while(true) do
    begin
        writeln('');
        writeln('');
        a := 9;
        computerMoves(a);
        p := readYourMove();
    	b := move(p + 1);
    	computerMoves(b);
    	q := readYourMove();
    	if (q = move(b + 4)) then
    	begin
    		c := move(b + 2);
    		computerMoves(c);
    		r := readYourMove();
    		if (r = move(c + 4)) then
    		begin
    			if (p mod 2 <> 0) then
    			begin
    				d := move(c + 3);
    				computerMoves(d);
    				s := readYourMove();
    				if (s = move(d + 4)) then
    				begin
    					e := move(d + 6);
    					computerMoves(e);
    					writeln('THE GAME IS A DRAW.');
    				end
    				else
    				begin
    					e := move(d + 4);
    					computerMoves(e);
    		            writeln('AND WINS ********');
    				end
    			end
    			else
    			begin
    				d := move(c + 7);
    				computerMoves(d);
    		        writeln('AND WINS ********');
    			end
			end
    		else 
    		begin
    			d := move(c + 4);
    			computerMoves(d);
    		writeln('AND WINS ********');
    		end
		end
    	else 
    	begin
    		c := move(b + 4);
    		computerMoves(c);
    		writeln('AND WINS ********');
    	end;
    end;
end.