namespace meta: {
    namespace cls: {
        function<Boolean> isPrivate(const cls) => _native("std", "class", "is_private", cls);
        
        function<Boolean> hasVariable(const cls, const<String> name) => _native("std", "class", "has_variable", cls, name);

        function<String> getName(const cls) => _native("std", "class", "get_name", cls);

        function getVariableValue(const cls, const varName):
            return _native("std", "class", "getVariableValue", cls, varName);

        function<String> getClassName(const cls) => _native("std", "class", "get_class_name", cls);
    }
}