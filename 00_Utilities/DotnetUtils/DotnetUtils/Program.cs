using System.Xml.Linq;
using DotnetUtils;
using static System.Console;
using static System.IO.Path;
using static DotnetUtils.Methods;
using static DotnetUtils.Functions;

var infos = PortInfos.Get;

var actions = new (Action action, string description)[] {
    (printInfos, "Output information -- solution, project, and code files"),
    (missingSln, "Output missing sln"),
    (unexpectedSlnName, "Output misnamed sln"),
    (multipleSlns, "Output multiple sln files"),
    (missingProj, "Output missing project file"),
    (unexpectedProjName, "Output misnamed project files"),
    (multipleProjs, "Output multiple project files"),
    (checkProjects, "Check .csproj/.vbproj files for target framework, nullability etc."),
    (checkExecutableProject, "Check that there is at least one executable project per port"),
    (printPortInfo, "Print info about a single port"),

    (generateMissingSlns, "Generate solution files when missing"),
    (generateMissingProjs, "Generate project files when missing")
};

foreach (var (_, description, index) in actions.WithIndex()) {
    WriteLine($"{index}: {description}");
}

WriteLine();

actions[getChoice(actions.Length - 1)].action();

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
        if (item.Slns.Contains(Combine(item.LangPath, expectedSlnName), StringComparer.InvariantCultureIgnoreCase)) { continue; }

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
        // We can't use the dotnet command to create a new project using the built-in console template, because part of that template
        // is a Program.cs / Program.vb file. If there already are code files, there's no need to add a new empty one; and 
        // if there's already such a file, it might try to overwrite it.

        var projText = item.Lang switch {
            "csharp" => @"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <LangVersion>10</LangVersion>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
",
            "vbnet" => @$"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <RootNamespace>{item.GameName}</RootNamespace>
    <TargetFramework>net6.0</TargetFramework>
    <LangVersion>16.9</LangVersion>
  </PropertyGroup>
</Project>
",
            _ => throw new InvalidOperationException()
        };
        var projFullPath = Combine(item.LangPath, $"{item.GameName}.{item.ProjExt}");
        File.WriteAllText(projFullPath, projText);

        if (item.Slns.Length == 1) {
            var result = RunProcess("dotnet", $"sln {item.Slns[0]} add {projFullPath}");
            WriteLine(result);
        }
    }
}

void checkProjects() {
    foreach (var info in infos) {
        printProjectWarnings(info);
        WriteLine();
    }
}

void printProjectWarnings(PortInfo info) {
    foreach (var proj in info.Projs) {
        var warnings = new List<string>();
        var parent = XDocument.Load(proj).Element("Project")?.Element("PropertyGroup");

        var (
            framework,
            nullable,
            implicitUsing,
            rootNamespace,
            langVersion
        ) = (
            getValue(parent, "TargetFramework", "TargetFrameworks"),
            getValue(parent, "Nullable"),
            getValue(parent, "ImplicitUsings"),
            getValue(parent, "RootNamespace"),
            getValue(parent, "LangVersion")
        );

        if (framework != "net6.0") {
            warnings.Add($"Target: {framework}");
        }

        if (info.Lang == "csharp") {
            if (nullable != "enable") {
                warnings.Add($"Nullable: {nullable}");
            }
            if (implicitUsing != "enable") {
                warnings.Add($"ImplicitUsings: {implicitUsing}");
            }
            if (rootNamespace != null && rootNamespace != info.GameName) {
                warnings.Add($"RootNamespace: {rootNamespace}");
            }
            if (langVersion != "10") {
                warnings.Add($"LangVersion: {langVersion}");
            }
        }

        if (info.Lang == "vbnet") {
            if (rootNamespace != info.GameName) {
                warnings.Add($"RootNamespace: {rootNamespace}");
            }
            if (langVersion != "16.9") {
                warnings.Add($"LangVersion: {langVersion}");
            }
        }

        if (warnings.Any()) {
            WriteLine(proj.RelativePath(info.LangPath));
            WriteLine(string.Join("\n", warnings));
            WriteLine();
        }
    }
}

void checkExecutableProject() {
    foreach (var item in infos) {
        if (item.Projs.All(proj => getValue(proj, "OutputType") != "Exe")) {
            WriteLine($"{item.LangPath}");
        }
    }
}

void tryBuild() {
    // if has code files, try to build
}

void printPortInfo() {
    // prompt for port number
    Write("Enter number from 1 to 96 ");
    var index = getChoice(1, 96);

    Write("Enter 0 for C#, 1 for VB ");
    var lang = getChoice(1) switch {
        0 => "csharp",
        1 => "vbnet",
        _ => throw new InvalidOperationException()
    };

    WriteLine();

    var info = infos.Single(x => x.Index == index && x.Lang == lang);

    WriteLine(info.LangPath);
    WriteLine(new string('-', 50));

    // print solutions
    printSlns(info);

    // mismatched solution name/location? (expected x)
    var expectedSlnName = Combine(info.LangPath, $"{info.GameName}.sln");
    if (!info.Slns.Contains(expectedSlnName)) {
        WriteLine($"Expected name/path: {expectedSlnName.RelativePath(info.LangPath)}");
    }

    // has executable project?
    if (info.Projs.All(proj => getValue(proj, "OutputType") != "Exe")) {
        WriteLine("No executable project");
    }

    WriteLine();

    // print projects
    printProjs(info);

    // mimsatched project name/location? (expected x)
    var expectedProjName = Combine(info.LangPath, $"{info.GameName}.{info.ProjExt}");
    if (info.Projs.Length < 2 && !info.Projs.Contains(expectedProjName)) {
        WriteLine($"Expected name/path: {expectedProjName.RelativePath(info.LangPath)}");
    }

    WriteLine();

    // verify project properties
    printProjectWarnings(info);

    WriteLine("Code files:");

    // list code files
    foreach (var codeFile in info.CodeFiles) {
        WriteLine(codeFile.RelativePath(info.LangPath));
    }

    // try build
}
