"""
LITQUIZ

A children's literature quiz

Ported by Dave LeCompte
"""

PAGE_WIDTH = 64


class Question:
    def __init__(
        self, question, answer_list, correct_number, incorrect_message, correct_message
    ):
        self.question = question
        self.answer_list = answer_list
        self.correct_number = correct_number
        self.incorrect_message = incorrect_message
        self.correct_message = correct_message

    def ask(self):
        print(self.question)

        options = [f"{i+1}){self.answer_list[i]}" for i in range(len(self.answer_list))]
        print(", ".join(options))

        response = int(input())

        if response == self.correct_number:
            print(self.correct_message)
            return True
        else:
            print(self.incorrect_message)
            return False


questions = [
    Question(
        "IN PINOCCHIO, WHAT WAS THE NAME OF THE CAT?",
        ["TIGGER", "CICERO", "FIGARO", "GUIPETTO"],
        3,
        "SORRY...FIGARO WAS HIS NAME.",
        "VERY GOOD!  HERE'S ANOTHER.",
    ),
    Question(
        "FROM WHOSE GARDEN DID BUGS BUNNY STEAL THE CARROTS?",
        ["MR. NIXON'S", "ELMER FUDD'S", "CLEM JUDD'S", "STROMBOLI'S"],
        2,
        "TOO BAD...IT WAS ELMER FUDD'S GARDEN.",
        "PRETTY GOOD!",
    ),
    Question(
        "IN THE WIZARD OF OS, DOROTHY'S DOG WAS NAMED?",
        ["CICERO", "TRIXIA", "KING", "TOTO"],
        4,
        "BACK TO THE BOOKS,...TOTO WAS HIS NAME.",
        "YEA!  YOU'RE A REAL LITERATURE GIANT.",
    ),
    Question(
        "WHO WAS THE FAIR MAIDEN WHO ATE THE POISON APPLE?",
        ["SLEEPING BEAUTY", "CINDERELLA", "SNOW WHITE", "WENDY"],
        3,
        "OH, COME ON NOW...IT WAS SNOW WHITE.",
        "GOOD MEMORY!",
    ),
]


def print_centered(msg):
    spaces = " " * ((64 - len(msg)) // 2)

    print(spaces + msg)


def print_instructions():
    print("TEST YOUR KNOWLEDGE OF CHILDREN'S LITERATURE.")
    print()
    print("THIS IS A MULTIPLE-CHOICE QUIZ.")
    print("TYPE A 1, 2, 3, OR 4 AFTER THE QUESTION MARK.")
    print()
    print("GOOD LUCK!")
    print()
    print()


def main():
    print_centered("LITERATURE QUIZ")
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()

    print_instructions()

    score = 0

    for q in questions:
        if q.ask():
            score += 1
        print()
        print()

    if score == len(questions):
        print("WOW!  THAT'S SUPER!  YOU REALLY KNOW YOUR NURSERY")
        print("YOUR NEXT QUIZ WILL BE ON 2ND CENTURY CHINESE")
        print("LITERATURE (HA, HA, HA)")
    elif score < len(questions) / 2:
        print("UGH.  THAT WAS DEFINITELY NOT TOO SWIFT.  BACK TO")
        print("NURSERY SCHOOL FOR YOU, MY FRIEND.")
    else:
        print("NOT BAD, BUT YOU MIGHT SPEND A LITTLE MORE TIME")
        print("READING THE NURSERY GREATS.")


if __name__ == "__main__":
    main()
