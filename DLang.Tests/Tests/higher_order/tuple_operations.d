// higher_order/tuple_operations.d
var makeTupleProcessor := func() => {
    process := func(t, f) => f(t),
    map := func(t, f) is
        var result := {};
        for k in t loop
            result[k] := f(t[k]);
        end
        return result;
    end
};

var processor := makeTupleProcessor();
var tuple := {x := 1, y := 2, z := 3};
var doubled := processor.process(tuple, func(t) => t.x * 2);
print doubled;  // 2