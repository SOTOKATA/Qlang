function format(const val, const toVal): {
    if toVal == "string":
        return Parser.asString(val);
    else if toVal == "number":
        return Parser.asNumber(val);
    else: return val;
}