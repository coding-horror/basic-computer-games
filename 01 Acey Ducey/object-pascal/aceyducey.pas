program aceyducey;

{$IFDEF FPC}
{$mode objfpc}{$H+}
{$ENDIF}

uses
  Game, Deck
;

var
  Acey_Ducey: TGame;

begin
  Acey_Ducey:= TGame.Create(100);
  Acey_Ducey.Run;
end.

