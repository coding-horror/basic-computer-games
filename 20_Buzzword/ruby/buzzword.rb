######################################################################
#
# Buzzword Generator
#
# From: BASIC Computer Games (1978)
#       Edited by David H. Ahl
#
# "This program is an invaluable aid for preparing speeches and
#  briefings about education technology.  This buzzword generator
#  provides sets of three highly-acceptable words to work into your
#  material.  Your audience will never know that the phrases don't
#  really mean much of anything because they sound so great!  Full
#  instructions for running are given in the program.
#
# "This version of Buzzword was written by David Ahl."
#
#
# Ruby port by Leslie Viljoen, 2021
#
######################################################################

WORDS = [["Ability", "Basal", "Behavioral", "Child-centered",
           "Differentiated", "Discovery", "Flexible", "Heterogeneous",
           "Homogenous", "Manipulative", "Modular", "Tavistock",
           "Individualized"],

          ["learning", "evaluative", "objective", "cognitive",
           "enrichment", "scheduling", "humanistic", "integrated",
           "non-graded", "training", "vertical age", "motivational",
           "creative"] ,

          ["grouping", "modification", "accountability", "process",
           "core curriculum", "algorithm", "performance",
           "reinforcement", "open classroom", "resource", "structure",
           "facility","environment"]]


# Display intro text

puts "\n           Buzzword Generator"
puts "Creative Computing  Morristown, New Jersey"
puts "\n\n"
puts "This program prints highly acceptable phrases in"
puts "'educator-speak' that you can work into reports"
puts "and speeches.  Whenever a question mark is printed,"
puts "type a 'Y' for another phrase or 'N' to quit."
puts "\n\nHere's the first phrase:"

loop do
    phrase = []

    prefix, body, postfix = WORDS

    phrase << prefix[rand(prefix.length)]
    phrase << body[rand(body.length)]
    phrase << postfix[rand(postfix.length)]

    puts phrase.join(' ')
    puts "\n"

    print "?"
    response = gets

    break unless response.upcase.start_with?('Y')
end

puts "Come back when you need help with another report!\n"


######################################################################
#
# Porting Notes
#
#   The original program stored all 39 words in one array, then
#   built the buzzword phrases by randomly sampling from each of the
#   three regions of the array (1-13, 14-26, and 27-39).
#
#   Instead, we're storing the words for each section in three
#   separate arrays. That makes it easy to loop through the sections
#   to stitch the phrase together, and it easily accomodates adding
#   (or removing) elements from any section.  They don't all need to
#   be the same length.
#
#   The author of this program (and founder of Creative Computing
#   magazine) first started working at DEC--Digital Equipment
#   Corporation--as a consultant helping the company market its
#   computers as educational products.  He later was editor of a DEC
#   newsletter named "EDU" that focused on using computers in an
#   educational setting.  No surprise, then, that the buzzwords in
#   this program were targeted towards educators!
#
#
# Ideas for Modifications
#
#   Try adding more/different words. Better yet, add a third
#   array to the WORDS array to add new sets of words that
#   might pertain to different fields. What would business buzzwords
#   be? Engineering buzzwords?  Art/music buzzwords?  Let the user
#   choose a field and pick the buzzwords accordingly.
#
######################################################################
