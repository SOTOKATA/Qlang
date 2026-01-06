Keyword(class) Identifier(String) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_value) Equals Identifier(___STRING_1___) Semicolon 
	Keyword(const) Identifier(zeroChar) Equals Identifier(___STRING_2___) Semicolon 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(Object) Dot Identifier(toString) LParen Identifier(_value) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(___create_from___) LParen Keyword(const) Identifier(obj) RParen Colon 
	Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(obj) RParen Semicolon 
	Keyword(function) Identifier(___operator_plus___) LParen Keyword(const) Identifier(obj1) Comma Keyword(const) Identifier(obj2) RParen Colon 
	Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(obj1) Dot Identifier(_value) Plus Identifier(obj2) Dot Identifier(_value) RParen Semicolon 
	Keyword(function) Identifier(___operator_star___) LParen Keyword(const) Identifier(obj1) Comma Keyword(const) Identifier(obj2) RParen Colon 
		LBrace 
		Keyword(let) Identifier(val) Equals Identifier(___STRING_3___) Semicolon 
		Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___NUMBER_0___) Semicolon 
		Identifier(i) Less Identifier(obj2) Dot Identifier(_value) Semicolon 
		Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_1___) Colon 
		Identifier(val) Equals Identifier(val) Plus Identifier(obj1) Semicolon 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(val) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(___operator_slash___) LParen Keyword(const) Identifier(obj1) Comma Keyword(const) Identifier(obj2) RParen Colon 
		LBrace 
		Keyword(let) Identifier(val) Equals Identifier(___STRING_4___) Semicolon 
		Keyword(const) Identifier(index) Equals Identifier(obj1) Dot Identifier(length) LParen RParen Slash Identifier(obj2) Dot Identifier(_value) Semicolon 
		Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___NUMBER_2___) Semicolon 
		Identifier(i) Less Identifier(index) Semicolon 
		Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_3___) Colon 
		Identifier(val) Equals Identifier(val) Plus Identifier(obj1) Dot Identifier(charAt) LParen Identifier(i) RParen Semicolon 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(val) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(___operator_equal_equal___) LParen Keyword(const) Identifier(obj1) Comma Keyword(const) Identifier(obj2) RParen Colon 
	Keyword(return) Identifier(obj1) Dot Identifier(_value) Equals Equals Identifier(obj2) Dot Identifier(_value) Semicolon 
	Keyword(function) Identifier(___operator_not_equal___) LParen Keyword(const) Identifier(obj1) Comma Keyword(const) Identifier(obj2) RParen Colon 
	Keyword(return) Identifier(obj1) Dot Identifier(_value) Equals Equals Identifier(obj2) Dot Identifier(_value) Semicolon 
	Keyword(function) Identifier(___operator_greater_equal___) LParen Keyword(const) Identifier(obj1) Comma Keyword(const) Identifier(obj2) RParen Colon 
	Keyword(return) Identifier(obj1) Dot Identifier(length) LParen RParen Greater Equals Identifier(obj2) Dot Identifier(length) LParen RParen Semicolon 
	Keyword(function) Identifier(___operator_less_equal___) LParen Keyword(const) Identifier(obj1) Comma Keyword(const) Identifier(obj2) RParen Colon 
	Keyword(return) Identifier(obj1) Dot Identifier(length) LParen RParen Less Equals Identifier(obj2) Dot Identifier(length) LParen RParen Semicolon 
	Keyword(function) Identifier(___operator_greater___) LParen Keyword(const) Identifier(obj1) Comma Keyword(const) Identifier(obj2) RParen Colon 
	Keyword(return) Identifier(obj1) Dot Identifier(length) LParen RParen Greater Identifier(obj2) Dot Identifier(length) LParen RParen Semicolon 
	Keyword(function) Identifier(___operator_less___) LParen Keyword(const) Identifier(obj1) Comma Keyword(const) Identifier(obj2) RParen Colon 
	Keyword(return) Identifier(obj1) Dot Identifier(length) LParen RParen Less Identifier(obj2) Dot Identifier(length) LParen RParen Semicolon 
	Keyword(function) Identifier(new) LParen Keyword(const) Less Identifier(String) Greater Identifier(input) RParen Colon 
		LBrace 
		Keyword(if) Identifier(String) Dot Identifier(isString) LParen Identifier(input) RParen Colon 
		Identifier(_value) Equals Identifier(input) Dot Identifier(toString) LParen RParen Semicolon 
		Keyword(else) Colon 
		Identifier(_value) Equals Identifier(input) Semicolon 
	RBrace 
	Keyword(function) Identifier(new) LParen Keyword(const) Less Identifier(String) Greater Identifier(char) Comma Keyword(const) Less Identifier(Number) Greater Identifier(count) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(Number) Dot Identifier(isNumber) LParen Identifier(count) RParen Equals Equals Keyword(false) RParen Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_5___) RParen Semicolon 
		Keyword(if) Identifier(count) Less Identifier(___NUMBER_4___) Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_6___) RParen Semicolon 
		Keyword(if) Identifier(String) Dot Identifier(new) LParen Identifier(char) RParen Dot Identifier(length) LParen RParen Less Identifier(___NUMBER_5___) Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_7___) RParen Semicolon 
		Identifier(_value) Equals Identifier(_native) LParen Identifier(___STRING_8___) Comma Identifier(char) Comma Identifier(count) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(getPrimitive) LParen Keyword(const) Identifier(strOrPrimite) Comma Keyword(let) Identifier(allowOther) Equals Keyword(false) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(Object) Dot Identifier(isNull) LParen Identifier(strOrPrimite) RParen RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_9___) RParen Semicolon 
		RBrace 
		Keyword(if) LParen Identifier(String) Dot Identifier(isString) LParen Identifier(strOrPrimite) RParen RParen Colon 
			LBrace 
			Keyword(return) Identifier(strOrPrimite) Dot Identifier(toString) LParen RParen Semicolon 
		RBrace 
		Keyword(if) LParen Identifier(String) Dot Identifier(isPrimitive) LParen Identifier(strOrPrimite) RParen RParen Colon 
			LBrace 
			Keyword(return) Identifier(strOrPrimite) Semicolon 
		RBrace 
		Keyword(if) Identifier(allowOther) Colon 
			LBrace 
			Keyword(return) Identifier(strOrPrimite) Semicolon 
		RBrace 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_10___) RParen Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_checkIsCollection) LParen Keyword(let) Identifier(value) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isCollection) LParen Identifier(value) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parceException) LParen Identifier(___STRING_11___) RParen Semicolon 
		RBrace 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_checkIsArray) LParen Keyword(let) Identifier(value) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isArray) LParen Identifier(value) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parceException) LParen Identifier(___STRING_12___) RParen Semicolon 
		RBrace 
	RBrace 
	Keyword(function) Identifier(append) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Identifier(_checkIsCollection) LParen Identifier(collection) RParen Semicolon 
		Keyword(let) Identifier(result) Equals Identifier(___STRING_13___) Semicolon 
		Keyword(let) Identifier(arr) Equals Identifier(Array) Dot Identifier(new) LParen Identifier(collection) RParen Semicolon 
		Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___NUMBER_6___) Semicolon 
		Identifier(i) Less Identifier(arr) Dot Identifier(length) LParen RParen Semicolon 
		Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_7___) Colon 
			LBrace 
			Identifier(result) Equals Identifier(result) Plus Identifier(arr) Dot Identifier(at) LParen Identifier(i) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(result) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toLower) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_14___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toUpper) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_15___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isString) LParen Keyword(let) Identifier(value) RParen Colon 
		LBrace 
		Identifier(value) Equals Identifier(_native) LParen Identifier(___STRING_16___) Comma Identifier(value) RParen Semicolon 
		Keyword(return) Identifier(value) Semicolon 
	RBrace 
	Keyword(function) Identifier(isPrimitive) LParen Keyword(let) Identifier(value) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_17___) Comma Identifier(value) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(split) LParen Keyword(let) Less Identifier(String) Greater Identifier(pattern) RParen Colon 
		LBrace 
		Keyword(return) Identifier(Array) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_18___) Comma Identifier(_str) LParen Identifier(_value) RParen Comma Identifier(_str) LParen Identifier(pattern) RParen RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(charAt) LParen Keyword(const) Less Identifier(Number) Greater Identifier(index) RParen Colon 
		LBrace 
		Keyword(if) Identifier(length) LParen RParen Less Equals Identifier(index) Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_19___) RParen Semicolon 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_20___) Comma Identifier(_str) LParen Identifier(_value) RParen Comma Identifier(index) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setAt) LParen Keyword(const) Less Identifier(Number) Greater Identifier(index) Comma Keyword(const) Less Identifier(String) Greater Identifier(replaceValue) RParen Colon 
		LBrace 
		Keyword(if) Identifier(length) LParen RParen Less Equals Identifier(index) Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_21___) RParen Semicolon 
		Identifier(_value) Equals Identifier(_native) LParen Identifier(___STRING_22___) Comma Identifier(_str) LParen Identifier(_value) RParen Comma Identifier(_str) LParen Identifier(replaceValue) RParen Comma Identifier(index) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(join) LParen Keyword(let) Identifier(strArr) Comma Keyword(let) Less Identifier(String) Greater Identifier(pattern) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(Array) Dot Identifier(isCollection) LParen Identifier(strArr) RParen Equals Equals Keyword(false) RParen And And LParen Identifier(Array) Dot Identifier(isArray) LParen Identifier(strArr) RParen Equals Equals Keyword(false) RParen Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_23___) RParen Semicolon 
		Keyword(if) Identifier(Array) Dot Identifier(isArray) LParen Identifier(strArr) RParen Colon 
		Identifier(strArr) Equals Identifier(strArr) Dot Identifier(getCollection) LParen RParen Semicolon 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_24___) Comma Identifier(strArr) Comma Identifier(pattern) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(length) LParen RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_25___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen Semicolon 
	Keyword(function) Identifier(isNullOrEmpty) LParen Keyword(let) Less Identifier(String) Greater Identifier(str) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(String) Dot Identifier(isPrimitive) LParen Identifier(str) RParen Equals Equals Keyword(false) RParen And And LParen Identifier(String) Dot Identifier(isString) LParen Identifier(str) RParen Equals Equals Keyword(false) RParen Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_26___) RParen Semicolon 
		Keyword(if) Identifier(String) Dot Identifier(isString) LParen Identifier(str) RParen Colon 
		Identifier(str) Equals Identifier(str) Dot Identifier(toString) LParen RParen Semicolon 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_27___) Comma Identifier(_str) LParen Identifier(str) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isNullOrWhitespace) LParen Keyword(let) Less Identifier(String) Greater Identifier(str) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(String) Dot Identifier(isPrimitive) LParen Identifier(str) RParen Equals Equals Keyword(false) RParen And And LParen Identifier(String) Dot Identifier(isString) LParen Identifier(str) RParen Equals Equals Keyword(false) RParen Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_28___) RParen Semicolon 
		Keyword(if) Identifier(String) Dot Identifier(isString) LParen Identifier(str) RParen Colon 
		Identifier(str) Equals Identifier(str) Dot Identifier(toString) LParen RParen Semicolon 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_29___) Comma Identifier(_str) LParen Identifier(str) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trim) LParen RParen Colon 
	Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_30___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen RParen Semicolon 
	Keyword(function) Identifier(trimStart) LParen RParen Colon 
	Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_31___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen RParen Semicolon 
	Keyword(function) Identifier(trimEnd) LParen RParen Colon 
	Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_32___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen RParen Semicolon 
	Keyword(function) Identifier(subString) LParen Keyword(let) Less Identifier(Number) Greater Identifier(startPos) Comma Keyword(let) Less Identifier(Number) Greater Identifier(length) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(startPos) RParen Equals Equals Keyword(false) Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_33___) RParen Semicolon 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(length) RParen Equals Equals Keyword(false) Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_34___) RParen Semicolon 
		Keyword(if) Identifier(length) LParen RParen Less Equals Identifier(length) Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_35___) RParen Semicolon 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_36___) Comma Identifier(_str) LParen Identifier(_value) RParen Comma Identifier(startPos) Comma Identifier(length) RParen RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Object) Colon 
	LBrace 
	Keyword(function) Identifier(isNull) LParen Keyword(let) Identifier(obj) RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_38___) Comma Identifier(obj) RParen Semicolon 
	Keyword(function) Identifier(isSimplify) LParen Keyword(const) Identifier(val) RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_39___) Comma Identifier(val) RParen Semicolon 
	Keyword(function) Identifier(toString) LParen Keyword(const) Identifier(obj) Equals Identifier(nameof) LParen Keyword(this) RParen RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_40___) Comma Identifier(obj) RParen Semicolon 
