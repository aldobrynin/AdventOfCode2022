package main

import (
	_ "embed"
	"fmt"
	"strconv"
	"strings"
	"time"
)

//go:embed input.txt
var input string

func main() {
	start := time.Now()
	defer func() {
		fmt.Println("elapsed:", time.Since(start))
	}()
	println("part1 =", part1(input))
}

func part1(input string) int {
	initialState, steps, rules := parseInput(input)

	tape := make(map[int]int)
	cursor, currentState := 0, initialState

	checksum := 0
	for i := 0; i < steps; i++ {
		currentValue := tape[cursor]
		rule := (*rules)[currentState*10+currentValue]
		tape[cursor] = rule.writeValue
		cursor += rule.moveDirection
		currentState = rule.nextState

		checksum += (rule.writeValue - currentValue)
	}

	return checksum
}

func parseInput(input string) (int, int, *map[int]*Rule) {
	initialState := 0
	steps := 0
	rulesMap := make(map[int]*Rule)

	currentState := 0
	currentKey := 0

	for _, line := range strings.Split(input, "\n") {
		line = strings.TrimSpace(line)
		if line == "" {
			continue
		}

		if strings.HasPrefix(line, "Begin in state ") {
			initialState = int(strings.TrimSuffix(strings.Fields(line)[3], ".")[0] - 'A')
		} else if strings.HasPrefix(line, "Perform a diagnostic checksum after ") {
			val := strings.Fields(line)[5]
			steps, _ = strconv.Atoi(val)
		} else if strings.HasPrefix(line, "In state ") {
			currentState = int(strings.TrimSuffix(strings.Fields(line)[2], ":")[0] - 'A')
		} else if strings.HasPrefix(line, "If the current value is ") {
			val := int(strings.Fields(line)[5][0] - '0')
			currentKey = currentState*10 + val
			rulesMap[currentKey] = &Rule{}
		} else if strings.HasPrefix(line, "- Write the value ") {
			lastRule := rulesMap[currentKey]
			lastRule.writeValue = int(strings.Fields(line)[4][0] - '0')
		} else if strings.HasPrefix(line, "- Move one slot to the ") {
			lastRule := rulesMap[currentKey]
			if strings.HasSuffix(line, "right.") {
				lastRule.moveDirection = 1
			} else {
				lastRule.moveDirection = -1
			}
		} else if strings.HasPrefix(line, "- Continue with state ") {
			lastRule := rulesMap[currentKey]
			lastRule.nextState = int(strings.TrimSuffix(strings.Fields(line)[4], ".")[0] - 'A')
		}
	}

	return initialState, steps, &rulesMap
}

type Rule struct {
	writeValue    int
	moveDirection int
	nextState     int
}
