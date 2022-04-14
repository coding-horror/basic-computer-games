/**
 * Print multible strings to the terminal.
 * Strings get concatinated (add together) without any space betweent them.
 * There will be no newline at the end!
 * If you want a linebrak at the end use `println`.
 * 
 * This function is normally used if you want to put something on the screen 
 * and later add some content to the same line.
 * For normal output (similar to `console.log`) use `println`!
 * 
 * @param  {...string} messages - the strings to print to the terminal.
 */
export function print(...messages) {
	process.stdout.write(messages.join(""));
}

/**
 * Add multible strings as a new line to the terminal.
 * Strings get concatinated (add together) without any space betweent them.
 * There will be a newline at the end!
 * If you want the terminal to stay active on the current line use `print`.
 * 
 * @param  {...any} messages - the strings to print to the terminal.
 */
export function println(...messages) {
	process.stdout.write(messages.join("") + "\n");
}

/**
 * Create an empty string with a given length
 * 
 * @param {number} length - the length of the string in space-characters.
 * @returns {string} returns a string containing only ampty spaces with a length of `count`.
 */
export function tab(length) {
	return " ".repeat(length);
}

/**
 * Read input from the keyboard and return it as a string.
 * TODO: to would be very helpfull to only allow a certain class of input (numbers, letters)
 * TODO: also we could convert all inputs to uppercase (where it makes sence).
 * 
 * @param {string=''} message - a message or question to print befor the input.
 * @returns {Promise<string>} - returns the entered text as a string
 * @async 
 */
export async function input(message = '') {
	/* First we need to print the mesage
	 * We append a space by default to seperate the message from the imput.
	 * TODO: If the message already contains a space at the end this is not needed! */
	process.stdout.write(message + ' ');
	
	return new Promise(resolve => {
		process.stdin.on('data', (input) => {
			/* onData returns a Buffer.
			 * First we need to convert it into a string. */
			const data = input.toString();

			/* add input to terminal
			 * The data should end with a newline! */
			process.stdout.write(data);

			/* The result fo onData is a string ending with an `\n`.
			 * We just need the actual content so let's remove the newline at the end: */
			const content = data.endsWith('\n') ? data.slice(0, -1) : data;

			resolve(content);
		});
	});
}
