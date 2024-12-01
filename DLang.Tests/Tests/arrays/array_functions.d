
var makeArray := func() => [1, 2, 3];
var arr := makeArray();
print arr[1];
print arr[2];
print arr[3];


var modifyElement := func(value) => value * 2;
arr[1] := modifyElement(arr[1]);
print arr[1];
