namespace meta: {
    namespace cls: {
        function<Boolean> isPrivate(cls) => _native("std", "class", "is_private", cls);
        
        function<Boolean> hasVariable(cls, <String> name) => _native("std", "class", "has_variable", cls, name);

        function<String> getName(cls) => _native("std", "class", "get_name", cls);

        function getVariableValue(cls, varName)
            => _native("std", "class", "getVariableValue", cls, varName);

        function<String> getClassName(cls) => _native("std", "class", "get_class_name", cls);
    }
}