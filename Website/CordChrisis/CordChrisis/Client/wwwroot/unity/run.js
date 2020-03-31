var unityWebGL;

//window.CordChrisis = {
     function start () {
        unityWebGL = UnityLoader.instantiate("unityContainer", "unity/Build/WebGLbuild.json", { onProgress: UnityProgress });
}

function quit()
{
    unityWebGL.Quit(function () { console.log("Ended Game Session."); });
    unityWebGL = null;
}
//};