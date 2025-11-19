class File:
    private let usings = "using System.IO; "

    function exists(let message):   
        return _csharp(usings + "File.Exists(" + _str(message) + ")")

    function create(let path):   
        Throw.nonImplementException()

    function remove(let path):   
        Throw.nonImplementException()

    function getContent(let path):   
        Throw.nonImplementException()

    function setContent(let path, let message):   
        Throw.nonImplementException()