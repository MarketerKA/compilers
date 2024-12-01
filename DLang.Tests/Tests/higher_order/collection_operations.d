// higher_order/collection_operations.d
var reduce := func(arr, f, initial) is
    var result := initial;
    var i := 1;
    while i <= arr[0] loop
        result := f(result, arr[i]);
        i := i + 1;
    end
    return result;
end;

var numbers := [5, 1, 2, 3, 4, 5];
numbers[0] := 5;

var sum := reduce(numbers, func(acc, x) => acc + x, 0);
print sum;  // 15