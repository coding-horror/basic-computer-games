# JVM gradle scripts

## Quickstart
You will need to install openjdk 17, because some games use advanced Java features.
We should be using version 17 anyway, because anything less than 17 is deprecated.

Build all the games:
```shell
 cd buildJvm
 ./gradlew -q clean assemble installDist distributeBin distributeLib
```

Then, run a game:

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
New games can be added:
- Create a new `build_NUMBER_NAME_[java/kotlin]` directory
- Add a `build.gradle` file to that directory.
All `build.gradle` files under `build_NUMBER_*`  are identical.
- Add a `gradle.properties` file to that directory, defining the source
directory for the java or kotlin file, and the class that contains the `main` method.
- Add an entry in `settings.gradle`

The `build.gradle` file **should** be identical to all the other `build.gradle` files
in all the other subprojects:
```groovy
 sourceSets {
     main {
         java {
             srcDirs "../../$gameSource"
         }
     }
 }
 application {
     mainClass = gameMain
 }
```

The `gradle.properties` file should look like this:

    gameSource=91_Train/java/src
    gameMain=Train

where `gameSource` is the root of the source code directory, and `gameMain` is the main class.

The `settings.gradle` must be maintained as a list of all subprojects. Add your new
project to the list.

```groovy
include ":build_91_Train_java"
```

### Adding a game with tests

You can add tests for JVM games with a `build.gradle` looking a little different.
Use the build files from `03_Animal` as a template to add tests:

```groovy
sourceSets {
    main {
        java {
            srcDirs "../../$gameSource"
        }
    }
    test {
        java {
            srcDirs "../../$gameTest"
        }
    }
}

application {
    mainClass = gameMain
}

dependencies {
    testImplementation(project(":build_00_utilities").sourceSets.test.output)
}
```

The gradle.properties needs an additional directory name for the tests, as `gameTest` :
```
gameSource=03_Animal/java/src
gameTest=03_Animal/java/test
gameMain=Animal
```

Each project should have its own test, and shouldn't share test source directories
with other projects, even if they are for the same game.

Tests are constructed by subclassing `ConsoleTest`. This allows you to use the
`assertConversation` function to check for correct interactive conversations.
```kotlin
import com.pcholt.console.testutils.ConsoleTest
import org.junit.Test

class AnimalJavaTest : ConsoleTest() {
    @Test
    fun `should have a simple conversation`() {
       assertConversation(
          """
            WHAT'S YOUR NAME? {PAUL}
            YOUR NAME IS PAUL? {YES}
            THANKS FOR PLAYING
            """
       ) {
          // The game's Main method
          main()
       }
    }
}
```

Curly brackets are the expected user input.
Note - this is actually just a way of defining the expected input as "PAUL" and "YES"
and not that the input happens at the exact prompt position. Thus this is equivalent:
```kotlin
"""
{PAUL} {YES} WHAT'S YOUR NAME?
YOUR NAME IS PAUL?
THANKS FOR PLAYING
"""
```

Amounts of whitespace are not counted, but whitespace is significant: You will get a failure if
your game emits `"NAME?"` when it expects `"NAME ?"`.

Run all the tests from within the buildJvm project directory:
```bash
cd buildJvm
./gradlew test
```
