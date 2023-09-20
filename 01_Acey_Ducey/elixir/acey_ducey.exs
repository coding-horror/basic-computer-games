########################################################
#
# Acey Ducey
#
# From: BASIC Computer Games (1978)
#       Edited by David Ahl
#
# "This is a simulation of the Acey Ducey card game.
#  In the game, the dealer (the computer) deals two
#  cards face up.  You have an option to bet or not to
#  bet depending on whether or not you feel the next
#  card dealt will have a value between the first two.
#
# "Your initial money is set to $100. The game keeps
#  going on until you lose all your money or interrupt
#  the program.
#
# "The original BASIC program author was Bill Palmby
#  of Prairie View, Illinois."
#
# To run this file:
# > mix run acey_ducey.exs --no-mix-exs
#
# This uses the following techniques:
#
#  - The `Game` module uses a recursive `play/1` function.
#  - The `Game` module stores the game state in a `%Game{}` struct.
#  - The classic 52 playing card deck is set as a module attribute generated via a comprehension.
#  - The deck is automatically shuffled when there are less than 3 cards remaining in the deck.
#  - The initial deck defaults to an empty list which triggers a shuffle when the game begins.
#  - The initial funds defaults to 100 but it can be explicitly set in the `%Game{}` struct.
#  - The prompt to place a bet will automatically re-prompt when given an invalid input.
#  - The bets are assumed to be the whole integers for simplicity.
#
########################################################

defmodule Game do
  @deck for suit <- [:spades, :hearts, :clubs, :diamonds], value <- 1..13, do: {suit, value}

  defstruct funds: 100, deck: []

  def play(), do: play(%__MODULE__{}) # for convenience

  def play(%__MODULE__{funds: funds}) when funds <= 0, do: IO.puts("~~~ game over ~~~")
  def play(%__MODULE__{deck: deck} = game) when length(deck) < 3, do: play(%{game | deck: Enum.shuffle(@deck)})
  def play(%__MODULE__{deck: deck, funds: funds} = game) do
    IO.gets("<hit enter>\n")

    [first_card, second_card, third_card | remaining_deck] = deck

    IO.puts("~~~ new round ~~~")
    IO.puts("first card: #{format(first_card)}")
    IO.puts("second card: #{format(second_card)}\n")
    IO.puts("funds: $#{funds}")

    bet = prompt_to_place_bet(funds)
    new_funds = if win?(first_card, second_card, third_card), do: funds + bet, else: funds - bet

    IO.puts("\nthird card: #{format(third_card)}")
    IO.puts("funds: $#{funds} => $#{new_funds}")
    IO.puts("~~~ end round ~~~\n")

    play(%{game | deck: remaining_deck, funds: new_funds})
  end

  # re-prompt if invalid integer and/or out of bounds
  defp prompt_to_place_bet(funds) do
    input = IO.gets("place your bet: $")
    case Integer.parse(input) do
      {bet, _} when bet in 0..funds -> bet
      _ -> prompt_to_place_bet(funds)
    end
  end

  # for a stricter win condition (non-inclusive)
  defp win?({_, first}, {_, second}, {_, third}) do
    [floor, ceiling] = Enum.sort([first, second])
    (floor < third) && (third < ceiling)
  end
  # for a looser win condition (inclusive)
  #defp win?({_, first}, {_, second}, {_, third}) do
    #[_, middle, _] = Enum.sort([first, second, third])
    #middle == third
  #end

  defp format({suit, value}) do
    case value do
      1 -> "ace of #{suit}"
      11 -> "prince of #{suit}"
      12 -> "queen of #{suit}"
      13 -> "king of #{suit}"
      value -> "#{value} of #{suit}"
    end
  end
end

Game.play() # equivalent to Game.play(%Game{funds: 100, deck: 100})
