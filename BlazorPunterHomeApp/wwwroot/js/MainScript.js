console.log('Hello from main')
console.log(document.getElementById("myBottomDiv"))

function onRenderComplete() {
    console.log('render complete')
}

function getElementHeight(elementName) {
    return document.getElementById(elementName).clientHeight;
}