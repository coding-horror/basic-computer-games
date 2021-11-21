/**
 * SUPER STARTREK - MAY 16,1978 - REQUIRES 24K MEMORY
 *
 *        **** STAR TREK ****        ****
 * SIMULATION OF A MISSION OF THE STARSHIP ENTERPRISE,
 * AS SEEN ON THE STAR TREK TV SHOW.
 * ORIGIONAL PROGRAM BY MIKE MAYFIELD, MODIFIED VERSION
 * PUBLISHED IN DEC'S "101 BASIC GAMES", BY DAVE AHL.
 * MODIFICATIONS TO THE LATTER (PLUS DEBUGGING) BY BOB
 * LEEDOM - APRIL & DECEMBER 1974,
 * WITH A LITTLE HELP FROM HIS FRIENDS . . .
 * COMMENTS, EPITHETS, AND SUGGESTIONS SOLICITED --
 * SEND TO:  R. C. LEEDOM
 *           WESTINGHOUSE DEFENSE & ELECTRONICS SYSTEMS CNTR.
 *           BOX 746, M.S. 338
 *           BALTIMORE, MD  21203
 *
 * CONVERTED TO MICROSOFT 8 K BASIC 3/16/78 BY JOHN GORDERS
 * LINE NUMBERS FROM VERSION STREK7 OF 1/12/75 PRESERVED AS
 * MUCH AS POSSIBLE WHILE USING MULTIPLE STATEMENTS PER LINE
 * SOME LINES ARE LONGER THAN 72 CHARACTERS; THIS WAS DONE
 * BY USING "?" INSTEAD OF "PRINT" WHEN ENTERING LINES
 *
 * Translated and reworked into JavaScript in February 2021
 * by Les Orchard <me@lmorchard.com>
 */

export const setGameOptions = (options = {}) =>
  Object.assign(gameOptions, options);
export const getGameState = () => ({ ...gameState });
export const onPrint = (fn) => (print = fn);
export const onInput = (fn) => (input = fn);
export const onExit = (fn) => (exit = fn);

export async function gameMain() {
  await gameReset();
  await gameLoop();
  await exit();
}

let gameState = {};
let print = () => {};
let input = () => {};
let exit = () => {};

export const gameOptions = {
  stardateStart: Math.floor(Math.random() * 20 + 20) * 100,
  timeLimit: 25 + Math.floor(Math.random() * 10),
  energyMax: 3000,
  photonTorpedoesMax: 10,
  starbaseSpawnChance: 0.96,
  enemyMaxShield: 200,
  enemySpawnChance: [0.8, 0.85, 0.98],
  maxStarsPerSector: 8,
  sectorWidth: 8,
  sectorHeight: 8,
  galaxyWidth: 8,
  galaxyHeight: 8,
  systemDamageChanceOnHit: 0.6,
  systemDamageHitThroughShields: 0.02,
  systemChanceAffectedInWarp: 0.2,
  systemChanceDamageInWarp: 0.6,
  nameEnemy: "KLINGON",
  nameEnemies: "KLINGONS",
  nameScienceOfficer: "SPOCK",
  nameNavigationOfficer: "LT. SULU",
  nameWeaponsOfficer: "ENSIGN CHEKOV",
  nameCommunicationsOfficer: "LT. UHURA",
  nameChiefEngineer: "SCOTT",
  sectorMapSymbols: {
    empty: "   ",
    star: " * ",
    base: ">!<",
    hero: "<*>",
    enemy: "+K+",
  },
  shipSystems: [
    "WARP ENGINES",
    "SHORT RANGE SENSORS",
    "LONG RANGE SENSORS",
    "PHASER CONTROL",
    "PHOTON TUBES",
    "DAMAGE CONTROL",
    "SHIELD CONTROL",
    "LIBRARY-COMPUTER",
  ],
  quadrantNames: [
    [
      "ANTARES",
      "RIGEL",
      "PROCYON",
      "VEGA",
      "CANOPUS",
      "ALTAIR",
      "SAGITTARIUS",
      "POLLUX",
    ],
    [
      "SIRIUS",
      "DENEB",
      "CAPELLA",
      "BETELGEUSE",
      "ALDEBARAN",
      "REGULUS",
      "ARCTURUS",
      "SPICA",
    ],
  ],
  quadrantNumbers: ["I", "II", "III", "IV"],
};

let SYSTEM_WARP_ENGINES,
  SYSTEM_SHORT_RANGE_SENSORS,
  SYSTEM_LONG_RANGE_SENSORS,
  SYSTEM_PHASER_CONTROL,
  SYSTEM_PHOTON_TUBES,
  SYSTEM_DAMAGE_CONTROL,
  SYSTEM_SHIELD_CONTROL,
  SYSTEM_LIBRARY_COMPUTER;

async function gameIntro() {
  print("\n".repeat(10));
  print("                                    ,------*------,");
  print("                    ,-------------   '---  ------'");
  print("                     '-------- --'      / /");
  print("                         ,---' '-------/ /--,");
  print("                          '----------------'");
  print("");
  print("                    THE USS ENTERPRISE --- NCC-1701");
  print("\n".repeat(4));

  print("YOUR ORDERS ARE AS FOLLOWS:");
  print();
  print(
    `  DESTROY THE ${gameState.enemiesRemaining} ${gameOptions.nameEnemy} WARSHIPS WHICH HAVE INVADED`
  );
  print("  THE GALAXY BEFORE THEY CAN ATTACK FEDERATION HEADQUARTERS");
  print(
    `  ON STARDATE ${formatStardate(
      gameOptions.stardateStart + gameOptions.timeLimit
    )} THIS GIVES YOU ${gameOptions.timeLimit} DAYS.  THERE${
      gameState.starbasesRemaining > 1 ? " ARE " : " IS "
    }`
  );
  print(
    `  ${gameState.starbasesRemaining} STARBASE${
      gameState.starbasesRemaining > 1 ? "S" : " ARE"
    } IN THE GALAXY FOR RESUPPLYING YOUR SHIP`
  );
}

