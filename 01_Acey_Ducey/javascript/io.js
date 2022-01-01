const outputEl = document.querySelector("#output");

export function print(string) {
  outputEl.append(string);
}

export function readLine() {
  return new Promise(resolve => {
    const inputEl = document.createElement("input");
    outputEl.append(inputEl);
    inputEl.focus();

    inputEl.addEventListener("keydown", event => {
      if (event.key === "Enter") {
        const result = inputEl.value;
        inputEl.remove();

        print(result);
        print("\n");

        resolve(result);
      }
    });
  });
}

export function spaces(numberOfSpaces) {
  return " ".repeat(numberOfSpaces);
}
