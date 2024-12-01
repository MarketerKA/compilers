// higher_order/function_composition.d
var compose := func(f, g) => func(x) => f(g(x));

var addTwo := func(x) => x + 2;
var multiplyByThree := func(x) => x * 3;

var composed := compose(addTwo, multiplyByThree);
print composed(4);  // (4 * 3) + 2 = 14