async function gameReset() {
  [
    SYSTEM_WARP_ENGINES,
    SYSTEM_SHORT_RANGE_SENSORS,
    SYSTEM_LONG_RANGE_SENSORS,
    SYSTEM_PHASER_CONTROL,
    SYSTEM_PHOTON_TUBES,
    SYSTEM_DAMAGE_CONTROL,
    SYSTEM_SHIELD_CONTROL,
    SYSTEM_LIBRARY_COMPUTER,
  ] = gameOptions.shipSystems;

  gameState = {
    gameOver: false,
    gameWon: false,
    gameQuit: false,
    destroyed: false,
    shouldRestart: false,
    sectorMap: "",
    alertCondition: "",
    stardateCurrent: gameOptions.stardateStart,
    isDocked: false,
    energyRemaining: gameOptions.energyMax,
    photonTorpedoesRemaining: gameOptions.photonTorpedoesMax,
    shieldsCurrent: 0,
    starbasesRemaining: 0,
    enemiesRemaining: 0,
    quadrantPositionY: randomInt(gameOptions.galaxyHeight, 1),
    quadrantPositionX: randomInt(gameOptions.galaxyWidth, 1),
    sectorPositionY: randomInt(gameOptions.sectorHeight, 1),
    sectorPositionX: randomInt(gameOptions.sectorWidth, 1),
    sectorEnemiesCount: 0,
    sectorStarbasesCount: 0,
    sectorStarsCount: 0,
    galacticMap: [],
    galacticMapDiscovered: [],
  };

  gameState.systemsDamage = {};
  for (const systemName of gameOptions.shipSystems) {
    gameState.systemsDamage[systemName] = 0;
  }

  for (let mapY = 1; mapY <= gameOptions.galaxyHeight; mapY++) {
    gameState.galacticMap[mapY] = [];
    gameState.galacticMapDiscovered[mapY] = [];
    for (let mapX = 1; mapX <= gameOptions.galaxyWidth; mapX++) {
      gameState.galacticMapDiscovered[mapY][mapX] = 0;

      gameState.sectorEnemiesCount = 0;

      const enemySpawnRoll = Math.random();
      if (enemySpawnRoll > gameOptions.enemySpawnChance[2]) {
        gameState.sectorEnemiesCount = 3;
        gameState.enemiesRemaining = gameState.enemiesRemaining + 3;
      } else if (enemySpawnRoll > gameOptions.enemySpawnChance[1]) {
        gameState.sectorEnemiesCount = 2;
        gameState.enemiesRemaining = gameState.enemiesRemaining + 2;
      } else if (enemySpawnRoll > gameOptions.enemySpawnChance[0]) {
        gameState.sectorEnemiesCount = 1;
        gameState.enemiesRemaining = gameState.enemiesRemaining + 1;
      }

      gameState.sectorStarbasesCount = 0;
      if (Math.random() > gameOptions.starbaseSpawnChance) {
        gameState.sectorStarbasesCount = 1;
        gameState.starbasesRemaining = gameState.starbasesRemaining + 1;
      }

      // 1040
      gameState.galacticMap[mapY][mapX] =
        gameState.sectorEnemiesCount * 100 +
        gameState.sectorStarbasesCount * 10 +
        randomInt(gameOptions.maxStarsPerSector, 1);
    }
  }

  if (gameState.enemiesRemaining > gameOptions.timeLimit) {
    // Ensure the player has at least one more stardate than the number of enemies
    gameOptions.timeLimit = gameState.enemiesRemaining + 1;
  }

  if (gameState.starbasesRemaining === 0) {
    if (
      gameState.galacticMap[gameState.quadrantPositionY][
        gameState.quadrantPositionX
      ] < 200
    ) {
      gameState.galacticMap[gameState.quadrantPositionY][
        gameState.quadrantPositionX
      ] =
        gameState.galacticMap[gameState.quadrantPositionY][
          gameState.quadrantPositionX
        ] + 120;
    }
    gameState.enemiesRemaining = gameState.enemiesRemaining + 1;
    gameState.starbasesRemaining = 1;
    gameState.galacticMap[gameState.quadrantPositionY][
      gameState.quadrantPositionX
    ] =
      gameState.galacticMap[gameState.quadrantPositionY][
        gameState.quadrantPositionX
      ] + 10;
    gameState.quadrantPositionY = randomInt(gameOptions.galaxyHeight, 1);
    gameState.quadrantPositionX = randomInt(gameOptions.galaxyWidth, 1);
  }

  gameState.enemiesInitialCount = gameState.enemiesRemaining;

  await gameIntro();
  await newQuadrantEntered();
}

async function newQuadrantEntered() {
  gameState.sectorEnemiesCount = 0;
  gameState.sectorStarbasesCount = 0;
  gameState.sectorStarsCount = 0;
  gameState.starbaseRepairDelay = 0.5 * Math.random();

  // Add this sector to the known map
  gameState.galacticMapDiscovered[gameState.quadrantPositionY][
    gameState.quadrantPositionX
  ] =
    gameState.galacticMap[gameState.quadrantPositionY][
      gameState.quadrantPositionX
    ];

  // Initialize a sector enemy for each that had a chance to spawn
  gameState.sectorEnemies = gameOptions.enemySpawnChance.map((c) => ({
    health: 0,
    posY: 0,
    posX: 0,
  }));

  if (
    gameState.quadrantPositionY >= 1 &&
    gameState.quadrantPositionY <= gameOptions.galaxyHeight &&
    gameState.quadrantPositionX >= 1 &&
    gameState.quadrantPositionX <= gameOptions.galaxyWidth
  ) {
    let currentQuadrantName = buildQuadrantName(
      gameState.quadrantPositionY,
      gameState.quadrantPositionX
    );
    print();
    if (gameOptions.stardateStart == gameState.stardateCurrent) {
      print("YOUR MISSION BEGINS WITH YOUR STARSHIP LOCATED");
      print(`IN THE GALACTIC QUADRANT, '${currentQuadrantName}'.`);
    } else {
      print(`NOW ENTERING ${currentQuadrantName} QUADRANT . . .`);
    }
    print();
    gameState.sectorEnemiesCount = Math.floor(
      gameState.galacticMap[gameState.quadrantPositionY][
        gameState.quadrantPositionX
      ] * 0.01
    );
    gameState.sectorStarbasesCount =
      Math.floor(
        gameState.galacticMap[gameState.quadrantPositionY][
          gameState.quadrantPositionX
        ] * 0.1
      ) -
      10 * gameState.sectorEnemiesCount;
    gameState.sectorStarsCount =
      gameState.galacticMap[gameState.quadrantPositionY][
        gameState.quadrantPositionX
      ] -
      100 * gameState.sectorEnemiesCount -
      10 * gameState.sectorStarbasesCount;

    if (gameState.sectorEnemiesCount != 0) {
      print("COMBAT AREA      CONDITION RED");
      if (gameState.shieldsCurrent <= 200) {
        print("   SHIELDS DANGEROUSLY LOW");
      }
    }

    for (
      let enemyIdx = 0;
      enemyIdx < gameOptions.enemySpawnChance.length;
      enemyIdx++
    ) {
      gameState.sectorEnemies[enemyIdx].posY = 0;
      gameState.sectorEnemies[enemyIdx].posX = 0;
    }
  }

  for (
    let enemyIdx = 0;
    enemyIdx < gameOptions.enemySpawnChance.length;
    enemyIdx++
  ) {
    gameState.sectorEnemies[enemyIdx].health = 0;
  }

  gameState.sectorMap = " ".repeat(
    gameOptions.sectorMapSymbols.empty.length *
      gameOptions.sectorHeight *
      gameOptions.sectorWidth
  );

  insertInSectorMap(
    gameOptions.sectorMapSymbols.hero,
    gameState.sectorPositionY,
    gameState.sectorPositionX
  );

  if (gameState.sectorEnemiesCount >= 1) {
    // 1720
    for (
      let enemyIdx = 0;
      enemyIdx < gameState.sectorEnemiesCount;
      enemyIdx++
    ) {
      const [posY, posX] = findSpaceInSectorMap();
      insertInSectorMap(gameOptions.sectorMapSymbols.enemy, posY, posX);
      gameState.sectorEnemies[enemyIdx] = {
        posY,
        posX,
        health: gameOptions.enemyMaxShield * (0.5 + Math.random()),
      };
    }
  }

  if (gameState.sectorStarbasesCount >= 1) {
    const [R1, R2] = findSpaceInSectorMap();
    gameState.sectorStarbaseY = R1;
    gameState.sectorStarbaseX = R2;
    insertInSectorMap(gameOptions.sectorMapSymbols.base, R1, R2);
  }

  for (let i = 1; i <= gameState.sectorStarsCount; i++) {
    insertInSectorMap(
      gameOptions.sectorMapSymbols.star,
      ...findSpaceInSectorMap()
    );
  }

  return shortRangeSensorScanAndStartup();
}

async function gameLoop() {
  while (!gameState.gameOver) {
    await acceptCommand();
    if (gameState.gameOver) {
      await endOfGame();
    }
    if (gameState.shouldRestart) {
      await gameReset();
    }
  }
}

