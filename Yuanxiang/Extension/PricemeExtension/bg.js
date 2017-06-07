
var port = chrome.extension.connect({ name: "knockknock" });

port.postMessage({ joke: "Knock knock" });

port.onMessage.addListener(function (msg) {
    if (msg.question == "Who's there?")
        port.postMessage({ answer: "Madame" });
    else if (msg.question == "Madame who?")
        port.postMessage({ answer: "Madame... Bovary" });
});


//chrome.extension.onConnect.addListener(function (port) {
//    console.assert(port.name == "priceme");
//    port.onMessage.addListener(function (msg) {
//        if (msg.joke == "begin") {
//            $('#surl').html(url);
//            port.postMessage({ question: "Who's there?" });
//        }
//    });
//});

//chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
//    chrome.tabs.sendMessage(tabs[0].id, { greeting: "hello" },function (response) {
        
//    });
//});

//chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
//    var url = "tewgerge ger";
//    chrome.tabs.sendMessage(tabs[0].id, { greeting: url }, function (response) {

//    });
//});

//chrome.runtime.onMessage.addListener(function (request, sender, sendResponse) {

//    var url = "tewgerge ger";
//    console.log(sender.tab ? "from a content script:" + sender.tab.url : "from the extension");
//    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
//        chrome.tabs.sendMessage(tabs[0].id, { greeting: url }, function (response) {

//        });
//    });
//});

//chrome.extension.onRequest.addListener(function (request, sender, sendResponse) {
//    var url = "tewgerge ger";

//    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
//        chrome.tabs.sendMessage(tabs[0].id, { greeting: url }, function (response) {

//        });
//    });
    

//    //getCurrentTabUrl(function (url) {
//    //    sendResponse({ farewell: url });
//    //});
//});

//function getCurrentTabUrl(callback) {
//    var queryInfo = {
//        active: true,
//        currentWindow: true
//    };

//    chrome.tabs.query(queryInfo, function (tabs) {
//        var tab = tabs[0];
//        var url = tab.url;
//        console.assert(typeof url == 'string', 'tab.url should be a string');
//        callback(url);
//    });
//}