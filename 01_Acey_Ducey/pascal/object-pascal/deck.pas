unit Deck;

{$IFDEF FPC}
{$mode objfpc}{$H+}
{$ENDIF}

interface

uses
  Classes
, SysUtils
;

type
{ TDeck }
  TDeck = class
  private
    FDealerLow: Integer;
    FDealerHigh: Integer;
    FPlayer: Integer;

    procedure PrintCard(const ACard: Integer);
  protected
  public
    property DealerLow: Integer
      read FDealerLow;
    property DealerHigh: Integer
      read FDealerHigh;
    property Player: Integer
      read FPlayer;

    procedure DrawCards;
    procedure ShowDealerCards;
    procedure ShowPlayerCard;
    function PlayerWins: Boolean;
  published
  end;

implementation

{ TDeck }

procedure TDeck.PrintCard(const ACard: Integer);
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

procedure TDeck.DrawCards;
var
  tmp: Integer;
begin
  repeat
    FDealerLow:= Random(14) + 2;
  until (FDealerLow >= 2) and (FDealerLow <= 14);
  repeat
    FDealerHigh:= Random(14) + 2;
  until (FDealerHigh >= 2) and (FDealerHigh <= 14) and (FDealerLow <> FDealerHigh);
  if FDealerLow > FDealerHigh then
  begin
    tmp:= FDealerHigh;
    FDealerHigh:= FDealerLow;
    FDealerLow:= tmp;
  end;
  repeat
    FPlayer:= Random(14) + 2;
  until (FPlayer >= 2) and (FPlayer <= 14);
end;

procedure TDeck.ShowDealerCards;
begin
  Write('HERE ARE YOUR NEXT TWO CARDS: ');
  PrintCard(FDealerLow);
  Write(' ');
  PrintCard(FDealerHigh);
  WriteLN;
  WriteLN;
end;

procedure TDeck.ShowPlayerCard;
begin
  PrintCard(FPlayer);
  WriteLN;
  WriteLN;
end;

function TDeck.PlayerWins: Boolean;
begin
  Result:= (FPlayer > FDealerLow) and (FPlayer < FDealerHigh);
end;

end.

