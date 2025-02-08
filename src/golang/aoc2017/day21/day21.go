package main

import (
	_ "embed"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	println("part1 =", solve(input, 5))
	println("part2 =", solve(input, 18))
}

func solve(input string, iterations int) int {
	pattern := [][]rune{{'.', '#', '.'}, {'.', '.', '#'}, {'#', '#', '#'}}
	rulesMap := make(map[string]string)

	for _, line := range strings.Split(input, "\n") {
		parts := strings.Split(line, " => ")
		input, output := parse(parts[0]), parts[1]
		for i := 0; i < 4; i++ {
			rulesMap[toStr(input)] = output
			input = rotate(input)
		}
		input = flip(input)
		for i := 0; i < 4; i++ {
			rulesMap[toStr(input)] = output
			input = rotate(input)
		}
	}

	for i := 0; i < iterations; i++ {
		pattern = apply(pattern, rulesMap)
	}

	count := 0
	for _, row := range pattern {
		for _, cell := range row {
			if cell == '#' {
				count++
			}
		}
	}

	return count
}

func toStr(a [][]rune) string {
	builder := strings.Builder{}
	for i, row := range a {
		if i > 0 {
			builder.WriteRune('/')
		}
		builder.WriteString(string(row))
	}
	return builder.String()
}

func apply(pattern [][]rune, rulesMap map[string]string) [][]rune {
	size := len(pattern)
	var squareSize int

	if size%2 == 0 {
		squareSize = 2
	} else {
		squareSize = 3
	}
	var res [][]rune
	res = make([][]rune, size/squareSize*(squareSize+1))
	for i := 0; i < len(res); i++ {
		res[i] = make([]rune, len(res))
	}

	for i := 0; i < size/squareSize; i++ {
		for j := 0; j < size/squareSize; j++ {
			startI, startJ := i*squareSize, j*squareSize
			squareBuilder := strings.Builder{}
			for k := 0; k < squareSize; k++ {
				if k > 0 {
					squareBuilder.WriteString("/")
				}
				squareBuilder.WriteString(string(pattern[startI+k][startJ : startJ+squareSize]))
			}

			square := squareBuilder.String()

			if output, ok := rulesMap[square]; ok {
				outputRow, outputCol := 0, 0
				for _, val := range output {
					if val == '/' {
						outputRow++
						outputCol = 0
					} else {
						res[i*(squareSize+1)+outputRow][j*(squareSize+1)+outputCol] = val
						outputCol++
					}
				}
			} else {
				println(square)
				panic("rule not found")
			}
		}
	}

	return res
}

func printSquare(a [][]rune) {
	for _, row := range a {
		println(string(row))
	}
}

func rotate(a [][]rune) [][]rune {
	size := len(a)
	res := make([][]rune, size)
	for i := 0; i < size; i++ {
		res[i] = make([]rune, size)
		for j := 0; j < size; j++ {
			res[i][j] = a[size-j-1][i]
		}
	}
	return res
}

func flip(a [][]rune) [][]rune {
	size := len(a)
	res := make([][]rune, size)
	for i := 0; i < size; i++ {
		res[i] = make([]rune, size)
		for j := 0; j < size; j++ {
			res[i][j] = a[i][size-j-1]
		}
	}
	return res
}

func parse(s string) [][]rune {
	rows := strings.Split(s, "/")
	res := make([][]rune, len(rows))
	for i, row := range rows {
		res[i] = []rune(row)
	}
	return res
}
