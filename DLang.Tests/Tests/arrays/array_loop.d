// Array operations in loops
var arr := [1, 2, 3, 4, 5];
var i := 1;
while i <= 5 loop
    arr[i] := arr[i] * 2;
    i := i + 1;
end

i := 1;
while i <= 5 loop
    print arr[i];  // Should print 2,4,6,8,10
    i := i + 1;
end