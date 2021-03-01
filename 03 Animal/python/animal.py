class Node:
    """
    Nodes of the binary tree of questions.
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
    # only accepts yes or no inputs
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


# Initial tree
yes_child = Node('Fish', None, None)
no_child = Node('Bird', None, None)
root = Node('Does it swim?', yes_child, no_child)

# Main loop of game
keep_playing = parse_input('Are you thinking of an animal?', True, root) == 'y'
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
            answer = parse_input('Is it a {}?'.format(
                actual_node.text), False, None)
            if answer == 'n':
                # add the new animal to the tree
                new_animal = avoid_void_input(
                    'The animal you were thinking of was a ?')
                new_question = avoid_void_input(
                    'Please type in a question that would distinguish a {} from a {}:'.format(new_animal, actual_node.text))
                answer_new_question = parse_input(
                    'for a {} the answer would be:'.format(new_animal), False, None)

                actual_node.update_node(
                    new_question+'?', answer_new_question, new_animal)

            else:
                print("Why not try another animal?")

            keep_asking = False

    keep_playing = parse_input(
        'Are you thinking of an animal?', True, root) == 'y'
