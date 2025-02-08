package main

import (
	_ "embed"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	println("part1 =", part1(input))
	println("part2 =", part2(input))
}

func part1(input string) int {
	infected := make(map[complex64]bool)
	rows := strings.Split(input, "\n")
	for row, line := range rows {
		for col, val := range line {
			if val == '#' {
				infected[complex(float32(col), float32(row))] = true
			}
		}
	}

	dir := complex(float32(0), float32(-1))
	pos := complex(float32(len(rows)/2), float32(len(rows)/2))

	bursts := 10000
	infections := 0

	for i := 0; i < bursts; i++ {
		if infected[pos] {
			dir = turnRight(dir)
			delete(infected, pos)
		} else {
			dir = turnLeft(dir)
			infected[pos] = true
			infections++
		}
		pos += dir
	}
	return infections
}

const CLEAN = 0
const WEAKENED = 1
const INFECTED = 2
const FLAGGED = 3

func part2(input string) int {
	infected := make(map[complex64]int)
	rows := strings.Split(input, "\n")
	for row, line := range rows {
		for col, val := range line {
			if val == '#' {
				infected[complex(float32(col), float32(row))] = INFECTED
			}
		}
	}

	dir := complex(float32(0), float32(-1))
	pos := complex(float32(len(rows)/2), float32(len(rows)/2))

	bursts := 10000000
	infections := 0

	for i := 0; i < bursts; i++ {
		switch infected[pos] {
		case CLEAN:
			dir = turnLeft(dir)
			infected[pos] = WEAKENED
		case WEAKENED:
			infected[pos] = INFECTED
			infections++
		case INFECTED:
			dir = turnRight(dir)
			infected[pos] = FLAGGED
		case FLAGGED:
			dir = reverse(dir)
			delete(infected, pos)
		}

		pos += dir
	}
	return infections
}

func turnLeft(dir complex64) complex64 {
	return complex(imag(dir), -real(dir))
}

func turnRight(dir complex64) complex64 {
	return complex(-imag(dir), real(dir))
}

func reverse(dir complex64) complex64 {
	return complex(-real(dir), -imag(dir))
}
