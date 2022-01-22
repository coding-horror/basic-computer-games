using System.Diagnostics;

namespace DotnetUtils;

public static class Methods {
    public static ProcessResult RunProcess(string filename, string arguments) {
        var process = new Process() {
            StartInfo = {
                FileName = filename,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            },
            EnableRaisingEvents = true
        };
        return RunProcess(process);
    }

    public static ProcessResult RunProcess(Process process, string input = "") {
        var (output, error) = ("", "");
        var (redirectOut, redirectErr) = (
            process.StartInfo.RedirectStandardOutput,
            process.StartInfo.RedirectStandardError
        );
        if (redirectOut) {
            process.OutputDataReceived += (s, ea) => output += ea.Data + "\n";
        }
        if (redirectErr) {
            process.ErrorDataReceived += (s, ea) => error += ea.Data + "\n";
        }

        if (!process.Start()) {
            throw new InvalidOperationException();
        };

        if (redirectOut) { process.BeginOutputReadLine(); }
        if (redirectErr) { process.BeginErrorReadLine(); }
        if (!string.IsNullOrEmpty(input)) {
            process.StandardInput.WriteLine(input);
            process.StandardInput.Close();
        }
        process.WaitForExit();
        return new ProcessResult(process.ExitCode, output, error);
    }

    public static Task<ProcessResult> RunProcessAsync(Process process, string input = "") {
        var tcs = new TaskCompletionSource<ProcessResult>();
        var (output, error) = ("", "");
        var (redirectOut, redirectErr) = (
            process.StartInfo.RedirectStandardOutput,
            process.StartInfo.RedirectStandardError
        );

        process.Exited += (s, e) => tcs.SetResult(new ProcessResult(process.ExitCode, output, error));

        if (redirectOut) {
            process.OutputDataReceived += (s, ea) => output += ea.Data + "\n";
        }
        if (redirectErr) {
            process.ErrorDataReceived += (s, ea) => error += ea.Data + "\n";
        }

        if (!process.Start()) {
            // what happens to the Exited event if process doesn't start successfully?
            throw new InvalidOperationException();
        }

        if (redirectOut) { process.BeginOutputReadLine(); }
        if (redirectErr) { process.BeginErrorReadLine(); }
        if (!string.IsNullOrEmpty(input)) {
            process.StandardInput.WriteLine(input);
            process.StandardInput.Close();
        }

        return tcs.Task;
    }
}

public sealed record ProcessResult(int ExitCode, string StdOut, string StdErr) {
    public override string? ToString() =>
        StdOut +
        (StdOut is not (null or "") && ExitCode > 0 ? "\n" : "") +
        (ExitCode != 0 ?
            $"{ExitCode}\n{StdErr}" :
            "");
}
