import (
	"list"
)

{
	a: [...int] | *[6]
	b: [...int] | *[4, 5, 6]
	c: {
		_a: [ for a in a if list.Contains(b, a) {a}]
		_b: [ for b in b if list.Contains(a, b) {b}]

		// NOTE: this comparison is failing with cue version 0.4.3.
		if _a == a {
			true
		}
		if _b == b {
			true
		}
	}
}
