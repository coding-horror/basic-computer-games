using System.Collections.Generic;
using System.Linq;
using static Tower.Resources.Strings;

namespace Tower.UI
{
    internal class Prompt
    {
        public static Prompt DiskCount =
            new(DiskCountPrompt, DiskCountRetry, DiskCountQuit, 1, 2, 3, 4, 5, 6, 7) { RetriesAllowed = 2 };

        public static Prompt Disk =
            new(DiskPrompt, DiskRetry, DiskQuit, 3, 5, 7, 9, 11, 13, 15) { RepeatPrompt = false };

        public static Prompt Needle = new(NeedlePrompt, NeedleRetry, NeedleQuit, 1, 2, 3);

        private readonly HashSet<int> _validValues;

        private Prompt(string prompt, string retryMessage, string quitMessage, params int[] validValues)
        {
            Message = prompt;
            RetryMessage = retryMessage;
            QuitMessage = quitMessage;
            _validValues = validValues.ToHashSet();
            RetriesAllowed = 1;
            RepeatPrompt = true;
        }

        public string Message { get; }
        public string RetryMessage { get; }
        public string QuitMessage { get; }
        public int RetriesAllowed { get; private set; }
        public bool RepeatPrompt { get; private set; }

        public bool TryValidateResponse(float number, out int integer)
        {
            integer = (int)number;
            return integer == number && _validValues.Contains(integer);
        }
    }
}