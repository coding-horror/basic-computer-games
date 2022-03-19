#!/usr/bin/env node
/**
 * This script creates an "index.html" file in the root of the directory.
 * 
 * Call this from the root of the project with 
 *  `node ./00_Utilities/build-index.js`
 * 
 * @author Alexander Wunschik <https://github.com/mojoaxel>
 */

const fs = require('fs');
const path = require('path');

const TITLE = 'BASIC Computer Games';
const JAVASCRIPT_FOLDER = 'javascript';
const IGNORE_FOLDERS_START_WITH = ['.', '00_', 'buildJvm', 'Sudoku'];

function createGameLinks(game) {
	const creatFileLink = (file, name = path.basename(file)) => {
		if (file.endsWith('.html')) {
			return `
				<li><a href="${file}">${name.replace('.html', '')}</a></li>
			`;
		} else if (file.endsWith('.mjs')) {
			return `
				<li><a href="./00_Common/javascript/WebTerminal/terminal.html#${file}">${name.replace('.mjs', '')} (node.js)</a></li>
			`;
		} else {
			throw new Error(`Unknown file-type found: ${file}`);
		}
	}

	if (game.files.length > 1) {
		const entries = game.files.map(file => {
			return creatFileLink(file);
		});
		return `
			<li>
				<span>${game.name}</span>
				<ul>${entries.map(e => `\t\t\t${e}`).join('\n')}</ul>
			</li>
		`;
	} else {
		return creatFileLink(game.files[0], game.name);
	}
}

function createIndexHtml(title, games) {
	const listEntries = games.map(game => 
		createGameLinks(game)
	).map(entry => `\t\t\t${entry}`).join('\n');

	const head = `
		<head>
			<meta charset="UTF-8">
			<title>${title}</title>
			<link rel="stylesheet" href="./00_Utilities/javascript/style_terminal.css" />
		</head>
	`;

	const body = `
		<body>
			<article id="output">
				<header>
					<h1>${title}</h1>
				</header>
				<main>
					<ul>
						${listEntries}
					</ul>
				</main>
			</article>
		</body>
	`;

	return `
		<!DOCTYPE html>
		<html lang="en">
		${head}
		${body}
		</html>
	`.trim().replace(/\s\s+/g, '');
}

function findJSFilesInFolder(folder) {
	// filter folders that do not include a subfolder called "javascript"
	const hasJavascript = fs.existsSync(`${folder}/${JAVASCRIPT_FOLDER}`);
	if (!hasJavascript) {
		throw new Error(`Game "${folder}" is missing a javascript folder`);
	}

	// get all files in the javascript folder
	const files = fs.readdirSync(`${folder}/${JAVASCRIPT_FOLDER}`);

	// filter files only allow .html files
	const htmlFiles = files.filter(file => file.endsWith('.html'));
	const mjsFiles = files.filter(file => file.endsWith('.mjs'));
	const entries = [
		...htmlFiles,
		...mjsFiles
	];
		

		
	if (entries.length == 0) {
		throw new Error(`Game "${folder}" is missing a HTML or node.js file in the folder "${folder}/${JAVASCRIPT_FOLDER}"`);
	}

	return entries.map(file => path.join(folder, JAVASCRIPT_FOLDER, file));
}

function main() {
	// Get the list of all folders in the current director
	let folders = fs.readdirSync(process.cwd());

	// filter files only allow folders
	folders = folders.filter(folder => fs.statSync(folder).isDirectory());

	// filter out the folders that start with a dot or 00_
	folders = folders.filter(folder => {
		return !IGNORE_FOLDERS_START_WITH.some(ignore => folder.startsWith(ignore));
	});

	// sort the folders alphabetically (by number)
	folders = folders.sort();

	// get name and javascript file from folder
	const games = folders.map(folder => {
		const name = folder.replace('_', ' ');
		let files;
		
		try {
			files = findJSFilesInFolder(folder);
		} catch (error) {
			console.warn(`Game "${name}" is missing a javascript implementation: ${error.message}`);
			return null;
		}

		return {
			name,
			files
		}
	}).filter(game => game !== null);

	// create a index.html file with a list of all games
	const htmlContent = createIndexHtml(TITLE, games);
	fs.writeFileSync('index.html', htmlContent);
	console.log(`index.html successfully created!`);
}

main();