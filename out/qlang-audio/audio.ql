using std;

namespace qlaudio: {
    function init():
        _native("QlAudio", "audio", "init");

    function exit():
        _native("QlAudio", "audio", "exit");

    function getMasterVolume():
        _native("QlAudio", "audio", "get_master_volume");

    function setMasterVolume(<Number> volume): 
        _native("QlAudio", "audio", "set_master_volume", math.max(math.min(volume, 1), 0));

    function add(<String> path):
        _native("QlAudio", "audio", "add_to_playlist", path);

    function remove(<String> path):
        _native("QlAudio", "audio", "remove_from_playlist", path);
}