// JavaScript source code
var GlobalHelper = window.Sdk || {};

(
    function () {

        this.DoSomething = function (executionContext) {
            alert("This is from the helper function");
        }


}).call(GlobalHelper);