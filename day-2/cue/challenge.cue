import (
	"list"
	"strings"
)

#Rock:    "A" | "X"
#Paper:   "B" | "Y"
#Scissor: "C" | "Z"

#Opponent: "A" | "B" | "C"
#Player:   "X" | "Y" | "Z"

#Win:  6
#Lose: 0
#Draw: 3

#Game: {
	a:     (#Rock | #Paper | #Scissor) & #Opponent
	b:     (#Rock | #Paper | #Scissor) & #Player
	score: int

	if a == (#Opponent & #Rock) {
		if b == (#Player & #Rock) {
			score: #Draw + 1
		}
		if b == (#Player & #Paper) {
			score: #Win + 2
		}
		if b == (#Player & #Scissor) {
			score: #Lose + 3
		}
	}
	if a == (#Opponent & #Paper) {
		if b == (#Player & #Rock) {
			score: #Lose + 1
		}
		if b == (#Player & #Paper) {
			score: #Draw + 2
		}
		if b == (#Player & #Scissor) {
			score: #Win + 3
		}
	}
	if a == (#Opponent & #Scissor) {
		if b == (#Player & #Rock) {
			score: #Win + 1
		}
		if b == (#Player & #Paper) {
			score: #Lose + 2
		}
		if b == (#Player & #Scissor) {
			score: #Draw + 3
		}
	}
}

input: string

games: [...#Game]
games: [ for line in strings.Split(input, "\n") if line != "" {
	let moves = strings.Split(line, " ")
	a: _|_ | *moves[0]
	b: _|_ | *moves[1]
}]

output: [string]: int
output: {
	"1": list.Sum([ for game in games {game.score}])
}
