using System.Diagnostics;
using DotnetUtils;
using static System.Console;
using static System.IO.Path;
using static DotnetUtils.Methods;

var infos = PortInfos.Get;

var actions = new (Action action, string description)[] {
    (printInfos, "Output information -- solution, project, and code files"),
    (missingSln, "Output missing sln"),
    (unexpectedSlnName, "Output misnamed sln"),
    (multipleSlns, "Output multiple sln files"),
    (missingProj, "Output missing project file"),
    (unexpectedProjName, "Output misnamed project files"),
    (multipleProjs, "Output multiple project files"),

    (generateMissingSlns, "Generate solution files when missing"),
    (generateMissingProjs, "Generate project files when missing")
};

foreach (var (_, description, index) in actions.WithIndex()) {
    WriteLine($"{index}: {description}");
}

WriteLine();

actions[getChoice(actions.Length - 1)].action();

int getChoice(int maxValue) {
    int result;
    do {
        Write("? ");
    } while (!int.TryParse(ReadLine(), out result) || result < 0 || result > maxValue);
    WriteLine();
    return result;
}

void printSlns(PortInfo pi) {
    switch (pi.Slns.Length) {
        case 0:
            WriteLine("No sln");
            break;
        case 1:
            WriteLine($"Solution: {pi.Slns[0].RelativePath(pi.LangPath)}");
            break;
        case > 1:
            WriteLine("Solutions:");
            foreach (var sln in pi.Slns) {
                Write(sln.RelativePath(pi.LangPath));
                WriteLine();
            }
            break;
    }
}

void printProjs(PortInfo pi) {
    switch (pi.Projs.Length) {
        case 0:
            WriteLine("No project");
            break;
        case 1:
            WriteLine($"Project: {pi.Projs[0].RelativePath(pi.LangPath)}");
            break;
        case > 1:
            WriteLine("Projects:");
            foreach (var proj in pi.Projs) {
                Write(proj.RelativePath(pi.LangPath));
                WriteLine();
            }
            break;
    }
    WriteLine();
}

void printInfos() {
    foreach (var item in infos) {
        WriteLine(item.LangPath);
        WriteLine();

        printSlns(item);
        WriteLine();

        printProjs(item);
        WriteLine();

        // get code files
        foreach (var file in item.CodeFiles) {
            WriteLine(file.RelativePath(item.LangPath));
        }
        WriteLine(new string('-', 50));
    }
}

void missingSln() {
    var data = infos.Where(x => !x.Slns.Any()).ToArray();
    foreach (var item in data) {
        WriteLine(item.LangPath);
    }
    WriteLine();
    WriteLine($"Count: {data.Length}");
}

void unexpectedSlnName() {
    var counter = 0;
    foreach (var item in infos) {
        if (!item.Slns.Any()) { continue; }

        var expectedSlnName = $"{item.GameName}.sln";
        if (item.Slns.Contains(Combine(item.LangPath, expectedSlnName))) { continue; }

        counter += 1;
        WriteLine(item.LangPath);
        WriteLine($"Expected: {expectedSlnName}");

        printSlns(item);

        WriteLine();
    }
    WriteLine($"Count: {counter}");
}

void multipleSlns() {
    var data = infos.Where(x => x.Slns.Length > 1).ToArray();
    foreach (var item in data) {
        WriteLine(item.LangPath);
        printSlns(item);
    }
    WriteLine();
    WriteLine($"Count: {data.Length}");
}

void missingProj() {
    var data = infos.Where(x => !x.Projs.Any()).ToArray();
    foreach (var item in data) {
        WriteLine(item.LangPath);
    }
    WriteLine();
    WriteLine($"Count: {data.Length}");
}

void unexpectedProjName() {
    var counter = 0;
    foreach (var item in infos) {
        if (!item.Projs.Any()) { continue; }

        var expectedProjName = $"{item.GameName}.{item.ProjExt}";
        if (item.Projs.Contains(Combine(item.LangPath, expectedProjName))) { continue; }

        counter += 1;
        WriteLine(item.LangPath);
        WriteLine($"Expected: {expectedProjName}");

        printProjs(item);

        WriteLine();
    }
    WriteLine($"Count: {counter}");
}

void multipleProjs() {
    var data = infos.Where(x => x.Projs.Length > 1).ToArray();
    foreach (var item in data) {
        WriteLine(item.LangPath);
        WriteLine();
        printProjs(item);
    }
    WriteLine();
    WriteLine($"Count: {data.Length}");
}

void generateMissingSlns() {
    foreach (var item in infos.Where(x => !x.Slns.Any())) {
        var result = RunProcess("dotnet", $"new sln -n {item.GameName} -o {item.LangPath}");
        WriteLine(result);

        var slnFullPath = Combine(item.LangPath, $"{item.GameName}.sln");
        foreach (var proj in item.Projs) {
            result = RunProcess("dotnet", $"sln {slnFullPath} add {proj}");
            WriteLine(result);
        }
    }
}

void generateMissingProjs() {
    foreach (var item in infos.Where(x => !x.Projs.Any())) {
        var (langArg, langVersion) = item.Lang switch {
            "csharp" => ("\"C#\"", 10),
            "vbnet" => ("\"VB\"", 16.9),
            _ => throw new InvalidOperationException()
        };

        var result = RunProcess("dotnet", $"new console --language {langArg} --name {item.GameName}.{item.ProjExt} -o {item.LangPath} -f net6.0 --langversion {langVersion}");
        WriteLine(result);

        var projFullPath = Combine(item.LangPath, $"{item.GameName}.{item.ProjExt}");
        if (item.Slns.Length == 1) {
            result = RunProcess("dotnet", $"sln {item.Slns[0]} add {projFullPath}");
            WriteLine(result);
        }
    }
}

void checkProjects() {
    // warn if project files do not:
    //      target .NET 6
    //      implicit using
    //      nullable enable
    // warn if none og the projects have:
    //      output type exe
}

void tryBuild() {
    // if has code files, try to build
}
