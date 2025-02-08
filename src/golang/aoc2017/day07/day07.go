package main

import (
	_ "embed"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	println("part1 =", part1(input))
	println("part2 =", part2(input))
}

func part1(input string) string {
	adjMap, _ := parse(input)
	return findRoot(adjMap)
}

func part2(input string) int {
	adjMap, weights := parse(input)
	initialWeights := make(map[string]int)
	for k, v := range weights {
		initialWeights[k] = v
	}
	root := findRoot(adjMap)
	visit(adjMap, &weights, root)

	current := root
	for {
		children := adjMap[current]
		childrenWeights := make([]int, 0, len(children))
		for _, v := range children {
			childrenWeights = append(childrenWeights, weights[v])
		}

		expected := childrenWeights[0]
		if childrenWeights[0] != childrenWeights[1] && childrenWeights[0] != childrenWeights[2] {
			expected = childrenWeights[1]
		}

		withError := 0
		for i := range children {
			if childrenWeights[i] != expected {
				withError = i
				break
			}
		}
		if isAllEqual(weights, adjMap[children[withError]]) {
			return initialWeights[children[withError]] - (childrenWeights[withError] - expected)
		}
		current = children[withError]
	}
}

func isAllEqual(weights map[string]int, nodes []string) bool {
	first := weights[nodes[0]]
	for _, node := range nodes {
		if weights[node] != first {
			return false
		}
	}
	return true
}

func visit(adjMap map[string][]string, weights *map[string]int, node string) int {
	if next, ok := adjMap[node]; ok {
		for _, nextNode := range next {
			(*weights)[node] += visit(adjMap, weights, nextNode)
		}
	}
	return (*weights)[node]
}

func findRoot(adjMap map[string][]string) string {
	for from := range adjMap {
		is_root := true
		for _, v := range adjMap {
			for _, to := range v {
				if to == from {
					is_root = false
				}
			}
		}
		if is_root {
			return from
		}
	}

	panic("No root")
}

func parse(input string) (map[string][]string, map[string]int) {
	lines := strings.Split(strings.TrimSuffix(input, "\n"), "\n")
	adjMap := make(map[string][]string, 0)
	weights := make(map[string]int, 0)

	for _, line := range lines {
		parts := strings.Split(line, " -> ")
		fromAndWeight := strings.Split(parts[0], " ")
		from := fromAndWeight[0]
		weight, err := strconv.Atoi(strings.Trim(fromAndWeight[1], "()"))
		if err != nil {
			panic(err)
		}
		weights[from] = weight
		if len(parts) > 1 {
			to := strings.Split(parts[1], ", ")
			adjMap[from] = append(adjMap[from], to...)
		}
	}
	return adjMap, weights
}
