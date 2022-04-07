#!/usr/bin/env node

import { println, input } from '../../00_Common/javascript/common.mjs';

function printAlign(message = "", align = "left") {
    // process.stdout.columns is the number of spaces per line in the terminal
    const maxWidth = process.stdout.columns
    if (align === "center") {
        // calculate the amount of spaces required to center the message
        const padColCount = Math.round((process.stdout.columns-message.length)/2);
        const padding = padColCount <= 0 ? '' : ' '.repeat(padColCount);
        println(padding, message);
    } else if (align === "right") {
        const padColCount = Math.round(process.stdout.columns-message.length);
        const padding = padColCount <= 0 ? '' : ' '.repeat(padColCount);
        println(padding, message);
    } else {
        println(message);
    }
}

function equalIgnoreCase(correct, provided){
    return correct.toString().toLowerCase() === provided.toString().toLowerCase()
}

async function evaluateQuestion(question, answerOptions, correctAnswer, correctMessage, wrongMessage){
    // ask the user to answer the given question
    println(question);
    println(answerOptions.map((answer, index) => `${index+1})${answer}`).join(', '));
    // this is a blocking wait
    const answer = await input('?')
    const isCorrect = equalIgnoreCase(correctAnswer, answer)
    println(isCorrect ? correctMessage : wrongMessage)
    return isCorrect ? 1 : 0
}

async function main(){
    let score = 0

    printAlign("LITERATURE QUIZ", "center")
    printAlign("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", "center")
    println("\n\n")

    println("TEST YOUR KNOWLEDGE OF CHILDREN'S LITERATURE.");
    println();
    println("THIS IS A MULTIPLE-CHOICE QUIZ.");
    println("TYPE A 1, 2, 3, OR 4 AFTER THE QUESTION MARK.");
    println();
    println("GOOD LUCK!");
    println("\n\n");

    score += await evaluateQuestion("IN PINOCCHIO, WHAT WAS THE NAME OF THE CAT?",
        [ "TIGGER", "CICERO", "FIGARO", "GUIPETTO"], 3,
        "VERY GOOD!  HERE'S ANOTHER.", "SORRY...FIGARO WAS HIS NAME.")
    println()

    score += await evaluateQuestion("FROM WHOSE GARDEN DID BUGS BUNNY STEAL THE CARROTS?",
        [ "MR. NIXON'S", "ELMER FUDD'S", "CLEM JUDD'S", "STROMBOLI'S" ], 2,
        "PRETTY GOOD!", "TOO BAD...IT WAS ELMER FUDD'S GARDEN.")
    println()

    score += await evaluateQuestion("IN THE WIZARD OF OS, DOROTHY'S DOG WAS NAMED",
        [ "CICERO", "TRIXIA", "KING", "TOTO" ], 4,
        "YEA!  YOU'RE A REAL LITERATURE GIANT.",
        "BACK TO THE BOOKS,...TOTO WAS HIS NAME.")
    println()

    score += await evaluateQuestion("WHO WAS THE FAIR MAIDEN WHO ATE THE POISON APPLE",
        [ "SLEEPING BEAUTY", "CINDERELLA", "SNOW WHITE", "WENDY" ], 3,
        "GOOD MEMORY!", "OH, COME ON NOW...IT WAS SNOW WHITE.")

    println("\n")

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
