// higher_order/multiple_args.d
var makeAdder := func(x) => func(y) => func(z) => x + y + z;

var add5 := makeAdder(5);
var add5and3 := add5(3);
print add5and3(2);  // 10