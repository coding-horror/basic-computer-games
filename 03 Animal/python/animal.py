class Node:
    """
    Nodes of the binary tree of questions.
    """

    def __init__(self, text, yes_node, no_node, is_leaf):
        self.text = text
        self.yes_node = yes_node
        self.no_node = no_node
        # the nodes that are leafs have as text the animal's name
        self.is_leaf = is_leaf

    def update_node(self, new_question, answer_new_ques, new_animal):
        # update the yes or no leaf with a question
        old_animal = self.text
        # we replace the animal with a new question
        self.text = new_question

        if answer_new_ques == 'y':
            self.yes_node = Node(new_animal, None, None, True)
            self.no_node = Node(old_animal, None, None, True)
        else:
            self.yes_node = Node(old_animal, None, None, True)
            self.no_node = Node(new_animal, None, None, True)

        self.is_leaf = False


def parse_input(message):
    # only accepts yes or no inputs
    correct_input = False
    while not correct_input:
        try:
            inp = input(message)
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


# Initial tree
yes_child = Node('Fish', None, None, True)
no_child = Node('Bird', None, None, True)
root = Node('Does it swim?', yes_child, no_child, False)

# Main loop of game
keep_playing = parse_input('Are you thinking of an animal?') == 'y'
while keep_playing:
    keep_asking = True
    # Start traversing the tree by the root
    actual_node = root

    while keep_asking:

        if not actual_node.is_leaf:
            # we have to keep asking i.e. traversing nodes
            answer = parse_input(actual_node.text)

            if answer == 'y':
                actual_node = actual_node.yes_node
            else:
                actual_node = actual_node.no_node
        else:
            # we have reached a possible answer
            answer = parse_input('Is it a {}?'.format(actual_node.text))
            if answer == 'n':
                # add the animal to the tree
                new_animal = avoid_void_input(
                    'The animal you were thinking of was a ?')
                new_question = avoid_void_input(
                    'Please type in a question that would distinguish a {} from a {}:'.format(new_animal, actual_node.text))
                print("new_animal:" + new_animal)
                answer_new_question = parse_input(
                    'for a {} the answer would be:'.format(new_animal))

                actual_node.update_node(
                    new_question+'?', answer_new_question, new_animal)

            else:
                print("Why not try another animal?")

            keep_asking = False

    keep_playing = parse_input('Are you thinking of an animal?') == 'y'