async function acceptCommand() {
  if (
    gameState.shieldsCurrent + gameState.energyRemaining <= 10 ||
    (gameState.energyRemaining < 10 &&
      gameState.systemsDamage[SYSTEM_SHIELD_CONTROL] != 0)
  ) {
    print();
    print("** FATAL ERROR **   YOU'VE JUST STRANDED YOUR SHIP IN SPACE");
    print("YOU HAVE INSUFFICIENT MANEUVERING ENERGY, AND SHIELD CONTROL");
    print("IS PRESENTLY INCAPABLE OF CROSS-CIRCUITING TO ENGINE ROOM!!");
    print();
    gameState.gameOver = true;
    return;
  }

  const commandInput = (await input("COMMAND")).trim().toUpperCase();
  const command = COMMANDS[commandInput] || commandHelp;
  await command();
}

/************************************************************************/

const COMMANDS = {
  NAV: commandCourseControl,
  SRS: shortRangeSensorScanAndStartup,
  LRS: commandLongRangeScan,
  PHA: commandPhaserControl,
  TOR: commandPhotonTorpedo,
  SHE: commandShieldControl,
  DAM: commandDamageControl,
  COM: commandLibraryComputer,
  XXX: () => {
    // todo more confirmation here?
    gameState.gameOver = true;
    gameState.gameQuit = true;
  },
  DUMP: () => {
    console.log(JSON.stringify(gameState));
  },
};

async function commandHelp() {
  print("ENTER ONE OF THE FOLLOWING:");
  print("  NAV  (TO SET COURSE)");
  print("  SRS  (FOR SHORT RANGE SENSOR SCAN)");
  print("  LRS  (FOR LONG RANGE SENSOR SCAN)");
  print("  PHA  (TO FIRE PHASERS)");
  print("  TOR  (TO FIRE PHOTON TORPEDOES)");
  print("  SHE  (TO RAISE OR LOWER SHIELDS)");
  print("  DAM  (FOR DAMAGE CONTROL REPORTS)");
  print("  COM  (TO CALL ON LIBRARY-COMPUTER)");
  print("  XXX  (TO RESIGN YOUR COMMAND)");
  print();
}

async function shortRangeSensorScanAndStartup() {
  checkIfDocked();

  if (gameState.isDocked) {
    gameState.alertCondition = "DOCKED";
    gameState.energyRemaining = gameOptions.energyMax;
    gameState.photonTorpedoesRemaining = gameOptions.photonTorpedoesMax;
    print("SHIELDS DROPPED FOR DOCKING PURPOSES");
    gameState.shieldsCurrent = 0;
  } else {
    gameState.alertCondition = "GREEN";
    if (gameState.energyRemaining < gameOptions.energyMax * 0.1)
      gameState.alertCondition = "YELLOW";
    if (gameState.sectorEnemiesCount > 0) gameState.alertCondition = "RED";
  }

  if (gameState.systemsDamage[SYSTEM_SHORT_RANGE_SENSORS] < 0) {
    print();
    print("*** SHORT RANGE SENSORS ARE OUT ***");
    print();
    return;
  }

  const statusLines = [
    `STARDATE           ${formatStardate(gameState.stardateCurrent)}`,
    `CONDITION          ${gameState.alertCondition}`,
    `QUADRANT           ${gameState.quadrantPositionY} , ${gameState.quadrantPositionX}`,
    `SECTOR             ${gameState.sectorPositionY} , ${gameState.sectorPositionX}`,
    `PHOTON TORPEDOES   ${gameState.photonTorpedoesRemaining}`,
    `TOTAL ENERGY       ${
      gameState.energyRemaining + gameState.shieldsCurrent
    }`,
    `SHIELDS            ${gameState.shieldsCurrent}`,
    `${gameOptions.nameEnemies} REMAINING ${gameState.enemiesRemaining}`,
  ];

  const lineSplit = new RegExp(
    `.{${gameOptions.sectorMapSymbols.empty.length * gameOptions.sectorWidth}}`,
    "g"
  );
  const cellSplit = new RegExp(
    `.{${gameOptions.sectorMapSymbols.empty.length}}`,
    "g"
  );
  const borderLine = "-".repeat(
    (gameOptions.sectorMapSymbols.empty.length + 1) * gameOptions.sectorWidth
  );
  print(borderLine);
  print(
    gameState.sectorMap
      // Split the map into lines of 24 chars
      .match(lineSplit)
      // Split each line into cells of 3 chars
      .map((line) => line.match(cellSplit))
      // Format each line with Y coord, spaced out cells, and a line of status
      .map((line, idx) => line.join(" ") + " ".repeat(4) + statusLines[idx])
      // Finally, join all the lines with returns
      .join("\n")
  );
  print(borderLine);
  print();
}

function checkIfDocked() {
  const { sectorPositionY: sY, sectorPositionX: sX } = gameState;
  for (let posY = sY - 1; posY <= sY + 1; posY++) {
    for (let posX = sX - 1; posX <= sX + 1; posX++) {
      if (
        posY >= 1 ||
        posY <= gameOptions.sectorHeight ||
        posX >= 1 ||
        posX <= gameOptions.sectorWidth
      ) {
        if (findInsectorMap(gameOptions.sectorMapSymbols.base, posY, posX)) {
          gameState.isDocked = true;
          return;
        }
      }
    }
  }
  gameState.isDocked = false;
}

