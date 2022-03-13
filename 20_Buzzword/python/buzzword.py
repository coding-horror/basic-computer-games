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
# Python port by Jeff Jetton, 2019
#
######################################################################


import random

WORDS = [
    [
        "Ability",
        "Basal",
        "Behavioral",
        "Child-centered",
        "Differentiated",
        "Discovery",
        "Flexible",
        "Heterogeneous",
        "Homogenous",
        "Manipulative",
        "Modular",
        "Tavistock",
        "Individualized",
    ],
    [
        "learning",
        "evaluative",
        "objective",
        "cognitive",
        "enrichment",
        "scheduling",
        "humanistic",
        "integrated",
        "non-graded",
        "training",
        "vertical age",
        "motivational",
        "creative",
    ],
    [
        "grouping",
        "modification",
        "accountability",
        "process",
        "core curriculum",
        "algorithm",
        "performance",
        "reinforcement",
        "open classroom",
        "resource",
        "structure",
        "facility",
        "environment",
    ],
]


# Display intro text
print("\n           Buzzword Generator")
print("Creative Computing  Morristown, New Jersey")
print("\n\n")
print("This program prints highly acceptable phrases in")
print("'educator-speak' that you can work into reports")
print("and speeches.  Whenever a question mark is printed,")
print("type a 'Y' for another phrase or 'N' to quit.")
print("\n\nHere's the first phrase:")

still_running = True
while still_running:
    phrase = ""
    for section in WORDS:
        if len(phrase) > 0:
            phrase += " "
        phrase += section[random.randint(0, len(section) - 1)]

    print(phrase)
    print("")

    response = input("? ")
    try:
        if response.upper()[0] != "Y":
            still_running = False
    except Exception:
        still_running = False


print("Come back when you need help with another report!\n")


######################################################################
#
# Porting Notes
#
#   The original program stored all 39 words in one array, then
#   built the buzzword phrases by randomly sampling from each of the
#   three regions of the array (1-13, 14-26, and 27-39).
#
#   Here, we're storing the words for each section in separate
#   tuples.  That makes it easy to just loop through the sections
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
#   Try adding more/different words.  Better yet, add a third
#   dimnension to our WORDS tuple to add new sets of words that
#   might pertain to different fields.  What would business buzzwords
#   be? Engineering buzzwords?  Art/music buzzwords?  Let the user
#   choose a field and pick the buzzwords accordingly.
#
######################################################################
