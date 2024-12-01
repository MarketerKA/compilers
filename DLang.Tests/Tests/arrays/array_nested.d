// Nested arrays
var arr := [[1, 2], [3, 4], [5, 6]];
print arr[1][1];  // Should print 1
print arr[2][2];  // Should print 4
print arr[3][1];  // Should print 5

arr[2][1] := 42;
print arr[2][1];  // Should print 42