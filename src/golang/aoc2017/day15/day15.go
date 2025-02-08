package main

const A_FACTOR = 16807
const B_FACTOR = 48271
const MOD = 2147483647

func main() {
	// a, b := 65, 8921
	a, b := 679, 771
	println("part1 =", solve(a, b, 1, 1, 40_000_000))
	println("part2 =", solve(a, b, 8, 4, 5_000_000))
}

func solve(a, b int, multiplierA, multiplierB int, iterations int) int {
	match := 0
	currentA, currentB := a, b
	for i := 0; i < iterations; i++ {
		currentA = Next(currentA, A_FACTOR, multiplierA)
		currentB = Next(currentB, B_FACTOR, multiplierB)
		if currentA&0xFFFF == currentB&0xFFFF {
			match++
		}
	}
	return match
}

func Next(current int, factor int, multiplier int) int {
	for {
		current = (current * factor) % MOD
		if current%multiplier == 0 {
			return current
		}
	}
}
