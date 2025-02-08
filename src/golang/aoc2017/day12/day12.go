package main

import (
	_ "embed"
	"strings"
)

type V struct {
	col, row int
}

//go:embed input.txt
var input string

func main() {
	part1 := part1(input)
	part2 := part2(input)
	println("part1 = ", part1)
	println("part2 = ", part2)
}

func part1(s string) int {
	adjMap := buildAdjMap(s)

	return bfs(adjMap, make(map[string]bool), "0")
}

func part2(s string) int {
	adjMap := buildAdjMap(s)

	visited := make(map[string]bool)
	count := 0
	for node := range adjMap {
		if visited[node] {
			continue
		}
		count++
		bfs(adjMap, visited, node)
	}
	return count
}

func buildAdjMap(s string) map[string][]string {
	adjMap := make(map[string][]string)
	for _, line := range strings.Split(s, "\n") {
		parts := strings.Split(line, " <-> ")
		adjMap[parts[0]] = strings.Split(parts[1], ", ")
	}
	return adjMap
}

func bfs(adjMap map[string][]string, visited map[string]bool, start string) int {
	queue := []string{start}
	visited[start] = true
	count := 0
	for len(queue) > 0 {
		node := queue[0]
		queue = queue[1:]
		count++
		for _, adj := range adjMap[node] {
			if !visited[adj] {
				visited[adj] = true
				queue = append(queue, adj)
			}
		}
	}
	return count
}
