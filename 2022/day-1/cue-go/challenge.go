package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"

	"cuelang.org/go/cue"
	"cuelang.org/go/cue/cuecontext"
)

func main() {
	var (
		c           *cue.Context
		inventory []int
		inventories [][]int
		i int
	)

	file, err := os.Open("input")
	if err != nil {
		log.Fatalf("err: %s", err)
	}

	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		s := scanner.Text()
		if s != "" {
			calories, err := strconv.Atoi(scanner.Text())
			if err != nil {
				log.Fatalf("err: %s", err)
			}
			inventory = append(inventory, calories)
		} else {
			inventories = append(inventories, inventory)
			inventory = nil
			i++
		}
	}

	c = cuecontext.New()

	data, err := os.ReadFile("challenge.cue")
	if err != nil {
		log.Fatalf("err: %s", err)
	}

	v := c.CompileBytes(data, cue.Filename("challenge.cue"))

	fmt.Println(v.FillPath(cue.ParsePath("elves"), inventories).LookupPath(cue.ParsePath("output")))
}