RBrace 
Keyword(function) Identifier(str) LParen Keyword(const) Identifier(obj) RParen Colon 
Keyword(return) Identifier(Object) Dot Identifier(toString) LParen Identifier(obj) RParen Semicolon 
Keyword(class) Identifier(Number) Colon 
	LBrace 
	Keyword(const) Identifier(MIN_VALUE) Equals Identifier(___STRING_42___) Semicolon 
	Keyword(const) Identifier(MAX_VALUE) Equals Identifier(___STRING_43___) Semicolon 
	Keyword(function) Identifier(isNumber) LParen Keyword(let) Identifier(var) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_44___) Comma Identifier(var) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(randInt) LParen Keyword(let) Identifier(min) Comma Keyword(let) Identifier(max) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(isNumber) LParen Identifier(min) RParen Equals Equals Keyword(false) RParen Or Or LParen Identifier(isNumber) LParen Identifier(max) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_45___) RParen Semicolon 
		RBrace 
		Identifier(min) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(min) RParen Semicolon 
		Identifier(max) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(max) RParen Semicolon 
		Keyword(if) Identifier(min) Greater Equals Identifier(max) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_46___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(Parser) Dot Identifier(asNumber) LParen Identifier(_native) LParen Identifier(___STRING_47___) Comma Identifier(min) Comma Identifier(max) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toFixedInt) LParen Keyword(let) Identifier(float) RParen Colon 
		LBrace 
		Keyword(return) Identifier(toFixed) LParen Identifier(float) Comma Identifier(___STRING_48___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toFixed) LParen Keyword(let) Identifier(number) Comma Keyword(let) Identifier(pattern) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_49___) Comma Identifier(number) Comma Identifier(_str) LParen Identifier(pattern) RParen RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Dictionary) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_keys) Semicolon 
	Keyword(private) Keyword(let) Identifier(_values) Semicolon 
	Keyword(function) Identifier(new) LParen RParen Colon 
		LBrace 
		Identifier(_keys) Equals Identifier(Array) Dot Identifier(new) LParen LSquareParen RSquareParen RParen Semicolon 
		Identifier(_values) Equals Identifier(Array) Dot Identifier(new) LParen LSquareParen RSquareParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(let) Identifier(str) Equals Identifier(_keys) Dot Identifier(toString) LParen RParen Dot Identifier(toString) LParen RParen Semicolon 
		Identifier(str) Equals Identifier(str) Plus Identifier(_values) Dot Identifier(toString) LParen RParen Dot Identifier(toString) LParen RParen Semicolon 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(str) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(set) LParen Keyword(let) Identifier(key) Comma Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(_keys) Dot Identifier(contains) LParen Identifier(key) RParen Equals Equals Keyword(true) RParen Colon 
			LBrace 
			Identifier(_values) Dot Identifier(setAt) LParen Identifier(get) LParen Identifier(key) RParen Comma Identifier(item) RParen Semicolon 
		RBrace 
		Keyword(else) Colon 
			LBrace 
			Identifier(_keys) Dot Identifier(push) LParen Identifier(key) RParen Semicolon 
			Identifier(_values) Dot Identifier(push) LParen Identifier(item) RParen Semicolon 
		RBrace 
	RBrace 
	Keyword(function) Identifier(containsKey) LParen Keyword(let) Identifier(key) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_keys) Dot Identifier(contains) LParen Identifier(key) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(containsValue) LParen Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_values) Dot Identifier(contains) LParen Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(getKeys) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_keys) Semicolon 
	RBrace 
	Keyword(function) Identifier(getValues) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_values) Semicolon 
	RBrace 
	Keyword(function) Identifier(clear) LParen RParen Colon 
		LBrace 
		Identifier(_keys) Dot Identifier(clear) LParen RParen Semicolon 
		Identifier(_values) Dot Identifier(clear) LParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(get) LParen Keyword(let) Identifier(key) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(_keys) Dot Identifier(contains) LParen Identifier(key) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_51___) RParen Semicolon 
		RBrace 
		Keyword(let) Identifier(index) Equals Identifier(_keys) Dot Identifier(indexOf) LParen Identifier(key) RParen Semicolon 
		Keyword(return) Identifier(_values) Dot Identifier(at) LParen Identifier(index) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Array) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_value) Semicolon 
	Keyword(function) Identifier(new) LParen Keyword(const) Less Identifier(Collection) Greater Identifier(collection) Equals LSquareParen RSquareParen RParen Colon 
	Identifier(_value) Equals Identifier(collection) Semicolon 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(let) Identifier(str) Equals Identifier(___STRING_53___) Semicolon 
		Keyword(if) Identifier(length) LParen RParen Greater Identifier(___NUMBER_8___) Colon 
			LBrace 
			Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___NUMBER_9___) Semicolon 
			Identifier(i) Less Identifier(length) LParen RParen Semicolon 
			Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_10___) Colon 
			Identifier(str) Equals Identifier(str) Plus Identifier(___STRING_54___) Plus Identifier(at) LParen Identifier(i) RParen Plus Identifier(___STRING_55___) Semicolon 
			Identifier(str) Equals Identifier(str) Dot Identifier(subString) LParen Identifier(___NUMBER_11___) Comma Identifier(str) Dot Identifier(length) LParen RParen Minus Identifier(___NUMBER_12___) RParen Semicolon 
			Identifier(str) Equals Identifier(str) Plus Identifier(___STRING_56___) Semicolon 
		RBrace 
		Keyword(else) Colon 
		Identifier(str) Equals Identifier(___STRING_57___) Semicolon 
		Keyword(return) Identifier(str) Dot Identifier(toString) LParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isArray) LParen Keyword(const) Identifier(var) RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_58___) Comma Identifier(var) RParen Semicolon 
	Keyword(function) Identifier(isCollection) LParen Keyword(let) Identifier(collection) RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_59___) Comma Identifier(collection) RParen Semicolon 
	Keyword(function) Identifier(getCollection) LParen RParen Colon 
	Keyword(return) Identifier(_value) Semicolon 
	Keyword(function) Identifier(contains) LParen Keyword(let) Identifier(item) RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_60___) Comma Identifier(_value) Comma Identifier(item) RParen Semicolon 
	Keyword(function) Identifier(push) LParen Keyword(let) Identifier(item) RParen Colon 
	Identifier(_native) LParen Identifier(___STRING_61___) Comma Identifier(_value) Comma Identifier(item) RParen Semicolon 
	Keyword(function) Identifier(clear) LParen RParen Colon 
	Identifier(_value) Equals Identifier(_native) LParen Identifier(___STRING_62___) RParen Semicolon 
	Keyword(function) Identifier(at) LParen Keyword(const) Less Identifier(Number) Greater Identifier(index) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(index) RParen Semicolon 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_63___) Comma Identifier(_value) Comma Identifier(index) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setAt) LParen Keyword(const) Less Identifier(Number) Greater Identifier(index) Comma Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(index) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_64___) Comma Identifier(_value) Comma Identifier(index) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(insert) LParen Keyword(const) Less Identifier(Number) Greater Identifier(index) Comma Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(index) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_65___) Comma Identifier(_value) Comma Identifier(index) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(removeAt) LParen Keyword(const) Less Identifier(Number) Greater Identifier(index) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(index) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_66___) Comma Identifier(_value) Comma Identifier(index) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(indexOf) LParen Keyword(let) Identifier(item) RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_67___) Comma Identifier(_value) Comma Identifier(item) RParen Semicolon 
	Keyword(function) Identifier(length) LParen RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_68___) Comma Identifier(_value) RParen Semicolon 
