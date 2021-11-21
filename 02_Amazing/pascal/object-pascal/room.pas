unit Room;

{$IFDEF FPC}
{$mode ObjFPC}{$H+}
{$ENDIF}

interface

uses
  Classes
, SysUtils
;

type
{ TRoom }
  TRoom = class(TObject)
  private
    FVisited: Integer;
    FWalls: Integer;
  protected
  public
    constructor Create;

    procedure PrintRoom;
    procedure PrintWall;

    property Visited: Integer
      read FVisited
      write FVisited;
    property Walls: Integer
      read FWalls
      write FWalls;
  published
  end;

implementation

{ TRoom }

constructor TRoom.Create;
begin
  FVisited:= 0;
  FWalls:= 0;
end;

procedure TRoom.PrintRoom;
begin
  if FWalls < 2 then
  begin
    Write('  I');
  end
  else
  begin
    Write('   ');
  end;
end;

procedure TRoom.PrintWall;
begin
  if (FWalls = 0) or (FWalls = 2) then
  begin
    Write(':--');
  end
  else
  begin
    Write(':  ');
  end;
end;

end.

