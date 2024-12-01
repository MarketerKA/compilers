var my_tuple := { a := 1, b := "Hello", c := {w := "World", g := 14}, d := func(x) => x * 2};

print my_tuple.a;
print my_tuple.2;
print my_tuple.3.w;

for i in 2..5 loop
    print my_tuple.d(i);
end
