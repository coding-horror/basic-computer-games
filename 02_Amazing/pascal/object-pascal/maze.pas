unit Maze;

{$IFDEF FPC}
{$mode ObjFPC}{$H+}
{$ENDIF}

interface

uses
  Classes
, SysUtils
, Room
;

type
  TDirection = (dUp, dRight, dDown, dLeft);
  TDirections = set of TDirection;
{ TMaze }
  TMaze = class(TObject)
  private
    FWidth: Integer;
    FLength: Integer;
    FEntry: Integer;
    FLabyrinth: Array of Array of TRoom;

    function GetRandomDirection(const ADirections: TDirections): TDirection;

    procedure DebugVisited;
    procedure DebugWalls;
  protected
  public
    constructor Create(const AWidth, ALength: Integer);
    destructor Destroy; override;

    procedure Build;
    procedure Print;
  published
  end;

implementation

const
  EXIT_DOWN  = 1;
  EXIT_RIGHT = 2;

{ TMaze }

function TMaze.GetRandomDirection(const ADirections: TDirections): TDirection;
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

procedure TMaze.DebugVisited;
var
  indexW: Integer;
  indexL: Integer;
begin
  WriteLN('Visited');
  for indexL:= 0 to Pred(FLength) do
  begin
    for indexW:= 0 to Pred(FWidth) do
    begin
      Write(FLabyrinth[indexW][indexL].Visited:3,' ');
    end;
    WriteLN;
  end;
  WriteLN;
end;

procedure TMaze.DebugWalls;
var
  indexW: Integer;
  indexL: Integer;
begin
  WriteLN('Walls');
  for indexL:= 0 to Pred(FLength) do
  begin
    for indexW:= 0 to Pred(FWidth) do
    begin
      Write(FLabyrinth[indexW][indexL].Walls:3,' ');
    end;
    WriteLN;
  end;
  WriteLN;
end;

constructor TMaze.Create(const AWidth, ALength: Integer);
var
  indexW: Integer;
  indexL: Integer;
begin
  Randomize;
  FWidth:= AWidth;
  FLength:= ALength;
  FEntry:= Random(FWidth);
  SetLength(FLabyrinth, FWidth, FLength);
  for indexW:= 0 to Pred(FWidth) do
  begin
    for indexL:= 0 to Pred(FLength) do
    begin
      FLabyrinth[indexW][indexL]:= TRoom.Create;
    end;
  end;
end;

destructor TMaze.Destroy;
var
  indexW: Integer;
  indexL: Integer;
begin
  for indexW:= 0 to Pred(FWidth) do
  begin
    for indexL:= 0 to Pred(FLength) do
    begin
      if Assigned(FLabyrinth[indexW][indexL]) then
      begin
        FLabyrinth[indexW][indexL].Free;
      end;
    end;
  end;
  inherited Destroy;
end;

procedure TMaze.Build;
var
  indexW: Integer;
  indexL: Integer;
  direction: TDirection;
  directions: TDirections;
  count: Integer;
begin
  FEntry:= Random(FWidth);
  indexW:= FEntry;
  indexL:= 0;
  count:= 1;
  FLabyrinth[indexW][indexL].Visited:= count;
  Inc(count);
  repeat
    directions:= [dUp, dRight, dDown, dLeft];
    if (indexW = 0) or (FLabyrinth[Pred(indexW)][indexL].Visited <> 0) then
    begin
      Exclude(directions, dLeft);
    end;
    if (indexL = 0) or (FLabyrinth[indexW][Pred(indexL)].Visited <> 0) then
    begin
      Exclude(directions, dUp);
    end;
    if (indexW = Pred(FWidth)) or (FLabyrinth[Succ(indexW)][indexL].Visited <> 0) then
    begin
      Exclude(directions, dRight);
    end;
    if (indexL = Pred(FLength)) or (FLabyrinth[indexW][Succ(indexL)].Visited <> 0) then
    begin
      Exclude(directions, dDown);
    end;

    if directions <> [] then
    begin
      direction:= GetRandomDirection(directions);
      case direction of
        dLeft:begin
          Dec(indexW);
          FLabyrinth[indexW][indexL].Walls:= EXIT_RIGHT;
        end;
        dUp:begin
          Dec(indexL);
          FLabyrinth[indexW][indexL].Walls:= EXIT_DOWN;
        end;
        dRight:begin
          FLabyrinth[indexW][indexL].Walls:= FLabyrinth[indexW][indexL].Walls + EXIT_RIGHT;
          Inc(indexW);
        end;
        dDown:begin
          FLabyrinth[indexW][indexL].Walls:= FLabyrinth[indexW][indexL].Walls + EXIT_DOWN;
          Inc(indexL);
        end;
      end;
      FLabyrinth[indexW][indexL].Visited:= count;
      Inc(count);
    end
    else
    begin
      while True do
      begin
        if indexW <> Pred(FWidth) then
        begin
          Inc(indexW);
        end
        else if indexL <> Pred(FLength) then
        begin
          Inc(indexL);
          indexW:= 0;
        end
        else
        begin
          indexW:= 0;
          indexL:= 0;
        end;
        if FLabyrinth[indexW][indexL].Visited <> 0 then
        begin
          break;
        end;
      end;
    end;
  until count = (FWidth * FLength) + 1;
  indexW:= Random(FWidth);
  indexL:= Pred(FLength);
  FLabyrinth[indexW][indexL].Walls:= FLabyrinth[indexW][indexL].Walls + 1;
end;

procedure TMaze.Print;
var
  indexW:Integer;
  indexL: Integer;
begin

  //DebugVisited;
  //DebugWalls;

  for indexW:= 0 to Pred(FWidth) do
  begin
    if indexW = FEntry then
    begin
      Write('.  ');
    end
    else
    begin
      Write('.--');
    end;
  end;
  WriteLN('.');

  for indexL:= 0 to Pred(FLength) do
  begin
    Write('I');
    for indexW:= 0 to Pred(FWidth) do
    begin
      FLabyrinth[indexW][indexL].PrintRoom;
    end;
    WriteLN;
    for indexW:= 0 to Pred(FWidth) do
    begin
      FLabyrinth[indexW][indexL].PrintWall;
    end;
    WriteLN('.');
  end;
end;

end.

