package main

import (
	_ "embed"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	println("part1 =", dance(input, 1))
	println("part2 =", dance(input, 1_000_000_000))
}

func dance(moves string, dances int) string {
	arr := make([]rune, 16)
	for i := range arr {
		arr[i] = rune('a' + i)
	}

	seq := strings.Split(moves, ",")
	visited := make(map[string]int)
	history := make([]string, 0)

	for i := 0; i < dances; i++ {
		for _, move := range seq {
			if move[0] == 's' {
				n, _ := strconv.Atoi(move[1:])
				arr = append(arr[len(arr)-n:], arr[:len(arr)-n]...)
			}
			if move[0] == 'x' {
				parts := strings.Split(move[1:], "/")
				a, _ := strconv.Atoi(parts[0])
				b, _ := strconv.Atoi(parts[1])
				arr[a], arr[b] = arr[b], arr[a]
			}
			if move[0] == 'p' {
				a := strings.IndexRune(string(arr), rune(move[1]))
				b := strings.IndexRune(string(arr), rune(move[3]))
				arr[a], arr[b] = arr[b], arr[a]
			}
		}

		key := toStr(arr)
		history = append(history, key)
		if prevIndex, ok := visited[key]; ok {
			period := i - prevIndex
			remaining := (dances - i - 1) % period
			return history[prevIndex+remaining]
		}
		visited[key] = i
	}

	return toStr(arr)
}

func toStr(arr []rune) string {
	builder := strings.Builder{}
	for _, c := range arr {
		builder.WriteRune(c)
	}
	return builder.String()
}
