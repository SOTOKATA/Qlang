// Base class for types like String, Array
// This class adds value and basic functions for aritmetic and rational operations
class DataType extends Object: {
    // '_value' means primitive value
    private let _value;

    // Get current value
    function getValue(): return _value;

    // Cast object to DataType
    function _cast(const object): std::Throw.notImplementedException();

    // Create new DataType instance
    function _createFrom(const object): std::Throw.notImplementedException();

    // Aritmetic operation addition
    function _operatorAddition(): std::Throw.notImplementedException();

    // Aritmetic operation subtraction
    function _operatorSubtraction(): std::Throw.notImplementedException();

    // Aritmetic operation multiplication
    function _operatorMultiplication(): std::Throw.notImplementedException();

    // Aritmetic operation division
    function _operatorDivision(): std::Throw.notImplementedException();

    // Rational operation equal
    function _operatorEqual(): std::Throw.notImplementedException();

    // Rational operation not equal
    function _operatorNotEqual(): std::Throw.notImplementedException();

    // Rational operation less or equal
    function _operatorLessOrEqual(): std::Throw.notImplementedException();

    // Rational operation less
    function _operatorLess(): std::Throw.notImplementedException();
    
    // Rational operation greater or equal
    function _operatorGreaterOrEqual(): std::Throw.notImplementedException();
    
    // Rational operation greater
    function _operatorGreater(): std::Throw.notImplementedException();
}