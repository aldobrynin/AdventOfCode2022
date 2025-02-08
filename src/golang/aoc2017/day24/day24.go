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

func part1(input string) int {
	components := parse(input)

	adj := make(map[int][][2]int)
	for _, c := range components {
		adj[c[0]] = append(adj[c[0]], c)
		if c[0] != c[1] {
			adj[c[1]] = append(adj[c[1]], c)
		}
	}

	used := make(map[[2]int]bool)
	var dfs func(int, int) int
	dfs = func(node int, sum int) int {
		max := sum
		for _, next := range adj[node] {
			out := next[0]
			if out == node {
				out = next[1]
			}
			key := getKey(node, out)
			if used[key] {
				continue
			}
			used[key] = true
			max = maxInt(max, dfs(out, sum+node+out))
			used[key] = false
		}
		return max
	}

	return dfs(0, 0)
}

func getKey(i, j int) [2]int {
	if i < j {
		return [2]int{i, j}
	}
	return [2]int{j, i}
}

func maxInt(a, b int) int {
	if a > b {
		return a
	}
	return b
}

func part2(input string) int {
	components := parse(input)

	adj := make(map[int][][2]int)
	for _, c := range components {
		adj[c[0]] = append(adj[c[0]], c)
		if c[0] != c[1] {
			adj[c[1]] = append(adj[c[1]], c)
		}
	}

	used := make(map[[2]int]bool)
	maxLen, max := 0, 0
	var dfs func(int, int, int)
	dfs = func(node int, sum int, len int) {
		for _, next := range adj[node] {
			out := next[0]
			if out == node {
				out = next[1]
			}
			key := getKey(node, out)
			if used[key] {
				continue
			}
			used[key] = true
			dfs(out, sum+node+out, len+1)
			used[key] = false
		}
		if len > maxLen || (len == maxLen && sum > max) {
			maxLen = len
			max = sum
		}
	}

	dfs(0, 0, 0)
	return max
}

func parse(input string) [][2]int {
	lines := strings.Split(input, "\n")
	res := make([][2]int, len(lines))
	for i, line := range lines {
		parts := strings.Split(line, "/")
		res[i] = [2]int{toInt(parts[0]), toInt(parts[1])}
	}
	return res
}

func toInt(s string) int {
	res, err := strconv.Atoi(s)
	if err != nil {
		panic(err)
	}
	return res
}
