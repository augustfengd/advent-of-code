import (
	"strconv"
	"list"
	"strings"
	// "math/bits"
)

#SectionInput: {
	_a: string | *"2-4"
	_b: string | *"6-8"

	#ParseRangeString: {
		_range: string
		_start: int & strconv.Atoi(string & strings.Split(_range, "-")[0])
		_end:   int & strconv.Atoi(string & strings.Split(_range, "-")[1])
		list.Range(_start, _end+1, 1)
	}

	a: #ParseRangeString & {_range: _a, _}
	b: #ParseRangeString & {_range: _b, _}
}

#SectionState: {
	a: [...int]
	b: [...int]
	c: bool | *false
	c: {
		_a: [ for a in a if list.Contains(b, a) {a}]
		_b: [ for b in b if list.Contains(a, b) {b}]
		if _a == a {
			true
		}
		if _b == b {
			true
		}
	}
}

#Sections: [...#Section]
#Section: {
	#SectionInput
	#SectionState
}

input: string | *"""
	2-4,6-8
	2-3,4-5
	5-7,7-9
	2-8,3-7
	6-6,4-6
	2-6,4-8
	"""

sections: #Sections & [ for line in strings.Split(input, "\n") {
	let _ab = strings.Split(line, ",")
	_a: _ab[0]
	_b: _ab[1]
}]

output: {
	"1": len([ for section in sections if section.c {_}])
}
