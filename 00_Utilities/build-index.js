#!/usr/bin/env node
/**
 * This script creates an "index.html" file in the root of the directory.
 * 
 * Call this from the root of the project with 
 *  `node ./00_Utilities/build-index.js`
 */

const fs = require('fs');
const path = require('path');

const TITLE = 'BASIC Computer Games';
const JAVASCRIPT_FOLDER = 'javascript';
const IGNORE_FOLDERS_START_WITH = ['.', '00_', 'buildJvm', 'Sudoku'];

function createIndexHtml(title, games) {
	const listEntries = games.map(game => 
		`<li><a href="${game.htmlFile}">${game.name}</a></li>`
	).map(entry => `\t\t\t${entry}`).join('\n');

	return `
<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="UTF-8">
	<title>${title}</title>
</head>
<body>
	<header>
		<h1>${title}</h1>
	</header>
	<main>
		<ul>
${listEntries}
		</ul>
	</main>
</body>
</html>
	`.replace(/\t/g, '  ').trim();
}

function findHtmlFileInFolder(folder) {
	// filter folders that do not include a subfolder called "javascript"
	const hasJavascript = fs.existsSync(`${folder}/${JAVASCRIPT_FOLDER}`);
	if (!hasJavascript) {
		throw new Error(`Game "${folder}" is missing a javascript implementation`);
	}

	// get all files in the javascript folder
	const files = fs.readdirSync(`${folder}/${JAVASCRIPT_FOLDER}`);

	// filter files only allow .html files
	const htmlFiles = files.filter(file => file.endsWith('.html'));

	if (htmlFiles.length == 0) {
		throw new Error(`Game "${folder}" is missing a html file in the "${folder}/${JAVASCRIPT_FOLDER}" folder`);
	} else if (htmlFiles.length > 1) {
		console.warn(`Game "${folder}" has multible html files. I'm just taking the first one "${htmlFiles[0]}"`);
	}

	return path.join(folder, JAVASCRIPT_FOLDER, htmlFiles[0]);
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
		let htmlFile;
		
		try {
			htmlFile = findHtmlFileInFolder(folder);
		} catch (error) {
			console.warn(`Game "${name}" is missing a javascript implementation: ${error.message}`);
			return null;
		}

		return {
			name,
			htmlFile
		}
	}).filter(game => game !== null);

	// create a index.html file with a list of all games
	const htmlContent = createIndexHtml(TITLE, games);
	fs.writeFileSync('index.html', htmlContent);
	console.log(`index.html successfully created!`);
}

main();