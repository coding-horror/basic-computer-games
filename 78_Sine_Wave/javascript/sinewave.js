print(tab(30), "SINE WAVE");
print(tab(15), "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
print("\n\n\n\n");

// REMARKABLE PROGRAM BY DAVID AHL
// Transliterated to Javascript by Les Orchard <me@lmorchard.com>

let toggleWord = true;

for (let step = 0; step < 40; step += 0.25) {
  let indent = Math.floor(26 + 25 * Math.sin(step));
  print(tab(indent), toggleWord ? "CREATIVE" : "COMPUTING");
  toggleWord = !toggleWord;
}

function print(...messages) {
  console.log(messages.join(" "));
}

function tab(count) {
  return " ".repeat(count);
}
