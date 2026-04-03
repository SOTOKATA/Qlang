namespace json: {
    // Сериализация: добавлена поддержка Array и рекурсия для Dictionary
    function<String> serialize(let input, <Boolean> indented = false, <Number> depth = 1): {
        const type = typeof(input);
        
        // 1. Обработка Dictionary
        if type == "Dictionary": {
            if input.length < 1: return "{}";

            let output = "{";
            const keys = input.getKeys();
            const len = input.length;

            for let i = 0; i < len; i++: {
                if i > 0: output += ",";
                
                if indented: output += "\n" + ("\t" * depth);

                const key = keys.at(i);
                const value = input.get(key);

                output += `\"{key}\":`;
                if indented: output += " ";

                // Рекурсивный вызов для значения
                output += serialize(value, indented, depth + 1);
            }

            if indented: output += "\n" + ("\t" * (depth - 1));
            output += "}";
            return output;
        }

        // 2. Обработка Array
        else if type == "Array": {
            let output = "[";
            const len = input.length;

            for let i = 0; i < len; i++: {
                if i > 0: output += ",";
                if indented: output += " ";

                output += serialize(input.at(i), indented, depth + 1);
            }
            output += "]";
            return output;
        }

        // 3. Базовые типы
        else if type == "Number" || type == "Boolean":
            return new String(input).toLower();
        
        // 4. Строки (и остальное как строки)
        else:
            return `\"{input}\"`;
    }

    // Десериализация: упрощенный пример поддержки вложенности
    function<Dictionary|Array|Any> deserialize(let content): {
        content = content.trim();

        // Если это массив
        if content.startsWith("["): {
            const resArray = new Array();
            const inner = content.subString(1, content.length - 2);
            inner.split(",").forEach(fn(item) => resArray.push(deserialize(item)));
            return resArray;
        }

        // Если это объект
        if content.startsWith("{"): {
            const result = new Dictionary();
            let inner = content.subString(1, content.length - 2);
            
            const pairs = inner.split(",");
            pairs.forEach(function(pair): {
                const parts = pair.split(":");
                const key = parts.at(0).trim().trim("\"");
                // Рекурсивно десериализуем значение
                const rawValue = parts.at(1).trim();
                result.set(key, deserialize(rawValue));
            });
            return result;
        }

        // Если это примитив
        if content.startsWith(`"`): return content.trim(`"`);
        if content == "true": return true;
        if content == "false": return false;
        
        return std::parser.asNumber(content); 
    }
}