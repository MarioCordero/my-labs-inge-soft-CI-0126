function b() {
    var myVar;
    console.log(myVar);
}

function a() {
    var myVar = 2;
    b();
    console.log(myVar);
    var something = function() {
        console.log(myVar);
    }
    something();
}

var myVar = 5;
console.log(myVar);
a();