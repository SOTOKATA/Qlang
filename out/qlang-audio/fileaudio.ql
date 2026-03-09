using std;

namespace qlaudio: {
    class FileAudio: {
        private let _path;

        function new(const<String> path): {
            if fs::file.exists(path) == false:
                throw.exception("Undefined audio path.");    
        
            _path = path;
            qlaudio::add(path);
        }

        function destroy(): qlaudio::remove(_path);

        function play():
            _native("QlAudio", "audio", "play", _path);

        function pause():
            _native("QlAudio", "audio", "pause", _path);

        function stop():
            _native("QlAudio", "audio", "stop", _path);

        function<Number> getState():
            return _native("QlAudio", "audio", "get_state", _path);

        function setVolume(const<Number> volume):
            _native("QlAudio", "audio", "set_volume", _path, math.max(math.min(volume, 1), 0));

        function<Number> getVolume():
            return _native("QlAudio", "audio", "get_volume", _path);

        function seek(const<Number> time):
            _native("QlAudio", "audio", "seek", _path, time);

        function<Number> getDuration():
            return _native("QlAudio", "audio", "get_duration", _path);

        function<Number> getPosition():
            return _native("QlAudio", "audio", "get_position", _path);

        function<Boolean> isPlaying(): return getState() == "playing";

        function<Boolean> isStopped(): return getState() == "stopped";

        function<Boolean> isPaused(): return getState() == "paused";
    }
}