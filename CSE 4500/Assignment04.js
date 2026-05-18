// Assignment 04 - JavaScript (Loop and Array)

// Create an array of 6 numbers
let numbers = [5, 20, 10, 75, 50, 100];

// Task 1: Display the array

console.log("Task 1: Display the array using for loop:");
for (let i = 0; i < numbers.length; i++) {
    process.stdout.write(numbers[i] + " ");
}
console.log("\n");

// Task 2: Find sum and average

let sum = 0;
for (let i = 0; i < numbers.length; i++) {
    sum += numbers[i];
}
let average = sum / numbers.length;

console.log("Task 2: Sum and Average:");
console.log("Sum = " + sum);
console.log("Average = " + average.toFixed(2));
console.log();

// Task 3: Find maximum and minimum

let max = numbers[0];
let min = numbers[0];

for (let i=1; i < numbers.length; i++) {
    if (numbers[i] > max) {
        max = numbers[i];
    }
    if (numbers[i] < min) {
        min = numbers[i];
    }
}

console.log("Task 3: Maximum and Minimum:");
console.log("Maximum = " + max);
console.log("Minimum = " + min);
console.log();

// Task 4: Use different loops

console.log("Task 4: Different loops:");

// for loop
console.log("Using for loop:");
for (let i = 0; i < numbers.length; i++) {
    process.stdout.write(numbers[i] + " ");
}
console.log("\n");
// while loop
console.log("Using while loop:");
let i = 0;
while (i < numbers.length) {
    process.stdout.write(numbers[i] + " ");
    i++;
}
console.log("\n");

// for...in loop
console.log("Using for...in loop:");
for (let index in numbers) {
    process.stdout.write(numbers[index] + " ");
}
console.log("\n");

// for...of loop
console.log("Using for...of loop:");
for (let value of numbers) { 
    process.stdout.write(value + " ");
}
console.log("\n");

// Task 5: Search for a value

// For Node.js input
const readline = require("readline");

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

rl.question("Enter a number to search: " , function(input) {
    let userNumber = Number(input);
    let found = false;

    for (let i = 0; i < numbers.length; i++) {
        if (numbers[i] === userNumber){
            found = true;
            break;
        }
    }
    if (found){
        console.log("Found");
    } else {
        console.log("Not Found");
    }

    rl.close();
});