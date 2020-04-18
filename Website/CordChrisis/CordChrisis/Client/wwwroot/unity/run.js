var unityWebGL;
var gameIsRunning = false;
//Boolean gamemode;
var passThroughData;


var settings =
{
    onProgress: UnityProgress,
    Module:
    {
        postRun: [function () {
            gameIsRunning = true;
         
            unityWebGL.SendMessage("SiteHandler", "GetSiteInfo",passThroughData);
            console.log("user stats sent");

        }]
    }
};
//window.CordChrisis = {
function start(mode, speed, offset, chart, audio) {
    // console.log(chart);
    var newdata = { audioURL: audio, chartURL: chart, gameMode: mode, userSpeed: speed, userOffset: offset };
    console.log(audio);
    passThroughData = JSON.stringify(newdata);
    unityWebGL = UnityLoader.instantiate("unityContainer", "unity/Build/webgl thing.json", settings);

    console.log("Game should be started ");
}

function FullScreen() {
    unityWebGL.SetFullscreen(1);

}

function quit()
{
    unityWebGL.Quit(function () { console.log("Ended Game Session."); });
    unityWebGL = null;
}

function ConvertByteArrayToImage(ByteArray)
{
    console.log("Hey mah look at me !");
    return "data:image/png;base64," + ByteArray;
}

function sleep(milliseconds) {
    const date = Date.now();
    let currentDate = null;
    do {
        currentDate = Date.now();
    } while (currentDate - date < milliseconds);
}


//};