package main

import (
	_ "embed"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	// 	input = `snd 1
	// snd 2
	// snd p
	// rcv a
	// rcv b
	// rcv c
	// rcv d`
	println("part1 =", part1(input))
	println("part2 =", part2(input))
}

func part1(input string) int {
	lines := strings.Split(input, "\n")
	program := Program{make(map[string]int), []int{}, false, 0, lines}
	lastPlayed := 0

	for i := 0; i < len(lines); i++ {
		line := lines[i]
		split := strings.Fields(line)
		command := split[0]

		switch command {
		case "snd":
			lastPlayed = program.getVal(split[1])
		case "set":
			program.registers[split[1]] = program.getVal(split[2])
		case "add":
			program.registers[split[1]] += program.getVal(split[2])
		case "mul":
			program.registers[split[1]] *= program.getVal(split[2])
		case "mod":
			program.registers[split[1]] %= program.getVal(split[2])
		case "rcv":
			if program.getVal(split[1]) != 0 {
				return lastPlayed
			}
		case "jgz":
			if program.getVal(split[1]) > 0 {
				i += program.getVal(split[2]) - 1
			}
		}
	}

	return -1
}

func part2(input string) int {
	lines := strings.Split(input, "\n")

	program1 := Program{make(map[string]int), []int{}, false, 0, lines}
	program2 := Program{make(map[string]int), []int{}, false, 0, lines}
	program1.registers["p"] = 0
	program2.registers["p"] = 1

	res := 0
	for !program1.isDone() || !program2.isDone() {
		program1Output, program2Output := program1.run(), program2.run()
		program2.send(program1Output)
		program1.send(program2Output)
		res += len(program2Output)
	}

	return res
}

type Program struct {
	registers    map[string]int
	queue        []int
	isWaiting    bool
	position     int
	instructions []string
}

func (p *Program) run() []int {
	out := []int{}

	for ; p.position < len(p.instructions); p.position++ {
		line := p.instructions[p.position]
		split := strings.Fields(line)
		command := split[0]

		switch command {
		case "snd":
			out = append(out, p.getVal(split[1]))
		case "set":
			p.registers[split[1]] = p.getVal(split[2])
		case "add":
			p.registers[split[1]] += p.getVal(split[2])
		case "mul":
			p.registers[split[1]] *= p.getVal(split[2])
		case "mod":
			p.registers[split[1]] %= p.getVal(split[2])
		case "rcv":
			if len(p.queue) == 0 {
				p.isWaiting = true
				return out
			} else {
				val := p.queue[0]
				p.queue = p.queue[1:]
				p.registers[split[1]] = val
			}
		case "jgz":
			if p.getVal(split[1]) > 0 {
				p.position += p.getVal(split[2]) - 1
			}
		}
	}

	return out
}

func (p *Program) send(val []int) {
	p.queue = append(p.queue, val...)
	p.isWaiting = len(p.queue) == 0
}

func (p *Program) getVal(s string) int {
	if len(s) == 1 && s[0] >= 'a' && s[0] <= 'z' {
		return p.registers[s]
	}

	if val, err := strconv.Atoi(s); err != nil {
		panic(err)
	} else {
		return val
	}
}

func (p *Program) isDone() bool {
	return p.position < 0 || p.position >= len(p.instructions) || (p.isWaiting && len(p.queue) == 0)
}
