Keyword(class) Identifier(Array) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_value) Equals Identifier(_csharp) LParen Identifier(___STRING_0___) RParen Semicolon 
	Keyword(function) Identifier(new) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isArray) LParen Identifier(collection) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parseException) LParen Identifier(___STRING_1___) RParen Semicolon 
		RBrace 
		Identifier(_value) Equals Identifier(collection) Semicolon 
	RBrace 
	Keyword(function) Identifier(isArray) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(collection) Plus Identifier(___STRING_2___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(push) LParen Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Identifier(_value) Equals Identifier(_csharp) LParen Identifier(___STRING_3___) Plus Identifier(_value) Plus Identifier(___STRING_4___) Plus Identifier(___STRING_5___) Plus Identifier(_str) LParen Identifier(item) RParen Plus Identifier(___STRING_6___) Plus Identifier(___STRING_7___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(clear) LParen RParen Colon 
		LBrace 
		Identifier(_value) Equals Identifier(_csharp) LParen Identifier(___STRING_8___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(at) LParen Keyword(let) Identifier(index) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toInt) LParen Identifier(index) RParen Semicolon 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_9___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(_value) Plus Identifier(___STRING_10___) Plus Identifier(index) Plus Identifier(___STRING_11___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setAt) LParen Keyword(let) Identifier(index) Comma Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toInt) LParen Identifier(index) RParen Semicolon 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_12___) RParen Semicolon 
		RBrace 
		Identifier(_value) Equals Identifier(_csharp) LParen Identifier(___STRING_13___) Plus Identifier(_value) Plus Identifier(___STRING_14___) Plus Identifier(___STRING_15___) Plus Identifier(index) Plus Identifier(___STRING_16___) Plus Identifier(_str) LParen Identifier(item) RParen Plus Identifier(___STRING_17___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(removeAt) LParen Keyword(let) Identifier(index) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toInt) LParen Identifier(index) RParen Semicolon 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_18___) RParen Semicolon 
		RBrace 
		Identifier(_value) Equals Identifier(_csharp) LParen Identifier(___STRING_19___) Plus Identifier(_value) Plus Identifier(___STRING_20___) Plus Identifier(index) Plus Identifier(___STRING_21___) Plus Identifier(___STRING_22___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(length) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(___STRING_23___) Plus Identifier(_value) Plus Identifier(___STRING_24___) Plus Identifier(___STRING_25___) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Number) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(usings) Equals Identifier(___STRING_26___) Semicolon 
	Keyword(const) Keyword(let) Identifier(MIN_VALUE) Equals Identifier(___STRING_27___) Semicolon 
	Keyword(const) Keyword(let) Identifier(MAX_VALUE) Equals Identifier(___STRING_28___) Semicolon 
	Keyword(function) Identifier(isNumber) LParen Keyword(let) Identifier(var) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(usings) Plus Identifier(___STRING_29___) Plus Identifier(_str) LParen Identifier(var) RParen Plus Identifier(___STRING_30___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(randInt) LParen Keyword(let) Identifier(min) Comma Keyword(let) Identifier(max) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(isNumber) LParen Identifier(min) RParen Equals Equals Keyword(false) RParen Or Or LParen Identifier(isNumber) LParen Identifier(max) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_31___) RParen Semicolon 
		RBrace 
		Identifier(min) Equals Identifier(toInt) LParen Identifier(min) RParen Semicolon 
		Identifier(max) Equals Identifier(toInt) LParen Identifier(max) RParen Semicolon 
		Keyword(if) Identifier(min) Greater Equals Identifier(max) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_32___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(usings) Plus Identifier(___STRING_33___) Plus Identifier(min) Plus Identifier(___STRING_34___) Plus Identifier(max) Plus Identifier(___STRING_35___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toInt) LParen Keyword(let) Identifier(float) RParen Colon 
		LBrace 
		Keyword(return) Identifier(toFixed) LParen Identifier(float) Comma Identifier(___STRING_36___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toFixed) LParen Keyword(let) Identifier(number) Comma Keyword(let) Identifier(pattern) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(usings) Plus Identifier(___STRING_37___) Plus Identifier(number) Plus Identifier(___STRING_38___) Plus Identifier(_str) LParen Identifier(pattern) RParen Plus Identifier(___STRING_39___) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(String) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_value) Equals Identifier(___STRING_40___) Semicolon 
	Keyword(function) Identifier(new) LParen Keyword(let) Identifier(input) RParen Colon 
		LBrace 
		Identifier(_value) Equals Identifier(input) Semicolon 
	RBrace 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_value) Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(getStr) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Keyword(return) Identifier(___STRING_41___) Plus Identifier(_str) LParen Identifier(str) RParen Plus Identifier(___STRING_42___) Semicolon 
	RBrace 
	Keyword(function) Identifier(append) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isArray) LParen Identifier(collection) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parceException) LParen Identifier(___STRING_43___) RParen Semicolon 
		RBrace 
		Keyword(let) Identifier(result) Equals Identifier(___STRING_44___) Semicolon 
		Keyword(let) Identifier(arr) Equals Identifier(Array) Dot Identifier(new) LParen Identifier(collection) RParen Semicolon 
		Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___NUMBER_0___) Semicolon 
		Identifier(i) Less Identifier(arr) Dot Identifier(length) LParen RParen Semicolon 
		Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_1___) Colon 
			LBrace 
			Identifier(result) Equals Identifier(result) Plus Identifier(arr) Dot Identifier(at) LParen Identifier(i) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(result) Semicolon 
	RBrace 
	Keyword(function) Identifier(length) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(getStr) LParen Identifier(_value) RParen Plus Identifier(___STRING_45___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isNullOrEmpty) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(___STRING_46___) Plus Identifier(_str) LParen Identifier(str) RParen Plus Identifier(___STRING_47___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isNullOrWhiteSpace) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(___STRING_48___) Plus Identifier(_str) LParen Identifier(str) RParen Plus Identifier(___STRING_49___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trim) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(getStr) LParen Identifier(str) RParen Plus Identifier(___STRING_50___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trimStart) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(getStr) LParen Identifier(_value) RParen Plus Identifier(___STRING_51___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trimEnd) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(getStr) LParen Identifier(_value) RParen Plus Identifier(___STRING_52___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(subString) LParen Keyword(let) Identifier(startPos) Comma Keyword(let) Identifier(length) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(startPos) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_53___) RParen Semicolon 
		RBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(length) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_54___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(getStr) LParen Identifier(_value) RParen Plus Identifier(___STRING_55___) Plus Identifier(startPos) Plus Identifier(___STRING_56___) Plus Identifier(length) Plus Identifier(___STRING_57___) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Console) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(usings) Equals Identifier(___STRING_58___) Semicolon 
	Keyword(private) Keyword(let) Identifier(defFColor) Equals Identifier(___STRING_59___) Semicolon 
	Keyword(private) Keyword(let) Identifier(defBColor) Equals Identifier(___STRING_60___) Semicolon 
	Keyword(function) Identifier(print) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_csharp) LParen Identifier(usings) Plus Identifier(___STRING_61___) Plus Identifier(_str) LParen Identifier(message) RParen Plus Identifier(___STRING_62___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(println) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(print) LParen Identifier(message) Plus Identifier(___STRING_63___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(readln) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_csharp) LParen Identifier(usings) Plus Identifier(___STRING_64___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(clear) LParen RParen Colon 
		LBrace 
		Identifier(_csharp) LParen Identifier(usings) Plus Identifier(___STRING_65___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setCursorPosition) LParen Keyword(let) Identifier(x) Comma Keyword(let) Identifier(y) RParen Colon 
		LBrace 
		Identifier(_csharp) LParen Identifier(usings) Plus Identifier(___STRING_66___) Plus Identifier(x) Plus Identifier(___STRING_67___) Plus Identifier(y) Plus Identifier(___STRING_68___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setForeColor) LParen Keyword(let) Identifier(color) RParen Colon 
		LBrace 
		Keyword(let) Identifier(line1) Equals Identifier(___STRING_69___) Plus Identifier(_str) LParen Identifier(color) RParen Plus Identifier(___STRING_70___) Semicolon 
		Keyword(let) Identifier(line2) Equals Identifier(___STRING_71___) Semicolon 
		Identifier(_csharp) LParen Identifier(usings) Plus Identifier(line1) Plus Identifier(line2) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setBackColor) LParen Keyword(let) Identifier(color) RParen Colon 
		LBrace 
		Keyword(let) Identifier(line1) Equals Identifier(___STRING_72___) Plus Identifier(_str) LParen Identifier(color) RParen Plus Identifier(___STRING_73___) Semicolon 
		Keyword(let) Identifier(line2) Equals Identifier(___STRING_74___) Semicolon 
		Identifier(_csharp) LParen Identifier(usings) Plus Identifier(line1) Plus Identifier(line2) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(resetColors) LParen RParen Colon 
		LBrace 
		Identifier(setForeColor) LParen Identifier(defFColor) RParen Semicolon 
		Identifier(setBackColor) LParen Identifier(defBColor) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Math) Colon 
	LBrace 
	Keyword(private) Keyword(function) Identifier(throwException) LParen Keyword(let) Identifier(num) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(num) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_75___) Plus Identifier(num) Plus Identifier(___STRING_76___) RParen Semicolon 
		RBrace 
	RBrace 
	Keyword(const) Keyword(let) Identifier(PI) Equals Identifier(___NUMBER_2___) Semicolon 
	Keyword(function) Identifier(abs) LParen Keyword(let) Identifier(num) RParen Colon 
		LBrace 
		Identifier(throwException) LParen Identifier(num) RParen Semicolon 
		Keyword(return) Identifier(___NUMBER_3___) Minus Identifier(num) Semicolon 
	RBrace 
	Keyword(function) Identifier(sum) LParen Keyword(let) Identifier(num) Comma Keyword(let) Identifier(num2) RParen Colon 
		LBrace 
		Identifier(throwException) LParen Identifier(num) RParen Semicolon 
		Identifier(throwException) LParen Identifier(num2) RParen Semicolon 
		Keyword(return) Identifier(num) Plus Identifier(num2) Semicolon 
	RBrace 
	Keyword(function) Identifier(sub) LParen Keyword(let) Identifier(num) Comma Keyword(let) Identifier(num2) RParen Colon 
		LBrace 
		Identifier(throwException) LParen Identifier(num) RParen Semicolon 
		Identifier(throwException) LParen Identifier(num2) RParen Semicolon 
		Keyword(return) Identifier(num) Minus Identifier(num2) Semicolon 
	RBrace 
	Keyword(function) Identifier(mult) LParen Keyword(let) Identifier(num) Comma Keyword(let) Identifier(num2) RParen Colon 
		LBrace 
		Identifier(throwException) LParen Identifier(num) RParen Semicolon 
		Identifier(throwException) LParen Identifier(num2) RParen Semicolon 
		Keyword(return) Identifier(num) Star Identifier(num2) Semicolon 
	RBrace 
	Keyword(function) Identifier(div) LParen Keyword(let) Identifier(num) Comma Keyword(let) Identifier(num2) RParen Colon 
		LBrace 
		Identifier(throwException) LParen Identifier(num) RParen Semicolon 
		Identifier(throwException) LParen Identifier(num2) RParen Semicolon 
		Keyword(if) Identifier(num2) Equals Equals Identifier(___NUMBER_4___) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_77___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(num) Slash Identifier(num2) Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Object) Colon 
	LBrace 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(___STRING_78___) Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Throw) Colon 
	LBrace 
	Keyword(function) Identifier(exception) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_csharp) LParen Identifier(___STRING_79___) Plus Identifier(_str) LParen Identifier(message) RParen Plus Identifier(___STRING_80___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(nonImplementException) LParen RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_81___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(parseException) LParen Keyword(let) Identifier(error) RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_82___) Plus Identifier(error) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(LanguageDemo) Colon 
	LBrace 
	Keyword(function) Identifier(run) LParen RParen Colon 
		LBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_83___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_84___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_85___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_86___) RParen Semicolon 
		Keyword(do_while) LParen Identifier(String) Dot Identifier(isNullOrWhiteSpace) LParen Identifier(choice) RParen Equals Equals Keyword(true) RParen Or Or LParen Identifier(Number) Dot Identifier(isNumber) LParen Identifier(choice) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Console) Dot Identifier(print) LParen Identifier(___STRING_87___) RParen Semicolon 
			Keyword(let) Identifier(choice) Equals Identifier(Console) Dot Identifier(readln) LParen RParen Semicolon 
			Keyword(if) Identifier(String) Dot Identifier(isNullOrWhiteSpace) LParen Identifier(choice) RParen Equals Equals Keyword(true) Colon 
				LBrace 
				Identifier(printException) LParen Identifier(___STRING_88___) RParen Semicolon 
			RBrace 
			Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(choice) RParen Equals Equals Keyword(false) Colon 
				LBrace 
				Identifier(printException) LParen Identifier(___STRING_89___) RParen Semicolon 
			RBrace 
		RBrace 
		Keyword(if) Identifier(choice) Equals Equals Identifier(___STRING_90___) Colon 
			LBrace 
			Identifier(numericDemo) LParen RParen Semicolon 
		RBrace 
		Keyword(else) Keyword(if) Identifier(choice) Equals Equals Identifier(___STRING_91___) Colon 
			LBrace 
			Identifier(stringDemo) LParen RParen Semicolon 
		RBrace 
		Keyword(else) Keyword(if) Identifier(choice) Equals Equals Identifier(___STRING_92___) Colon 
			LBrace 
			Identifier(circleDemo) LParen RParen Semicolon 
		RBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_93___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(stringDemo) LParen RParen Colon 
		LBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_94___) RParen Semicolon 
		Keyword(do_while) Identifier(String) Dot Identifier(isNullOrWhiteSpace) LParen Identifier(input) RParen Equals Equals Keyword(true) Colon 
			LBrace 
			Identifier(Console) Dot Identifier(print) LParen Identifier(___STRING_95___) RParen Semicolon 
			Keyword(let) Identifier(input) Equals Identifier(Console) Dot Identifier(readln) LParen RParen Semicolon 
			Keyword(if) Identifier(String) Dot Identifier(isNullOrWhiteSpace) LParen Identifier(input) RParen Equals Equals Keyword(true) Colon 
				LBrace 
				Identifier(printException) LParen Identifier(___STRING_96___) RParen Semicolon 
			RBrace 
		RBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_97___) Plus Identifier(String) Dot Identifier(getLength) LParen Identifier(input) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_98___) Plus Identifier(String) Dot Identifier(trim) LParen Identifier(input) RParen Plus Identifier(___STRING_99___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_100___) Plus Identifier(String) Dot Identifier(trimStart) LParen Identifier(input) RParen Plus Identifier(___STRING_101___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_102___) Plus Identifier(String) Dot Identifier(trimEnd) LParen Identifier(input) RParen Plus Identifier(___STRING_103___) RParen Semicolon 
		Keyword(if) Identifier(String) Dot Identifier(getLength) LParen Identifier(input) RParen Greater Identifier(___NUMBER_5___) Colon 
			LBrace 
			Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_104___) Plus Identifier(String) Dot Identifier(subString) LParen Identifier(input) Comma Identifier(___NUMBER_6___) Comma Identifier(___NUMBER_7___) RParen Plus Identifier(___STRING_105___) RParen Semicolon 
		RBrace 
	RBrace 
	Keyword(function) Identifier(numericDemo) LParen RParen Colon 
		LBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_106___) RParen Semicolon 
		Keyword(let) Identifier(num1) Equals Identifier(getNumberFromConsole) LParen Identifier(___STRING_107___) RParen Semicolon 
		Keyword(let) Identifier(num2) Equals Identifier(getNumberFromConsole) LParen Identifier(___STRING_108___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_109___) Plus Identifier(num1) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_110___) Plus Identifier(num2) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_111___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_112___) Plus Identifier(Math) Dot Identifier(sum) LParen Identifier(num1) Comma Identifier(num2) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_113___) Plus Identifier(Math) Dot Identifier(sub) LParen Identifier(num1) Comma Identifier(num2) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_114___) Plus Identifier(Math) Dot Identifier(mult) LParen Identifier(num1) Comma Identifier(num2) RParen RParen Semicolon 
		Keyword(if) Identifier(num2) Not Equals Identifier(___NUMBER_8___) Colon 
			LBrace 
			Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_115___) Plus Identifier(Number) Dot Identifier(toFixed) LParen Identifier(Math) Dot Identifier(div) LParen Identifier(num1) Comma Identifier(num2) RParen Comma Identifier(___STRING_116___) RParen RParen Semicolon 
		RBrace 
		Keyword(else) Colon 
			LBrace 
			Identifier(printException) LParen Identifier(___STRING_117___) RParen Semicolon 
		RBrace 
		Keyword(if) Identifier(num1) Less Identifier(num2) Colon 
			LBrace 
			Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_118___) Plus Identifier(num1) Plus Identifier(___STRING_119___) Plus Identifier(num2) Plus Identifier(___STRING_120___) Plus Identifier(Number) Dot Identifier(randInt) LParen Identifier(num1) Comma Identifier(num2) RParen RParen Semicolon 
		RBrace 
		Keyword(else) Colon 
			LBrace 
			Identifier(printException) LParen Identifier(___STRING_121___) RParen Semicolon 
		RBrace 
		Keyword(for) Keyword(let) Identifier(i) Equals Identifier(num1) Minus Identifier(___NUMBER_9___) Semicolon 
		Identifier(i) Less Identifier(num1) Semicolon 
		Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_10___) Colon 
			LBrace 
			Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_122___) Plus Identifier(i) RParen Semicolon 
		RBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_123___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_124___) Plus Identifier(Number) Dot Identifier(toInt) LParen Identifier(num1) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_125___) Plus Identifier(Number) Dot Identifier(toInt) LParen Identifier(num2) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_126___) Plus Identifier(Number) Dot Identifier(toFixed) LParen Identifier(num1) Comma Identifier(___STRING_127___) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_128___) Plus Identifier(Number) Dot Identifier(toFixed) LParen Identifier(num2) Comma Identifier(___STRING_129___) RParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_130___) Plus Identifier(Math) Dot Identifier(PI) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_131___) Plus Identifier(Number) Dot Identifier(MIN_VALUE) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_132___) Plus Identifier(Number) Dot Identifier(MAX_VALUE) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(circleDemo) LParen RParen Colon 
		LBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_133___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_134___) RParen Semicolon 
		Identifier(Console) Dot Identifier(print) LParen Identifier(___STRING_135___) RParen Semicolon 
		Keyword(let) Identifier(randNum) Equals Identifier(___NUMBER_11___) Minus Identifier(___NUMBER_12___) Semicolon 
		Keyword(while) Identifier(randNum) Not Equals Identifier(___NUMBER_13___) Colon 
			LBrace 
			Identifier(randNum) Equals Identifier(Number) Dot Identifier(randInt) LParen Identifier(___NUMBER_14___) Comma Identifier(___NUMBER_15___) RParen Semicolon 
			Identifier(Console) Dot Identifier(print) LParen Identifier(randNum) Plus Identifier(___STRING_136___) RParen Semicolon 
		RBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_137___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_138___) RParen Semicolon 
		Identifier(acceptContinue) LParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_139___) RParen Semicolon 
		Keyword(do_while) Identifier(input) Not Equals Identifier(___STRING_140___) Colon 
			LBrace 
			Identifier(Console) Dot Identifier(print) LParen Identifier(___STRING_141___) RParen Semicolon 
			Keyword(let) Identifier(input) Equals Identifier(Console) Dot Identifier(readln) LParen RParen Semicolon 
		RBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_142___) RParen Semicolon 
		Identifier(acceptContinue) LParen RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_143___) RParen Semicolon 
		Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___NUMBER_16___) Semicolon 
		Identifier(i) Less Identifier(___NUMBER_17___) Semicolon 
		Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_18___) Colon 
			LBrace 
			Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_144___) Plus Identifier(i) RParen Semicolon 
		RBrace 
	RBrace 
	Keyword(function) Identifier(acceptContinue) LParen RParen Colon 
		LBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_145___) RParen Semicolon 
		Identifier(Console) Dot Identifier(readln) LParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(printException) LParen Keyword(let) Identifier(msg) RParen Colon 
		LBrace 
		Identifier(Console) Dot Identifier(setForeColor) LParen Identifier(___STRING_146___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(msg) RParen Semicolon 
		Identifier(Console) Dot Identifier(resetColors) LParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(getNumberFromConsole) LParen Keyword(let) Identifier(msg) RParen Colon 
		LBrace 
		Keyword(let) Identifier(num) Equals Identifier(___STRING_147___) Semicolon 
		Keyword(do_while) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(num) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Console) Dot Identifier(print) LParen Identifier(msg) RParen Semicolon 
			Identifier(num) Equals Identifier(Console) Dot Identifier(readln) LParen RParen Semicolon 
			Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(num) RParen Equals Equals Keyword(false) Colon 
				LBrace 
				Identifier(printException) LParen Identifier(___STRING_148___) RParen Semicolon 
			RBrace 
		RBrace 
		Keyword(return) Identifier(num) Semicolon 
	RBrace 
RBrace 
Keyword(function) Identifier(main) LParen RParen Colon 
	LBrace 
	Keyword(let) Identifier(arr) Equals Identifier(Array) Dot Identifier(new) LParen LSquareParen Identifier(___STRING_149___) Comma Identifier(___NUMBER_19___) RSquareParen RParen Semicolon 
	Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___NUMBER_20___) Semicolon 
	Identifier(i) Less Identifier(___NUMBER_21___) Semicolon 
	Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_22___) Colon 
		LBrace 
		Identifier(Console) Dot Identifier(println) LParen Identifier(arr) Dot Identifier(at) LParen Identifier(i) RParen RParen Semicolon 
	RBrace 
	Identifier(Console) Dot Identifier(println) LParen Identifier(String) Dot Identifier(append) LParen LSquareParen Identifier(___STRING_150___) Comma Identifier(___STRING_151___) Comma Identifier(___STRING_152___) RSquareParen RParen RParen Semicolon 
RBrace 
