import (
	"list"
	"strings"
)

#Rock:    "Rock"
#Paper:   "Paper"
#Scissor: "Scissor"

#Lose: 0
#Draw: 3
#Win:  6

#GameInput: {
	input: {
		part: 1 | 2
		"1":  "A" | "B" | "C"
		"2":  "X" | "Y" | "Z"
	}

	if input."1" == "A" {
		a: #Rock
	}
	if input."1" == "B" {
		a: #Paper
	}
	if input."1" == "C" {
		a: #Scissor
	}

	if input.part == 1 {
		if input."2" == "X" {
			b: #Rock
		}
		if input."2" == "Y" {
			b: #Paper
		}
		if input."2" == "Z" {
			b: #Scissor
		}
	}

	if input.part == 2 {
		if input."2" == "X" {
			score: x: #Lose
		}
		if input."2" == "Y" {
			score: x: #Draw
		}
		if input."2" == "Z" {
			score: x: #Win
		}
	}
}

#GameState: {
	a: #Rock | #Paper | #Scissor
	b: #Rock | #Paper | #Scissor
	score: {
		x: #Win | #Lose | #Draw
		y: (1 | 2 | 3) & {
			if b == #Rock {1}
			if b == #Paper {2}
			if b == #Scissor {3}
		}
		total: x + y
	}
}

#GameRules: {
	a: _
	b: _
	score: x: _

	if a == #Rock {
		if score.x == #Lose {
			b: #Scissor
		}
		if score.x == #Draw {
			b: #Rock
		}
		if score.x == #Win {
			b: #Paper
		}
		if b == #Rock {
			score: x: #Draw
		}
		if b == #Paper {
			score: x: #Win
		}
		if b == #Scissor {
			score: x: #Lose
		}
	}
	if a == #Paper {
		if score.x == #Lose {
			b: #Rock
		}
		if score.x == #Draw {
			b: #Paper
		}
		if score.x == #Win {
			b: #Scissor
		}
		if b == #Rock {
			score: x: #Lose
		}
		if b == #Paper {
			score: x: #Draw
		}
		if b == #Scissor {
			score: x: #Win
		}
	}
	if a == #Scissor {
		if score.x == #Lose {
			b: #Paper
		}
		if score.x == #Draw {
			b: #Scissor
		}
		if score.x == #Win {
			b: #Rock
		}
		if b == #Rock {
			score: x: #Win
		}
		if b == #Paper {
			score: x: #Lose
		}
		if b == #Scissor {
			score: x: #Draw
		}
	}
}

#Games: [...#Game]
#Game: {
	#GameInput
	#GameState
	#GameRules
}

input: string

games: #Games & [ for line in strings.Split(input, "\n") if line != "" {
	let inputs = strings.Split(line, " ")
	input: "1": inputs[0]
	input: "2": inputs[1]
}]

output: [string]: int
output: {
	"1": list.Sum([ for game in games & [...{input: part: 1}] {game.score.total}])
	"2": list.Sum([ for game in games & [...{input: part: 2}] {game.score.total}])
}
