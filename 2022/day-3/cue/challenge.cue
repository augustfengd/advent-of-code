import (
	"strings"
	"list"
	"strconv"
)

#Priority: {
	{
		for i, c in strings.Split("abcdefghijklmnopqrstuvwxyz", "") {
			(c): (i) + 1
		}
	}
	{
		for i, c in strings.Split("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "") {
			(c): (i) + 27
		}
	}
}

#Rucksacks: [...#Rucksack]
#Rucksack: {
	ab: [...string]
	a: list.Slice(ab, 0, div(len(ab), 2))
	b: list.Slice(ab, div(len(ab), 2), len(ab))

	c: {
		all: [ for c in a if list.Contains(b, c) {c}]
		unique: {
			let characters = {for c in all {(c): _}}
			[ for c, _ in characters {c}]
		}
	}
	priority: list.Sum([ for c in c.unique {#Priority[c]}])
}

#RucksackGroups: [...#RucksackGroup]
#RucksackGroup: {
	a: #Rucksack
	b: #Rucksack
	c: #Rucksack

	d: {
		all: [ for d in a.ab if list.Contains(b.ab, d) if list.Contains(c.ab, d) {d}]
		unique: {
			let characters = {for c in all {(c): _}}
			[ for c, _ in characters {c}]
		}
	}
	priority: list.Sum([ for c in d.unique {#Priority[c]}])
}

input: string

rucksacks: #Rucksacks & [ for line in strings.Split(input, "\n") if line != "" {
	ab: strings.Split(line, "")
}]

rucksackGroups: #RucksackGroups & {
	let groups = {
		for i, rucksack in rucksacks {
			(strconv.FormatInt(div(i, 3), 10)): (strconv.FormatInt(mod(i, 3), 10)): rucksack // NOTE: requires cue version greater than 0.4.3; there's a bug that causes early termination of the comprehension.
		}
	}
	[ for group in groups {a: group."0", b: group."1", c: group."2"}]
}

output: [string]: int
output: {
	"1": list.Sum([ for rucksack in rucksacks {rucksack.priority}])
	"2": list.Sum([ for rucksackGroup in rucksackGroups {rucksackGroup.priority}])
}
