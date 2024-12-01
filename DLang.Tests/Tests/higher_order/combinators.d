// higher_order/combinators.d
var pipe := func(f, g) => func(x) => g(f(x));
var flip := func(f) => func(x, y) => f(y, x);

var divideBy := func(x, y) => x / y;
var flippedDivide := flip(divideBy);

print flippedDivide(2, 10);  // 5