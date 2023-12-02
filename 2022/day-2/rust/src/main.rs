use std::{
    fs::File,
    io::{BufRead, BufReader},
};

enum Part {
    One,
    Two,
}

enum ShapeOrOutcome {
    Shape(Shape),
    Outcome(Outcome),
}

enum Shape {
    Rock,
    Paper,
    Scissor,
}

enum Outcome {
    Lose,
    Draw,
    Win,
}

struct Input {
    a: Shape,
    b: ShapeOrOutcome,
}

#[allow(unused)]
struct Game {
    a: Shape,
    b: Shape,
    c: Outcome,
}

fn into_input(s: String, p: &Part) -> Option<Input> {
    let (a, b) = s.split_once(" ")?;

    let a = match a {
        "A" => Shape::Rock,
        "B" => Shape::Paper,
        "C" => Shape::Scissor,
        c => {
            eprintln!("invalid input: {}", c);
            return None;
        }
    };

    let b = match &p {
        Part::One => match b {
            "X" => ShapeOrOutcome::Shape(Shape::Rock),
            "Y" => ShapeOrOutcome::Shape(Shape::Paper),
            "Z" => ShapeOrOutcome::Shape(Shape::Scissor),
            c => {
                eprintln!("invalid input: {}", c);
                return None;
            }
        },
        Part::Two => match b {
            "X" => ShapeOrOutcome::Outcome(Outcome::Lose),
            "Y" => ShapeOrOutcome::Outcome(Outcome::Draw),
            "Z" => ShapeOrOutcome::Outcome(Outcome::Win),
            c => {
                eprintln!("invalid input: {}", c);
                return None;
            }
        },
    };

    Some(Input { a, b })
}

fn into_inputs(r: impl BufRead, p: Part) -> Result<Vec<Input>, std::io::Error> {
    let mut inputs = Vec::new();

    for (i, s) in r.lines().into_iter().enumerate() {
        match into_input(s?, &p) {
            Some(input) => inputs.push(input),
            None => eprintln!("err: failed to read line {}", i),
        };
    }
    Ok(inputs)
}

fn to_game(input: Input) -> Game {
    match input.b {
        ShapeOrOutcome::Shape(s) => {
            let o = match input.a {
                Shape::Rock => match s {
                    Shape::Rock => Outcome::Draw,
                    Shape::Paper => Outcome::Win,
                    Shape::Scissor => Outcome::Lose,
                },
                Shape::Paper => match s {
                    Shape::Rock => Outcome::Lose,
                    Shape::Paper => Outcome::Draw,
                    Shape::Scissor => Outcome::Win,
                },
                Shape::Scissor => match s {
                    Shape::Rock => Outcome::Win,
                    Shape::Paper => Outcome::Lose,
                    Shape::Scissor => Outcome::Draw,
                },
            };
            return Game {
                a: input.a,
                b: s,
                c: o,
            };
        }
        ShapeOrOutcome::Outcome(o) => {
            let s = match input.a {
                Shape::Rock => match o {
                    Outcome::Lose => Shape::Scissor,
                    Outcome::Draw => Shape::Rock,
                    Outcome::Win => Shape::Paper,
                },
                Shape::Paper => match o {
                    Outcome::Lose => Shape::Rock,
                    Outcome::Draw => Shape::Paper,
                    Outcome::Win => Shape::Scissor,
                },
                Shape::Scissor => match o {
                    Outcome::Lose => Shape::Paper,
                    Outcome::Draw => Shape::Scissor,
                    Outcome::Win => Shape::Rock,
                },
            };
            return Game {
                a: input.a,
                b: s,
                c: o,
            };
        }
    };
}

fn to_games(inputs: Vec<Input>) -> Vec<Game> {
    inputs.into_iter().map(to_game).collect::<Vec<Game>>()
}

fn calculate_point(game: Game) -> i32 {
    let x = match game.c {
        Outcome::Lose => 0,
        Outcome::Draw => 3,
        Outcome::Win => 6,
    };

    let y = match game.b {
        Shape::Rock => 1,
        Shape::Paper => 2,
        Shape::Scissor => 3,
    };

    x + y
}

fn calculate_points(games: Vec<Game>) -> i32 {
    games.into_iter().map(calculate_point).sum::<i32>()
}

fn main() {
    // part 1
    match File::open("input").and_then(|f| into_inputs(BufReader::new(f), Part::One).map(to_games))
    {
        Ok(games) => {
            println!("{}", calculate_points(games))
        }
        Err(e) => eprintln!("{}", e),
    };

    // part 2
    match File::open("input").and_then(|f| into_inputs(BufReader::new(f), Part::Two).map(to_games))
    {
        Ok(games) => {
            println!("{}", calculate_points(games))
        }
        Err(e) => eprintln!("{}", e),
    };
}
