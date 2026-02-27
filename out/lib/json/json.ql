// Warning! Can serialize and deserialize primitive types only. Like: Boolean, Number, String
namespace json: {
    function<String> serialize(const<Dictionary> dict, const<Boolean> indented = false): {
        if dict.length() < 1:
            std::Throw.message("Cannot serialize empty dictionary");

        let first = true;

        let output = "{";

        const length = dict.length();
        for let i = 0; i < length; i++: {
            if first == false:
                output += ",";

            if indented:
                output += "\n\t";

            const key = dict.getKeys().at(i);
            
            output += `\"{key}\":`;
           
            if indented: 
                output += " ";

            const value = dict.get(key);
            if typeof(value) == "Number" || typeof(value) == "Boolean":
                output += String.new(value).toLower();
            else: output += `\"{value}\"`;

            first = false;
        }

        if indented:
            output += "\n";

        output += "}";
        return output;
    }

    function<Dictionary> deserialize(let content): {
        const result = Dictionary.new();

        content = content.trim().trim("{").trim("}");

        const pairs = content.split(",");
        
        pairs.forEach(function(const pair): {
            const parts = pair.split(":");

            const key = parts.at(0).trim().trim("\"");
            const value = parts.at(1).trim().trim("\"");

            result.set(key.toString(), value);
        });

        return result;
    }
}