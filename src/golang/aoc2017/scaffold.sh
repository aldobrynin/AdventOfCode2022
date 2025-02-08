#!/bin/sh

day="$1"
directory="day$day"
filename="day$day.go"

mkdir -p $directory

content="package main

import (
    _ \"embed\"
)

//go:embed input.txt
var input string

func main() {
    println(\"part1 =\", part1(input))
    println(\"part2 =\", part2(input))
}

func part1(input string) int {
    return 0
}

func part2(input string) int {
    return 0
}";

echo "${content}" >> "$directory/$filename"
