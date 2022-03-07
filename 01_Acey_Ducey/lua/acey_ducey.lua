print [[
            ACEY DUCEY CARD GAME
 CREATIVE COMPUTING  MORRISTOWN, NEW JERSY



ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER
THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP
YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING
ON WHETHER OF NOT YOU FEEL THE CARD WILL HAVE
A VALUE BETWEEN THE FIRST TWO.
IF YOU DO NOT WANT TO BET INPUT A 0]]

local starting_cash = 100

local card_names = {[11] = "JACK", [12] = "QUEEN", [13] = "KING", [14] = "ACE"}
for i = 2, 10 do
  card_names[i] = string.format(" %d", i)
end

function play_round(player_cash, skip_total)
  assert(player_cash > 0, "can't play with nothing to bet")

  if not skip_total then
    print(string.format("YOU NOW HAVE %u DOLLARS", player_cash))
  end

  print "\nHERE ARE YOUR NEXT TWO CARDS"

  local first_card, second_card;

  repeat
    first_card = math.random(2, 14)
    second_card = math.random(2, 14)
  until first_card < second_card

  print(card_names[first_card])
  print(card_names[second_card])
  print("")

  local bet = get_bet(player_cash)

  if bet == 0 then
    print "CHICKEN!!"
    return play_round(player_cash, "skip total")
  end

  local third_card = math.random(2, 14)
  print(card_names[third_card])

  if first_card < third_card and third_card < second_card then
    print "YOU WIN!!!"
    return play_round(player_cash + bet)
  end

  print "SORRY, YOU LOSE"
  if bet < player_cash then
    return play_round(player_cash - bet)
  end

  print "SORRY, FRIEND BUT YOU BLEW YOUR WAD"
  io.write "TRY AGAIN (YES OR NO)? "
  local keep_playing = io.read("l")
  if keep_playing:upper():match("%f[%a]YES%f[^%a]") then
    return play_round(starting_cash)
  end

  print "OK HOPE YOU HAD FUN"
end

function get_bet(player_cash)
  assert(player_cash > 0, "can't play with nothing to bet")

  io.write("WHAT IS YOUR BET? ")
  local input = io.read("l")
  if not input then
    print "GOODBYE"
    os.exit(0)
  end

  local digits = input:match("(%d+)")
  if not digits then
    print "I DON'T UNDERSTAND THAT NUMBER"
    return get_bet(player_cash)
  end

  local bet = tonumber(digits)
  assert(bet, "pattern matched something that doesn't convert to number")

  if bet > player_cash then
    print "SORRY, MY FRIEND BUT YOU BET TOO MUCH"
    print(string.format("YOU HAVE ONLY %u DOLLARS TO BET", player_cash))
    return get_bet(player_cash)
  end

  assert(0 <= bet and bet <= player_cash, "invalid bet")
  return bet
end

play_round(starting_cash)
