package main

import (
	"testing"
)

func Test_part1(t *testing.T) {
	tests := []struct {
		input string
		want  int
	}{
		{"ne,ne,ne", 3},
		{"ne,ne,sw,sw", 0},
		{"ne,ne,s,s", 2},
		{"se,sw,se,sw,sw", 3},
	}
	for _, tt := range tests {
		t.Run(tt.input, func(t *testing.T) {
			if got, _ := solve(tt.input); got != tt.want {
				t.Errorf("part1() = %v, want %v", got, tt.want)
			}
		})
	}
}
