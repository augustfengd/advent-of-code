import (
	"strings"
	"list"
)

#Priority: {
	{
		for i, c in ["a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"] {
			(c): (i) + 1
		}
	}
	{
		for i, c in ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"] {
			(c): (i) + 27
		}
	}
}

#Rucksack: {
	a: [...string]
	b: [...string]

	c: {for c in a if list.Contains(b, c) {(c): _}}
	priority: list.Sum([ for x, _ in c {#Priority[x]}])
}

input: string

rucksacks: [...#Rucksack]
rucksacks: [ for line in strings.Split(input, "\n") if line != "" {
	a: {
		let compartment = strings.SliceRunes(line, 0, div(len(line), 2))
		strings.Split(compartment, "")
	}
	b: {
		let compartment = strings.SliceRunes(line, div(len(line), 2), len(line))
		strings.Split(compartment, "")
	}
}]

output: int
output: list.Sum([ for rucksack in rucksacks {rucksack.priority}])
