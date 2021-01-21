import { getCurrentTime } from './Quagga-lib';

export function GetCurrentTime() {
    return getCurrentTime();
};

//<script src="https://cdn.rawgit.com/serratus/quaggaJS/0420d5e0/dist/quagga.min.js"></script>

//Quagga.init({
//    inputStream: {
//        name: "Live",
//        type: "LiveStream",
//        target: document.querySelector('#scanner-container')    // Or '#yourElement' (optional)
//    },
//    decoder: {
//        readers: ["code_128_reader"]
//    }
//}, function (err) {
//    if (err) {
//        console.log(err);
//        return
//    }
//    console.log("Initialization finished. Ready to start");
//    Quagga.start();
//});