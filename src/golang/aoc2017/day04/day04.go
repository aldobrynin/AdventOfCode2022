package main

import (
	_ "embed"
	"sort"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	lines := strings.Split(strings.TrimSuffix(input, "\n"), "\n")
	println("part1 =", part1(lines))
	println("part2 =", part2(lines))
}

func part1(lines []string) int {
	res := 0
	for _, line := range lines {
		if isValid(line) {
			res++
		}
	}
	return res
}

func part2(lines []string) int {
	res := 0
	for _, line := range lines {
		if isValid2(line) {
			res++
		}
	}
	return res
}

func isValid(line string) bool {
	words := make(map[string]bool)
	for _, word := range strings.Fields(line) {
		if words[word] == true {
			return false
		}
		words[word] = true
	}

	return true
}

func isValid2(line string) bool {
	words := make(map[string]bool)
	for _, word := range strings.Fields(line) {
		sorted := SortString(word)
		if words[sorted] == true {
			return false
		}
		words[sorted] = true
	}

	return true
}

type sortRunes []rune

func (s sortRunes) Less(i, j int) bool {
	return s[i] < s[j]
}

func (s sortRunes) Swap(i, j int) {
	s[i], s[j] = s[j], s[i]
}

func (s sortRunes) Len() int {
	return len(s)
}

func SortString(s string) string {
	r := []rune(s)
	sort.Sort(sortRunes(r))
	return string(r)
}
