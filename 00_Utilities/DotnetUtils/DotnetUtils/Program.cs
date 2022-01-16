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
    foreach (var (proj,item) in infos.SelectMany(item => item.Projs.Select(proj => (proj,item)))) {
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

        if (item.Lang == "csharp") {
            if (nullable != "enable") {
                warnings.Add($"Nullable: {nullable}");
            }
            if (implicitUsing != "enable") {
                warnings.Add($"ImplicitUsings: {implicitUsing}");
            }
            if (rootNamespace != null && rootNamespace != item.GameName) {
                warnings.Add($"RootNamespace: {rootNamespace}");
            }
            if (langVersion != "10") {
                warnings.Add($"LangVersion: {langVersion}");
            }
        }

        if (item.Lang == "vbnet") {
            if (rootNamespace != item.GameName) {
                warnings.Add($"RootNamespace: {rootNamespace}");
            }
            if (langVersion != "16.9") {
                warnings.Add($"LangVersion: {langVersion}");
            }
        }

        if (warnings.Any()) {
            WriteLine(proj);
            WriteLine(string.Join("\n", warnings));
            WriteLine();
        }
    }
}

void checkExecutableProject() {
    foreach (var item in infos) {
        if (item.Projs.All(proj => getValue(proj,"OutputType") != "Exe")) {
            WriteLine($"{item.LangPath}");
        }
    }
}

void tryBuild() {
    // if has code files, try to build
}
