

puts <<~INSTRUCTIONS
                                 SYNONYM
                 CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY

A SYNONYM OF A WORD MEANS ANOTHER WORD IN THE ENGLISH
LANGUAGE WHICH HAS THE SAME OR VERY NEARLY THE SAME MEANING.
I CHOOSE A WORD -- YOU TYPE A SYNONYM.
IF YOU CAN'T THINK OF A SYNONYM, TYPE THE WORD 'HELP'
AND I WILL TELL YOU A SYNONYM.


INSTRUCTIONS

right_words = ["RIGHT", "CORRECT", "FINE", "GOOD!", "CHECK"]

synonym_words = [
    ["FIRST", "START", "BEGINNING", "ONSET", "INITIAL"],
    ["SIMILAR", "ALIKE", "SAME", "LIKE", "RESEMBLING"],
    ["MODEL", "PATTERN", "PROTOTYPE", "STANDARD", "CRITERION"],
    ["SMALL", "INSIGNIFICANT", "LITTLE", "TINY", "MINUTE"],
    ["STOP", "HALT", "STAY", "ARREST", "CHECK", "STANDSTILL"],
    ["HOUSE", "DWELLING", "RESIDENCE", "DOMICILE", "LODGING", "HABITATION"],
    ["PIT", "HOLE", "HOLLOW", "WELL", "GULF", "CHASM", "ABYSS"],
    ["PUSH", "SHOVE", "THRUST", "PROD", "POKE", "BUTT", "PRESS"],
    ["RED", "ROUGE", "SCARLET", "CRIMSON", "FLAME", "RUBY"],
    ["PAIN", "SUFFERING", "HURT", "MISERY", "DISTRESS", "ACHE", "DISCOMFORT"],
]

synonym_words.shuffle.each {|words_ar| 

}


synonym_words.each {|words_ar| 
    answer = false 
    keyword = words_ar.shift

    while not answer and words_ar.length != 0
        puts "     WHAT IS A SYNONYM OF #{keyword}? "
        inp = gets.chomp.upcase

        if inp  == "HELP"
            clue = words_ar.sample
            puts "**** A SYNONYM OF #{keyword} IS #{clue}."
            words_ar.delete(clue)
        elsif words_ar.include? inp
            puts right_words.sample
            answer = true
        else
            puts "TRY AGAIN."
        end

    end

}

puts "SYNONYM DRILL COMPLETED"