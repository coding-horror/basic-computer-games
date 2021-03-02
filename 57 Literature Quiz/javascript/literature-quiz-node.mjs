import * as readline from 'readline'

// start reusable code
async function input(prompt = "") {
    const rl = readline.createInterface({
        input: process.stdin,
        output: process.stdout
    })

    return new Promise((resolve, _) => {
        rl.setPrompt(prompt)
        rl.prompt()
        rl.on('line', answer => {
            rl.close()
            resolve(answer)
        })
    })
}

function println(message = "", align = "left"){
    let padColCount = 0
    if(align === "center"){
        padColCount = Math.round(process.stdout.columns / 2 + message.length / 2)
    }
    console.log(message.padStart(padColCount, " "))
}
// end reusable code


function equalIgnoreCase(correct, provided){
    return correct.toString().toLowerCase() === provided.toString().toLowerCase()
}

async function evaluateQuestion(question, answerOptions, correctAnswer, correctMessage, wrongMessage){
    const answer = await input(question + "\n" + answerOptions + "\n")
    const isCorrect = equalIgnoreCase(correctAnswer, answer)
    println(isCorrect ? correctMessage : wrongMessage)
    return isCorrect ? 1 : 0
}

async function main(){
    let score = 0
    println("LITERATURE QUIZ", "center")
    println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", "center")
    println();println();println()

    score += await evaluateQuestion("IN PINOCCHIO, WHAT WAS THE NAME OF THE CAT?", 
        "1)TIGGER, 2)CICERO, 3)FIGARO, 4)GUIPETTO", 3,
        "VERY GOOD!  HERE'S ANOTHER.", "SORRY...FIGARO WAS HIS NAME.")
    println()

    score += await evaluateQuestion("FROM WHOSE GARDEN DID BUGS BUNNY STEAL THE CARROTS?", 
        "1)MR. NIXON'S, 2)ELMER FUDD'S, 3)CLEM JUDD'S, 4)STROMBOLI'S", 2,
        "PRETTY GOOD!", "TOO BAD...IT WAS ELMER FUDD'S GARDEN.")
    println()

    score += await evaluateQuestion("IN THE WIZARD OF OS, DOROTHY'S DOG WAS NAMED",
        "1)CICERO, 2)TRIXIA, 3)KING, 4)TOTO", 4,
        "YEA!  YOU'RE A REAL LITERATURE GIANT.",
        "BACK TO THE BOOKS,...TOTO WAS HIS NAME.")
    println()

    score += await evaluateQuestion("WHO WAS THE FAIR MAIDEN WHO ATE THE POISON APPLE",
        "1)SLEEPING BEAUTY, 2)CINDERELLA, 3)SNOW WHITE, 4)WENDY", 3,
        "GOOD MEMORY!", "OH, COME ON NOW...IT WAS SNOW WHITE.")

    println();println()

    if(score === 4) {
        println("WOW!  THAT'S SUPER!  YOU REALLY KNOW YOUR NURSERY\n"+
        "YOUR NEXT QUIZ WILL BE ON 2ND CENTURY CHINESE\n"+
        "LITERATURE (HA, HA, HA)")
    } else if(score <= 2){
        println("UGH.  THAT WAS DEFINITELY NOT TOO SWIFT.  BACK TO\n" +
        "NURSERY SCHOOL FOR YOU, MY FRIEND.")
    } else {
        println("NOT BAD, BUT YOU MIGHT SPEND A LITTLE MORE TIME\n"+
        "READING THE NURSERY GREATS.")
    }
}

main()