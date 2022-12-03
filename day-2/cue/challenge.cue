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

#Games: [...#Game]
#Game: {
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

#ConsumeInput: {
	_eat: {
		_part: 1 | 2
		_line: string
		_col:  strings.Split(_line, " ")
		_col: [
			{"A" | "B" | "C"},
			{"X" | "Y" | "Z"},
		]

		if _part == 1 {
			let map = {
				"A": #Rock
				"X": #Rock
				"B": #Paper
				"Y": #Paper
				"C": #Scissor
				"Z": #Scissor
			}
			a: map[_col[0]]
			b: map[_col[1]]
		}
		if _part == 2 {
			let map = {
				"A": #Rock
				"X": #Lose
				"B": #Paper
				"Y": #Draw
				"C": #Scissor
				"Z": #Win
			}
			a: map[_col[0]]
			score: x: map[_col[1]]
		}

		#Game
	}
	[ for line in strings.Split(input, "\n") if line != "" {_eat & {_line: line}}]
}

input: string

games: #Games & #ConsumeInput

output: [string]: int
output: {
	"1": list.Sum([ for game in games & [...{_part: 1}] {game.score.total}])
	"2": list.Sum([ for game in games & [...{_part: 2}] {game.score.total}])
}
