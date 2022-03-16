program sinewave;

procedure tabWriteLn(text: string; indent: integer);
begin
  Writeln(text:length(text)+indent);
end;

var
  a, t, b: integer;
begin
  tabWriteLn('SINE WAVE', 30);
  tabWriteLn('CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY', 15);
  Writeln();
  Writeln();
  Writeln();
  Writeln();
  Writeln();
  // REMARKABLE PROGRAM BY DAVID AHL
  b := 0;
  // START LONG LOOP
  for t := 0 to 40*4 do
  begin
    a := Trunc(26+25*Sin(t/4));
    if (b = 0) then
    begin
      tabWriteLn('CREATIVE', a);
      b := 1;
    end
    else
    begin
      tabWriteLn('COMPUTING', a);
      b := 0;
    end;
  end;
end.
