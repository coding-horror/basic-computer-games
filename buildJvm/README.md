# JVM gradle scripts

## Quickstart
You will need to install openjdk 17, because some games use advanced Java features.
We should be using version 17 anyway, because anything less than 17 is deprecated.

Build all the games:
```shell
 cd buildJvm
 ./gradlew -q assemble installDist distributeBin distributeLib
```

Then, run a game

### Mac or linux:
```shell
build/distrib/bin/build_53_King_kotlin
```
### Windows
[not tested yet]

```shell
build\distrib\bin\build_53_King_kotlin.bat
```

---
## Using an IDE to work on JVM games

You can open the entire Basic Computer Games project in an IDE, with any IDE capable
of importing from a gradle project.

### IntelliJ / Android Studio

1. (Optional) If you want to make changes, or contribute a new kotlin or java version
of one of the games, use [github "fork"](https://docs.github.com/en/get-started/quickstart/fork-a-repo)
to create your own editable fork of the project.
2. Check out the code using `File` -> `New` -> `Project from Version Control`
   1. Enter the URL of the project. For the main project this will be `https://github.com/coding-horror/basic-computer-games.git`, for your
own fork this will be `https://github.com/YOURNAMEHERE/basic-computer-games.git`
   2. Choose a directory for the clone to live in
3. Click `Clone`

The project will open, and eventually you will get a little alert box in the bottom right corner saying "Gradle build script found". 

Click the "Load" link in the alert box, to load the gradle project.

You should see all the games appear on the left side of the screen. If you have loaded
your own fork, you can modify, commit and push your changes to github.

If you are using the main `coding-horror` branch, you can still make and run your own changes.  If
your git skills are up to the task, you might even fork the project and change your
local clone to point to your new forked project.


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
```groovy
    plugins {
        id 'application'
        // id 'org.jetbrains.kotlin.jvm' // UNCOMMENT for kotlin projects
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
```

And the `gradle.properties` file should look like this:

    gameSource=91_Train/java/src
    gameMain=Train

where `gameSource` is the root of the source code directory, and `gameMain` is the main class.
