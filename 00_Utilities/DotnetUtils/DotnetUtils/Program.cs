using DotnetUtils;
using static System.Console;
using static System.IO.Path;

var infos = PortInfos.Get;

var actions = new (Action action, string description)[] {
    (printInfos, "Output information -- solution, project, and code files"),
    (missingSln, "Output missing sln"),
    (unexpectedSlnName, "Output misnamed sln"),
    (multipleSlns, "Output multiple sln files"),
    (missingProj, "Output missing project file"),
    (unexpectedProjName, "Output misnamed project files"),
    (multipleProjs, "Output multiple project files")
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
