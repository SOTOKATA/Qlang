Keyword(function) Identifier(main) LParen RParen Colon 
	LBrace 
	Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_0___) Plus LParen Identifier(___N0___) Star Identifier(___N1___) Plus Identifier(___N2___) RParen RParen Semicolon 
	Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_1___) Plus LParen Identifier(___N3___) Star LParen Identifier(___N4___) Plus Identifier(___N5___) RParen RParen RParen Semicolon 
RBrace 
Keyword(class) Identifier(Console) Colon 
	LBrace 
	Keyword(function) Identifier(print) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_2___) Comma Identifier(_str) LParen Identifier(message) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(println) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_3___) Comma Identifier(_str) LParen Identifier(message) Plus Identifier(___STRING_4___) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(readln) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_5___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(readkey) LParen Keyword(let) Identifier(intercept) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_6___) Comma Identifier(intercept) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isKeyAvailable) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_7___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(cursorVisible) LParen Keyword(let) Identifier(visible) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_8___) Comma Identifier(visible) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(clear) LParen RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_9___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setCursorPosition) LParen Keyword(let) Identifier(x) Comma Keyword(let) Identifier(y) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_10___) Comma Identifier(x) Comma Identifier(y) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setForeColor) LParen Keyword(let) Identifier(color) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_11___) Comma Identifier(color) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setBackColor) LParen Keyword(let) Identifier(color) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_12___) Comma Identifier(color) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(resetColors) LParen RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_13___) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Parser) Colon 
	LBrace 
	Keyword(static) Keyword(function) Identifier(asInt) LParen Keyword(let) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_14___) Comma Identifier(object) RParen Semicolon 
	RBrace 
	Keyword(static) Keyword(function) Identifier(asFloat) LParen Keyword(let) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_15___) Comma Identifier(object) RParen Semicolon 
	RBrace 
	Keyword(static) Keyword(function) Identifier(asString) LParen Keyword(let) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_16___) Comma Identifier(object) RParen Semicolon 
	RBrace 
	Keyword(static) Keyword(function) Identifier(asNumber) LParen Keyword(let) Identifier(object) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_17___) Comma Identifier(object) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Throw) Colon 
	LBrace 
	Keyword(function) Identifier(exception) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_18___) Comma Identifier(_str) LParen Identifier(message) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(nonImplementException) LParen RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_19___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(parseException) LParen Keyword(let) Identifier(error) RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_20___) Plus Identifier(error) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Array) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_value) Equals Identifier(_native) LParen Identifier(___STRING_21___) RParen Semicolon 
	Keyword(function) Identifier(new) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isArray) LParen Identifier(collection) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parseException) LParen Identifier(___STRING_22___) RParen Semicolon 
		RBrace 
		Identifier(_value) Equals Identifier(collection) Semicolon 
	RBrace 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_value) Semicolon 
	RBrace 
	Keyword(function) Identifier(isArray) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_23___) Comma Identifier(collection) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(getCollection) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_value) Semicolon 
	RBrace 
	Keyword(function) Identifier(contains) LParen Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_24___) Comma Identifier(_value) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(push) LParen Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_25___) Comma Identifier(_value) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(clear) LParen RParen Colon 
		LBrace 
		Identifier(_value) Equals Identifier(_native) LParen Identifier(___STRING_26___) Comma Identifier(_value) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(at) LParen Keyword(let) Identifier(index) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toFixedInt) LParen Identifier(index) RParen Semicolon 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_27___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_28___) Comma Identifier(_value) Comma Identifier(index) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setAt) LParen Keyword(let) Identifier(index) Comma Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_29___) RParen Semicolon 
		RBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toFixedInt) LParen Identifier(index) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_30___) Comma Identifier(_value) Comma Identifier(index) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(insert) LParen Keyword(let) Identifier(index) Comma Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_31___) RParen Semicolon 
		RBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toFixedInt) LParen Identifier(index) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_32___) Comma Identifier(_value) Comma Identifier(index) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(removeAt) LParen Keyword(let) Identifier(index) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toFixedInt) LParen Identifier(index) RParen Semicolon 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_33___) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_34___) Comma Identifier(_value) Comma Identifier(index) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(indexOf) LParen Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_35___) Comma Identifier(_value) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(length) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_36___) Comma Identifier(_value) RParen Semicolon 
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
		Keyword(return) Identifier(___STRING_37___) Semicolon 
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
	Keyword(function) Identifier(get) LParen Keyword(let) Identifier(key) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(_keys) Dot Identifier(contains) LParen Identifier(key) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_38___) RParen Semicolon 
		RBrace 
		Keyword(let) Identifier(index) Equals Identifier(_keys) Dot Identifier(indexOf) LParen Identifier(key) RParen Semicolon 
		Keyword(return) Identifier(_values) Dot Identifier(at) LParen Identifier(index) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Number) Colon 
	LBrace 
	Keyword(const) Keyword(let) Identifier(MIN_VALUE) Equals Identifier(___STRING_39___) Semicolon 
	Keyword(const) Keyword(let) Identifier(MAX_VALUE) Equals Identifier(___STRING_40___) Semicolon 
	Keyword(function) Identifier(isNumber) LParen Keyword(let) Identifier(var) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_41___) Comma Identifier(var) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(randInt) LParen Keyword(let) Identifier(min) Comma Keyword(let) Identifier(max) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(isNumber) LParen Identifier(min) RParen Equals Equals Keyword(false) RParen Or Or LParen Identifier(isNumber) LParen Identifier(max) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_42___) RParen Semicolon 
		RBrace 
		Identifier(min) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(min) RParen Semicolon 
		Identifier(max) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(max) RParen Semicolon 
		Keyword(if) Identifier(min) Greater Equals Identifier(max) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_43___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(Parser) Dot Identifier(asNumber) LParen Identifier(_native) LParen Identifier(___STRING_44___) Comma Identifier(min) Comma Identifier(max) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toFixedInt) LParen Keyword(let) Identifier(float) RParen Colon 
		LBrace 
		Keyword(return) Identifier(toFixed) LParen Identifier(float) Comma Identifier(___STRING_45___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toFixed) LParen Keyword(let) Identifier(number) Comma Keyword(let) Identifier(pattern) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_46___) Comma Identifier(number) Comma Identifier(_str) LParen Identifier(pattern) RParen RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(String) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_value) Equals Identifier(___STRING_47___) Semicolon 
	Keyword(function) Identifier(new) LParen Keyword(let) Identifier(input) RParen Colon 
		LBrace 
		Identifier(_value) Equals Identifier(input) Semicolon 
	RBrace 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_value) Semicolon 
	RBrace 
	Keyword(function) Identifier(append) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isArray) LParen Identifier(collection) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parceException) LParen Identifier(___STRING_48___) RParen Semicolon 
		RBrace 
		Keyword(let) Identifier(result) Equals Identifier(___STRING_49___) Semicolon 
		Keyword(let) Identifier(arr) Equals Identifier(Array) Dot Identifier(new) LParen Identifier(collection) RParen Semicolon 
		Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___N6___) Semicolon 
		Identifier(i) Less Identifier(arr) Dot Identifier(length) LParen RParen Semicolon 
		Identifier(i) Equals Identifier(i) Plus Identifier(___N7___) Colon 
			LBrace 
			Identifier(result) Equals Identifier(result) Plus Identifier(arr) Dot Identifier(at) LParen Identifier(i) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(result) Semicolon 
	RBrace 
	Keyword(function) Identifier(length) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_50___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isNullOrEmpty) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_51___) Comma Identifier(_str) LParen Identifier(str) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isNullOrWhitespace) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_52___) Comma Identifier(_str) LParen Identifier(str) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trim) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_53___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trimStart) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_54___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trimEnd) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_55___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(subString) LParen Keyword(let) Identifier(startPos) Comma Keyword(let) Identifier(length) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(startPos) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_56___) RParen Semicolon 
		RBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(length) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_57___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_58___) Comma Identifier(_str) LParen Identifier(_value) RParen Comma Identifier(startPos) Comma Identifier(length) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Vector2) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_x) Semicolon 
	Keyword(private) Keyword(let) Identifier(_y) Semicolon 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(___STRING_59___) Plus Identifier(_x) Plus Identifier(___STRING_60___) Plus Identifier(_y) Semicolon 
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
	Keyword(function) Identifier(wait) LParen Keyword(let) Identifier(millisec) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(millisec) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parseException) LParen Identifier(___STRING_61___) RParen Semicolon 
		RBrace 
		Identifier(millisec) Equals Identifier(Parser) Dot Identifier(asInt) LParen Identifier(millisec) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_62___) Comma Identifier(millisec) RParen Semicolon 
	RBrace 
RBrace 