RBrace 
Keyword(class) Identifier(Throw) Colon 
	LBrace 
	Keyword(function) Identifier(exception) LParen Keyword(const) Less Identifier(String) Greater Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_70___) Comma Identifier(_str) LParen Identifier(message) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(nonImplementException) LParen RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_71___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(parseException) LParen Keyword(const) Less Identifier(String) Greater Identifier(error) RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_72___) Plus Identifier(error) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(nullException) LParen RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_73___) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Regex) Colon 
	LBrace 
	Keyword(function) Identifier(replace) LParen Keyword(const) Less Identifier(String) Greater Identifier(input) Comma Keyword(const) Less Identifier(String) Greater Identifier(pattern) Comma Keyword(const) Less Identifier(String) Greater Identifier(replacement) Equals Identifier(___STRING_75___) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_76___) Comma Identifier(input) Comma Identifier(pattern) Comma Identifier(replacement) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Parser) Colon 
	LBrace 
	Keyword(static) Keyword(function) Identifier(asInt) LParen Keyword(const) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_78___) Comma Identifier(object) RParen Semicolon 
	RBrace 
	Keyword(static) Keyword(function) Identifier(asFloat) LParen Keyword(const) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_79___) Comma Identifier(object) RParen Semicolon 
	RBrace 
	Keyword(static) Keyword(function) Identifier(asString) LParen Keyword(const) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_80___) Comma Identifier(object) RParen Semicolon 
	RBrace 
	Keyword(static) Keyword(function) Identifier(asNumber) LParen Keyword(const) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_81___) Comma Identifier(object) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Math) Colon 
	LBrace 
	Keyword(function) Identifier(max) LParen Keyword(const) Less Identifier(Number) Greater Identifier(a) Comma Keyword(const) Less Identifier(Number) Greater Identifier(b) RParen Colon 
		LBrace 
		Keyword(if) Identifier(a) Greater Identifier(b) Colon 
		Keyword(return) Identifier(a) Semicolon 
		Keyword(return) Identifier(b) Semicolon 
	RBrace 
	Keyword(function) Identifier(min) LParen Keyword(const) Less Identifier(Number) Greater Identifier(a) Comma Keyword(const) Less Identifier(Number) Greater Identifier(b) RParen Colon 
		LBrace 
		Keyword(if) Identifier(a) Less Identifier(b) Colon 
		Keyword(return) Identifier(a) Semicolon 
		Keyword(return) Identifier(b) Semicolon 
	RBrace 
	Keyword(function) Identifier(abs) LParen Keyword(const) Less Identifier(Number) Greater Identifier(n) RParen Colon 
		LBrace 
		Keyword(return) Identifier(___NUMBER_13___) Minus Identifier(n) Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Vector2) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_x) Semicolon 
	Keyword(private) Keyword(let) Identifier(_y) Semicolon 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(___STRING_84___) Plus Identifier(_x) Plus Identifier(___STRING_85___) Plus Identifier(_y) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(equals) LParen Keyword(let) Identifier(other) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(other) Dot Identifier(X) LParen RParen Equals Equals Identifier(_x) RParen And And LParen Identifier(other) Dot Identifier(Y) LParen RParen Equals Equals Identifier(_y) RParen Colon 
			LBrace 
			Keyword(return) Keyword(true) Semicolon 
		RBrace 
		Keyword(return) Keyword(false) Semicolon 
	RBrace 
	Keyword(function) Identifier(new) LParen Keyword(let) Identifier(x) Comma Keyword(let) Identifier(y) RParen Colon 
		LBrace 
		Identifier(_x) Equals Identifier(Parser) Dot Identifier(asNumber) LParen Identifier(x) RParen Semicolon 
		Identifier(_y) Equals Identifier(Parser) Dot Identifier(asNumber) LParen Identifier(y) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(X) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(Parser) Dot Identifier(asNumber) LParen Identifier(_x) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(Y) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(Parser) Dot Identifier(asNumber) LParen Identifier(_y) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Time) Colon 
	LBrace 
	Keyword(function) Identifier(wait) LParen Keyword(let) Less Identifier(Number) Greater Identifier(millisec) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(millisec) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parseException) LParen Identifier(___STRING_87___) RParen Semicolon 
		RBrace 
		Identifier(millisec) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(millisec) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_88___) Comma Identifier(millisec) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Meta) Keyword(extends) Identifier(Object) Colon 
	LBrace 
	Keyword(private) Keyword(function) Identifier(_isClass) LParen Keyword(const) Identifier(object) RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_91___) Comma Identifier(object) RParen Semicolon 
	Keyword(function) Identifier(getMethodListOf) LParen Keyword(const) Identifier(object) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(_isClass) LParen Identifier(object) RParen Equals Equals Keyword(false) RParen Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_92___) RParen Semicolon 
		Keyword(return) Identifier(Array) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_93___) Comma Identifier(object) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(getVariableListOf) LParen Keyword(const) Identifier(object) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(_isClass) LParen Identifier(object) RParen Equals Equals Keyword(false) RParen Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_94___) RParen Semicolon 
		Keyword(return) Identifier(Array) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_95___) Comma Identifier(object) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(getInfoOf) LParen Keyword(const) Identifier(object) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(_isClass) LParen Identifier(object) RParen Equals Equals Keyword(false) RParen Colon 
		Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_96___) RParen Semicolon 
		Keyword(return) Identifier(Array) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_97___) Comma Identifier(object) RParen RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Console) Colon 
	LBrace 
	Keyword(function) Identifier(print) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(Object) Dot Identifier(isSimplify) LParen Identifier(message) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Keyword(if) LParen Identifier(Meta) Dot Identifier(getMethodListOf) LParen Identifier(message) RParen Dot Identifier(contains) LParen Identifier(___STRING_99___) RParen RParen Colon 
			Identifier(message) Equals Identifier(message) Dot Identifier(toString) LParen RParen Semicolon 
			Keyword(else) Colon 
			Identifier(str) LParen Identifier(message) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_100___) Comma Identifier(_str) LParen Identifier(message) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(println) LParen Keyword(let) Identifier(message) Equals Identifier(___STRING_101___) RParen Colon 
		LBrace 
		Identifier(message) Equals Identifier(str) LParen Identifier(message) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_102___) Comma Identifier(_str) LParen Identifier(message) Plus Identifier(___STRING_103___) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(printVerbatim) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(Object) Dot Identifier(isSimplify) LParen Identifier(message) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Keyword(if) LParen Identifier(Meta) Dot Identifier(getMethodListOf) LParen Identifier(message) RParen Dot Identifier(contains) LParen Identifier(___STRING_104___) RParen RParen Colon 
			Identifier(message) Equals Identifier(message) Dot Identifier(toString) LParen RParen Semicolon 
			Keyword(else) Colon 
			Identifier(str) LParen Identifier(message) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_105___) Comma Identifier(message) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(printlnVerbatim) LParen Keyword(let) Identifier(message) Equals Identifier(___STRING_106___) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(Object) Dot Identifier(isSimplify) LParen Identifier(message) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Keyword(if) LParen Identifier(Meta) Dot Identifier(getMethodListOf) LParen Identifier(message) RParen Dot Identifier(contains) LParen Identifier(___STRING_107___) RParen RParen Colon 
			Identifier(message) Equals Identifier(message) Dot Identifier(toString) LParen RParen Semicolon 
			Keyword(else) Colon 
			Identifier(str) LParen Identifier(message) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_108___) Comma Identifier(message) Plus Identifier(_str) LParen Identifier(___STRING_109___) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(readln) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_110___) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(readkey) LParen Keyword(const) Less Identifier(Boolean) Greater Identifier(intercept) Equals Keyword(false) RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_111___) Comma Identifier(intercept) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isKeyAvailable) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_112___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(cursorVisible) LParen Keyword(const) Less Identifier(Boolean) Greater Identifier(visible) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_113___) Comma Identifier(visible) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(clear) LParen RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_114___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setCursorPosition) LParen Keyword(let) Less Identifier(Number) Greater Identifier(x) Comma Keyword(let) Less Identifier(Number) Greater Identifier(y) RParen Colon 
		LBrace 
		Identifier(x) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(x) RParen Semicolon 
		Identifier(y) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(y) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_115___) Comma Identifier(x) Comma Identifier(y) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setForeColor) LParen Keyword(let) Less Identifier(String) Greater Identifier(color) RParen Colon 
	Identifier(_native) LParen Identifier(___STRING_116___) Comma Identifier(color) RParen Semicolon 
	Keyword(function) Identifier(setBackColor) LParen Keyword(let) Less Identifier(String) Greater Identifier(color) RParen Colon 
	Identifier(_native) LParen Identifier(___STRING_117___) Comma Identifier(color) RParen Semicolon 
	Keyword(function) Identifier(resetColors) LParen RParen Colon 
	Identifier(_native) LParen Identifier(___STRING_118___) RParen Semicolon 
	Keyword(function) Identifier(width) LParen RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_119___) RParen Semicolon 
	Keyword(function) Identifier(height) LParen RParen Colon 
	Keyword(return) Identifier(_native) LParen Identifier(___STRING_120___) RParen Semicolon 
