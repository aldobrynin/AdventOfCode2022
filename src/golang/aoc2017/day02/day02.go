package main

import (
	_ "embed"
	"math"
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
	lines := strings.Split(input, "\n")
	var result = 0
	for _, line := range lines {
		var min, max = math.MaxInt, math.MinInt
		for _, v := range strings.Fields(line) {
			n, error := strconv.Atoi(v)
			if error != nil {
				panic(error)
			}
			if n < min {
				min = n
			}
			if n > max {
				max = n
			}
		}

		result += max - min
	}
	return result
}

func part2(input string) int {
	lines := strings.Split(input, "\n")
	var result = 0
	for _, line := range lines {
		parts := strings.Fields(line)
		var numbers []int
		for _, v := range parts {
			n, error := strconv.Atoi(v)
			if error != nil {
				panic(error)
			}
			numbers = append(numbers, n)
		}

		for i, n := range numbers {
			for j, m := range numbers {
				if i == j {
					continue
				}
				if n%m == 0 {
					println(n, m, n/m)
					result += n / m
				}
			}
		}
	}
	return result
}
