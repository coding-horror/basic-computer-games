#!/usr/bin/env node

import { println, tab } from '../../00_Common/javascript/common.mjs';

println(tab(30), "SINE WAVE");
println(tab(15), "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
println("\n".repeat(4));

// REMARKABLE PROGRAM BY DAVID AHL
// Transliterated to Javascript by Les Orchard <me@lmorchard.com>

let toggleWord = true;

for (let step = 0; step < 40; step += 0.25) {
  let indent = Math.floor(26 + 25 * Math.sin(step));
  println(tab(indent), toggleWord ? "CREATIVE" : "COMPUTING");
  toggleWord = !toggleWord;
}
