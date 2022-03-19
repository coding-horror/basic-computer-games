# How to run the games

The games in this repository have been translated into a number of different languages. How to run them depends on the target language.

## csharp

### dotnet command-line

The best cross-platform method for running the csharp examples is with the `dotnet` command-line tool. This can be downloaded for **MacOS**, **Windows** and **Linux** from [dotnet.microsoft.com](https://dotnet.microsoft.com/).

From there, the program can be run by

1. Opening a terminal window
1. Navigating to the corresponding directory
1. Starting with `dotnet run`

### Visual Studio

Alternatively, for non-dotnet compatible translations, you will need [Visual Studio](https://visualstudio.microsoft.com/vs/community/) which can be used to both open the project and run the example.

1. Open the corresponding `.csproj` or `.sln` file
1. Click `Run` from within the Visual Studio IDE

## java

**TIP:** You can build all the java and kotlin games at once
using the instructions in the [buildJvm directory](buildJvm/README.md)

The Java translations can be run via the command line or from an IDE such as [Eclipse](https://www.eclipse.org/downloads/packages/release/kepler/sr1/eclipse-ide-java-developers) or [IntelliJ](https://www.jetbrains.com/idea/)

To run from the command line, you will need a Java SDK (eg. [Oracle JDK](https://www.oracle.com/java/technologies/downloads/) or [Open JDK](https://openjdk.java.net/)).

1. Navigate to the corresponding directory.
1. Compile the program with `javac`:
   * eg. `javac AceyDuceyGame.java`
1. Run the compiled program with `java`:
   * eg. `java AceyDuceyGame`

or if you are **using JDK11 or later** you can now execute a self contained java file that has a main method directly with `java <filename>.java`.

## javascript

There are two ways of javascript implementations:

### browser

The html examples can be run from within your web browser. Simply open the corresponding `.html` file from your web browser.

### node.js

Some games are implemented as a [node.js](https://nodejs.org/) script. In this case there is no `*.html` file in the folder.

1. [install node.js](https://nodejs.org/en/download/) for your system.
1. change directory to the root of this repository (e.g. `cd basic-computer-games`).
1. from a terminal call the script you want to run (e.g. `node 78_Sine_Wave/javascript/sinewave.mjs`).

_Hint: Normally javascript files have a `*.js` extension. We are using `*.mjs` to let node know , that we are using [ES modules](https://nodejs.org/docs/latest/api/esm.html#modules-ecmascript-modules) instead of [CommonJS](https://nodejs.org/docs/latest/api/modules.html#modules-commonjs-modules)._

## kotlin

Use the directions in [buildJvm](buildJvm/README.md) to build for kotlin. You can also use those directions to
build java games.

## pascal

The pascal examples can be run using [Free Pascal](https://www.freepascal.org/). Additionally, `.lsi` project files can be opened with the [Lazarus Project IDE](https://www.lazarus-ide.org/).

The pascal examples include both *simple* (single-file) and *object-oriented* (in the `/object-pascal`directories) examples.

1. You can compile the program from the command line with the `fpc` command.
   * eg. `fpc amazing.pas`
1. The output is an executable file that can be run directly.

## perl

The perl translations can be run using a perl interpreter (a copy can be downloaded from [perl.org](https://www.perl.org/)) if not already installed.

1. From the command-line, navigate to the corresponding directory.
1. Invoke with the `perl` command.
   * eg. `perl aceyducey.pl`

## python

The python translations can be run from the command line by using the `py` interpreter. If not already installed, a copy can be downloaded from [python.org](https://www.python.org/downloads/) for **Windows**, **MacOS** and **Linux**.

1. From the command-line, navigate to the corresponding directory.
1. Invoke with the `py` or `python` interpreter (depending on your python version).
   * eg. `py acey_ducey_oo.py`
   * eg. `python aceyducey.py`

**Note**

Some translations include multiple versions for python, such as `acey ducey` which features versions for Python 2 (`aceyducey.py`) and Python 3 (`acey_ducey.py`) as well as an extra object-oriented version (`acey_ducey_oo.py`).

You can manage and use different versions of python with [pip](https://pypi.org/project/pip/).

## ruby

If you don't already have a ruby interpreter, you can download it from the [ruby project site](https://www.ruby-lang.org/en/).

1. From the command-line, navigate to the corresponding directory.
1. Invoke with the `ruby` tool.
   * eg. `ruby aceyducey.rb`

## vbnet

Follow the same steps as for the [csharp](#csharp) translations. This can be run with `dotnet` or `Visual Studio`.
