import (
	"strings"
	"strconv"
	"list"
)

#Datastream: {
	message: string
	message: strings.MinRunes(4)

	marker: {
		length: int
		position: {
			let characters = strings.Split(message, "")
			let unique_combinations = {
				for i, _ in characters if i >= length-1 {
					let combination = list.Slice(characters, i-length+1, i+1)
					if list.UniqueItems(list.Slice(characters, i-length+1, i+1)) {
						"\(i+1)": combination // ?bug?: seems to cause conflict if i use combination as argument here.
					}
				}
			}
			[ for i, _ in unique_combinations {i}][0]
		}
	}
}

input: string | *"""
	mjqjpqmgbljsphdztnvjfqwrcgsmlb
	"""

datastream: #Datastream & {message: input}

output: [string]: string
output: {
	"1": (datastream & {marker: length: 4}).marker.position
	"2": (datastream & {marker: length: 14}).marker.position
}