async function commandCourseControl() {
  let courseInput = parseFloat(await input("COURSE (0-9)"));
  if (courseInput == 9) courseInput = 1;
  if (isNaN(courseInput) || courseInput < 1 || courseInput > 9) {
    print(
      `   ${gameOptions.nameNavigationOfficer} REPORTS, 'INCORRECT COURSE DATA, SIR!'`
    );
    return;
  }

  const warpFactorInput = parseFloat(
    await input(
      `WARP FACTOR (0-${
        gameState.systemsDamage[SYSTEM_WARP_ENGINES] < 0 ? "0.2" : "8"
      })`
    )
  );
  if (warpFactorInput == 0 || isNaN(warpFactorInput)) return;
  if (
    gameState.systemsDamage[SYSTEM_WARP_ENGINES] < 0 &&
    warpFactorInput > 0.2
  ) {
    return print("WARP ENGINES ARE DAMAGED.  MAXIMUM SPEED = WARP 0.2");
  }
  if (warpFactorInput < 0 && warpFactorInput > 8) {
    return print(
      `   CHIEF ENGINEER ${gameOptions.nameChiefEngineer} REPORTS 'THE ENGINES WON'T TAKE WARP ${warpFactorInput}!'`
    );
  }

  // FIXME: This seems to depend on square sectors - which we have, but could be changed in config
  const sectorsToWarp = Math.floor(warpFactorInput * gameOptions.sectorWidth + 0.5);

  if (gameState.energyRemaining - sectorsToWarp < 0) {
    print("ENGINEERING REPORTS   'INSUFFICIENT ENERGY AVAILABLE");
    print(
      "                       FOR MANEUVERING AT WARP ",
      warpFactorInput,
      " !'"
    );
    if (
      gameState.shieldsCurrent > sectorsToWarp - gameState.energyRemaining &&
      gameState.systemsDamage[SYSTEM_SHIELD_CONTROL] > 0
    ) {
      print(
        "DEFLECTOR CONTROL ROOM ACKNOWLEDGES ",
        gameState.shieldsCurrent,
        " UNITS OF ENERGY"
      );
      print("                         PRESENTLY DEPLOYED TO SHIELDS.");
    }
  }

  for (
    let enemyIdx = 0;
    enemyIdx < gameOptions.enemySpawnChance.length;
    enemyIdx++
  ) {
    if (gameState.sectorEnemies[enemyIdx].health > 0) {
      insertInSectorMap(
        gameOptions.sectorMapSymbols.empty,
        gameState.sectorEnemies[enemyIdx].posY,
        gameState.sectorEnemies[enemyIdx].posX
      );
      const [rY, rX] = findSpaceInSectorMap();
      gameState.sectorEnemies[enemyIdx].posY = rY;
      gameState.sectorEnemies[enemyIdx].posX = rX;
      insertInSectorMap(
        gameOptions.sectorMapSymbols.enemy,
        gameState.sectorEnemies[enemyIdx].posY,
        gameState.sectorEnemies[enemyIdx].posX
      );
    }
  }

  enemiesShoot();

  let damageControlHeaderPrinted = false;
  const printDamageReport = (msg) => {
    if (!damageControlHeaderPrinted) {
      damageControlHeaderPrinted = true;
      print("DAMAGE CONTROL REPORT:");
    }
    print(msg);
  };

  let repairFactorDuringWarp = Math.min(1, warpFactorInput);

  // Continually repair damaged systems during warp
  for (const systemName of gameOptions.shipSystems) {
    if (gameState.systemsDamage[systemName] >= 0) continue;

    gameState.systemsDamage[systemName] =
      gameState.systemsDamage[systemName] + repairFactorDuringWarp;

    if (
      gameState.systemsDamage[systemName] > -0.1 &&
      gameState.systemsDamage[systemName] < 0
    ) {
      gameState.systemsDamage[systemName] = -0.1;
      continue;
    }

    if (gameState.systemsDamage[systemName] < 0) continue;

    printDamageReport(`        ${systemName} REPAIR COMPLETED.`);
  }

  // 20% chance of a random system being damaged, repaired, or improved in warp
  if (Math.random() < gameOptions.systemChanceAffectedInWarp) {
    const systemIdx = randomInt(gameOptions.shipSystems.length);
    const systemName = gameOptions.shipSystems[systemIdx];

    if (Math.random() < gameOptions.systemChanceDamageInWarp) {
      // 60% chance of random system damage
      gameState.systemsDamage[systemName] =
        gameState.systemsDamage[systemName] - (Math.random() * 5 + 1);
      printDamageReport(`        ${systemName} DAMAGED`);
    } else {
      // 40% chance of random system repair or improvement
      gameState.systemsDamage[systemName] =
        gameState.systemsDamage[systemName] + Math.random() * 3 + 1;
      printDamageReport(`        ${systemName} STATE OF REPAIR IMPROVED`);
    }
    print();
  }

  // 3060 REM BEGIN MOVING STARSHIP
  insertInSectorMap(
    gameOptions.sectorMapSymbols.empty,
    Math.floor(gameState.sectorPositionY),
    Math.floor(gameState.sectorPositionX)
  );

  const [courseDeltaY, courseDeltaX] = courseToDeltaXY(courseInput);
  let currentSectorPositionY = gameState.sectorPositionY;
  let currentSectorPositionX = gameState.sectorPositionX;
  let currentQuadrantPosY = gameState.quadrantPositionY;
  let currentQuadrantPosX = gameState.quadrantPositionX;

  for (let sectorsWarped = 1; sectorsWarped < sectorsToWarp; sectorsWarped++) {
    gameState.sectorPositionY = gameState.sectorPositionY + courseDeltaY;
    gameState.sectorPositionX = gameState.sectorPositionX + courseDeltaX;

    if (
      gameState.sectorPositionY < 1 ||
      gameState.sectorPositionY >= 9 ||
      gameState.sectorPositionX < 1 ||
      gameState.sectorPositionX >= 9
    ) {
      // 3490 REM EXCEEDED QUADRANT LIMITS
      currentSectorPositionY =
        gameOptions.sectorHeight * gameState.quadrantPositionY +
        currentSectorPositionY +
        sectorsToWarp * courseDeltaY;

      currentSectorPositionX =
        gameOptions.sectorWidth * gameState.quadrantPositionX +
        currentSectorPositionX +
        sectorsToWarp * courseDeltaX;

      gameState.quadrantPositionY = Math.floor(currentSectorPositionY / 8);
      gameState.quadrantPositionX = Math.floor(currentSectorPositionX / 8);

      gameState.sectorPositionY = Math.floor(
        currentSectorPositionY - gameState.quadrantPositionY * 8
      );
      gameState.sectorPositionX = Math.floor(
        currentSectorPositionX - gameState.quadrantPositionX * 8
      );

      if (gameState.sectorPositionY == 0) {
        gameState.quadrantPositionY = gameState.quadrantPositionY - 1;
        gameState.sectorPositionY = 8;
      }
      if (gameState.sectorPositionX == 0) {
        gameState.quadrantPositionX = gameState.quadrantPositionX - 1;
        gameState.sectorPositionX = 8;
      }

      let galacticPerimeterHit = false;
      if (gameState.quadrantPositionY < 1) {
        galacticPerimeterHit = true;
        gameState.quadrantPositionY = 1;
        gameState.sectorPositionY = 1;
      }
      if (gameState.quadrantPositionY > 8) {
        galacticPerimeterHit = true;
        gameState.quadrantPositionY = 8;
        gameState.sectorPositionY = 8;
      }
      if (gameState.quadrantPositionX < 1) {
        galacticPerimeterHit = true;
        gameState.quadrantPositionX = 1;
        gameState.sectorPositionX = 1;
      }
      if (gameState.quadrantPositionX > 8) {
        galacticPerimeterHit = true;
        gameState.quadrantPositionX = 8;
        gameState.sectorPositionX = 8;
      }

      if (galacticPerimeterHit) {
        print(
          `${gameOptions.nameCommunicationsOfficer} REPORTS MESSAGE FROM STARFLEET COMMAND:`
        );
        print("  'PERMISSION TO ATTEMPT CROSSING OF GALACTIC PERIMETER");
        print("  IS HEREBY *DENIED*.  SHUT DOWN YOUR ENGINES.'");
        print(
          `CHIEF ENGINEER ${gameOptions.nameChiefEngineer} REPORTS  'WARP ENGINES SHUT DOWN`
        );
        print(
          `  AT SECTOR ${gameState.sectorPositionY} , ${gameState.sectorPositionX} OF QUADRANT ${gameState.quadrantPositionY} , ${gameState.quadrantPositionX}.'`
        );

        if (checkIfTimeExpired()) {
          return;
        }
      }

      if (
        gameOptions.sectorHeight * gameState.quadrantPositionY + gameState.quadrantPositionX ==
        gameOptions.sectorHeight * currentQuadrantPosY + currentQuadrantPosX
      ) {
        break;
      }

      gameState.stardateCurrent = gameState.stardateCurrent + 1;
      consumeEnergyForWarp(sectorsToWarp);
      return newQuadrantEntered();
    }

    if (
      !findInsectorMap(
        gameOptions.sectorMapSymbols.empty,
        gameState.sectorPositionY,
        gameState.sectorPositionX
      )
    ) {
      // Undo this step of warp travel if the space isn't empty
      gameState.sectorPositionY = Math.floor(
        gameState.sectorPositionY - courseDeltaY
      );
      gameState.sectorPositionX = Math.floor(
        gameState.sectorPositionX - courseDeltaX
      );
      print(
        `WARP ENGINES SHUT DOWN AT SECTOR ${gameState.sectorPositionY} , ${gameState.sectorPositionX} DUE TO BAD NAVAGATION`
      );
      break;
    }
  }

  gameState.sectorPositionY = Math.floor(gameState.sectorPositionY);
  gameState.sectorPositionX = Math.floor(gameState.sectorPositionX);

  insertInSectorMap(
    gameOptions.sectorMapSymbols.hero,
    Math.floor(gameState.sectorPositionY),
    Math.floor(gameState.sectorPositionX)
  );

  consumeEnergyForWarp(sectorsToWarp);

  let timeElapsedDuringWarp = 1;
  if (warpFactorInput < 1) {
    timeElapsedDuringWarp = 0.1 * Math.floor(10 * warpFactorInput);
  }

  gameState.stardateCurrent = gameState.stardateCurrent + timeElapsedDuringWarp;
  if (checkIfTimeExpired()) {
    return;
  }

  await shortRangeSensorScanAndStartup();
}

