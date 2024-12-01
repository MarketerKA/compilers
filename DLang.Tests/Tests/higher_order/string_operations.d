// higher_order/string_operations.d
var stringProcessor := func(operation) => func(str) is
    if operation is func then
        return operation(str);
    end
    return str;
end;

var makeUpperCase := stringProcessor(func(s) => s + "!!!!");
print makeUpperCase("hello");  // "hello!!!!"