// higher_order/partial_application.d
var partial := func(f, x) => func(y) => f(x, y);

var multiply := func(x, y) => x * y;
var multiplyByTen := partial(multiply, 10);

print multiplyByTen(5);  // 50