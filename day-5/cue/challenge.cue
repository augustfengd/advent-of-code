import (
	"list"
	"strings"
	"strconv"
)

#Stacks: {[string]: [...string]}

#Ops: [...#Op]
#Op: {
	src: string
	dst: string
	qty: int
} | *{}

#States: {
	stacks: #Stacks
	op:     #Op
	next:   #States | *null
}

input: string | *"""
	move 2 from 8 to 2
	move 3 from 9 to 2
	move 1 from 3 to 8
	move 5 from 1 to 7
	"""

#ConsumeInput: [ for line in strings.Split(input, "\n") if line =~ "move \\d+ from \\d+ to \\d+" {
	let words = strings.Split(line, " ")
	src: words[3]
	dst: words[5]
	qty: strconv.Atoi(words[1])
}]

Ops: #ConsumeInput

S: [#States & {
	stacks: {
		"1": ["F", "C", "J", "P", "H", "T", "W"]
		"2": ["G", "R", "V", "F", "Z", "J", "B", "H"]
		"3": ["H", "P", "T", "R"]
		"4": ["Z", "S", "N", "P", "H", "T"]
		"5": ["N", "V", "F", "Z", "H", "J", "C", "D"]
		"6": ["P", "M", "G", "F", "W", "D", "Z"]
		"7": ["M", "V", "Z", "W", "S", "J", "D", "P"]
		"8": ["N", "D", "S"]
		"9": ["D", "Z", "S", "F", "M"]
	}
	next: #States
}, ...#States]

S: [ for i, o in Ops {
	if i > 0 {
		let previous = S[i-1]
		stacks: {
			let crates = list.Slice(list.Sort(previous.stacks[previous.op.src], {less: true}), 0, previous.op.qty)
			(previous.op.dst): previous.stacks[previous.op.dst] + crates
			(previous.op.src): list.Slice(previous.stacks[previous.op.src], 0, len(previous.stacks[previous.op.src])-previous.op.qty)

			for stack, content in previous.stacks if stack != previous.op.dst && stack != previous.op.src {
				(stack): content
			}
		}
	}
	op: o & {
		src: string
		dst: string
		qty: int
	}
	if i < len(S)-1 {
		next: S[i+1]
	}
}]

output: "1": S[0].stacks
