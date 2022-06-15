require("math")
require("string")

print("\n                         Sine Wave")
print("           Creative Computing Morriston, New Jersy")
print("\n\n\n\n")

-- Original BASIC version by David Ahl
-- Ported to lua by BeeverFeever(github), 2022

local toggleWord = true

for t = 0, 40, 0.25 do
    local gap = math.floor(26 + 25 * math.sin(t))
    if toggleWord == true then
        -- string.rep used to add the gat at the front of the printed out words
        print(string.rep(" ", math.floor(gap)) .. "Creative")
    elseif toggleWord == false then
        print(string.rep(" ", math.floor(gap)) .. "Computing")
    end
end
