// Base class for types like String, Array
// This class adds value and basic functions for aritmetic and rational operations
// For primitive values only
class DataType extends Object: {
    // '_value' means primitive value
    private let _value;

    // Get current value
    function getValue() => _value;

    // Cast object to DataType
    function _cast(const object): std::throw.notImplementedException();

    // Create new DataType instance
    function _createFrom(const object): std::throw.notImplementedException();

    // Aritmetic operation addition
    function _operatorAddition(): std::throw.notImplementedException();

    // Aritmetic operation subtraction
    function _operatorSubtraction(): std::throw.notImplementedException();

    // Aritmetic operation multiplication
    function _operatorMultiplication(): std::throw.notImplementedException();

    // Aritmetic operation division
    function _operatorDivision(): std::throw.notImplementedException();

    // Rational operation equal
    function _operatorEqual(): std::throw.notImplementedException();

    // Rational operation not equal
    function _operatorNotEqual(): std::throw.notImplementedException();

    // Rational operation less or equal
    function _operatorLessOrEqual(): std::throw.notImplementedException();

    // Rational operation less
    function _operatorLess(): std::throw.notImplementedException();
    
    // Rational operation greater or equal
    function _operatorGreaterOrEqual(): std::throw.notImplementedException();
    
    // Rational operation greater
    function _operatorGreater(): std::throw.notImplementedException();
}