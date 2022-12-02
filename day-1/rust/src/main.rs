use std::{
    fs::File,
    i32,
    io::{BufRead, BufReader},
};

type Inventory = Vec<i32>;

struct Elves {
    reader: BufReader<File>,
}

impl Iterator for Elves {
    type Item = Inventory;

    fn next(&mut self) -> Option<Self::Item> {
        parse_elf(&mut self.reader)
    }
}

fn parse_elf(r: &mut impl BufRead) -> Option<Inventory> {
    let mut line = String::new();
    let mut inventory = Vec::new();

    loop {
        match r.read_line(&mut line) {
            Ok(0) => {
                if inventory.len() == 0 {
                    return None;
                } else {
                    return Some(inventory);
                }
            }
            Ok(_) => match line.trim() {
                "" => {
                    if inventory.len() > 0 {
                        return Some(inventory);
                    }
                }
                calories => match calories.parse::<i32>() {
                    Ok(c) => inventory.push(c),
                    Err(e) => eprintln!("err: {}", e),
                },
            },
            Err(e) => println!("err: {}", e),
        };
        line.clear()
    }
}

fn main() {
    match File::open("input") {
        Ok(f) => {
            let elves = Elves {
                reader: BufReader::new(f),
            };

            match elves
                .into_iter()
                .map(|inventory| inventory.iter().sum::<i32>())
                .max()
            {
                Some(max) => println!("{}", max),
                None => eprintln!(
                    "something failed along the aggregation, and I'm kind of curious where."
                ),
            }
        }
        Err(e) => eprintln!("{}", e),
    };
}
