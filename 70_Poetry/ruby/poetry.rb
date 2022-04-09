PAGE_WIDTH = 64

class State
    attr_accessor :u, :i, :j, :k, :phrase, :line

    def initialize
        self.u = 0
        self.i = 0
        self.j = 0
        self.k = 0
        self.phrase = 1
        self.line = ""
    end
end


def print_centered msg
    spaces = " " * ((PAGE_WIDTH - msg.length).fdiv(2))
    print(spaces + msg)
end

def process_phrase_1 state
    line_1_options = [
        "MIDNIGHT DREARY",
        "FIERY EYES",
        "BIRD OR FIEND",
        "THING OF EVIL",
        "PROPHET"
    ]

    state.line = state.line + line_1_options[state.i]
    return state.line
end

def process_phrase_2 state
    line_2_options = [
        ["BEGUILING ME", 2],
        ["THRILLED ME", nil],
        ["STILL SITTING....", nil],
        ["NEVER FLITTING", 2],
        ["BURNED", nil]
    ]
    words, u_modifier = line_2_options[state.i]
    state.line += words
    if !u_modifier.nil?
        state.u = u_modifier
    end
end

def process_phrase_3 state
    phrases = [
        [false, "AND MY SOUL"],
        [false, "DARKNESS THERE"],
        [false, "SHALL BE LIFTED"],
        [false, "QUOTH THE RAVEN"],
        [true, "SIGN OF PARTING"]
    ]

    only_if_u, words = phrases[state.i]
    if !only_if_u || state.u > 0
        state.line = state.line + words
    end
end

def process_phrase_4 state
    phrases = [
        "NOTHING MORE",
        "YET AGAIN",
        "SLOWLY CREEPING",
        "...EVERMORE",
        "NEVERMORE"
    ]

    state.line += phrases[state.i]
end

def maybe_comma state
    if state.line.length > 0 && state.line[-1] == "."
        return
    end

    if state.u != 0 && Random.rand <= 0.19
        state.line += ", "
        state.u = 2
    end

    if Random.rand <= 0.65
        state.line += " "
        state.u += 1
    else
        puts state.line
        state.line = ""
        state.u = 0
    end
end

def pick_phrase state
    state.i = Random.rand(0..4)
    state.j += 1
    state.k += 1

    if state.u <= 0 && (state.j % 2) != 0
        state.line += (" " * 5)
    end
    state.phrase = state.j + 1
end

def main
    print_centered("POETRY")
    state = State.new
    phrase_processors = {
        '1' => 'process_phrase_1',
        '2' => 'process_phrase_2',
        '3' => 'process_phrase_3',
        '4' => 'process_phrase_4'
    }

    while true
        if state.phrase >= 1 && state.phrase <= 4
            method(phrase_processors[state.phrase.to_s]).call(state)
            maybe_comma state
        elsif state.phrase == 5
            state.j = 0
            puts state.line
            state.line = ""
            if state.k > 20
                puts ""
                state.u = 0
                state.k = 0
            else
                state.phrase = 2
                next
            end
        end
        pick_phrase state
    end
end

if __FILE__ == $0
    main
end