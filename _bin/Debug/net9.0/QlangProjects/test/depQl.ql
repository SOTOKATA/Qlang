class Dep: {
    function isArgs(const args): {
        if (typeof(args) != typeof(Array)):
            return false;
        
        if (args.length() == 0):
            return false;
    
        return true;
    }

    function opArgs(const args): {
        
    }
}