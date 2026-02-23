namespace meta: {
    function getFunctionNameList(const cls):
        return _native("std", "meta", "fn_list", cls);

    function getVariableList(const cls): {
        const names = _native("std", "meta", "var_name_list", cls);
        const types = _native("std", "meta", "var_type_list", cls);
        const values = _native("std", "meta", "var_value_list", cls);

        const arr = Array.new([]);

        const length = names.length();
        for let i = 0; i < length; i++:
            arr.push(Variable.new(types.at(i), names.at(i), values.at(i)));

        return arr;
    }
}