package main

import (
	_ "embed"
	"slices"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	part1, part2 := solve(input)
	println("part1 =", part1)
	println("part2 =", part2)
}

var dirs = [][]int{{1, 0}, {0, 1}, {0, -1}, {-1, 0}}

func solve(input string) (string, int) {
	grid := parseGrid(input)
	letters := make([]rune, 0)
	row, col := 0, slices.Index(grid[0], '|')
	dir, steps := 0, 0

	var isValid = func(row, col int) bool {
		return row >= 0 && row < len(grid) && col >= 0 && col < len(grid[row]) && grid[row][col] != ' '
	}

	for {
		steps++
		if grid[row][col] >= 'A' && grid[row][col] <= 'Z' {
			letters = append(letters, grid[row][col])
		}

		found := false
		for i := 0; i < len(dirs) && !found; i++ {
			ind := (dir + i) % len(dirs)
			if dirs[ind][0] != -dirs[dir][0] || dirs[ind][1] != -dirs[dir][1] {
				nextRow, nextCol := row+dirs[ind][0], col+dirs[ind][1]
				if isValid(nextRow, nextCol) {
					row, col, dir = nextRow, nextCol, ind
					found = true
				}
			}
		}

		if !found {
			break
		}
	}

	return string(letters), steps
}

func parseGrid(input string) [][]rune {
	lines := strings.Split(input, "\n")
	grid := make([][]rune, len(lines))
	for i, line := range lines {
		grid[i] = []rune(line)
	}
	return grid
}
