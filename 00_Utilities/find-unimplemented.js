/**
 * Program to show unimplemented games by language, optionally filtered by
 * language
 *
 * Usage: node find-unimplemented.js [[[lang1] lang2] ...]
 *
 * Adapted from find-missing-implementtion.js
 */

const fs = require("fs");
const glob = require("glob");

// relative path to the repository root
const ROOT_PATH = "../.";

let languages = [
  { name: "csharp", extension: "cs" },
  { name: "java", extension: "java" },
  { name: "javascript", extension: "html" },
  { name: "pascal", extension: "pas" },
  { name: "perl", extension: "pl" },
  { name: "python", extension: "py" },
  { name: "ruby", extension: "rb" },
  { name: "vbnet", extension: "vb" },
];

const getFilesRecursive = async (path, extension) => {
  return new Promise((resolve, reject) => {
    glob(`${path}/**/*.${extension}`, (err, matches) => {
      if (err) {
        reject(err);
      }
      resolve(matches);
    });
  });
};

const getPuzzleFolders = () => {
  return fs
    .readdirSync(ROOT_PATH, { withFileTypes: true })
    .filter((dirEntry) => dirEntry.isDirectory())
    .filter(
      (dirEntry) =>
        ![".git", "node_modules", "00_Utilities", "buildJvm"].includes(dirEntry.name)
    )
    .map((dirEntry) => dirEntry.name);
};

(async () => {
  const result = {};
  if (process.argv.length > 2) {
    languages = languages.filter((language) => process.argv.slice(2).includes(language.name));
  }
  for (const { name: language } of languages) {
    result[language] = [];
  }

  const puzzleFolders = getPuzzleFolders();
  for (const puzzleFolder of puzzleFolders) {
    for (const { name: language, extension } of languages) {
      const files = await getFilesRecursive(
        `${ROOT_PATH}/${puzzleFolder}/${language}`, extension
      );
      if (files.length === 0) {
        result[language].push(puzzleFolder);
      }
    }
  }
  console.log('Unimplementation by language:')
  console.dir(result);
})();

return;
