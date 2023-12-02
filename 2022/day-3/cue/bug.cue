import (
	"strconv"
)

let foobar = [{foobar: "hello"}, {foobar: "world"}]

{
	for a, b in foobar {
		(strconv.FormatInt(div(a, 3), 10)): b // NOTE: this should fail, but passes w/ cue version 0.4.3.
	}
}
