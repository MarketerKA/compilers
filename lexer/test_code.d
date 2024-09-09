// Function to calculate the factorial of a number
var factorial := func(n) => {
    if n <= 1 then
        return 1;
    else
        return n * factorial(n - 1);
    end;
};

// Function to check if a number is prime
var is_prime := func(n) => {
    if n <= 1 then
        return false;
    end;
    for i in 2..(n - 1) loop
        if n / i * i == n then
            return false;
        end;
    return true;
};

// Function to create a tuple containing a number, its factorial, and a prime check function
var create_tuple := func(n) => {
    return {
        num := n,
        fact := factorial(n),
        prime_check := func(x) => is_prime(x)
    };
};

// Main program
var main := func() => {
    var num := 5;
    var my_tuple := create_tuple(num);

    print "Number: " + my_tuple.num;
    print "Factorial: " + my_tuple.fact;
    print "Is prime: " + my_tuple.prime_check(my_tuple.num);

    var numbers := [1, 2, 3, 4, 5];
    var results := [];

    for i in numbers loop
        var tuple := create_tuple(i);
        results := results + [tuple];
    end;

    for res in results loop
        print "Number: " + res.num;
        print "Factorial: " + res.fact;
        print "Is prime: " + res.prime_check(res.num);
    end;
};

main();
