program aceyducey;

{$IFDEF FPC}
{$mode objfpc}{$H+}
{$ENDIF}

uses
 Crt;

var
  Stash: Integer;
  CardA: Integer;
  CardB: Integer;
  CardC: Integer;
  Bet: Integer;

procedure PrintGreeting;
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

procedure PrintBalance;
begin
  WriteLN('YOU NOW HAVE ', Stash,' DOLLARS.');
  WriteLN;
end;

procedure PrintCard(const ACard: Integer);
begin
  if ACard < 11 then
  begin
    Write(ACard);
  end;
  if ACard = 11 then
  begin
    Write('JACK');
  end;
  if ACard = 12 then
  begin
    Write('QUEEN');
  end;
  if ACard = 13 then
  begin
    Write('KING');
  end;
  if ACard = 14 then
  begin
    Write('ACE');
  end;
end;

procedure DrawDealerCards;
var
  tmp: Integer;
begin
  Write('HERE ARE YOUR NEXT TWO CARDS: ');
  repeat
    CardA:= Random(14) + 2;
  until (CardA >= 2) and (CardA <= 14);
  repeat
    CardB:= Random(14) + 2;
  until (CardB >= 2) and (CardB <= 14) and (CardA <> CardB);
  if CardA > CardB then
  begin
    tmp:= CardB;
    CardB:= CardA;
    CardA:= tmp;
  end;
  PrintCard(CardA);
  Write(' ');
  PrintCard(CardB);
  WriteLN;
  WriteLN;
end;

procedure DrawPlayerCard;
begin
  repeat
    CardC:= Random(14) + 2;
  until (CardC >= 2) and (CardC <= 14);
  PrintCard(CardC);
  WriteLN;
  WriteLN;
end;

function GetBet: Integer;
begin
  Result:= 0;
  repeat
    Write('WHAT IS YOUR BET: ');
    ReadLN(Result);
    if Result > Stash then
    begin
      WriteLn('SORRY, MY FRIEND, BUT YOU BET TOO MUCH.');
      WriteLn('YOU HAVE ONLY ', Stash,' DOLLARS TO BET.');
    end;
  until (Result >=0) and (Result <= Stash);
end;

function TryAgain: Boolean;
var
  answer: String;
begin
  Result:= False;
  Write('TRY AGAIN (YES OR NO)');
  ReadLn(answer);
  Result:= (LowerCase(answer)='yes') or (LowerCase(answer)='y');
end;

begin
  Randomize;
  ClrScr;
  PrintGreeting;
  repeat
    Stash:= 100;
    repeat
      PrintBalance;
      DrawDealerCards;
      Bet:= GetBet;
      if Bet = 0 then
      begin
        WriteLN('CHICKEN!!');
        continue;
      end;
      DrawPlayerCard;
      if (CardC > CardA) and (CardC < CardB) then
      begin
        WriteLN('YOU WIN!!!');
        Inc(Stash, Bet)
      end
      else
      begin
        WriteLN('SORRY, YOU LOSE');
        Dec(Stash, Bet)
      end;
    until Stash = 0;
    WriteLN('SORRY, FRIEND, BUT YOU BLEW YOUR WAD.');
    WriteLN;
  until not TryAgain;
  WriteLN('O.K., HOPE YOU HAD FUN!');
end.

