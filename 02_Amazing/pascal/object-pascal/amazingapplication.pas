unit AmazingApplication;

{$IFDEF FPC}
{$mode ObjFPC}{$H+}
{$ENDIF}

interface

uses
  Classes
, SysUtils
, Crt
, Maze
;

type
{ TAmazingApplication }
  TAmazingApplication = class(TObject)
  private
    FMaze: TMaze;

    procedure PrintGreeting;
    procedure GetDimensions;
    procedure BuildMaze;
    procedure PrintMaze;
  protected
  public
    constructor Create;
    destructor Destroy; override;

    procedure Run;
  published
  end;

implementation

{ TAmazingApplication }

procedure TAmazingApplication.PrintGreeting;
begin
  WriteLN(' ':28, 'AMAZING PROGRAM');
  WriteLN(' ':15, 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY');
  WriteLN;
  WriteLN;
  WriteLN;
  WriteLN;
end;

procedure TAmazingApplication.GetDimensions;
var
  width: Integer;
  length: Integer;
begin
  repeat
    Write('WHAT ARE YOUR WIDTH AND LENGTH (SPACE IN BETWEEN): ');
    ReadLN(width, length);
    if (width = 1) or (length = 1) then
    begin
      WriteLN('MEANINGLESS DIMENSIONS.  TRY AGAIN.');
    end;
  until (width > 1) and (length > 1);
  FMaze:= TMaze.Create(width, length);
  WriteLN;
  WriteLN;
  WriteLN;
  WriteLN;
end;

procedure TAmazingApplication.BuildMaze;
begin
  FMaze.Build;
end;

procedure TAmazingApplication.PrintMaze;
begin
  FMaze.Print;
  WriteLN;
end;

constructor TAmazingApplication.Create;
begin
  //
end;

destructor TAmazingApplication.Destroy;
begin
  if Assigned(FMaze) then
  begin
    FMaze.Free;
  end;
  inherited Destroy;
end;

procedure TAmazingApplication.Run;
begin
  //ClrScr;
  PrintGreeting;
  GetDimensions;
  BuildMaze;
  PrintMaze;
end;

end.

