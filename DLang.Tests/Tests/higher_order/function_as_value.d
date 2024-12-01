// higher_order/function_as_value.d
var addOne := func(x) => x + 1;
var multiplyByTwo := func(x) => x * 2;
var combined := func(x) => multiplyByTwo(addOne(x));

print combined(5); // Должно вывести 12