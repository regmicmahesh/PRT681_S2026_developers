
**I asked Gemini to give me list of projects to build to first grasp the concepts of dotnet. It gave me 3 different project ideas.**

## Prerequisites & Setup

Before starting, ensure your dev environment is ready. Open your terminal and verify your installation:

```bash
dotnet --version
```

Create a folder for Week 1 and initialize a console project using the CLI:

```bash
mkdir Week1_Assignments
cd Week1_Assignments
dotnet new console
```

---

## 🛠️ Assignment 1: Basic CLI & Type System

**Goal:** Practice type declarations, parsing input, string interpolation, and standard console output.

### Requirements
1. Prompt the user to enter their **Name**, **Hourly Pay Rate ($)**, and **Hours Worked this Week**.
2. Convert the input strings to appropriate numerical types (`decimal`, `double`, or `int`).
3. Calculate:
   * **Gross Pay** ($\text{Hours} \times \text{Rate}$)
   * **Tax Withheld** assuming a flat 20% rate ($\text{Gross Pay} \times 0.20$)
   * **Net Pay** ($\text{Gross Pay} - \text{Tax}$)
4. Output a formatted summary receipt using string interpolation (formatted as currency `$0.00`).

---

## 🛠️ Assignment 2: Control Flow & Menu Loop

**Goal:** Work with `if`/`switch` statements, `while` loops, and input validation.

### Requirements
Build a simple interactive program that stays open in a loop until the user explicitly exits.

1. **Display a menu with 4 options:**
   * `[1]` Convert Temperature (Celsius to Fahrenheit)
   * `[2]` Calculate Grade Average
   * `[3]` Check if a Number is Prime
   * `[4]` Exit

2. **Feature Logic:**
   * **Option 1:** Formula $F = (C \times 9/5) + 32$
   * **Option 2:** Ask the user how many grades they want to enter, accept that many numbers, and compute the arithmetic mean.
   * **Option 3:** Take an integer input and print whether it is prime.

3. **Edge Cases:** Handle invalid user inputs gracefully using `int.TryParse()` or `double.TryParse()` without crashing.

---

## 🛠️ Assignment 3: Core Challenge — Expense Tracker CLI

**Goal:** Combine collections (`List<T>`), methods, string parsing, and state management into a functional console app.

### Requirements
Build a mini expense logging application:

1. **Data Storage:** Define a simple way to store an expense item (either using parallel lists like `List<string>` descriptions and `List<decimal>` amounts, or a lightweight `struct`/`tuple`/class).
2. **Features to Implement:**
   * **Add Expense:** Prompt for description and amount.
   * **View All Expenses:** Print an indexed list of logged expenses.
   * **View Total & Average:** Display the total amount spent and average expense cost.
   * **Filter Expenses:** Prompt for a threshold amount and show only items above that value.
   * **Delete Expense:** Allow the user to remove an item by its index.


I implemented all of these apps and now I'm looking into OOP concepts.
