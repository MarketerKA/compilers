// higher_order/decorators.d
var withLogging := func(f) => func(x) is
    print "Calling function with argument:";
    print x;
    var result := f(x);
    print "Result:";
    print result;
    return result;
end;

var square := func(x) => x * x;
var loggedSquare := withLogging(square);
print loggedSquare(5);