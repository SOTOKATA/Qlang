namespace meta: {
    function<Array> getFunctionNameList(cls)
        => #std.Meta.FnList(cls);

    function<Array> getVariableList(cls): {
        const names = #std.Meta.VarNameList(cls);
        const types = #std.Meta.VarTypeList(cls);
        const values = #std.Meta.VarValueList(cls);

        const arr = new Array([]);

        const len = names.length;
        for let i = 0; i < len; i++:
            arr.push(new Variable(types.at(i), names.at(i), values.at(i)));

        return arr;
    }
}