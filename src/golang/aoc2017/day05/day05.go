package main

import (
	_ "embed"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	lines := strings.Split(strings.TrimSuffix(input, "\n"), "\n")
	println("part1 =", part1(lines))
	println("part2 =", part2(lines))
}

func parse(lines []string) []int {
	arr := make([]int, 0, len(lines))
	for _, line := range lines {
		num, err := strconv.Atoi(line)
		if err != nil {
			panic(err)
		}
		arr = append(arr, num)
	}
	return arr
}

func part1(lines []string) int {
	arr := parse(lines)

	index := 0
	step := 0
	for index < len(arr) {
		jump := arr[index]
		arr[index]++
		index += jump
		step++
	}
	return step
}

func part2(lines []string) int {
	arr := parse(lines)

	index := 0
	step := 0
	for index < len(arr) {
		jump := arr[index]
		if jump >= 3 {
			arr[index]--
		} else {
			arr[index]++
		}
		index += jump
		step++
	}
	return step
}
