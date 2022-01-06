import os


lang_pos = {
    "csharp": 1, "java": 2, "javascript": 3,
    "pascal": 4, "perl": 5, "python": 6, "ruby": 7, "vbnet": 8
}

write_string = "# TODO list \n game | csharp | java | javascript | pascal | perl | python | ruby | vbnet \n --- | --- | --- | --- | --- | --- | --- | --- | ---  \n"
# Set the directory you want to start from
rootDir = '..'

checklist = ["game", "csharp", "java", "javascript",
             "pascal", "perl", "python", "ruby", "vbnet"]

prev_game = ""

for dirName, subdirList, fileList in os.walk(rootDir):
    split_dir = dirName.split("/")

    if len(split_dir) == 2 and not split_dir[1] in ['.git', '00_Utilities']:
        if prev_game == "":
            prev_game = split_dir[1]
            checklist[0] = split_dir[1]

        if prev_game != split_dir[1]:
            # it's a new dir
            write_string += " | ".join(checklist) + "\n"
            checklist = [split_dir[1], "csharp", "java", "javascript",
                         "pascal", "perl", "python", "ruby", "vbnet"]
            prev_game = split_dir[1]

    elif len(split_dir) == 3 and split_dir[1] != '.git':
        if split_dir[2] in lang_pos.keys():
            if len(fileList) > 1 or len(subdirList) > 0:
                # there is more files than the readme
                checklist[lang_pos[split_dir[2]]] = "✅"
            else:
                checklist[lang_pos[split_dir[2]]] = "⬜️"


with open("README.md", "w") as f:
    f.write(write_string)
