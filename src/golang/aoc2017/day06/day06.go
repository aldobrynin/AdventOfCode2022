package main

import (
	_ "embed"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	input = "0 2 7 0"
	part1, part2 := solve(input)
	println("part1 =", part1)
	println("part2 =", part2)
}

func solve(input string) (int, int) {
	visited := make(map[string]int)
	cycle := 0
	arr := make([]int, 0)
	for _, part := range strings.Fields(input) {
		num, err := strconv.Atoi(part)
		if err != nil {
			panic(err)
		}
		arr = append(arr, num)
	}

	for {
		builder := strings.Builder{}
		for i, el := range arr {
			if i > 0 {
				builder.WriteString(",")
			}
			builder.WriteString(strconv.Itoa(el))
		}
		key := builder.String()
		// println(key)
		if visited[key] > 0 {
			return cycle, cycle - visited[key]
		}
		visited[key] = cycle

		max_ind := 0
		for i := 0; i < len(arr); i++ {
			if arr[i] > arr[max_ind] {
				max_ind = i
			}
		}

		value := arr[max_ind]
		arr[max_ind] = 0
		cur_ind := max_ind + 1
		for value > 0 {
			arr[cur_ind%len(arr)]++
			value--
			cur_ind++
		}

		cycle++
	}
}
