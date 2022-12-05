{
	a: {
		_foobar: int | *1
		_foobaz: int | *1
		_foobar + _foobaz
	}

	b: a & {_foobar: 2} // a simple fix is this: `a & {_foobar: 2, _}`, nonetheless, is it appropriate for this to conflict?
}