function checkIfTimeExpired() {
  if (
    gameState.stardateCurrent >
    gameOptions.stardateStart + gameOptions.timeLimit
  ) {
    gameState.gameOver = true;
  }
  return gameState.gameOver;
}

function consumeEnergyForWarp(sectorsToWarp) {
  // 3900 REM MANEUVER ENERGY S/R **
  gameState.energyRemaining = gameState.energyRemaining - sectorsToWarp - 10;
  if (gameState.energyRemaining >= 0) {
    return;
  }

  print("SHIELD CONTROL SUPPLIES ENERGY TO COMPLETE THE MANEUVER.");
  gameState.shieldsCurrent =
    gameState.shieldsCurrent + gameState.energyRemaining;
  gameState.energyRemaining = 0;
  if (gameState.shieldsCurrent <= 0) {
    gameState.shieldsCurrent = 0;
  }
}

async function commandLongRangeScan() {
  // 3990 REM LONG RANGE SENSOR SCAN CODE
  if (gameState.systemsDamage[SYSTEM_LONG_RANGE_SENSORS] < 0) {
    print("LONG RANGE SENSORS ARE INOPERABLE");
    return;
  }

  print(
    "LONG RANGE SCAN FOR QUADRANT ",
    gameState.quadrantPositionY,
    " , ",
    gameState.quadrantPositionX
  );

  const separatorLine = "-------------------";
  print(separatorLine);

  for (
    let posY = gameState.quadrantPositionY - 1;
    posY <= gameState.quadrantPositionY + 1;
    posY++
  ) {
    // Scan a line of sectors
    const lineSectors = [null, null, null];
    for (
      let posX = gameState.quadrantPositionX - 1;
      posX <= gameState.quadrantPositionX + 1;
      posX++
    ) {
      if (posY > 0 && posY < 9 && posX > 0 && posX < 9) {
        // Add the scanned cell to the current scan output
        lineSectors[posX - gameState.quadrantPositionX + 1] =
          gameState.galacticMap[posY][posX];
        // Add the scanned cell to the discovered map
        gameState.galacticMapDiscovered[posY][posX] =
          gameState.galacticMap[posY][posX];
      }
    }

    // Print a formatted line of the scan - e.g. ": 004 : 205 : 004 :"
    print(
      ": " +
        lineSectors
          .map((sector) =>
            sector === null ? "***" : sector.toString().padStart(3, "0")
          )
          .join(" : ") +
        " :"
    );

    print(separatorLine);
  }
}

async function commandPhaserControl() {
  // 4250 REM PHASER CONTROL CODE BEGINS HERE
  if (gameState.systemsDamage[SYSTEM_PHASER_CONTROL] < 0) {
    print("PHASERS INOPERATIVE");
    return;
  }

  if (gameState.sectorEnemiesCount <= 0) {
    print(
      `SCIENCE OFFICER ${gameOptions.nameScienceOfficer} REPORTS  'SENSORS SHOW NO ENEMY SHIPS`
    );
    print("                                IN THIS QUADRANT'");
    return;
  }

  if (gameState.systemsDamage[SYSTEM_LIBRARY_COMPUTER] < 0) {
    print("COMPUTER FAILURE HAMPERS ACCURACY");
  }

  print(
    "PHASERS LOCKED ON TARGET;  ENERGY AVAILABLE = ",
    gameState.energyRemaining,
    " UNITS"
  );
  let phaserUnitsToFire;
  const continueCommandLoop = true;
  while (continueCommandLoop) {
    phaserUnitsToFire = parseFloat(await input("NUMBER OF UNITS TO FIRE"));
    if (phaserUnitsToFire <= 0) return;
    if (gameState.energyRemaining - phaserUnitsToFire >= 0) {
      break;
    }
    print(`ENERGY AVAILABLE = ${gameState.energyRemaining} UNITS`);
  }

  gameState.energyRemaining = gameState.energyRemaining - phaserUnitsToFire;

  // FIXED: in the original, this was shield system. Changed to phaser system.
  if (gameState.systemsDamage[SYSTEM_PHASER_CONTROL] < 0) {
    phaserUnitsToFire = phaserUnitsToFire * Math.random();
  }

  // Spread phaser fire between all enemies
  let phaserUnitsPerEnemy = Math.floor(
    phaserUnitsToFire / gameState.sectorEnemiesCount
  );
  for (
    let enemyIdx = 0;
    enemyIdx < gameOptions.enemySpawnChance.length;
    enemyIdx++
  ) {
    if (gameState.sectorEnemies[enemyIdx].health <= 0) {
      // Skip dead enemies
      continue;
    }
    print();

    // Phaser damage falls off based on distance and a bit of chance
    let phaserDamage = Math.floor(
      (phaserUnitsPerEnemy / distanceFromEnemy(enemyIdx)) * (Math.random() + 2)
    );
    if (phaserDamage <= 0.15 * gameState.sectorEnemies[enemyIdx].health) {
      print(
        "SENSORS SHOW NO DAMAGE TO ENEMY AT ",
        gameState.sectorEnemies[enemyIdx].posY,
        " , ",
        gameState.sectorEnemies[enemyIdx].posX
      );
      continue;
    }
    gameState.sectorEnemies[enemyIdx].health -= phaserDamage;

    print(
      `${phaserDamage} UNIT HIT ON ${gameOptions.nameEnemy} AT SECTOR ${gameState.sectorEnemies[enemyIdx].posY} , ${gameState.sectorEnemies[enemyIdx].posX}`
    );

    if (gameState.sectorEnemies[enemyIdx].health > 0) {
      print(
        `   (SENSORS SHOW ${gameState.sectorEnemies[enemyIdx].health} UNITS REMAINING)`
      );
      print();
    } else {
      print(`*** ${gameOptions.nameEnemy} DESTROYED ***`);
      print();
      gameState.sectorEnemiesCount = gameState.sectorEnemiesCount - 1;
      gameState.enemiesRemaining = gameState.enemiesRemaining - 1;

      // Remove enemy from display
      insertInSectorMap(
        gameOptions.sectorMapSymbols.empty,
        gameState.sectorEnemies[enemyIdx].posY,
        gameState.sectorEnemies[enemyIdx].posX
      );

      // Set enemy health at exactly zero
      gameState.sectorEnemies[enemyIdx].health = 0;

      // Update the galactic map with one fewer enemy
      gameState.galacticMap[gameState.quadrantPositionY][
        gameState.quadrantPositionX
      ] -= 100;

      // Copy updated galactic map sector to discovered map.
      gameState.galacticMapDiscovered[gameState.quadrantPositionY][
        gameState.quadrantPositionX
      ] =
        gameState.galacticMap[gameState.quadrantPositionY][
          gameState.quadrantPositionX
        ];

      if (gameState.enemiesRemaining <= 0) {
        // If that was the last enemy, we've won!
        gameState.gameOver = true;
        gameState.gameWon = true;
        return;
      }
    }
  }

  enemiesShoot();
}

