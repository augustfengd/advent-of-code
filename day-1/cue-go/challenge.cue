import (
	"list"
)

#calories: number

#inventory: X={
	[...#calories]
	_sum: list.Sum(X)
}

#elves: [...#inventory]

elves: #elves

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
