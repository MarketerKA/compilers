var process_tuple := func(tup) is
    print tup.a;
    print tup.b;
end;

var my_tuple := {a := 10, b := "Hello"};
print process_tuple(my_tuple);
