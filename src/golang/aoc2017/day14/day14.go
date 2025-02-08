package main

import (
	"math/bits"
	"strconv"
	"strings"
)

const LIST_SIZE = 256
const ROUNDS = 64

var input string = "hfdlxzhv"

// var input string = "flqrgnkx"

func main() {
	println("part1 =", part1(input))
	println("part2 =", part2(input))
}

func part1(input string) int {
	used := 0
	for i := 0; i < 128; i++ {
		hash := knotHash(input + "-" + strconv.Itoa(i))
		for _, v := range hash {
			num, _ := strconv.ParseInt(string(v), 16, 64)
			used += bits.OnesCount(uint(num))
		}
	}
	return used
}

func part2(input string) int {
	grid := make([][]int, 128)
	for i := 0; i < 128; i++ {
		hash := knotHash(input + "-" + strconv.Itoa(i))
		grid[i] = make([]int, 0, 128)
		for _, v := range hash {
			num, _ := strconv.ParseInt(string(v), 16, 64)
			for j := 0; j < 4; j++ {
				bit := num & (1 << uint(3-j))
				grid[i] = append(grid[i], int(bit))
			}
		}
	}

	visited := make([][]bool, 128)
	for i := 0; i < 128; i++ {
		visited[i] = make([]bool, 128)
	}
	queue := make([][2]int, 0)
	regions := 0
	for i := 0; i < 128; i++ {
		for j := 0; j < 128; j++ {
			if visited[i][j] || grid[i][j] == 0 {
				continue
			}
			regions++
			queue = append(queue, [2]int{i, j})
			visited[i][j] = true
			for len(queue) > 0 {
				node := queue[0]
				queue = queue[1:]
				for _, dir := range [][2]int{{-1, 0}, {1, 0}, {0, -1}, {0, 1}} {
					newNode := [2]int{node[0] + dir[0], node[1] + dir[1]}
					if newNode[0] < 0 || newNode[0] >= 128 || newNode[1] < 0 || newNode[1] >= 128 {
						continue
					}
					if visited[newNode[0]][newNode[1]] || grid[newNode[0]][newNode[1]] == 0 {
						continue
					}
					visited[newNode[0]][newNode[1]] = true
					queue = append(queue, newNode)
				}
			}
		}
	}

	return regions
}

func knotHash(s string) string {
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
		hex := strconv.FormatInt(int64(v), 16)
		if len(hex) == 1 {
			builder.WriteString("0")
		}
		builder.WriteString(hex)
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
