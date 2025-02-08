package main

import (
	_ "embed"
)

//go:embed input.txt
var input string

func main() {
	part1, part2 := solve(input)
	println("part1 =", part1)
	println("part2 =", part2)
}

func solve(s string) (int, int) {
	stack := make([]rune, 0)
	score := 0
	garbage_count := 0
	garbage := false
	ignore := false
	for _, r := range s {
		switch {
		case ignore:
			ignore = false
		case r == '!':
			ignore = true
		case garbage:
			if r == '>' {
				garbage = false
			} else {
				garbage_count++
			}
		case r == '<':
			garbage = true
		case r == '{':
			stack = append(stack, r)
		case r == '}':
			score += len(stack)
			stack = stack[:len(stack)-1]
		}
	}

	return score, garbage_count
}
