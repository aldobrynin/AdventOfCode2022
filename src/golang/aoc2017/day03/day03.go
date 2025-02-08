package main

import (
	_ "embed"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	println("part1 =", part1(strings.TrimSuffix(input, "\n")))
	println("part2 =", part2(strings.TrimSuffix(input, "\n")))
}

func part1(input string) int {
	num, err := strconv.Atoi(input)
	if err != nil {
		panic(err)
	}

	prev := 0
	current := 1
	level := 1

	for current < num {
		level += 2
		length := 4 * (level - 1)
		prev, current = current, current+length
	}

	if level == 1 {
		return 0
	}
	offset := (num - prev - 1) % (level - 1)
	mid := level/2 - 1
	res := level/2 + abs(mid-offset)

	return res
}

func part2(input string) int {
	target, err := strconv.Atoi(input)
	if err != nil {
		panic(err)
	}

	current := [][]int{{1}}

	for {
		next_size := len(current) + 2
		println("next_size =", next_size)
		next := make([][]int, next_size)
		for i := 0; i < next_size; i++ {
			next[i] = make([]int, next_size)
		}

		for row := 0; row < len(current); row++ {
			for col := 0; col < len(current); col++ {
				next[row+1][col+1] = current[row][col]
			}
		}

		for row := next_size - 2; row >= 0; row-- {
			next[row][next_size-1] = calc(next, row, next_size-1)
			if next[row][next_size-1] > target {
				return next[row][next_size-1]
			}
		}

		for col := next_size - 2; col >= 0; col-- {
			next[0][col] = calc(next, 0, col)
			if next[0][col] > target {
				return next[0][col]
			}
		}

		for row := 1; row < next_size; row++ {
			next[row][0] = calc(next, row, 0)
			if next[row][0] > target {
				return next[row][0]
			}
		}

		for col := 1; col < next_size; col++ {
			next[next_size-1][col] = calc(next, next_size-1, col)
			if next[next_size-1][col] > target {
				return next[next_size-1][col]
			}
		}

		current = next
	}
}

func calc(mat [][]int, row int, col int) int {
	res := 0
	for dy := -1; dy <= 1; dy++ {
		for dx := -1; dx <= 1; dx++ {
			y, x := row+dy, col+dx
			if y >= 0 && y < len(mat) && x >= 0 && x < len(mat) {
				res += mat[y][x]
			}
		}
	}

	return res
}

func abs(a int) int {
	if a >= 0 {
		return a
	}
	return -a
}
