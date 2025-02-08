package main

import (
	_ "embed"
	"strings"
)

type V struct {
	col, row int
}

var directions = map[string]V{
	"n":  {0, -2},
	"ne": {1, -1},
	"se": {1, 1},
	"s":  {0, 2},
	"sw": {-1, 1},
	"nw": {-1, -1},
}

//go:embed input.txt
var input string

func main() {
	part1, part2 := solve(input)
	println("part1 =", part1)
	println("part2 =", part2)
}

func solve(s string) (int, int) {
	zero := V{0, 0}
	current := V{0, 0}
	maxDistance := 0

	for _, dir := range strings.Split(s, ",") {
		current.row += directions[dir].row
		current.col += directions[dir].col
		maxDistance = max(maxDistance, distance(zero, current))
	}

	return distance(zero, current), maxDistance
}

func distance(a, b V) int {
	var dcol = abs(a.col - b.col)
	var drow = abs(a.row - b.row)
	return dcol + max(0, (drow-dcol)/2)
}

func abs(x int) int {
	if x < 0 {
		return -x
	}
	return x
}

func max(a, b int) int {
	if a > b {
		return a
	}
	return b
}
