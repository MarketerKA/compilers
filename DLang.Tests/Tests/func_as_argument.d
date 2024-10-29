var apply_function := func(fn, arg) => fn(arg);

var double := func(x) => x * 2;
print apply_function(double, 4);
