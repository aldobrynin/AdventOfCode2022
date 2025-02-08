package main

import (
	_ "embed"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	// print()
	part1AnswerSimulation, _ := simulate(0)
	_, part2AnswerSimulation := simulate(1)
	part1AnswerOpt, _ := solve(input, 0)
	_, part2AnswerOpt := solve(input, 1)
	println("part1 =", part1AnswerOpt)
	println("part1 (simulation) =", part1AnswerSimulation)
	println("part2 =", part2AnswerOpt)
	println("part2 (simulation) =", part2AnswerSimulation)
}

func solve(input string, a int) (int, int) {
	registers := make(map[string]int)
	registers["a"] = a

	var getValue = func(s string) int {
		if len(s) == 1 && s[0] >= 'a' && s[0] <= 'h' {
			return registers[s]
		}

		val, err := strconv.Atoi(s)
		if err != nil {
			panic(err)
		}
		return val
	}

	mul := 0
	instructions := strings.Split(input, "\n")
	for i := 0; i >= 0 && i < len(instructions); i++ {
		if i == 8 {
			b := registers["b"]
			mul += (b - 2) * (b - 2)
			for d := 2; d*d < b; d++ {
				if b%d == 0 {
					registers["h"]++
					break
				}
			}
			i = 25
			continue
		}

		parts := strings.Split(instructions[i], " ")
		reg, value := parts[1], getValue(parts[2])
		switch parts[0] {
		case "set":
			registers[reg] = value
		case "sub":
			registers[reg] = registers[reg] - value
		case "mul":
			registers[reg] = registers[reg] * value
			mul++
		case "jnz":
			if getValue(reg) != 0 {
				i += value - 1
			}
		default:
			panic("unknown instruction")
		}
	}

	return mul, registers["h"]
}

func simulate(a int) (int, int) {
	mul, b, h := 0, 84, 0

	if a != 0 {
		b, mul = 108400, 1
	}

	for i := 0; i <= a*1000; i++ {
		mul += (b - 2) * (b - 2)
		for d := 2; d*d < b; d++ {
			if b%d == 0 {
				h++
				break
			}
		}
		b += 17
	}
	return mul, h
}

func print() {
	for ind, line := range strings.Split(input, "\n") {
		parts := strings.Split(line, " ")
		var getValue = func(s string) string {
			if len(s) == 1 && s[0] >= 'a' && s[0] <= 'h' {
				return s
			}
			return s
		}
		switch parts[0] {
		case "set":
			println(ind, ":", parts[1], "=", getValue(parts[2]))
		case "sub":
			println(ind, ":", parts[1], "=", parts[1], "-", getValue(parts[2]))
		case "mul":
			println(ind, ":", parts[1], "=", parts[1], "*", getValue(parts[2]))
		case "jnz":
			offset, _ := strconv.Atoi(parts[2])
			println(ind, ":", "if", parts[1], "!=", 0, "goto", (ind + offset))
		}
	}
}
