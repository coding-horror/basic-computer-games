import os
from typing import Dict, List


def has_implementation(lang: str, file_list: List[str], subdir_list: List[str]) -> bool:
    if lang == "csharp":
        return any(file.endswith(".cs") for file in file_list)
    else:
        return len(file_list) > 1 or len(subdir_list) > 0


def get_data(checklist_orig: List[str], root_dir: str = "..") -> List[List[str]]:
    """

    Parameters
    ----------
    root_dir : str
        The root directory you want to start from.
    """
    lang_pos: Dict[str, int] = {
        lang: i for i, lang in enumerate(checklist_orig[1:], start=1)
    }
    strings_done: List[List[str]] = []

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

    empty_boxes = ["⬜️" for _ in checklist_orig]
    checklist = empty_boxes[:]
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
                    f"{split_dir[1]:<30}",
                ] + empty_boxes[1:]
                prev_game = split_dir[1]
        elif (
            len(split_dir) == 3 and split_dir[1] != ".git" and split_dir[2] in lang_pos
        ):
            out = (
                "✅"
                if has_implementation(split_dir[2], file_list, subdir_list)
                else "⬜️"
            )
            if split_dir[2] not in lang_pos or lang_pos[split_dir[2]] >= len(checklist):
                print(f"Could not find {split_dir[2]}: {dir_name}")
                checklist[lang_pos[split_dir[2]]] = "⬜️"
                continue
            checklist[lang_pos[split_dir[2]]] = out
    return strings_done


def write_file(path: str, languages: List[str], strings_done: List[List[str]]) -> None:
    dashes = " | ".join(["---"] * (len(languages) + 1))
    write_string = f"# TODO list\n game | {' | '.join(languages)}\n{dashes}\n"
    sorted_strings = list(
        map(lambda l: " | ".join(l) + "\n", sorted(strings_done, key=lambda x: x[0]))
    )
    write_string += "".join(sorted_strings)

    with open(path, "w", encoding="utf-8") as f:
        f.write(write_string)


if __name__ == "__main__":
    languages = [
        "csharp",
        "java",
        "javascript",
        "kotlin",
        "lua",
        "perl",
        "python",
        "ruby",
        "rust",
        "vbnet",
    ]
    strings_done = get_data(["game"] + languages[:])
    write_file("TODO.md", languages, strings_done)
