// Dynamic array modification
var arr := [1, 2, 3];
arr[1] := 10;
arr[2] := arr[1] + 5;
arr[3] := arr[1] + arr[2];
print arr[1];  // Should print 10
print arr[2];  // Should print 15
print arr[3];  // Should print 25