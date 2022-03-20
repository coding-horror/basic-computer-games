import os
from typing import Dict, List


def has_implementation(lang: str, file_list: List[str], subdir_list: List[str]) -> bool:
    if lang == "csharp":
        return any(file.endswith(".cs") for file in file_list)
    else:
        return len(file_list) > 1 or len(subdir_list) > 0


def get_data(root_dir: str = "..") -> List[List[str]]:
    """

    Parameters
    ----------
    root_dir : str
        The root directory you want to start from.
    """
    lang_pos: Dict[str, int] = {
        "csharp": 1,
        "java": 2,
        "javascript": 3,
        "pascal": 4,
        "perl": 5,
        "python": 6,
        "ruby": 7,
        "vbnet": 8,
    }
    strings_done: List[List[str]] = []

    checklist = [
        "game",
        "csharp",
        "java",
        "javascript",
        "pascal",
        "perl",
        "python",
        "ruby",
        "vbnet",
    ]

    ignore_folders = [
        ".git",
        "00_Utilities",
        ".github",
        ".mypy_cache",
        ".pytest_cache",
        "00_Alternate_Languages",
        "00_Common",
        "buildJvm",
        "htmlcov",
    ]

    prev_game = ""

    for dir_name, subdir_list, file_list in os.walk(root_dir):
        # split_dir[1] is the game
        # split_dir[2] is the language
        split_dir = dir_name.split(os.path.sep)

        if len(split_dir) == 2 and split_dir[1] not in ignore_folders:
            if prev_game == "":
                prev_game = split_dir[1]
                checklist[0] = split_dir[1]

            if prev_game != split_dir[1]:
                # it's a new dir
                strings_done.append(checklist)
                checklist = [
                    split_dir[1],
                    "csharp",
                    "java",
                    "javascript",
                    "pascal",
                    "perl",
                    "python",
                    "ruby",
                    "vbnet",
                ]
                prev_game = split_dir[1]
        elif (
            len(split_dir) == 3 and split_dir[1] != ".git" and split_dir[2] in lang_pos
        ):
            out = (
                "✅"
                if has_implementation(split_dir[2], file_list, subdir_list)
                else "⬜️"
            )
            checklist[lang_pos[split_dir[2]]] = out
    return strings_done


def write_file(path: str, strings_done: List[List[str]]) -> None:
    write_string = (
        "# TODO list\n"
        " game | csharp | java | javascript | pascal | perl | python | ruby | vbnet\n"
        " --- | --- | --- | --- | --- | --- | --- | --- | ---\n"
    )
    sorted_strings = list(
        map(lambda l: " | ".join(l) + "\n", sorted(strings_done, key=lambda x: x[0]))
    )
    write_string += "".join(sorted_strings)

    with open(path, "w", encoding="utf-8") as f:
        f.write(write_string)


if __name__ == "__main__":
    strings_done = get_data()
    write_file("TODO.md", strings_done)
