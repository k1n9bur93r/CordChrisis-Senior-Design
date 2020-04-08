var unityWebGL;

//window.CordChrisis = {
     function start () {
        unityWebGL = UnityLoader.instantiate("unityContainer", "unity/Build/webgl thing.json", { onProgress: UnityProgress });
}

function quit()
{
    unityWebGL.Quit(function () { console.log("Ended Game Session."); });
    unityWebGL = null;
}
//};