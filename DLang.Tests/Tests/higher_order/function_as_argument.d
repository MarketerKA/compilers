// higher_order/function_as_argument.d
var apply := func(f, x) => f(x);
var square := func(x) => x * x;
var double := func(x) => x * 2;

print apply(square, 4);  // 16
print apply(double, 5);  // 10