RBrace 
Keyword(namespace) Identifier(base) Colon 
	LBrace 
RBrace 
Keyword(namespace) Identifier(system) Colon 
	LBrace 
	Keyword(class) Identifier(Console) Colon 
		LBrace 
		Keyword(function) Identifier(WriteLine) LParen Keyword(const) Identifier(message) RParen Colon 
			LBrace 
			Identifier(Console) Dot Identifier(println) LParen Identifier(message) RParen Semicolon 
		RBrace 
	RBrace 
	Keyword(class) Identifier(Console2) Keyword(extends) Identifier(Object) Colon 
		LBrace 
		Keyword(function) Identifier(WriteLine) LParen Keyword(const) Identifier(message) RParen Colon 
			LBrace 
			Identifier(Console) Dot Identifier(println) LParen Identifier(message) RParen Semicolon 
		RBrace 
	RBrace 
	Keyword(const) Identifier(var) Equals Identifier(___STRING_122___) Semicolon 
	Keyword(function) Identifier(out) LParen RParen Colon 
		LBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_123___) RParen Semicolon 
	RBrace 
RBrace 
Keyword(namespace) Identifier(system) Colon 
	LBrace 
	Keyword(class) Identifier(OtherClass) Colon 
		LBrace 
		Keyword(function) Identifier(DoSomething) LParen RParen Colon 
			LBrace 
			Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_124___) RParen Semicolon 
		RBrace 
	RBrace 
RBrace 
Keyword(function) Identifier(main) LParen RParen Colon 
	LBrace 
	Identifier(system) Colon 
	Colon 
	Identifier(Console) Dot Identifier(WriteLine) LParen Identifier(system) Colon 
	Colon 
	Identifier(var) RParen Semicolon 
	Identifier(system) Colon 
	Colon 
	Identifier(out) LParen RParen Semicolon 
	Identifier(system) Colon 
	Colon 
	Identifier(Console) Dot Identifier(WriteLine) LParen Identifier(system) Colon 
	Colon 
	Identifier(Console2) RParen Semicolon 
RBrace 
