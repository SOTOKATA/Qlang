Keyword(class) Identifier(Array) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_value) Equals Identifier(_native) LParen Identifier(___STRING_0___) RParen Semicolon 
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
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_2___) Comma Identifier(collection) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(push) LParen Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_3___) Comma Identifier(_value) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(clear) LParen RParen Colon 
		LBrace 
		Identifier(_value) Equals Identifier(_native) LParen Identifier(___STRING_4___) Comma Identifier(_value) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(at) LParen Keyword(let) Identifier(index) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toInt) LParen Identifier(index) RParen Semicolon 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_5___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_6___) Comma Identifier(_value) Comma Identifier(index) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setAt) LParen Keyword(let) Identifier(index) Comma Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_7___) RParen Semicolon 
		RBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toInt) LParen Identifier(index) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_8___) Comma Identifier(_value) Comma Identifier(index) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(insert) LParen Keyword(let) Identifier(index) Comma Keyword(let) Identifier(item) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_9___) RParen Semicolon 
		RBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toInt) LParen Identifier(index) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_10___) Comma Identifier(_value) Comma Identifier(index) Comma Identifier(item) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(removeAt) LParen Keyword(let) Identifier(index) RParen Colon 
		LBrace 
		Identifier(index) Equals Identifier(Number) Dot Identifier(toInt) LParen Identifier(index) RParen Semicolon 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(index) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_11___) RParen Semicolon 
		RBrace 
		Identifier(_native) LParen Identifier(___STRING_12___) Comma Identifier(_value) Comma Identifier(index) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(length) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_13___) Comma Identifier(_value) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Number) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(usings) Equals Identifier(___STRING_14___) Semicolon 
	Keyword(const) Keyword(let) Identifier(MIN_VALUE) Equals Identifier(___STRING_15___) Semicolon 
	Keyword(const) Keyword(let) Identifier(MAX_VALUE) Equals Identifier(___STRING_16___) Semicolon 
	Keyword(function) Identifier(isNumber) LParen Keyword(let) Identifier(var) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_17___) Comma Identifier(var) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(randInt) LParen Keyword(let) Identifier(min) Comma Keyword(let) Identifier(max) RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(isNumber) LParen Identifier(min) RParen Equals Equals Keyword(false) RParen Or Or LParen Identifier(isNumber) LParen Identifier(max) RParen Equals Equals Keyword(false) RParen Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_18___) RParen Semicolon 
		RBrace 
		Identifier(min) Equals Identifier(toInt) LParen Identifier(min) RParen Semicolon 
		Identifier(max) Equals Identifier(toInt) LParen Identifier(max) RParen Semicolon 
		Keyword(if) Identifier(min) Greater Equals Identifier(max) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_19___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_20___) Comma Identifier(min) Comma Identifier(max) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toInt) LParen Keyword(let) Identifier(float) RParen Colon 
		LBrace 
		Keyword(return) Identifier(toFixed) LParen Identifier(float) Comma Identifier(___STRING_21___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(toFixed) LParen Keyword(let) Identifier(number) Comma Keyword(let) Identifier(pattern) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_22___) Comma Identifier(number) Comma Identifier(_str) LParen Identifier(pattern) RParen RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(String) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(_value) Equals Identifier(___STRING_23___) Semicolon 
	Keyword(function) Identifier(new) LParen Keyword(let) Identifier(input) RParen Colon 
		LBrace 
		Identifier(_value) Equals Identifier(input) Semicolon 
	RBrace 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_value) Semicolon 
	RBrace 
	Keyword(private) Keyword(function) Identifier(_str) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Keyword(return) Identifier(___STRING_24___) Plus Identifier(_str) LParen Identifier(str) RParen Plus Identifier(___STRING_25___) Semicolon 
	RBrace 
	Keyword(function) Identifier(append) LParen Keyword(let) Identifier(collection) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Array) Dot Identifier(isArray) LParen Identifier(collection) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parceException) LParen Identifier(___STRING_26___) RParen Semicolon 
		RBrace 
		Keyword(let) Identifier(result) Equals Identifier(___STRING_27___) Semicolon 
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
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_28___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isNullOrEmpty) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_29___) Comma Identifier(_str) LParen Identifier(str) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isNullOrWhiteSpace) LParen Keyword(let) Identifier(str) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_30___) Comma Identifier(_str) LParen Identifier(str) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trim) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_31___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trimStart) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_32___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(trimEnd) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_33___) Comma Identifier(_str) LParen Identifier(_value) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(subString) LParen Keyword(let) Identifier(startPos) Comma Keyword(let) Identifier(length) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(startPos) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_34___) RParen Semicolon 
		RBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(length) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_35___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_36___) Comma Identifier(_str) LParen Identifier(_value) RParen Comma Identifier(startPos) Comma Identifier(length) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Time) Colon 
	LBrace 
	Keyword(function) Identifier(wait) LParen Keyword(let) Identifier(millisec) RParen Colon 
		LBrace 
		Keyword(if) Identifier(Number) Dot Identifier(isNumber) LParen Identifier(millisec) RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(Throw) Dot Identifier(parseException) LParen Identifier(___STRING_37___) RParen Semicolon 
		RBrace 
		Identifier(millisec) Equals Identifier(Number) Dot Identifier(toInt) LParen Identifier(millisec) RParen Semicolon 
		Identifier(_native) LParen Identifier(___STRING_38___) Comma Identifier(millisec) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Console) Colon 
	LBrace 
	Keyword(private) Keyword(let) Identifier(usings) Equals Identifier(___STRING_39___) Semicolon 
	Keyword(private) Keyword(let) Identifier(defFColor) Equals Identifier(___STRING_40___) Semicolon 
	Keyword(private) Keyword(let) Identifier(defBColor) Equals Identifier(___STRING_41___) Semicolon 
	Keyword(function) Identifier(print) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_42___) Comma Identifier(_str) LParen Identifier(message) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(println) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(print) LParen Identifier(message) Plus Identifier(___STRING_43___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(readln) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_44___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(readkey) LParen Keyword(let) Identifier(intercept) RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_45___) Comma Identifier(intercept) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(isKeyAvailable) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(_native) LParen Identifier(___STRING_46___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(cursorVisible) LParen Keyword(let) Identifier(visible) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_47___) Comma Identifier(visible) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(clear) LParen RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_48___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setCursorPosition) LParen Keyword(let) Identifier(x) Comma Keyword(let) Identifier(y) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_49___) Comma Identifier(x) Comma Identifier(y) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setForeColor) LParen Keyword(let) Identifier(color) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_50___) Comma Identifier(color) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(setBackColor) LParen Keyword(let) Identifier(color) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_51___) Comma Identifier(color) RParen Semicolon 
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
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_52___) Plus Identifier(num) Plus Identifier(___STRING_53___) RParen Semicolon 
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
			Identifier(Throw) Dot Identifier(exception) LParen Identifier(___STRING_54___) RParen Semicolon 
		RBrace 
		Keyword(return) Identifier(num) Slash Identifier(num2) Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Object) Colon 
	LBrace 
	Keyword(function) Identifier(toString) LParen RParen Colon 
		LBrace 
		Keyword(return) Identifier(___STRING_55___) Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(Throw) Colon 
	LBrace 
	Keyword(function) Identifier(exception) LParen Keyword(let) Identifier(message) RParen Colon 
		LBrace 
		Identifier(_native) LParen Identifier(___STRING_56___) Comma Identifier(_str) LParen Identifier(message) RParen RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(nonImplementException) LParen RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_57___) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(parseException) LParen Keyword(let) Identifier(error) RParen Colon 
		LBrace 
		Identifier(exception) LParen Identifier(___STRING_58___) Plus Identifier(error) RParen Semicolon 
	RBrace 
