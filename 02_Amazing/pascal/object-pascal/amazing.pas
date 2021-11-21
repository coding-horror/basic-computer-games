program amazing;

{$IFDEF FPC}
{$mode ObjFPC}{$H+}
{$ENDIF}

uses
  AmazingApplication, maze, Room;

var
  AmazingApp: TAmazingApplication;

begin
  AmazingApp:= TAmazingApplication.Create;
  AmazingApp.Run;
end.

