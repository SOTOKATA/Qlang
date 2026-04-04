namespace meta: {
    namespace cls: {
        function<Boolean> isPrivate(cls) => #std.MetaClass.IsPrivate(cls);
        
        function<Boolean> hasVariable(cls, <String> name) => #std.MetaClass.HasVariable(cls, name);

        function<String> getName(cls) => #std.MetaClass.GetName(cls);

        function getVariableValue(cls, varName)
            => #std.MetaClass.GetVariableValue(cls, varName);

        function<String> getClassName(cls) => #std.MetaClass.GetClassName(cls);
    }
}