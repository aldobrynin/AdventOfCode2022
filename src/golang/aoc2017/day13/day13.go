package main

import (
	_ "embed"
	"log"
	"sort"
	"strconv"
	"strings"
	"time"
)

//go:embed input.txt
var input string

func main() {
	println("part1 =", part1(input))
	println("part2 =", part2(input))
	println("part2 =", part2v2(input))
}

func part1(s string) int {
	layers := parseInput(s)

	return calcSeverity(layers)
}

func part2(s string) int {
	defer timeTrack(time.Now(), "part2")
	layers := parseInput(s)

	delay := 0
	for ; isCaughtByAny(layers, delay); delay++ {
	}
	return delay
}

func part2v2(s string) int {
	defer timeTrack(time.Now(), "part2v2")
	layers := parseInput(s)

	neq := make(map[int][]int)
	for depth := range layers {
		key := layers[depth]*2 - 2
		remainder := mod(-depth, key)
		neq[key] = append(neq[key], remainder)
	}

	moduli := make([]int, 0, len(neq))
	for key := range neq {
		moduli = append(moduli, key)
	}
	sort.Ints(moduli)

	residues := []int{0}
	prevLcm, currentLcm := 1, 1

	for _, m := range moduli {
		prevLcm = currentLcm
		currentLcm = lcm(currentLcm, m)
		nextResidues := []int{}

		for _, r1 := range residues {
			for i := r1; i < currentLcm; i += prevLcm {
				if !contains(neq[m], i%m) {
					nextResidues = append(nextResidues, i)
				}
			}
		}
		residues = nextResidues
	}

	min := residues[0]
	for _, r := range residues {
		if r < min {
			min = r
		}
	}
	return min
}

func lcm(a, b int) int {
	return a * (b / gcd(a, b))
}

func mod(a, b int) int {
	return (a%b + b) % b
}

func contains(arr []int, val int) bool {
	for _, v := range arr {
		if v == val {
			return true
		}
	}
	return false
}

func gcd(a, b int) int {
	for b != 0 {
		a, b = b, a%b
	}
	return a
}

func calcSeverity(layers map[int]int) int {
	severity := 0
	for depth, rangeValue := range layers {
		if isCaughtBy(depth, rangeValue, 0) {
			severity += depth * rangeValue
		}
	}
	return severity
}

func isCaughtByAny(layers map[int]int, delay int) bool {
	for depth, rangeValue := range layers {
		if isCaughtBy(depth, rangeValue, delay) {
			return true
		}
	}
	return false
}

func isCaughtBy(depth int, rangeValue int, delay int) bool {
	return getScannerPosition(rangeValue, depth+delay) == 0
}

func getScannerPosition(rangeValue, time int) int {
	return (time) % ((rangeValue - 1) * 2)
}

func parseInput(s string) map[int]int {
	layers := make(map[int]int)
	for _, line := range strings.Split(s, "\n") {
		parts := strings.Split(line, ": ")
		depth, _ := strconv.Atoi(parts[0])
		rangeValue, _ := strconv.Atoi(parts[1])
		layers[depth] = rangeValue
	}
	return layers
}

func timeTrack(start time.Time, name string) {
	elapsed := time.Since(start)
	log.Printf("%s took %s", name, elapsed)
}
