import (
	"list"
	"strings"
	"strconv"
)

#calories: number

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
		[ for d in c {strconv.Atoi(d)}]
	}]
}

output: number
output: {
	let calories_sum_by_elf = [ for elf in elves {elf._sum}]

	if len(calories_sum_by_elf) > 0 {
		list.Max(calories_sum_by_elf)
	}
	if len(calories_sum_by_elf) == 0 {
		0
	}
}
