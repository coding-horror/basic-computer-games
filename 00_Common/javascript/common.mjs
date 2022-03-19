
export function print(...messages) {
    process.stdout.write(messages.join(""));
}

export function println(...messages) {
    process.stdout.write(messages.join("") + "\n");
}

export function tab(count) {
    return " ".repeat(count);
}

export async function input(message = "") {
    process.stdout.write(message + ' ');
    return new Promise(resolve => {
        process.stdin.on('data', (input) => {
            resolve(input.toString().replace('\n', ''));
        });
    });
}
