# JVM gradle scripts

## Quickstart

Build all the games:

    cd buildJvm
    ./gradlew -q assemble installDist distributeBin distributeLib

Then, run a game

### Mac or linux:
    
    build/distrib/bin/build_53_King_kotlin

### Windows
[not tested yet]

    build\distrib\bin\build_53_King_kotlin.bat

You will need to install openjdk 17, because some games use advanced Java features. 
We should be using version 17 anyway, because anything less than 17 is deprecated.

---
## Adding a new game

These are build scripts for all JVM games contributed so far.
New games can be added by:
- Creating a `build_NUMBER_NAME_[java/kotlin]` directory
- Adding a `build.gradle` file to that directory. 
All `build.gradle` files under `build_NUMBER_*` should be nearly identical, unless
there is some special requirement.
- Adding a `gradle.properties` file to that directory, defining the source
directory for the java or kotlin file, and the class that contains the `main` method.

The `build.gradle` file will normally be identical to this:

    plugins {
        id 'application'
    }
    
    sourceSets {
        main {
            java {
                srcDirs "../../$gameSource"
            }
        }
    }
    
    repositories {
        mavenCentral()
    }
    
    application {
        mainClass = gameMain
    }

And the `gradle.properties` file should look like this:

    gameSource=91_Train/java/src
    gameMain=Train

where `gameSource` is the root of the source code directory, and `gameMain` is the main class.
