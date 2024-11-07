var create_adder := func(x) => func(y) => x + y;

var add_five := create_adder(5);
print add_five(3);
