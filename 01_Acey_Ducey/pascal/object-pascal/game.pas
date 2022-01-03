unit Game;

{$IFDEF FPC}
{$mode objfpc}{$H+}
{$ENDIF}

interface

uses
  Classes
, SysUtils
, Crt
, Deck
;

type
{ TGame }
  TGame = class
  private
    FStash: Integer;
    FBet: Integer;
    FDeck: TDeck;

    procedure PrintGreeting;
    procedure PrintBalance;
    function GetBet: Integer;
    function TryAgain: Boolean;
  protected
  public
    constructor Create;
    destructor Destroy; override;

    procedure Run;
  published
  end;

implementation

{ TGame }

procedure TGame.PrintGreeting;
begin
  WriteLN(' ':26, 'ACEY DUCEY CARD GAME');
  WriteLN(' ':15, 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY');
  WriteLN;
  WriteLN;
  WriteLN('ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER ');
  WriteLN('THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP');
  WriteLN('YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING');
  WriteLN('ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE');
  WriteLN('A VALUE BETWEEN THE FIRST TWO.');
  WriteLN('IF YOU DO NOT WANT TO BET, INPUT A 0');
  WriteLN;
end;

procedure TGame.PrintBalance;
begin
  WriteLN('YOU NOW HAVE ', FStash,' DOLLARS.');
  WriteLN;
end;

function TGame.GetBet: Integer;
begin
  Result:= 0;
  repeat
    Write('WHAT IS YOUR BET: ');
    ReadLN(Result);
    if Result > FStash then
    begin
      WriteLn('SORRY, MY FRIEND, BUT YOU BET TOO MUCH.');
      WriteLn('YOU HAVE ONLY ', FStash,' DOLLARS TO BET.');
    end;
  until (Result >=0) and (Result <= FStash);
end;

function TGame.TryAgain: Boolean;
var
  answer: String;
begin
  Result:= False;
  Write('TRY AGAIN (YES OR NO)');
  ReadLn(answer);
  Result:= (LowerCase(answer)='yes') or (LowerCase(answer)='y');
end;

constructor TGame.Create;
begin
  Randomize;
  FDeck:= TDeck.Create;
end;

destructor TGame.Destroy;
begin
  FDeck.Free;
  inherited Destroy;
end;

procedure TGame.Run;
begin
  ClrScr;
  PrintGreeting;
  repeat
    FStash:= 100;
    repeat
      PrintBalance;
      FDeck.DrawCards;
      //DrawDealerCards;
      FDeck.ShowDealerCards;
      FBet:= GetBet;
      if FBet = 0 then
      begin
        WriteLN('CHICKEN!!');
        continue;
      end;
      //DrawPlayerCard;
      FDeck.ShowPlayerCard;
      //if (FCardC > FCardA) and (FCardC < FCardB) then
      if FDeck.PlayerWins then
      begin
        WriteLN('YOU WIN!!!');
        Inc(FStash, FBet)
      end
      else
      begin
        WriteLN('SORRY, YOU LOSE');
        Dec(FStash, FBet)
      end;
    until FStash = 0;
    WriteLN('SORRY, FRIEND, BUT YOU BLEW YOUR WAD.');
    WriteLN;
  until not TryAgain;
  WriteLN('O.K., HOPE YOU HAD FUN!');
end;

end.

