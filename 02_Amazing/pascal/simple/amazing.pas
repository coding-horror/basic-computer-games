program Amazing;

{$IFDEF FPC}
{$mode objfpc}{$H+}
{$ENDIF}

uses
  Crt;

type
  TDirection = (dUp, dRight, dDown, dLeft);
  TDirections = set of TDirection;

var
  Width: Integer;   // H
  Length: Integer;  // V
  Entry: Integer;
  MatrixWalls: Array of Array of Integer;
  MatrixVisited: Array of Array of Integer;

const
  EXIT_DOWN  = 1;
  EXIT_RIGHT = 2;

procedure PrintGreeting;
begin
  WriteLN(' ':28, 'AMAZING PROGRAM');
  WriteLN(' ':15, 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY');
  WriteLN;
  WriteLN;
  WriteLN;
  WriteLN;
end;

procedure GetDimensions;
begin
  repeat
    Write('WHAT ARE YOUR WIDTH AND LENGTH (SPACE IN BETWEEN): ');
    ReadLN(Width, Length);
    if (Width = 1) or (Length = 1) then
    begin
      WriteLN('MEANINGLESS DIMENSIONS.  TRY AGAIN.');
    end;
  until (Width > 1) and (Length > 1);
  WriteLN;
  WriteLN;
  WriteLN;
  WriteLN;
end;

procedure ClearMatrices;
var
  indexW: Integer;
  indexL: Integer;
begin
  SetLength(MatrixWalls, Width, Length);
  SetLength(MatrixVisited, Width, Length);
  for indexW:= 0 to Pred(Width) do
  begin
    for indexL:= 0 to Pred(Length) do
    begin
      MatrixWalls[indexW][indexL]:= 0;
      MatrixVisited[indexW][indexL]:= 0;
    end;
  end;
end;

function GetRandomDirection(const ADirections: TDirections): TDirection;
var
  count: Integer;
  position: Integer;
  directions: array [0..3] of TDirection;
begin
  count:= 0;
  position:= 0;
  if dUp in ADirections then
  begin
    Inc(count);
    directions[position]:= dUp;
    Inc(position);
  end;
  if dRight in ADirections then
  begin
    Inc(count);
    directions[position]:= dRight;
    Inc(position);
  end;
  if dDown in ADirections then
  begin
    Inc(count);
    directions[position]:= dDown;
    Inc(position);
  end;
  if dLeft in ADirections then
  begin
    Inc(count);
    directions[position]:= dLeft;
    Inc(position);
  end;
  Result:= directions[Random(count)];
end;

procedure BuildMaze;
var
  indexW: Integer;
  indexL: Integer;
  direction: TDirection;
  directions: TDirections;
  count: Integer;
begin
  Entry:= Random(Width);
  indexW:= Entry;
  indexL:= 0;
  count:= 1;
  MatrixVisited[indexW][indexL]:= count;
  Inc(count);
  repeat
    directions:= [dUp, dRight, dDown, dLeft];
    if (indexW = 0) or (MatrixVisited[Pred(indexW)][indexL] <> 0) then
    begin
      Exclude(directions, dLeft);
    end;
    if (indexL = 0) or (MatrixVisited[indexW][Pred(indexL)] <> 0) then
    begin
      Exclude(directions, dUp);
    end;
    if (indexW = Pred(Width)) or (MatrixVisited[Succ(indexW)][indexL] <> 0) then
    begin
      Exclude(directions, dRight);
    end;
    if (indexL = Pred(Length)) or (MatrixVisited[indexW][Succ(indexL)] <> 0) then
    begin
      Exclude(directions, dDown);
    end;

    if directions <> [] then
    begin
      direction:= GetRandomDirection(directions);
      case direction of
        dLeft:begin
          Dec(indexW);
          MatrixWalls[indexW][indexL]:= EXIT_RIGHT;
        end;
        dUp:begin
          Dec(indexL);
          MatrixWalls[indexW][indexL]:= EXIT_DOWN;
        end;
        dRight:begin
          Inc(MatrixWalls[indexW][indexL], EXIT_RIGHT);
          Inc(indexW);
        end;
        dDown:begin
          Inc(MatrixWalls[indexW][indexL], EXIT_DOWN);
          Inc(indexL);
        end;
      end;
      MatrixVisited[indexW][indexL]:= count;
      Inc(count);
    end
    else
    begin
      while True do
      begin
        if indexW <> Pred(Width) then
        begin
          Inc(indexW);
        end
        else if indexL <> Pred(Length) then
        begin
          Inc(indexL);
          indexW:= 0;
        end
        else
        begin
          indexW:= 0;
          indexL:= 0;
        end;
        if MatrixVisited[indexW][indexL] <> 0 then
        begin
          break;
        end;
      end;
    end;
  until count = (Width * Length) + 1;
  indexW:= Random(Width);
  indexL:= Pred(Length);
  Inc(MatrixWalls[indexW][indexL]);
end;

procedure DegubVisited;
var
  indexW: Integer;
  indexL: Integer;
begin
  WriteLN('Visited');
  for indexL:= 0 to Pred(Length) do
  begin
    for indexW:= 0 to Pred(Width) do
    begin
      Write(MatrixVisited[indexW][indexL]:2,' ');
    end;
    WriteLN;
  end;
  WriteLN;
end;

procedure DebugWalls;
var
  indexW: Integer;
  indexL: Integer;
begin
  WriteLN('Walls');
  for indexL:= 0 to Pred(Length) do
  begin
    for indexW:= 0 to Pred(Width) do
    begin
      Write(MatrixWalls[indexW, indexL]:2, ' ');
    end;
    WriteLN;
  end;
  WriteLN;
end;

procedure PrintMaze;
var
  indexW: Integer;
  indexL: Integer;
begin

  for indexW:= 0 to Pred(Width) do
  begin
    if indexW = Entry then
    begin
      Write('.  ');
    end
    else
    begin
      Write('.--');
    end;
  end;
  WriteLN('.');
  for indexL:= 0 to Pred(Length) do
  begin
    Write('I');
    for indexW:= 0 to Pred(Width) do
    begin
      if MatrixWalls[indexW, indexL] < 2 then
      begin
        Write('  I');
      end
      else
      begin
        Write('   ');
      end;
    end;
    WriteLN;
    for indexW:= 0 to Pred(Width) do
    begin
      if (MatrixWalls[indexW, indexL] = 0) or (MatrixWalls[indexW, indexL] = 2) then
      begin
        Write(':--');
      end
      else
      begin
        Write(':  ');
      end;
    end;
    WriteLN('.');
  end;
  WriteLN;
end;

begin
  Randomize;
  ClrScr;
  PrintGreeting;
  GetDimensions;
  ClearMatrices;
  BuildMaze;
  //DegubVisited;
  //DebugWalls;
  PrintMaze;
end.

