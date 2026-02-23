namespace meta: {
    function getFunctionNameList(const cls):
        return _native("std", "meta", "fn_list", cls);

    function getVariableNameList(const cls):
        return _native("std", "meta", "var_list", cls);

    function getVariableValueList(const cls):
        return _native("std", "meta", "var_value_list", cls);
}