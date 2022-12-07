import (
	"list"
)

data: [true, true, true, false]

foobar: [ for i, d in data if i > 0 && data[i-1] {
	d
}]