async function commandPhotonTorpedo() {
  // 4690 REM PHOTON TORPEDO CODE BEGINS HERE
  // 4700
  if (gameState.photonTorpedoesRemaining <= 0) {
    return print("ALL PHOTON TORPEDOES EXPENDED");
  }
  if (gameState.systemsDamage[SYSTEM_PHOTON_TUBES] < 0) {
    return print("PHOTON TUBES ARE NOT OPERATIONAL");
  }

  let torpedoCourse = parseFloat(await input("PHOTON TORPEDO COURSE (1-9)"));
  if (torpedoCourse == 9) torpedoCourse = 1;

  if (torpedoCourse < 1 || torpedoCourse > 9) {
    print(
      `${gameOptions.nameWeaponsOfficer} REPORTS,  'INCORRECT COURSE DATA, SIR!'`
    );
  }

  const [courseDeltaY, courseDeltaX] = courseToDeltaXY(torpedoCourse);

  gameState.energyRemaining = gameState.energyRemaining - 2;
  gameState.photonTorpedoesRemaining = gameState.photonTorpedoesRemaining - 1;
  let currPosY = gameState.sectorPositionY;
  let currPosX = gameState.sectorPositionX;

  print("TORPEDO TRACK:");

  // Fly the torpedo along its course...
  let quantizedPosY, quantizedPosX;
  const forever = true;
  while (forever) {
    currPosY = currPosY + courseDeltaY;
    currPosX = currPosX + courseDeltaX;

    // The course will move in decimals, quantize to whole numbers
    quantizedPosY = Math.floor(currPosY + 0.5);
    quantizedPosX = Math.floor(currPosX + 0.5);

    // Exiting the sector means the torpedo missed
    if (
      quantizedPosY < 1 ||
      quantizedPosY > gameOptions.sectorHeight ||
      quantizedPosX < 1 ||
      quantizedPosX > gameOptions.sectorWidth
    ) {
      print("TORPEDO MISSED");
      return enemiesShoot();
    }

    print(`               ${quantizedPosY} , ${quantizedPosX}`);

    if (
      !findInsectorMap(
        gameOptions.sectorMapSymbols.empty,
        quantizedPosY,
        quantizedPosX
      )
    ) {
      // Torpedo hit something solid, so stop flying.
      break;
    }
  }

  // Did the torpedo hit an enemy?
  if (
    findInsectorMap(
      gameOptions.sectorMapSymbols.enemy,
      quantizedPosY,
      quantizedPosX
    )
  ) {
    print(`*** ${gameOptions.nameEnemy} DESTROYED ***`);
    gameState.sectorEnemiesCount = gameState.sectorEnemiesCount - 1;
    gameState.enemiesRemaining = gameState.enemiesRemaining - 1;

    if (gameState.enemiesRemaining <= 0) {
      // If that was the last enemy, then we've won!
      gameState.gameOver = true;
      gameState.gameWon = true;
      return;
    }

    // Find which enemy was hit and set health to zero
    for (
      let enemyIdx = 0;
      enemyIdx < gameOptions.enemySpawnChance.length;
      enemyIdx++
    ) {
      if (
        quantizedPosY == gameState.sectorEnemies[enemyIdx].posY &&
        quantizedPosX == gameState.sectorEnemies[enemyIdx].posX
      ) {
        gameState.sectorEnemies[enemyIdx].health = 0;
        break;
      }
    }
  }

  // Did the torpedo hit a star?
  if (
    findInsectorMap(
      gameOptions.sectorMapSymbols.star,
      quantizedPosY,
      quantizedPosX
    )
  ) {
    print(
      `STAR AT ${quantizedPosY} , ${quantizedPosX} ABSORBED TORPEDO ENERGY.`
    );
    return enemiesShoot();
  }

  // Did the torpedo hit a starbase?
  if (
    findInsectorMap(
      gameOptions.sectorMapSymbols.base,
      quantizedPosY,
      quantizedPosX
    )
  ) {
    print("*** STARBASE DESTROYED ***");
    gameState.sectorStarbasesCount = gameState.sectorStarbasesCount - 1;
    gameState.starbasesRemaining = gameState.starbasesRemaining - 1;
    if (
      gameState.starbasesRemaining <= 0 ||
      gameState.enemiesRemaining <=
        gameState.stardateCurrent -
          gameOptions.stardateStart -
          gameOptions.timeLimit
    ) {
      print("THAT DOES IT, CAPTAIN!!  YOU ARE HEREBY RELIEVED OF COMMAND");
      print("AND SENTENCED TO 99 STARDATES AT HARD LABOR ON CYGNUS 12!!");
      gameState.gameOver = true;
      return;
    } else {
      print("STARFLEET COMMAND REVIEWING YOUR RECORD TO CONSIDER");
      print("COURT MARTIAL!");
      gameState.isDocked = false;
    }
  }

  // If we hit an enemy or a starbase, update the sector and galaxy map to
  // remove the thing destroyed
  insertInSectorMap(
    gameOptions.sectorMapSymbols.empty,
    quantizedPosY,
    quantizedPosX
  );
  gameState.galacticMap[gameState.quadrantPositionY][
    gameState.quadrantPositionX
  ] =
    gameState.sectorEnemiesCount * 100 +
    gameState.sectorStarbasesCount * 10 +
    gameState.sectorStarsCount;
  gameState.galacticMapDiscovered[gameState.quadrantPositionY][
    gameState.quadrantPositionX
  ] =
    gameState.galacticMap[gameState.quadrantPositionY][
      gameState.quadrantPositionX
    ];

  return enemiesShoot();
}

async function enemiesShoot() {
  if (gameState.sectorEnemiesCount <= 0) {
    return;
  }

  if (gameState.isDocked) {
    print("STARBASE SHIELDS PROTECT THE ENTERPRISE");
    return;
  }

  for (
    let enemyIdx = 0;
    enemyIdx < gameOptions.enemySpawnChance.length;
    enemyIdx++
  ) {
    if (gameState.sectorEnemies[enemyIdx].health <= 0) {
      continue;
    }

    // Enemy damage based on health with drop-off for distance and chance
    const enemyWeaponDamage = Math.floor(
      (gameState.sectorEnemies[enemyIdx].health / distanceFromEnemy(enemyIdx)) *
        (2 + Math.random())
    );
    gameState.shieldsCurrent = gameState.shieldsCurrent - enemyWeaponDamage;

    // Consume enemy health for firing weapon
    gameState.sectorEnemies[enemyIdx].health = Math.floor(
      gameState.sectorEnemies[enemyIdx].health / (3 + Math.random())
    );

    print(
      `${enemyWeaponDamage} UNIT HIT ON ENTERPRISE FROM SECTOR ${gameState.sectorEnemies[enemyIdx].posY} , ${gameState.sectorEnemies[enemyIdx].posX}`
    );

    if (gameState.shieldsCurrent <= 0) {
      // If we're out of shields, we're out of luck
      gameState.gameOver = true;
      gameState.destroyed = true;
      return;
    }

    print(`      <SHIELDS DOWN TO ${gameState.shieldsCurrent} UNITS>`);
    if (enemyWeaponDamage < 20) {
      continue;
    }

    // Systems damage with 60% chance or a hit of more than 2% of shields
    if (
      Math.random() > gameOptions.systemDamageChanceOnHit ||
      enemyWeaponDamage / gameState.shieldsCurrent <=
        gameOptions.systemDamageHitThroughShields
    ) {
      continue;
    }

    // Random system damaged proportional to enemy damage and current shields
    const systemIdx = randomInt(gameOptions.shipSystems.length);
    const systemName = gameOptions.shipSystems[systemIdx];
    gameState.systemsDamage[systemName] =
      gameState.systemsDamage[systemName] -
      enemyWeaponDamage / gameState.shieldsCurrent -
      0.5 * Math.random();

    print(`DAMAGE CONTROL REPORTS ${systemName} DAMAGED BY THE HIT`);
  }
}

