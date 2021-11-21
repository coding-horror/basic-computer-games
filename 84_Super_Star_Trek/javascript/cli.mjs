import {
  onExit,
  onPrint,
  onInput,
  setGameOptions,
  getGameState,
  gameMain,
} from "./superstartrek.mjs";

import util from "util";
import readline from "readline";

onExit(function exit() {
  process.exit();
});

onPrint(function print(...messages) {
  console.log(messages.join(""));
});

onInput(async function input(prompt) {
  const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout,
    terminal: false,
  });
  return new Promise((resolve, reject) => {
    rl.question(`${prompt}? `, (response) => {
      rl.close();
      resolve(response);
    });
  });
});

gameMain().then(process.exit).catch(console.log);
