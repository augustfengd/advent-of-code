import (
	"list"
	"strings"
	"strconv"
)

#calories: int

#inventory: X={
	[...#calories]
	_sum: list.Sum(X)
}

#elves: [...#inventory]

input: string

elves: #elves
elves: {
	let a = strings.Split(input, "\n\n")
	[ for b in a {
		let c = strings.Split(b, "\n")
		[ for d in c if d != "" {strconv.Atoi(d)}]
	}]
}

output: [string]: int
output: {
	let calories_sum_by_elf = [ for elf in elves {elf._sum}]

	"1": list.Max(calories_sum_by_elf)
	"2": list.Sum([
		0 | *list.Sort(calories_sum_by_elf, list.Descending)[0],
		0 | *list.Sort(calories_sum_by_elf, list.Descending)[1],
		0 | *list.Sort(calories_sum_by_elf, list.Descending)[2],
	])
}
