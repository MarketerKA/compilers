// Array with mixed types
var arr := [1, 2.5, "three", true, func(x)=>x+1];
print arr[1];        // integer
print arr[2];        // real
print arr[3];        // string
print arr[4];        // boolean
print arr[5](10);    // function call should print 11