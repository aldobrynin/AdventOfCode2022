package main

import (
	_ "embed"
	"math"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	part1, part2 := solve(input)
	println("part1 =", part1)
	println("part2 =", part2)
}

func solve(input string) (int, int) {
	registers := make(map[string]int)
	lines := strings.Split(input, "\n")

	intermediateMax := math.MinInt32

	for _, line := range lines {
		parts := strings.Fields(line)
		if len(parts) == 0 {
			continue
		}

		reg := parts[0]
		op := parts[1]
		val, _ := strconv.Atoi(parts[2])

		condReg := parts[4]
		condOp := parts[5]
		condVal, _ := strconv.Atoi(parts[6])

		condRegValue := registers[condReg]

		isTrue := false
		switch condOp {
		case "==":
			isTrue = condRegValue == condVal
		case "!=":
			isTrue = condRegValue != condVal
		case ">":
			isTrue = condRegValue > condVal
		case "<":
			isTrue = condRegValue < condVal
		case ">=":
			isTrue = condRegValue >= condVal
		case "<=":
			isTrue = condRegValue <= condVal
		default:
			panic("unknown condition operator")
		}
		if isTrue {
			switch op {
			case "inc":
				registers[reg] += val
			case "dec":
				registers[reg] -= val
			default:
				panic("unknown operation")
			}
		}

		if registers[reg] > intermediateMax {
			intermediateMax = registers[reg]
		}
	}

	max := math.MinInt32
	for _, v := range registers {
		if v > max {
			max = v
		}
	}
	return max, intermediateMax
}
