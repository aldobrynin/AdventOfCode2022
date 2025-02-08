package main

import (
	"testing"
)

func Test_part1(t *testing.T) {
	input := `5 1 9 5
	7 5 3
	2 4 6 8`
	want := 18
	t.Run("base", func(t *testing.T) {
		if got := part1(input); got != want {
			t.Errorf("part1() = %v, want %v", got, want)
		}
	})
}

func Test_part2(t *testing.T) {
	input := `5 9 2 8
	9 4 7 3
	3 8 6 5`
	want := 9
	t.Run("base", func(t *testing.T) {
		if got := part2(input); got != want {
			t.Errorf("part2() = %v, want %v", got, want)
		}
	})
}
