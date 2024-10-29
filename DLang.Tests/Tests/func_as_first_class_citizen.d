var add := func(a, b) => a + b;
var operate := func(fn, x, y) => fn(x, y);
print operate(add, 5, 7);
