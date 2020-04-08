var unityWebGL;
var gameIsRunning=false;


var settings =
{
    onProgress: UnityProgress,
    Module:
    {
        postRun: [function () { console.log("Hey the game is running now!"); gameIsRunning = true; }]
    }
};
//window.CordChrisis = {
     function start () {
         unityWebGL = UnityLoader.instantiate("unityContainer", "unity/Build/webgl thing.json", settings);
}

function quit()
{
    unityWebGL.Quit(function () { console.log("Ended Game Session."); });
    unityWebGL = null;
}
//};