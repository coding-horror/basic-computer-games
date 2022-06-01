#!/usr/bin/env ruby

def instructions
    puts <<~EOF
    THIS PROGRAM IS A 'BATTLE OF NUMBERS' GAME, WHERE THE
    COMPUTER IS YOUR OPPONENT.

    THE GAME STARTS WITH AN ASSUMED PILE OF OBJECTS. YOU
    AND YOUR OPPONENT ALTERNATELY REMOVE OBJECTS FROM THE PILE.
    WINNING IS DEFINED IN ADVANCE AS TAKING THE LAST OBJECT OR
    NOT. YOU CAN ALSO SPECIFY SOME OTHER BEGINNING CONDITIONS.
    DON'T USE ZERO, HOWEVER, IN PLAYING THE GAME.
    ENTER A NEGATIVE NUMBER FOR NEW PILE SIZE TO STOP PLAYING.
    EOF
end

def ask_for_pile_size
    loop do
        puts "ENTER PILE SIZE:"
        pile_size = gets.to_i
        break pile_size if pile_size != 0
    end
end

GOAL_TAKE_LAST = 1
GOAL_AVOID_LAST = 2
def ask_for_goal
    loop do
        puts "ENTER WIN OPTION - #{GOAL_TAKE_LAST} TO TAKE LAST, #{GOAL_AVOID_LAST} TO AVOID LAST:"
        response = gets.to_i
        break response if [GOAL_TAKE_LAST, GOAL_AVOID_LAST].member? response
    end
end

def ask_for_min_max_take
    loop do
        puts "ENTER MIN AND MAX, SEPARATED BY A COMMA:"
        response = gets.split(',').map {|piece| piece.to_i}
        next if response.length != 2
        min_take, max_take = response
        break min_take, max_take if 0 < min_take && min_take <= max_take
    end
end

COMPUTER_MOVE = 1
PLAYER_MOVE = 2
def ask_who_makes_first_move
    loop do
        puts "ENTER START OPTION - #{COMPUTER_MOVE} COMPUTER FIRST, #{PLAYER_MOVE} YOU FIRST:"
        response = gets.to_i
        break response if [COMPUTER_MOVE, PLAYER_MOVE].member? response
    end
end

def ask_for_player_take(min_take:, max_take:)
    loop do
        puts "YOUR MOVE:"
        response = gets.to_i
        break response if response == 0 || response == response.clamp(min_take, max_take)
        puts "ILLEGAL MOVE, REENTER IT."
    end
end

def battle(pile:, goal:, min_take:, max_take:, first_move:)
    next_move = first_move
    loop do
        if next_move == COMPUTER_MOVE
            next_move = PLAYER_MOVE
            take = (
                if goal == GOAL_TAKE_LAST
                    pile % (min_take + max_take)
                else
                    (pile - 1) % (min_take + max_take)
                end
            ).clamp(min_take, max_take)
            pile = pile - take
            if pile > 0
                puts "COMPUTER TAKES #{take} AND LEAVES #{pile}"
            else
                if goal == GOAL_TAKE_LAST
                    puts "COMPUTER TAKES #{take} AND WINS."
                    break
                else
                    puts "COMPUTER TAKES #{take} AND LOSES."
                    break
                end
            end
        else
            next_move = COMPUTER_MOVE
            take = ask_for_player_take(min_take: min_take, max_take: max_take)
            if take == 0
                puts "I TOLD YOU NOT TO USE ZERO! COMPUTER WINS BY FORFEIT."
                break
            end
            pile = pile - take
            if pile > 0
                puts "PLAYER TAKES #{take} AND LEAVES #{pile}"
            else
                if goal == GOAL_TAKE_LAST
                    puts "CONGRATULATIONS, YOU WIN."
                    break
                else
                    puts "TOUGH LUCK, YOU LOSE."
                    break
                end
            end
        end
    end
end


def main
    instructions
    loop do
        pile = ask_for_pile_size
        break if pile < 0
        goal = ask_for_goal
        min_take, max_take = ask_for_min_max_take
        first_move = ask_who_makes_first_move
        battle(
            pile: pile,
            goal: goal,
            min_take: min_take,
            max_take: max_take,
            first_move: first_move,
        )
    end
end

main