async function commandShieldControl() {
  // 5520 REM SHIELD CONTROL
  if (gameState.systemsDamage[SYSTEM_SHIELD_CONTROL] < 0) {
    print("SHIELD CONTROL INOPERABLE");
    return;
  }

  print(
    "ENERGY AVAILABLE = ",
    gameState.energyRemaining + gameState.shieldsCurrent
  );
  const shieldUnits = parseFloat(await input("NUMBER OF UNITS TO SHIELDS"));
  if (shieldUnits < 0 || gameState.shieldsCurrent == shieldUnits) {
    print("<SHIELDS UNCHANGED>");
    return;
  }
  if (shieldUnits > gameState.energyRemaining + gameState.shieldsCurrent) {
    print("SHIELD CONTROL REPORTS  'THIS IS NOT THE FEDERATION TREASURY.'");
    print("<SHIELDS UNCHANGED>");
    return;
  }

  gameState.energyRemaining =
    gameState.energyRemaining + gameState.shieldsCurrent - shieldUnits;
  gameState.shieldsCurrent = shieldUnits;

  print("DEFLECTOR CONTROL ROOM REPORT:");
  print(
    `  'SHIELDS NOW AT ${Math.floor(
      gameState.shieldsCurrent
    )} UNITS PER YOUR COMMAND.`
  );
}

async function commandDamageControl() {
  // 5680 REM DAMAGE CONTROL
  // 5690
  // FIXME: Seems like damage control should work while docked?
  if (gameState.systemsDamage[SYSTEM_DAMAGE_CONTROL] < 0) {
    print("DAMAGE CONTROL REPORT NOT AVAILABLE");
    return;
  }

  // 5910
  print();
  print("DEVICE             STATE OF REPAIR");
  for (const systemName of gameOptions.shipSystems) {
    print(
      systemName.padEnd(25, " "),
      Math.floor(gameState.systemsDamage[systemName] * 100) * 0.01
    );
  }
  print();

  if (gameState.isDocked) {
    let repairTimeEstimate = 0;
    for (const systemName of gameOptions.shipSystems) {
      if (gameState.systemsDamage[systemName] < 0) {
        repairTimeEstimate = repairTimeEstimate + 0.1;
      }
    }
    if (repairTimeEstimate == 0) {
      return;
    }
    print();
    repairTimeEstimate = repairTimeEstimate + gameState.starbaseRepairDelay;
    if (repairTimeEstimate >= 1) {
      repairTimeEstimate = 0.9;
    }
    print("TECHNICIANS STANDING BY TO EFFECT REPAIRS TO YOUR SHIP;");
    print(
      `ESTIMATED TIME TO REPAIR: ${
        0.01 * Math.floor(100 * repairTimeEstimate)
      } STARDATES`
    );
    const authorizeRepairInput = await input(
      "WILL YOU AUTHORIZE THE REPAIR ORDER (Y/N)"
    );
    if (authorizeRepairInput.toUpperCase() != "Y") {
      return;
    }
    for (const systemName of gameOptions.shipSystems) {
      gameState.systemsDamage[systemName] = 0;
    }
    gameState.stardateCurrent =
      gameState.stardateCurrent + repairTimeEstimate + 0.1;
  }
}

async function commandLibraryComputer() {
  // 7280 REM LIBRARY COMPUTER CODE
  // 7290
  if (gameState.systemsDamage[SYSTEM_LIBRARY_COMPUTER] < 0) {
    print("COMPUTER DISABLED");
    return;
  }
  const commandInput = parseInt(
    await input("COMPUTER ACTIVE AND AWAITING COMMAND")
  );
  if (commandInput < 0) return;
  const command = COMMANDS_COMPUTER[commandInput] || computerHelp;
  print();
  await command();
}

const COMMANDS_COMPUTER = [
  computerCumulativeRecord,
  computerStatusReport,
  computerPhotonData,
  computerStarbaseData,
  computerDirectionData,
  computerGalaxyMap,
];

async function computerHelp() {
  print("FUNCTIONS AVAILABLE FROM LIBRARY-COMPUTER:");
  print("   0 = CUMULATIVE GALACTIC RECORD");
  print("   1 = STATUS REPORT");
  print("   2 = PHOTON TORPEDO DATA");
  print("   3 = STARBASE NAV DATA");
  print("   4 = DIRECTION/DISTANCE CALCULATOR");
  print("   5 = GALAXY 'REGION NAME' MAP");
  print();
}

async function computerPhotonData() {
  if (gameState.sectorEnemiesCount <= 0) {
    print(
      `SCIENCE OFFICER ${gameOptions.nameScienceOfficer} REPORTS  'SENSORS SHOW NO ENEMY SHIPS`
    );
    print("                                IN THIS QUADRANT'");
    return;
  }

  print(
    `FROM ENTERPRISE TO ${gameOptions.nameEnemy} BATTLE CRUISER${
      gameState.sectorEnemiesCount > 1 ? "S" : ""
    }`
  );

  for (
    let enemyIdx = 0;
    enemyIdx < gameOptions.enemySpawnChance.length;
    enemyIdx++
  ) {
    if (gameState.sectorEnemies[enemyIdx].health <= 0) continue;
    computerDirectionCommon({
      fromY: gameState.sectorPositionY,
      fromX: gameState.sectorPositionX,
      toY: gameState.sectorEnemies[enemyIdx].posY,
      toX: gameState.sectorEnemies[enemyIdx].posX,
    });
  }
}

async function computerStarbaseData() {
  if (gameState.sectorStarbasesCount == 0) {
    print(
      `MR. ${gameOptions.nameScienceOfficer} REPORTS,  'SENSORS SHOW NO STARBASES IN THIS QUADRANT.'`
    );
    return;
  }
  print("FROM ENTERPRISE TO STARBASE:");
  computerDirectionCommon({
    fromY: gameState.sectorPositionY,
    fromX: gameState.sectorPositionX,
    toY: gameState.sectorStarbaseY,
    toX: gameState.sectorStarbaseX,
  });
}

const inputCoords = async (prompt) =>
  (await input(prompt)).split(",").map((s) => parseInt(s.trim()));

async function computerDirectionData() {
  print("DIRECTION/DISTANCE CALCULATOR:");
  print(
    `YOU ARE AT QUADRANT ${gameState.quadrantPositionY} , ${gameState.quadrantPositionX} SECTOR ${gameState.sectorPositionY} , ${gameState.sectorPositionX}`
  );
  print("PLEASE ENTER");
  const [fromY, fromX] = await inputCoords("  INITIAL COORDINATES (Y,X)");
  const [toY, toX] = await inputCoords("  FINAL COORDINATES (Y,X)");
  computerDirectionCommon({ fromX, fromY, toX, toY });
}

async function computerDirectionCommon({ fromX, fromY, toX, toY }) {
  const distance = Math.sqrt(
    Math.pow(toX - fromX, 2) + Math.pow(toY - fromY, 2)
  );
  const direction =
    1 +
    (8 / (Math.PI * 2)) *
      ((Math.atan2(0 - fromY - (0 - toY), fromX - toX) + Math.PI) %
        (Math.PI * 2));

  print(`DIRECTION = ${direction}`);
  print(`DISTANCE = ${distance}`);
}

