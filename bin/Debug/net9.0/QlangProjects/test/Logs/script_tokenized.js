Keyword(function) Identifier(main) LParen RParen Colon 
	LBrace 
	Keyword(let) Identifier(ex) Equals Identifier(___STRING_0___) Semicolon 
	Identifier(ex) Equals Identifier(___STRING_1___) Semicolon 
RBrace 
Keyword(class) Identifier(Console) Colon 
	LBrace 
	Keyword(function) Identifier(print) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_3___) Comma Identifier(_str) LParen Identifier(message) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(println) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_4___) Comma Identifier(_str) LParen Identifier(message) Plus Identifier(___STRING_5___) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(printVerbatim) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_6___) Comma Identifier(message) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(printlnVerbatim) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_7___) Comma Identifier(message) Plus Identifier(_str) LParen Identifier(___STRING_8___) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(readln) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_9___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(readkey) LParen Keyword(let) Identifier(intercept) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_10___) Comma Identifier(intercept) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isKeyAvailable) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_11___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(cursorVisible) LParen Keyword(let) Identifier(visible) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_12___) Comma Identifier(visible) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(clear) LParen RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_13___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setCursorPosition) LParen Keyword(let) Identifier(x) Comma Keyword(let) Identifier(y) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_14___) Comma Identifier(x) Comma Identifier(y) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setForeColor) LParen Keyword(let) Identifier(color) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_15___) Comma Identifier(color) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setBackColor) LParen Keyword(let) Identifier(color) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_16___) Comma Identifier(color) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(resetColors) LParen RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_17___) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Array) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_value) Equals Identifier(_native) LParen Identifier(___STRING_20___) RParen Semicolon 
	Keyword(function) Identifier(new) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isCollection) LParen Identifier(collection) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parseException) LParen Identifier(___STRING_21___) RParen Semicolon 
		RBrace 
		Identifier(_value) Equals Identifier(collection) Semicolon 
	RBrace 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(let) Identifier(str) Equals Identifier(___STRING_22___) Semicolon 
		Keyword(if) Identifier(length) LParen RParen Greater Identifier(___NUMBER_0___) Colon 
			LBrace 
			Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___NUMBER_1___) Semicolon 
			Identifier(i) Less Identifier(length) LParen RParen Semicolon 
			Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_2___) Colon 
				LBrace 
				Identifier(str) Equals Identifier(str) Plus Identifier(___STRING_23___) Plus Identifier(at) LParen Identifier(i) RParen Plus Identifier(___STRING_24___) Semicolon 
			RBrace 
			Identifier(str) Equals Identifier(String) Dot Identifier(new) LParen Identifier(str) RParen Semicolon 
			Identifier(str) Equals Identifier(str) Dot Identifier(subString) LParen Identifier(___NUMBER_3___) Comma Identifier(str) Dot Identifier(length) LParen RParen Minus Identifier(___NUMBER_4___) RParen Semicolon 
			Identifier(str) Equals Identifier(String) Dot Identifier(new) LParen Identifier(str) Dot Identifier(str) LParen RParen Plus Identifier(___STRING_25___) RParen Semicolon 
		RBrace 
		Keyword(else) Colon 
			LBrace 
			Identifier(str) Equals Identifier(String) Dot Identifier(new) LParen Identifier(___STRING_26___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(str) Semicolon 
	RBrace 
	Keyword(function) Identifier(isArray) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_27___) Comma Identifier(collection) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isCollection) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_28___) Comma Identifier(collection) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(getCollection) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_value) Semicolon 
	RBrace 
	Keyword(function) Identifier(contains) LParen Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_29___) Comma Identifier(_value) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(push) LParen Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_30___) Comma Identifier(_value) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(clear) LParen RParen Colon 
		LBrace 
		Identifier(_value) Equals Identifier(_native) LParen Identifier(___STRING_31___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(at) LParen Keyword(let) Identifier(index) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toFixedInt) LParen Identifier(index) RParen Semicolon 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_32___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_33___) Comma Identifier(_value) Comma Identifier(index) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setAt) LParen Keyword(let) Identifier(index) Comma Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_34___) RParen Semicolon 
		RBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toFixedInt) LParen Identifier(index) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_35___) Comma Identifier(_value) Comma Identifier(index) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(insert) LParen Keyword(let) Identifier(index) Comma Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_36___) RParen Semicolon 
		RBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toFixedInt) LParen Identifier(index) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_37___) Comma Identifier(_value) Comma Identifier(index) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(removeAt) LParen Keyword(let) Identifier(index) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toFixedInt) LParen Identifier(index) RParen Semicolon 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_38___) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_39___) Comma Identifier(_value) Comma Identifier(index) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(indexOf) LParen Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_40___) Comma Identifier(_value) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(length) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_41___) Comma Identifier(_value) RParen Semicolon 
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
		Keyword(let) Identifier(str) Equals Identifier(_keys) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen Semicolon 
		Identifier(str) Equals Identifier(str) Plus Identifier(_values) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen Semicolon 
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
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_43___) RParen Semicolon 
		RBrace 
		Keyword(let) Identifier(index) Equals Identifier(_keys) Dot Identifier(indexOf) LParen Identifier(key) RParen Semicolon 
		Keyword(return) Identifier(_values) Dot Identifier(at) LParen Identifier(index) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Vector2) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_x) Semicolon 
	Keyword(private) Keyword(let) Identifier(_y) Semicolon 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(___STRING_46___) Plus Identifier(_x) Plus Identifier(___STRING_47___) Plus Identifier(_y) RParen Semicolon 
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
Keyword(class) Identifier(Number) Colon 
	LBrace 
	Keyword(const) Identifier(MIN_VALUE) Equals Identifier(___STRING_49___) Semicolon 
	Keyword(const) Identifier(MAX_VALUE) Equals Identifier(___STRING_50___) Semicolon 
	Keyword(function) Identifier(isNumber) LParen Keyword(let) Identifier(var) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_51___) Comma Identifier(var) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(randInt) LParen Keyword(let) Identifier(min) Comma Keyword(let) Identifier(max) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(isNumber) LParen Identifier(min) RParen Equals Equals Keyword(false) RParen Or Or LParen Identifier(isNumber) LParen Identifier(max) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_52___) RParen Semicolon 
		RBrace 
		Identifier(min) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(min) RParen Semicolon 
		Identifier(max) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(max) RParen Semicolon 
		Keyword(if) Identifier(min) Greater Equals Identifier(max) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_53___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(Parser) Dot Identifier(asNumber) LParen Identifier(_native) LParen Identifier(___STRING_54___) Comma Identifier(min) Comma Identifier(max) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toFixedInt) LParen Keyword(let) Identifier(float) RParen Colon 
		LBrace 
		Keyword(return) Identifier(toFixed) LParen Identifier(float) Comma Identifier(___STRING_55___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toFixed) LParen Keyword(let) Identifier(number) Comma Keyword(let) Identifier(pattern) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_56___) Comma Identifier(number) Comma Identifier(_str) LParen Identifier(pattern) RParen RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Object) Colon 
	LBrace 
	Keyword(function) Identifier(isNull) LParen Keyword(let) Identifier(obj) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_58___) Comma Identifier(obj) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(String) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_value) Equals Identifier(___STRING_60___) Semicolon 
	Keyword(function) Identifier(new) LParen Keyword(let) Identifier(input) RParen Colon 
		LBrace 
		Identifier(_value) Equals Identifier(input) Semicolon 
	RBrace 
	Keyword(function) Identifier(str) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_value) Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_checkIsCollection) LParen Keyword(let) Identifier(value) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isCollection) LParen Identifier(value) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parceException) LParen Identifier(___STRING_61___) RParen Semicolon 
		RBrace 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_checkIsArray) LParen Keyword(let) Identifier(value) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isArray) LParen Identifier(value) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parceException) LParen Identifier(___STRING_62___) RParen Semicolon 
		RBrace 
	RBrace 
	Keyword(function) Identifier(append) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Identifier(_checkIsCollection) LParen Identifier(collection) RParen Semicolon 
		Keyword(let) Identifier(result) Equals Identifier(___STRING_63___) Semicolon 
		Keyword(let) Identifier(arr) Equals Identifier(Array) Dot Identifier(new) LParen Identifier(collection) RParen Semicolon 
		Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___NUMBER_5___) Semicolon 
		Identifier(i) Less Identifier(arr) Dot Identifier(length) LParen RParen Semicolon 
		Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_6___) Colon 
			LBrace 
			Identifier(result) Equals Identifier(result) Plus Identifier(arr) Dot Identifier(at) LParen Identifier(i) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(result) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toLower) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_64___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toUpper) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_65___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isString) LParen Keyword(let) Identifier(value) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_66___) Comma Identifier(value) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isPrimitive) LParen Keyword(let) Identifier(value) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_67___) Comma Identifier(value) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(split) LParen Keyword(let) Identifier(pattern) RParen Colon 
		LBrace 
		Keyword(return) Identifier(Array) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_68___) Comma Identifier(_str) LParen Identifier(_value) RParen Comma Identifier(pattern) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(join) LParen Keyword(let) Identifier(strArr) Comma Keyword(let) Identifier(pattern) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(Array) Dot Identifier(isCollection) LParen Identifier(strArr) RParen Equals Equals Keyword(false) RParen And And LParen Identifier(Array) Dot Identifier(isArray) LParen Identifier(strArr) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_69___) RParen Semicolon 
		RBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isArray) LParen Identifier(strArr) RParen Equals Equals Keyword(true) Colon 
			LBrace 
			Identifier(strArr) Equals Identifier(strArr) Dot Identifier(getCollection) LParen RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_70___) Comma Identifier(strArr) Comma Identifier(pattern) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(length) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_71___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isNullOrEmpty) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(String) Dot Identifier(isPrimitive) LParen Identifier(str) RParen Equals Equals Keyword(false) RParen And And LParen Identifier(String) Dot Identifier(isString) LParen Identifier(str) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_72___) RParen Semicolon 
		RBrace 
		Keyword(if) Identifier(String) Dot Identifier(isString) LParen Identifier(str) RParen Colon 
			LBrace 
			Identifier(str) Equals Identifier(str) Dot Identifier(str) LParen RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_73___) Comma Identifier(_str) LParen Identifier(str) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isNullOrWhitespace) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(String) Dot Identifier(isPrimitive) LParen Identifier(str) RParen Equals Equals Keyword(false) RParen And And LParen Identifier(String) Dot Identifier(isString) LParen Identifier(str) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_74___) RParen Semicolon 
		RBrace 
		Keyword(if) Identifier(String) Dot Identifier(isString) LParen Identifier(str) RParen Colon 
			LBrace 
			Identifier(str) Equals Identifier(str) Dot Identifier(str) LParen RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_75___) Comma Identifier(_str) LParen Identifier(str) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trim) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_76___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trimStart) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_77___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trimEnd) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_78___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(subString) LParen Keyword(let) Identifier(startPos) Comma Keyword(let) Identifier(length) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(startPos) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_79___) RParen Semicolon 
		RBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(length) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_80___) RParen Semicolon 
		RBrace 
		Keyword(if) Identifier(length) LParen RParen Less Equals Identifier(length) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_81___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_82___) Comma Identifier(_str) LParen Identifier(_value) RParen Comma Identifier(startPos) Comma Identifier(length) RParen RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Time) Colon 
	LBrace 
	Keyword(function) Identifier(wait) LParen Keyword(let) Identifier(millisec) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(millisec) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parseException) LParen Identifier(___STRING_84___) RParen Semicolon 
		RBrace 
		Identifier(millisec) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(millisec) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_85___) Comma Identifier(millisec) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Parser) Colon 
	LBrace 
	Keyword(static) Keyword(function) Identifier(asInt) LParen Keyword(let) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_87___) Comma Identifier(object) RParen Semicolon 
	RBrace 
	Keyword(static) Keyword(function) Identifier(asFloat) LParen Keyword(let) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_88___) Comma Identifier(object) RParen Semicolon 
	RBrace 
	Keyword(static) Keyword(function) Identifier(asString) LParen Keyword(let) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_89___) Comma Identifier(object) RParen Semicolon 
	RBrace 
	Keyword(static) Keyword(function) Identifier(asNumber) LParen Keyword(let) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_90___) Comma Identifier(object) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Regex) Colon 
	LBrace 
	Keyword(function) Identifier(replace) LParen Keyword(let) Identifier(input) Comma Keyword(let) Identifier(pattern) Comma Keyword(let) Identifier(replacement) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_92___) Comma Identifier(input) Comma Identifier(pattern) Comma Identifier(replacement) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(match) LParen Keyword(let) Identifier(input) Comma Keyword(let) Identifier(pattern) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_93___) Comma Identifier(input) Comma Identifier(pattern) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Throw) Colon 
	LBrace 
	Keyword(function) Identifier(exception) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_95___) Comma Identifier(_str) LParen Identifier(message) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(nonImplementException) LParen RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_96___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(parseException) LParen Keyword(let) Identifier(error) RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_97___) Plus Identifier(error) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Directory) Colon 
	LBrace 
	Keyword(function) Identifier(exists) LParen Keyword(let) Identifier(path) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_99___) Comma Identifier(path) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(create) LParen Keyword(let) Identifier(path) RParen Colon 
		LBrace 
		Keyword(if) Identifier(exists) LParen Identifier(path) RParen Equals Equals Keyword(true) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_100___) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_101___) Comma Identifier(path) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(remove) LParen Keyword(let) Identifier(path) RParen Colon 
		LBrace 
		Keyword(if) Identifier(exists) LParen Identifier(path) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_102___) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_103___) Comma Identifier(path) Comma Keyword(true) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(File) Colon 
	LBrace 
	Keyword(function) Identifier(exists) LParen Keyword(let) Identifier(path) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_105___) Comma Identifier(path) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setContent) LParen Keyword(let) Identifier(path) Comma Keyword(let) Identifier(content) RParen Colon 
		LBrace 
		Keyword(if) Identifier(exists) LParen Identifier(path) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_106___) Plus Identifier(path) Plus Identifier(___STRING_107___) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_108___) Comma Identifier(path) Comma Identifier(content) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(appendContent) LParen Keyword(let) Identifier(path) Comma Keyword(let) Identifier(content) RParen Colon 
		LBrace 
		Keyword(if) Identifier(exists) LParen Identifier(path) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_109___) Plus Identifier(path) Plus Identifier(___STRING_110___) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_111___) Comma Identifier(path) Comma Identifier(content) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(getContent) LParen Keyword(let) Identifier(path) RParen Colon 
		LBrace 
		Keyword(if) Identifier(exists) LParen Identifier(path) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_112___) Plus Identifier(path) Plus Identifier(___STRING_113___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_114___) Comma Identifier(path) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(create) LParen Keyword(let) Identifier(path) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_115___) Comma Identifier(path) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(remove) LParen Keyword(let) Identifier(path) RParen Colon 
		LBrace 
		Keyword(if) Identifier(exists) LParen Identifier(path) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_116___) Plus Identifier(path) Plus Identifier(___STRING_117___) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_118___) Comma Identifier(path) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Path) Colon 
	LBrace 
	Keyword(function) Identifier(combine) LParen Keyword(let) Identifier(arr) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(Array) Dot Identifier(isCollection) LParen Identifier(arr) RParen Equals Equals Keyword(false) RParen And And LParen Identifier(Array) Dot Identifier(isArray) LParen Identifier(arr) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_120___) RParen Semicolon 
		RBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isArray) LParen Identifier(arr) RParen Equals Equals Keyword(true) Colon 
			LBrace 
			Identifier(arr) Equals Identifier(arr) Dot Identifier(getCollection) LParen RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(String) Dot Identifier(new) LParen Identifier(_native) LParen Identifier(___STRING_121___) Comma Identifier(arr) RParen RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(BaseTest) Colon 
	LBrace 
	Keyword(function) Identifier(runTest) LParen RParen Colon 
		LBrace 
		Identifier(_stringTest) LParen RParen Semicolon 
		Identifier(_numberTest) LParen RParen Semicolon 
		Identifier(_arrayTest) LParen RParen Semicolon 
		Identifier(_dictionaryTest) LParen RParen Semicolon 
		Identifier(_objectTest) LParen RParen Semicolon 
		Identifier(_regexTest) LParen RParen Semicolon 
		Identifier(_vectorTest) LParen RParen Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(printHeader) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Identifier(Console) Dot Identifier(setForeColor) LParen Identifier(___STRING_123___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_124___) Plus Identifier(str) Plus Identifier(___STRING_125___) RParen Semicolon 
		Identifier(Console) Dot Identifier(resetColors) LParen RParen Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_numberTest) LParen RParen Colon 
		LBrace 
		Keyword(let) Identifier(a) Equals Identifier(___NUMBER_7___) Semicolon 
		Keyword(let) Identifier(b) Equals Identifier(___NUMBER_8___) Semicolon 
		Identifier(printHeader) LParen Identifier(___STRING_126___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_127___) Plus Identifier(a) Plus Identifier(___STRING_128___) Plus Identifier(Number) Dot Identifier(isNumber) LParen Identifier(a) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_129___) Plus Identifier(b) Plus Identifier(___STRING_130___) Plus Identifier(Number) Dot Identifier(isNumber) LParen Identifier(b) RParen Plus Identifier(___STRING_131___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(a) Plus Identifier(___STRING_132___) Plus Identifier(b) Plus Identifier(___STRING_133___) Plus LParen Identifier(a) Plus Identifier(b) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(a) Plus Identifier(___STRING_134___) Plus Identifier(b) Plus Identifier(___STRING_135___) Plus LParen Identifier(a) Minus Identifier(b) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(a) Plus Identifier(___STRING_136___) Plus Identifier(b) Plus Identifier(___STRING_137___) Plus LParen Identifier(a) Star Identifier(b) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(a) Plus Identifier(___STRING_138___) Plus Identifier(b) Plus Identifier(___STRING_139___) Plus LParen Identifier(a) Slash Identifier(b) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(a) Plus Identifier(___STRING_140___) Plus Identifier(b) Plus Identifier(___STRING_141___) Plus LParen Identifier(a) Percent Identifier(b) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_142___) Plus Identifier(Number) Dot Identifier(MIN_VALUE) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_143___) Plus Identifier(Number) Dot Identifier(MAX_VALUE) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_144___) Plus Identifier(a) Plus Identifier(___STRING_145___) Plus Identifier(b) Plus Identifier(___STRING_146___) Plus Identifier(Number) Dot Identifier(randInt) LParen Identifier(a) Comma Identifier(b) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_147___) Plus Identifier(b) Plus Identifier(___STRING_148___) Plus Identifier(Number) Dot Identifier(toFixed) LParen Identifier(b) Comma Identifier(___STRING_149___) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_150___) Plus Identifier(a) Plus Identifier(___STRING_151___) Plus Identifier(Number) Dot Identifier(toFixedInt) LParen Identifier(a) RParen RParen Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_arrayTest) LParen RParen Colon 
		LBrace 
		Keyword(let) Identifier(collection) Equals LSquareParen Identifier(___STRING_152___) Comma Identifier(___STRING_153___) Comma Identifier(___STRING_154___) RSquareParen Semicolon 
		Keyword(let) Identifier(array) Equals Identifier(Array) Dot Identifier(new) LParen Identifier(collection) RParen Semicolon 
		Identifier(printHeader) LParen Identifier(___STRING_155___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_156___) Plus Identifier(Array) Dot Identifier(new) LParen Identifier(collection) RParen Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen Plus Identifier(___STRING_157___) Plus Identifier(Array) Dot Identifier(isCollection) LParen Identifier(collection) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_158___) Plus Identifier(array) Plus Identifier(___STRING_159___) Plus Identifier(array) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen Plus Identifier(___STRING_160___) Plus Identifier(Array) Dot Identifier(isArray) LParen Identifier(array) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_161___) Plus Identifier(array) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_162___) Plus Identifier(array) Dot Identifier(contains) LParen Identifier(___STRING_163___) RParen RParen Semicolon 
		Identifier(array) Dot Identifier(push) LParen Identifier(___STRING_164___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_165___) Plus Identifier(array) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen RParen Semicolon 
		Identifier(array) Dot Identifier(setAt) LParen Identifier(___NUMBER_9___) Comma Identifier(___STRING_166___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_167___) Plus Identifier(array) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_168___) Plus Identifier(array) Dot Identifier(at) LParen Identifier(___NUMBER_10___) RParen RParen Semicolon 
		Identifier(array) Dot Identifier(insert) LParen Identifier(___NUMBER_11___) Comma Identifier(___STRING_169___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_170___) Plus Identifier(array) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_171___) Plus Identifier(array) Dot Identifier(indexOf) LParen Identifier(___STRING_172___) RParen RParen Semicolon 
		Identifier(array) Dot Identifier(removeAt) LParen Identifier(___NUMBER_12___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_173___) Plus Identifier(array) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_174___) Plus Identifier(array) Dot Identifier(length) LParen RParen RParen Semicolon 
		Identifier(array) Dot Identifier(clear) LParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_175___) Plus Identifier(array) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen RParen Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_dictionaryTest) LParen RParen Colon 
		LBrace 
		Keyword(let) Identifier(dict) Equals Identifier(Dictionary) Dot Identifier(new) LParen RParen Semicolon 
		Identifier(printHeader) LParen Identifier(___STRING_176___) RParen Semicolon 
		Identifier(dict) Dot Identifier(set) LParen Identifier(___STRING_177___) Comma Identifier(___STRING_178___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_179___) Plus Identifier(dict) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_180___) Plus Identifier(dict) Dot Identifier(containsKey) LParen Identifier(___STRING_181___) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_182___) Plus Identifier(dict) Dot Identifier(containsValue) LParen Identifier(___STRING_183___) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_184___) Plus Identifier(dict) Dot Identifier(get) LParen Identifier(___STRING_185___) RParen RParen Semicolon 
		Identifier(dict) Dot Identifier(clear) LParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_186___) Plus Identifier(dict) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen RParen Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_objectTest) LParen RParen Colon 
		LBrace 
		Identifier(printHeader) LParen Identifier(___STRING_187___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_188___) Plus Identifier(Object) Dot Identifier(isNull) LParen Identifier(___NUMBER_13___) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_189___) Plus Identifier(Object) Dot Identifier(isNull) LParen Keyword(null) RParen RParen Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_stringTest) LParen RParen Colon 
		LBrace 
		Keyword(let) Identifier(str) Equals Identifier(String) Dot Identifier(new) LParen Identifier(___STRING_190___) RParen Semicolon 
		Keyword(let) Identifier(primitive) Equals Identifier(___STRING_191___) Semicolon 
		Identifier(printHeader) LParen Identifier(___STRING_192___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_193___) Plus Identifier(primitive) Plus Identifier(___STRING_194___) Plus Identifier(String) Dot Identifier(isPrimitive) LParen Identifier(primitive) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_195___) Plus Identifier(str) Plus Identifier(___STRING_196___) Plus Identifier(str) Dot Identifier(str) LParen RParen Plus Identifier(___STRING_197___) Plus Identifier(String) Dot Identifier(isString) LParen Identifier(str) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_198___) Plus Identifier(String) Dot Identifier(append) LParen LSquareParen Identifier(___STRING_199___) Comma Identifier(___STRING_200___) RSquareParen RParen Dot Identifier(str) LParen RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_201___) Plus Identifier(str) Dot Identifier(str) LParen RParen Plus Identifier(___STRING_202___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_203___) Plus Identifier(str) Dot Identifier(length) LParen RParen Plus Identifier(___STRING_204___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_205___) Plus Identifier(str) Dot Identifier(trim) LParen RParen Dot Identifier(str) LParen RParen Plus Identifier(___STRING_206___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_207___) Plus Identifier(str) Dot Identifier(trimStart) LParen RParen Dot Identifier(str) LParen RParen Plus Identifier(___STRING_208___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_209___) Plus Identifier(str) Dot Identifier(trimEnd) LParen RParen Dot Identifier(str) LParen RParen Plus Identifier(___STRING_210___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_211___) Plus Identifier(str) Dot Identifier(toLower) LParen RParen Dot Identifier(str) LParen RParen Plus Identifier(___STRING_212___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_213___) Plus Identifier(str) Dot Identifier(toUpper) LParen RParen Dot Identifier(str) LParen RParen Plus Identifier(___STRING_214___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_215___) Plus Identifier(str) Dot Identifier(isNullOrEmpty) LParen Identifier(str) RParen Plus Identifier(___STRING_216___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_217___) Plus Identifier(str) Dot Identifier(isNullOrWhitespace) LParen Identifier(str) RParen Plus Identifier(___STRING_218___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_219___) Plus Identifier(str) Dot Identifier(subString) LParen Identifier(___NUMBER_14___) Comma Identifier(___NUMBER_15___) RParen Dot Identifier(str) LParen RParen Plus Identifier(___STRING_220___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_221___) Plus Identifier(str) Dot Identifier(split) LParen Identifier(___STRING_222___) RParen Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen Plus Identifier(___STRING_223___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_224___) Plus Identifier(String) Dot Identifier(join) LParen Identifier(str) Dot Identifier(split) LParen Identifier(___STRING_225___) RParen Comma Identifier(___STRING_226___) RParen Dot Identifier(str) LParen RParen Plus Identifier(___STRING_227___) RParen Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_vectorTest) LParen RParen Colon 
		LBrace 
		Keyword(let) Identifier(vect) Equals Identifier(Vector2) Dot Identifier(new) LParen Identifier(___NUMBER_16___) Comma Identifier(___NUMBER_17___) RParen Semicolon 
		Identifier(printHeader) LParen Identifier(___STRING_228___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_229___) Plus Identifier(vect) Dot Identifier(toString) LParen RParen Dot Identifier(str) LParen RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_230___) Plus Identifier(vect) Dot Identifier(X) LParen RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_231___) Plus Identifier(vect) Dot Identifier(Y) LParen RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_232___) Plus Identifier(vect) Dot Identifier(equals) LParen Identifier(Vector2) Dot Identifier(new) LParen Identifier(___NUMBER_18___) Comma Identifier(___NUMBER_19___) RParen RParen RParen Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_regexTest) LParen RParen Colon 
		LBrace 
		Identifier(printHeader) LParen Identifier(___STRING_233___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_234___) Plus Identifier(Regex) Dot Identifier(replace) LParen Identifier(___STRING_235___) Comma Identifier(___STRING_236___) Comma Identifier(___STRING_237___) RParen RParen Semicolon 
	RBrace 
RBrace 
