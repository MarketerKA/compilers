// higher_order/array_operations.d
var map := func(arr, f) is
    var result := [];
    var i := 1;
    while i <= arr[0] loop
        result[i] := f(arr[i]);
        i := i + 1;
    end
    result[0] := arr[0];
    return result;
end;

var filter := func(arr, predicate) is
    var result := [];
    var count := 0;
    var i := 1;
    while i <= arr[0] loop
        if predicate(arr[i]) then
            count := count + 1;
            result[count] := arr[i];
        end
        i := i + 1;
    end
    result[0] := count;
    return result;
end;

var numbers := [5, 1, 2, 3, 4, 5];
numbers[0] := 5;

var doubled := map(numbers, func(x) => x * 2);
var evens := filter(numbers, func(x) => x - (x/2)*2 = 0);

print doubled[1];  // 2
print evens[1];    // 2