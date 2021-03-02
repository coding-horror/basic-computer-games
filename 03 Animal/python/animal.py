########################################################
#
# Animal
#
# From: Basic computer Games(1978)
#
#    Unlike other computer games in which the computer
#   picks a number or letter and you must guess what it is,
#   in this game you think of an animal and the computer asks
#   you questions and tries to guess the name of your animal.
#   If the computer guesses incorrectly, it will ask you for a
#   question that differentiates the animal it guessed
#   from the one you were thinking of. In this way the
#   computer "learns" new animals. Questions to differentiate
#   new animals should be input without a question mark.
#    This version of the game does not have a SAVE feature.
#   If your sistem allows, you may modify the program to
#   save array A$, then reload the array  when you want
#   to play the game again. This way you can save what the
#   computer learns over a series of games.
#    At any time if you reply 'LIST' to the question "ARE YOU
#   THINKING OF AN ANIMAL", the computer will tell you all the
#   animals it knows so far.
#    The program starts originally by knowing only FISH and BIRD.
#   As you build up a file of animals you should use broad,
#   general questions first and then narrow down to more specific
#   ones with later animals. For example, If an elephant was to be
#   your first animal, the computer would ask for a question to distinguish
#   an elephant from a bird. Naturally there are hundreds of possibilities,
#   however, if you plan to build a large file of animals a good question
#   would be "IS IT A MAMAL".
#    This program can be easily modified to deal with categories of
#   things other than animals by simply modifying the initial data
#   in Line 530 and the dialogue references to animal in Lines 10,
#   40, 50, 130, 230, 240 and 600. In an educational environment, this
#   would be a valuable program to teach the distinguishing chacteristics
#   of many classes of objects -- rock formations, geography, marine life,
#   cell structures, etc.
#    Originally developed by Arthur Luehrmann at Dartmouth College,
#   Animal was subsequently shortened and modified by Nathan Teichholtz at
#   DEC and Steve North at Creative Computing
#
########################################################


class Node:
    """
    Node of the binary tree of questions.
    """

    def __init__(self, text, yes_node, no_node):
        # the nodes that are leafs have as text the animal's name, otherwise
        # a yes/no question
        self.text = text
        self.yes_node = yes_node
        self.no_node = no_node

    def update_node(self, new_question, answer_new_ques, new_animal):
        # update the leaf with a question
        old_animal = self.text
        # we replace the animal with a new question
        self.text = new_question

        if answer_new_ques == 'y':
            self.yes_node = Node(new_animal, None, None)
            self.no_node = Node(old_animal, None, None)
        else:
            self.yes_node = Node(old_animal, None, None)
            self.no_node = Node(new_animal, None, None)

    # the leafs have as children None
    def is_leaf(self):
        return self.yes_node == None and self.no_node == None


def list_known_animals(root_node):
    # Traversing the tree by recursion until we reach the leafs
    if root_node == None:
        return

    if root_node.is_leaf():
        print(root_node.text, end=' '*11)
        return

    if root_node.yes_node:
        list_known_animals(root_node.yes_node)

    if root_node.no_node:
        list_known_animals(root_node.no_node)


def parse_input(message, check_list, root_node):
    # only accepts yes or no inputs and recognizes list operation
    correct_input = False
    while not correct_input:
        try:
            inp = input(message)

            if check_list and inp.lower() == 'list':
                print('Animals I already know are:')
                list_known_animals(root_node)
                print('\n')

            token = inp[0].lower()
            if token == 'y' or token == 'n':
                correct_input = True
        except IndexError:
            pass

    return token


def avoid_void_input(message):
    answer = ''
    while answer == '':
        answer = input(message)
    return answer


def initial_message():
    print(' '*32 + 'Animal')
    print(' '*15 + 'Creative Computing Morristown, New Jersey\n')
    print('Play ´Guess the Animal´')
    print('Think of an animal and the computer will try to guess it.\n')


# Initial tree
yes_child = Node('Fish', None, None)
no_child = Node('Bird', None, None)
root = Node('Does it swim?', yes_child, no_child)

# Main loop of game
initial_message()
keep_playing = parse_input(
    'Are you thinking of an animal? ', True, root) == 'y'
while keep_playing:
    keep_asking = True
    # Start traversing the tree by the root
    actual_node = root

    while keep_asking:

        if not actual_node.is_leaf():
            # we have to keep asking i.e. traversing nodes
            answer = parse_input(actual_node.text, False, None)

            if answer == 'y':
                actual_node = actual_node.yes_node
            else:
                actual_node = actual_node.no_node
        else:
            # we have reached a possible answer
            answer = parse_input('Is it a {}? '.format(
                actual_node.text), False, None)
            if answer == 'n':
                # add the new animal to the tree
                new_animal = avoid_void_input(
                    'The animal you were thinking of was a ? ')
                new_question = avoid_void_input(
                    'Please type in a question that would distinguish a {} from a {}: '.format(new_animal, actual_node.text))
                answer_new_question = parse_input(
                    'for a {} the answer would be: '.format(new_animal), False, None)

                actual_node.update_node(
                    new_question+'?', answer_new_question, new_animal)

            else:
                print("Why not try another animal?")

            keep_asking = False

    keep_playing = parse_input(
        'Are you thinking of an animal? ', True, root) == 'y'


########################################################
# Porting Notes
#
#   The data structure used for storing questions and
#   animals is a binary tree where each non-leaf node
#   has a question, while the leafs store the animals.
#
#   As the original program, this program doesn't store
#   old questions and animals. A good modification would
#   be to add a database to store the tree.
#    Also as the original program, this one can be easily
#   modified to not only make guesses about animals, by
#   modyfing the initial data of the tree, the questions
#   that are asked to the user and the initial message
#   function  (Lines 120 to 130, 135, 158, 160, 168, 173)

########################################################
