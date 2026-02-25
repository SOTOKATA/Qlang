namespace meta: {
    namespace cls: {
        function<Boolean> isPrivate(const cls): return _native("std", "class", "is_private", cls);
        
        function<Boolean> hasVariable(const cls, const<String> name): return _native("std", "class", "has_variable", cls, name);

        function<String> getName(const cls): return _native("std", "class", "get_name", cls);

        function<String> getClassName(const cls): return _native("std", "class", "get_class_name", cls);
    }
}