RBrace 
Keyword(class) Identifier(SnakeGame) Colon 
	LBrace 
	Keyword(let) Identifier(X) Equals Identifier(___NUMBER_5___) Semicolon 
	Keyword(let) Identifier(Y) Equals Identifier(___NUMBER_6___) Semicolon 
	Keyword(let) Identifier(tailX) Equals Identifier(Array) Dot Identifier(new) LParen LSquareParen RSquareParen RParen Semicolon 
	Keyword(let) Identifier(tailY) Equals Identifier(Array) Dot Identifier(new) LParen LSquareParen RSquareParen RParen Semicolon 
	Keyword(let) Identifier(tailLength) Equals Identifier(___NUMBER_7___) Semicolon 
	Keyword(let) Identifier(lastChar) Equals Identifier(___STRING_59___) Semicolon 
	Keyword(let) Identifier(fruitX) Equals Identifier(___NUMBER_8___) Semicolon 
	Keyword(let) Identifier(fruitY) Equals Identifier(___NUMBER_9___) Semicolon 
	Keyword(let) Identifier(width) Equals Identifier(___NUMBER_10___) Semicolon 
	Keyword(let) Identifier(height) Equals Identifier(___NUMBER_11___) Semicolon 
	Keyword(let) Identifier(score) Equals Identifier(___NUMBER_12___) Semicolon 
	Keyword(let) Identifier(gameOver) Equals Keyword(false) Semicolon 
	Keyword(function) Identifier(setup) LParen RParen Colon 
		LBrace 
		Identifier(Console) Dot Identifier(clear) LParen RParen Semicolon 
		Identifier(Console) Dot Identifier(cursorVisible) LParen Keyword(false) RParen Semicolon 
		Identifier(fruitX) Equals Identifier(Number) Dot Identifier(randInt) LParen Identifier(___NUMBER_13___) Comma Identifier(width) RParen Semicolon 
		Identifier(fruitY) Equals Identifier(Number) Dot Identifier(randInt) LParen Identifier(___NUMBER_14___) Comma Identifier(height) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(draw) LParen RParen Colon 
		LBrace 
		Identifier(Console) Dot Identifier(setCursorPosition) LParen Identifier(___NUMBER_15___) Comma Identifier(___NUMBER_16___) RParen Semicolon 
		Identifier(Console) Dot Identifier(println) LParen Identifier(___STRING_60___) Plus Identifier(score) RParen Semicolon 
		Keyword(for) Keyword(let) Identifier(j) Equals Identifier(___NUMBER_17___) Semicolon 
		Identifier(j) Less LParen Identifier(height) Plus Identifier(___NUMBER_18___) RParen Semicolon 
		Identifier(j) Equals Identifier(j) Plus Identifier(___NUMBER_19___) Colon 
			LBrace 
			Keyword(for) Keyword(let) Identifier(i) Equals Identifier(___NUMBER_20___) Semicolon 
			Identifier(i) Less LParen Identifier(width) Plus Identifier(___NUMBER_21___) RParen Semicolon 
			Identifier(i) Equals Identifier(i) Plus Identifier(___NUMBER_22___) Colon 
				LBrace 
				Keyword(let) Identifier(isTailPosition) Equals Keyword(false) Semicolon 
				Keyword(for) Keyword(let) Identifier(tX) Equals Identifier(___NUMBER_23___) Semicolon 
				Identifier(tX) Less Identifier(tailX) Dot Identifier(length) LParen RParen Semicolon 
				Identifier(tX) Equals Identifier(tX) Plus Identifier(___NUMBER_24___) Colon 
					LBrace 
					Keyword(let) Identifier(x) Equals Identifier(tailX) Dot Identifier(at) LParen Identifier(tX) RParen Semicolon 
					Keyword(for) Keyword(let) Identifier(tY) Equals Identifier(___NUMBER_25___) Semicolon 
					Identifier(tY) Less Identifier(tailY) Dot Identifier(length) LParen RParen Semicolon 
					Identifier(tY) Equals Identifier(tY) Plus Identifier(___NUMBER_26___) Colon 
						LBrace 
						Keyword(let) Identifier(y) Equals Identifier(tailY) Dot Identifier(at) LParen Identifier(tY) RParen Semicolon 
						Keyword(if) LParen Identifier(x) Equals Equals Identifier(i) RParen And And LParen Identifier(y) Equals Equals Identifier(j) RParen Colon 
							LBrace 
							Identifier(isTailPosition) Equals Keyword(true) Semicolon 
						RBrace 
					RBrace 
				RBrace 
				Identifier(Console) Dot Identifier(setCursorPosition) LParen Identifier(i) Comma LParen Identifier(j) Plus Identifier(___NUMBER_27___) RParen RParen Semicolon 
				Keyword(if) LParen Identifier(i) Equals Equals Identifier(X) RParen And And LParen Identifier(j) Equals Equals Identifier(Y) RParen Colon 
					LBrace 
					Identifier(Console) Dot Identifier(print) LParen Identifier(___STRING_61___) RParen Semicolon 
				RBrace 
				Keyword(else) Keyword(if) LParen Identifier(i) Equals Equals Identifier(fruitX) RParen And And LParen Identifier(j) Equals Equals Identifier(fruitY) RParen Colon 
					LBrace 
					Identifier(Console) Dot Identifier(print) LParen Identifier(___STRING_62___) RParen Semicolon 
				RBrace 
				Keyword(else) Keyword(if) LParen Identifier(i) Equals Equals Identifier(___NUMBER_28___) RParen Or Or LParen Identifier(j) Equals Equals Identifier(___NUMBER_29___) RParen Or Or LParen Identifier(i) Equals Equals Identifier(width) RParen Or Or LParen Identifier(j) Equals Equals Identifier(height) RParen Colon 
					LBrace 
					Identifier(Console) Dot Identifier(print) LParen Identifier(___STRING_63___) RParen Semicolon 
				RBrace 
				Keyword(else) Keyword(if) Identifier(isTailPosition) Equals Equals Keyword(true) Colon 
					LBrace 
					Identifier(Console) Dot Identifier(print) LParen Identifier(___STRING_64___) RParen Semicolon 
				RBrace 
				Keyword(else) Colon 
					LBrace 
					Identifier(Console) Dot Identifier(print) LParen Identifier(___STRING_65___) RParen Semicolon 
				RBrace 
			RBrace 
		RBrace 
	RBrace 
	Keyword(function) Identifier(logic) LParen RParen Colon 
		LBrace 
		Keyword(if) LParen Identifier(X) Equals Equals Identifier(fruitX) RParen And And LParen Identifier(Y) Equals Equals Identifier(fruitY) RParen Colon 
			LBrace 
			Identifier(score) Equals Identifier(score) Plus Identifier(___NUMBER_30___) Semicolon 
			Identifier(tailLength) Equals Identifier(tailLength) Plus Identifier(___NUMBER_31___) Semicolon 
			Identifier(fruitX) Equals Identifier(Number) Dot Identifier(randInt) LParen Identifier(___NUMBER_32___) Comma Identifier(width) RParen Semicolon 
			Identifier(fruitY) Equals Identifier(Number) Dot Identifier(randInt) LParen Identifier(___NUMBER_33___) Comma Identifier(height) RParen Semicolon 
		RBrace 
		Identifier(tailX) Dot Identifier(insert) LParen Identifier(___NUMBER_34___) Comma Identifier(X) RParen Semicolon 
		Identifier(tailY) Dot Identifier(insert) LParen Identifier(___NUMBER_35___) Comma Identifier(Y) RParen Semicolon 
		Keyword(if) Identifier(tailX) Dot Identifier(length) LParen RParen Greater Identifier(tailLength) Colon 
			LBrace 
			Identifier(tailX) Dot Identifier(removeAt) LParen Identifier(tailX) Dot Identifier(length) LParen RParen Minus Identifier(___NUMBER_36___) RParen Semicolon 
			Identifier(tailY) Dot Identifier(removeAt) LParen Identifier(tailX) Dot Identifier(length) LParen RParen Minus Identifier(___NUMBER_37___) RParen Semicolon 
		RBrace 
		Identifier(input) LParen RParen Semicolon 
		Keyword(if) Identifier(X) Less Identifier(___NUMBER_38___) Colon 
			LBrace 
			Identifier(X) Equals Identifier(width) Minus Identifier(___NUMBER_39___) Semicolon 
		RBrace 
		Keyword(else) Keyword(if) Identifier(X) Greater LParen Identifier(width) Minus Identifier(___NUMBER_40___) RParen Colon 
			LBrace 
			Identifier(X) Equals Identifier(___NUMBER_41___) Semicolon 
		RBrace 
		Keyword(if) Identifier(Y) Less Identifier(___NUMBER_42___) Colon 
			LBrace 
			Identifier(Y) Equals Identifier(height) Minus Identifier(___NUMBER_43___) Semicolon 
		RBrace 
		Keyword(else) Keyword(if) Identifier(Y) Greater LParen Identifier(height) Minus Identifier(___NUMBER_44___) RParen Colon 
			LBrace 
			Identifier(Y) Equals Identifier(___NUMBER_45___) Semicolon 
		RBrace 
	RBrace 
	Keyword(function) Identifier(input) LParen RParen Colon 
		LBrace 
		Keyword(if) Identifier(Console) Dot Identifier(isKeyAvailable) LParen RParen Equals Equals Keyword(false) Colon 
			LBrace 
			Keyword(if) Identifier(lastChar) Not Equals Identifier(___STRING_66___) Colon 
				LBrace 
				Identifier(control) LParen Identifier(lastChar) RParen Semicolon 
			RBrace 
			Keyword(return) Identifier(___STRING_67___) Semicolon 
		RBrace 
		Identifier(lastChar) Equals Identifier(Console) Dot Identifier(readkey) LParen Keyword(true) RParen Semicolon 
	RBrace 
	Keyword(function) Identifier(control) LParen Keyword(let) Identifier(char) RParen Colon 
		LBrace 
		Keyword(if) Identifier(char) Equals Equals Identifier(___STRING_68___) Colon 
			LBrace 
			Identifier(X) Equals Identifier(X) Minus Identifier(___NUMBER_46___) Semicolon 
		RBrace 
		Keyword(else) Keyword(if) Identifier(char) Equals Equals Identifier(___STRING_69___) Colon 
			LBrace 
			Identifier(X) Equals Identifier(X) Plus Identifier(___NUMBER_47___) Semicolon 
		RBrace 
		Keyword(else) Keyword(if) Identifier(char) Equals Equals Identifier(___STRING_70___) Colon 
			LBrace 
			Identifier(Y) Equals Identifier(Y) Minus Identifier(___NUMBER_48___) Semicolon 
		RBrace 
		Keyword(else) Keyword(if) Identifier(char) Equals Equals Identifier(___STRING_71___) Colon 
			LBrace 
			Identifier(Y) Equals Identifier(Y) Plus Identifier(___NUMBER_49___) Semicolon 
		RBrace 
	RBrace 
	Keyword(function) Identifier(run) LParen RParen Colon 
		LBrace 
		Identifier(setup) LParen RParen Semicolon 
		Keyword(while) Identifier(gameOver) Equals Equals Keyword(false) Colon 
			LBrace 
			Identifier(logic) LParen RParen Semicolon 
			Identifier(draw) LParen RParen Semicolon 
			Identifier(Time) Dot Identifier(wait) LParen Identifier(___NUMBER_50___) RParen Semicolon 
		RBrace 
	RBrace 
RBrace 
Keyword(function) Identifier(main) LParen RParen Colon 
	LBrace 
	Identifier(SnakeGame) Dot Identifier(run) LParen RParen Semicolon 
RBrace 
