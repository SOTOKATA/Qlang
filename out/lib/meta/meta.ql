namespace meta: {
    function<Array> getFunctionNameList(cls)
        => _native("std", "meta", "fn_list", cls);

    function<Array> getVariableList(cls): {
        const names = _native("std", "meta", "var_name_list", cls);
        const types = _native("std", "meta", "var_type_list", cls);
        const values = _native("std", "meta", "var_value_list", cls);

        const arr = new Array([]);

        const len = names.length;
        for let i = 0; i < len; i++:
            arr.push(new Variable(types.at(i), names.at(i), values.at(i)));

        return arr;
    }
}