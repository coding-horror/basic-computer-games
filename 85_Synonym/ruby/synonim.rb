########################################################
#
# Synonym
#
# From Basic Computer Games (1978)
#
#A synonym of a word is another word (in the English language) which has the same,
#or very nearly the same, meaning. This program tests your knowledge of synonyms
#of a few common words.
#
#The computer chooses a word and asks you for a synonym. The computer then tells
#you whether you’re right or wrong. If you can’t think of a synonym, type “HELP”
#which causes a synonym to be printed.
#You may put in words of your choice in the data statements.
#The number following DATA in Statement 500 is the total number of data statements.
#In each data statement, the first number is the number of words in that statement.
#
#Can you think of a way to make this into a more general kind of CAI program for any subject?
#Walt Koetke of Lexington High School, Massachusetts created this program.
#
#
########################################################

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


######################################################################
#
# Porting notes
#
#  There is a bug in the original program where if you keep asking for
# synoyms of a given word it ends up running out of synonyms
# in the array and the program crashes.
#  The bug has been fixed in this version and now when
# it runs out of words it continues with the next
# array.
#
######################################################################
