package main

import "slices"

func main() {
	input := 366
	println("part1 =", part1(input))
	println("part2 =", part2(input))
}

func part1(input int) int {
	buffer := make([]int, 1, 2017)
	pos := 0

	for i := 1; i <= 2017; i++ {
		pos = (pos+input)%i + 1
		buffer = slices.Insert(buffer, pos, i)
	}

	return buffer[(pos+1)%len(buffer)]
}

func part2(input int) int {
	pos, res := 0, 0
	for i := 1; i <= 50_000_000; i++ {
		pos = (pos+input)%i + 1
		if pos == 1 {
			res = i
		}
	}
	return res
}