async function computerStatusReport() {
  print("STATUS REPORT:");
  print();
  print(
    `${
      gameState.enemiesRemaining > 1
        ? gameOptions.nameEnemies
        : gameOptions.nameEnemy
    } LEFT: ${gameState.enemiesRemaining}`
  );
  print(
    `MISSION MUST BE COMPLETED IN ${
      0.1 *
      Math.floor(
        (gameOptions.stardateStart +
          gameOptions.timeLimit -
          gameState.stardateCurrent) *
          10
      )
    }  STARDATES`
  );
  if (gameState.starbasesRemaining < 1) {
    print("YOUR STUPIDITY HAS LEFT YOU ON YOUR ON IN");
    print("  THE GALAXY -- YOU HAVE NO STARBASES LEFT!");
  } else {
    print(
      `THE FEDERATION IS MAINTAINING ${gameState.starbasesRemaining} STARBASE${
        gameState.starbasesRemaining < 2 ? "" : "S"
      } IN THE GALAXY`
    );
  }
  commandDamageControl();
}

async function computerGalaxyMap() {
  print("                        THE GALAXY");
  computerCommonMap(false);
}

async function computerCumulativeRecord() {
  print();
  print(
    `        COMPUTER RECORD OF GALAXY FOR QUADRANT ${gameState.quadrantPositionY} , ${gameState.quadrantPositionX}`
  );
  print();
  computerCommonMap();
}

async function computerCommonMap(showMapCells = true) {
  // Print the X column number header based on width of first galaxy row
  print(
    "  " +
      gameState.galacticMap[1]
        .map((_, idx) => idx.toString().padStart(3, " "))
        .join("   ")
  );

  // Assemble X column separator based on width of first galaxy row
  const separator =
    "     " + gameState.galacticMap[1].map((_, idx) => "----- ").join("");

  print(separator);
  for (let mapY = 1; mapY <= gameOptions.galaxyHeight; mapY++) {
    let out = mapY.toString().padStart(3, " ");

    if (showMapCells) {
      // 7630
      for (let mapX = 1; mapX <= gameOptions.galaxyWidth; mapX++) {
        out += `   ${
          gameState.galacticMapDiscovered[mapY][mapX] == 0
            ? "***"
            : ("" + gameState.galacticMapDiscovered[mapY][mapX]).padStart(
                3,
                "0"
              )
        }`;
      }
    } else {
      let quadrantName = buildQuadrantName(mapY, 1, true);
      let centerSpacing = Math.floor(12 - 0.5 * quadrantName.length);
      out += `  ${" ".repeat(centerSpacing)}${quadrantName}${" ".repeat(
        centerSpacing
      )}`;
      quadrantName = buildQuadrantName(mapY, 5, true);
      centerSpacing = Math.floor(12 - 0.5 * quadrantName.length);
      out += `${" ".repeat(centerSpacing)}${quadrantName}`;
    }

    print(out);
    print(separator);
  }
}

async function endOfGame() {
  if (gameState.destroyed) {
    print();
    print(
      "THE ENTERPRISE HAS BEEN DESTROYED.  THEN FEDERATION WILL BE CONQUERED"
    );
  }

  print(`IT IS STARDATE ${formatStardate(gameState.stardateCurrent)}`);

  if (!gameState.gameWon) {
    print(
      `THERE WERE ${gameState.enemiesRemaining} ${gameOptions.nameEnemy} BATTLE CRUISERS LEFT AT`
    );
    print("THE END OF YOUR MISSION.");
  } else {
    print(
      `CONGRULATION, CAPTAIN!  THEN LAST ${gameOptions.nameEnemy} BATTLE CRUISER`
    );
    print("MENACING THE FEDERATION HAS BEEN DESTROYED.");
    print();
    print(
      "YOUR EFFICIENCY RATING IS ",
      (1000 *
        (gameState.enemiesInitialCount /
          (gameState.stardateCurrent - gameOptions.stardateStart))) ^
        2
    );
  }

  print();
  print();

  if (gameState.starbasesRemaining > 0) {
    print("THE FEDERATION IS IN NEED OF A NEW STARSHIP COMMANDER");
    print("FOR A SIMILAR MISSION -- IF THERE IS A VOLUNTEER,");
    const playAgainInput = await input("LET HIM STEP FORWARD AND ENTER 'AYE'");
    if (playAgainInput.toUpperCase() == "AYE") {
      gameState.shouldRestart = true;
      return;
    }
  }
}

const COURSE_TO_XY = [
  [0, 1],
  [-1, 1],
  [-1, 0],
  [-1, -1],
  [0, -1],
  [1, -1],
  [1, 0],
  [1, 1],
  [0, 1],
];

function courseToDeltaXY(course) {
  const courseIdx = Math.floor(course) - 1;
  //3110 X1=C(C1,1)+(C(C1+1,1)-C(C1,1))*(C1-INT(C1)):X=S1:Y=S2
  //3140 X2=C(C1,2)+(C(C1+1,2)-C(C1,2))*(C1-INT(C1)):Q4=Q1:Q5=Q2
  const courseDeltaY =
    COURSE_TO_XY[courseIdx][0] +
    (COURSE_TO_XY[courseIdx + 1][0] - COURSE_TO_XY[courseIdx][0]) *
      (course - Math.floor(course));
  const courseDeltaX =
    COURSE_TO_XY[courseIdx][1] +
    (COURSE_TO_XY[courseIdx + 1][1] - COURSE_TO_XY[courseIdx][1]) *
      (course - Math.floor(course));
  return [courseDeltaY, courseDeltaX];
}

function findSpaceInSectorMap() {
  let posY,
    posX,
    foundEmptyPlace = false;
  while (!foundEmptyPlace) {
    posY = randomInt(8, 1);
    posX = randomInt(8, 1);
    foundEmptyPlace = findInsectorMap(
      gameOptions.sectorMapSymbols.empty,
      posY,
      posX
    );
  }
  return [posY, posX];
}

function findInsectorMap(str, y, x) {
  const idx = (x - 1) * 3 + (y - 1) * 24;
  return gameState.sectorMap.substring(idx, idx + 3) == str;
}

// 8660 REM INSERT IN STRING ARRAY FOR QUADRANT
function insertInSectorMap(str, y, x) {
  // 8670
  const strPos = (x - 1) * 3 + (y - 1) * 24;
  if (str.length != 3) {
    throw "ERROR";
  }
  gameState.sectorMap =
    gameState.sectorMap.slice(0, strPos) +
    str +
    gameState.sectorMap.slice(strPos + 3);
}

function buildQuadrantName(y, x, regionNameOnly = false) {
  const xIdx = x - 1;
  const yIdx = y - 1;
  const name = gameOptions.quadrantNames[xIdx < 4 ? 0 : 1][yIdx];
  return `${name}${
    regionNameOnly ? "" : ` ${gameOptions.quadrantNumbers[xIdx % 4]}`
  }`;
}

const randomInt = (max, min = 0) =>
  Math.floor(min + Math.random() * (max - min));

const formatStardate = (stardate) => Math.floor(stardate * 10) / 10;

const distanceFromEnemy = (sectorEnemyIndex) =>
  Math.sqrt(
    Math.pow(
      gameState.sectorEnemies[sectorEnemyIndex].posY -
        gameState.sectorPositionY,
      2
    ) +
      Math.pow(
        gameState.sectorEnemies[sectorEnemyIndex].posX -
          gameState.sectorPositionX,
        2
      )
  );
