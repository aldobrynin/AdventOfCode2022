package main

import (
	_ "embed"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

func main() {
	// 	input = `p=<3,0,0>, v=<2,0,0>, a=<-1,0,0>
	// p=<4,0,0>, v=<0,0,0>, a=<-2,0,0>`
	println("part1 =", part1(input))
	println("part2 =", part2(input))
}

func part1(input string) int {
	particles := parse(input)
	for i := 0; i < 1000; i++ {
		for ind := range particles {
			particles[ind].Step()
		}
	}
	minDist := particles[0].Distance()
	minIndex := 0
	for i, p := range particles {
		if dist := p.Distance(); dist < minDist {
			minDist = dist
			minIndex = i
		}
	}
	return minIndex
}

func part2(input string) int {
	particles := parse(input)

	for i := 0; i < 1000; i++ {
		for ind := range particles {
			particles[ind].Step()
		}

		collisions := make(map[[3]int][]int)
		for i := 0; i < len(particles); i++ {
			collisions[particles[i].p] = append(collisions[particles[i].p], i)
		}

		nextParticles := []Particle{}
		for _, indices := range collisions {
			if len(indices) == 1 {
				nextParticles = append(nextParticles, particles[indices[0]])
			}
		}
		particles = nextParticles
	}

	return len(particles)
}

func parse(input string) []Particle {
	lines := strings.Split(input, "\n")
	particles := make([]Particle, len(lines))
	for i, line := range lines {
		particles[i] = parseParticle(line)
	}
	return particles
}

func parseParticle(line string) Particle {
	p := Particle{}
	parts := strings.Fields(line)
	p.p = parseCoord(parts[0])
	p.v = parseCoord(parts[1])
	p.a = parseCoord(parts[2])
	return p
}

func parseCoord(coord string) [3]int {
	coord = strings.Trim(coord, "pva=")
	res := [3]int{}
	for i, part := range strings.Split(strings.Trim(coord, ",<>"), ",") {
		if val, err := strconv.Atoi(part); err == nil {
			res[i] = val
		} else {
			panic(err)
		}
	}
	return res
}

type Particle struct {
	p, v, a [3]int
}

func (p *Particle) Step() {
	for i := 0; i < 3; i++ {
		p.v[i] += p.a[i]
		p.p[i] += p.v[i]
	}
}

func (p *Particle) Distance() int {
	return abs(p.p[0]) + abs(p.p[1]) + abs(p.p[2])
}

func (p *Particle) Equals(other Particle) bool {
	return p.p == other.p
}

func (p *Particle) String() string {
	return "p=" + coordToString(p.p) + ", v=" + coordToString(p.v) + ", a=" + coordToString(p.a)
}

func (p *Particle) CanChangeDirection() bool {
	for i := range 3 {
		if p.a[i] != 0 && sign(p.a[i]) != sign(p.v[i]) {
			return true
		}
	}
	return false
}

func coordToString(coord [3]int) string {
	return "<" + strconv.Itoa(coord[0]) + "," + strconv.Itoa(coord[1]) + "," + strconv.Itoa(coord[2]) + ">"
}

func abs(x int) int {
	if x < 0 {
		return -x
	}
	return x
}

func sign(x int) int {
	if x > 0 {
		return 1
	}
	return -1
}
