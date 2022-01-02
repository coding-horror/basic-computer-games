// By default:
// — Browsers have a window object
// — Node.js does not
// Checking for an undefined window object is a loose check
// to enable browser and Node.js support
const isRunningInBrowser = typeof window !== 'undefined';

const outputElement = document.querySelector('#output');

function print(string) {
    if (isRunningInBrowser) {
        // Adds trailing newline to match console.log behavior
        document
            .getElementById('output')
            .appendChild(document.createTextNode(string + '\n'));
    } else {
        console.log(string);
    }
}

function input() {
    if (isRunningInBrowser) {
        // Accept input from the browser DOM input
        return new Promise((resolve) => {
            const inputEl = document.createElement('input');
            outputElement.append(inputEl);
            inputEl.focus();

            inputEl.addEventListener('keydown', (event) => {
                if (event.key === 'Enter') {
                    const result = inputEl.value;
                    inputEl.remove();
                    print(result);
                    print('');
                    resolve(result);
                }
            });
        });
    } else {
        // Accept input from the command line in Node.js
        // See: https://nodejs.dev/learn/accept-input-from-the-command-line-in-nodejs
        return new Promise(function (resolve) {
            const readline = require('readline').createInterface({
                input: process.stdin,
                output: process.stdout,
            });
            readline.question('', function (input) {
                resolve(input);
                readline.close();
            });
        });
    }
}

function printInline(string) {
    if (isRunningInBrowser) {
        document
            .getElementById('output')
            .appendChild(document.createTextNode(string));
    } else {
        process.stdout.write(string);
    }
}

function spaces(numberOfSpaces) {
    return ' '.repeat(numberOfSpaces);
}
