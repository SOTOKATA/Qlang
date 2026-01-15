include "$lib/meta"
include "$lib/standard"

namespace meta: {
    class Meta extends Object: {
        private function _isClass(const object):
            return _native("std.meta.is_dynamic_class", object);

        function getMethodListOf(const object): {
            if (_isClass(object) == false):
                Throw.exception("Object is not a class");

            return Array.new(_native("std.meta.get_method_list_of", object));
        }

        function getVariableListOf(const object): {
            if (_isClass(object) == false):
                Throw.exception("Object is not a class");

            return Array.new(_native("std.meta.get_variable_list_of", object));
        }

        function getInfoOf(const object): {
            if (_isClass(object) == false):
                Throw.exception("Object is not a class");

            return Array.new(_native("std.meta.get_info_of", object));
        }
    }
}