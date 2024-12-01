// higher_order/currying.d
var curry := func(f) => func(x) => func(y) => f(x, y);

var add := func(x, y) => x + y;
var curriedAdd := curry(add);

var addFive := curriedAdd(5);
print addFive(3);  // 8