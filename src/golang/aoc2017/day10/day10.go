package main

import (
	_ "embed"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

const LIST_SIZE = 256
const ROUNDS = 64

func main() {
	println("part1 =", part1(input))
	println("part2 =", part2(input))
}

func part1(s string) int {
	list := make([]int, LIST_SIZE)
	for i := 0; i < LIST_SIZE; i++ {
		list[i] = i
	}

	lengths := parseArray(s)
	current := 0
	skip := 0
	for _, length := range lengths {
		reverse(list, current, length)
		current += length + skip
		skip++
	}
	return list[0] * list[1]
}

func part2(s string) string {
	list := make([]int, LIST_SIZE)
	for i := 0; i < LIST_SIZE; i++ {
		list[i] = i
	}

	lengths := make([]int, 0)
	for _, v := range s {
		lengths = append(lengths, int(v))
	}
	lengths = append(lengths, 17, 31, 73, 47, 23)
	current := 0
	skip := 0
	for round := 0; round < ROUNDS; round++ {
		for _, length := range lengths {
			reverse(list, current, length)
			current += length + skip
			skip++
		}
	}

	hash := toDenseHash(list)

	builder := strings.Builder{}
	for _, v := range hash {
		builder.WriteString(strconv.FormatInt(int64(v), 16))
	}
	return builder.String()
}

func toDenseHash(list []int) []int {
	blocks := make([]int, 16)
	for i := 0; i < len(list); i += 16 {
		block := 0
		for j := 0; j < 16; j++ {
			block ^= list[i+j]
		}
		blocks[i/16] = block
	}
	return blocks
}

func reverse(list []int, start int, length int) {
	for i := 0; i < length/2; i++ {
		a := (start + i) % len(list)
		b := (start + length - i - 1) % len(list)
		list[a], list[b] = list[b], list[a]
	}
}

func parseArray(s string) []int {
	res := make([]int, 0)

	for _, v := range strings.Split(s, ",") {
		num, err := strconv.Atoi(v)
		if err != nil {
			panic(err)
		}
		res = append(res, num)
	}
	return res
}
