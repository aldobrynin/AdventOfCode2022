package main

import (
	_ "embed"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	s := strings.TrimSuffix(input, "\n")
	println("part1 =", solve(s, 1))
	println("part2 =", solve(s, len(s)/2))
}

func solve(input string, step int) int {
	n := len(input)
	var result = 0
	for v := range input {
		if input[v] == input[(v+step)%n] {
			result += int(input[v] - '0')
		}
	}
	return result
}
