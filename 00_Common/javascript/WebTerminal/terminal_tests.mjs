#!/usr/bin/env node

import { print, println, tab, input } from '../common.mjs';

async function main() {
  println(tab(20), "Minimal node.js terminal 2");
  println("");
  println(tab(0), "tab 0");
  println(tab(5), "tab 5");
  println(tab(10), "tab 10");
  println(tab(15), "tab 15");
  println(tab(20), "tab 20");
  println(tab(25), "tab 25");
  println("");
  println("1234567890", " ",  "ABCDEFGHIJKLMNOPRSTUVWXYZ");
  println("");
  print("\nHallo"); print(" "); print("Welt!\n");
  println("");
  print("Line 1\nLine 2\nLine 3\nLine 4");
  println("");

  const value = await input("input");
  println(`input value was "${value}"`);

  println("End of script");

  // 320 END
  process.exit(0);
}
main();
