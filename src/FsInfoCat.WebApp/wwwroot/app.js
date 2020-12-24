var rootBroadcaster;
(function (rootBroadcaster) {
    rootBroadcaster.SERVICE_NAME = "rootBroadcaster";
    const EVENTNAME_ROUTECHANGESUCCESS = "$routeChangeSuccess";
    function register(module) { return module.service(rootBroadcaster.SERVICE_NAME, ['$rootScope', Service]); }
    rootBroadcaster.register = register;
    class Service {
        constructor($rootScope) {
            this.$rootScope = $rootScope;
            this._eventRegistration = [];
            this[Symbol.toStringTag] = rootBroadcaster.SERVICE_NAME;
        }
        registerPrivateEventName(name, id) {
            if (typeof name !== "string")
                throw new Error("Name must be a string");
            if (name.length == 0)
                throw new Error("Name cannot be empty");
            if (name.startsWith("$"))
                throw new Error("Name cannot start with a \"$\" symbol");
            for (let i = 0; i < this._eventRegistration.length; i++) {
                if (this._eventRegistration[i].name === name) {
                    if (typeof id === "symbol" && id === this._eventRegistration[i].id)
                        return id;
                    throw new Error("That event name is already registered");
                }
            }
            let eventRegistration = { id: Symbol(), name: name };
            this._eventRegistration.push(eventRegistration);
            return eventRegistration.id;
        }
        registerSharedEventName(name) {
            if (typeof name !== "string")
                throw new Error("Name must be a string");
            if (name.length == 0)
                throw new Error("Name cannot be empty");
            if (name.startsWith("$"))
                throw new Error("Name cannot start with a \"$\" symbol");
            for (let i = 0; i < this._eventRegistration.length; i++) {
                if (this._eventRegistration[i].name === name) {
                    if (typeof this._eventRegistration[i].id === "symbol")
                        throw new Error("That event name is not available");
                    return false;
                }
            }
            let eventRegistration = { name: name };
            this._eventRegistration.push(eventRegistration);
            return true;
        }
        broadcastEvent(id, ...args) {
            let name;
            if (typeof id === "symbol")
                for (let i = 0; i < this._eventRegistration.length; i++) {
                    if (this._eventRegistration[i].id === id) {
                        name = this._eventRegistration[i].name;
                        break;
                    }
                }
            else if (typeof id === "string")
                for (let i = 0; i < this._eventRegistration.length; i++) {
                    if (this._eventRegistration[i].name === name) {
                        if (typeof this._eventRegistration[i].id !== "symbol")
                            name = this._eventRegistration[i].name;
                        break;
                    }
                }
            if (typeof name !== "string")
                throw new Error((typeof id === "symbol") ? "Event ID not registered" : "Event name not registered or is private");
            if (typeof args === "object" && args !== null)
                this.$rootScope.$broadcast(name, args);
            else
                this.$rootScope.$broadcast(name, []);
        }
        onEvent($scope, id, listener, thisArg) {
            let name;
            if (typeof id === "symbol")
                for (let i = 0; i < this._eventRegistration.length; i++) {
                    if (this._eventRegistration[i].id === id) {
                        name = this._eventRegistration[i].name;
                        break;
                    }
                }
            else if (typeof id === "string")
                for (let i = 0; i < this._eventRegistration.length; i++) {
                    if (this._eventRegistration[i].name === name) {
                        if (typeof this._eventRegistration[i].id !== "symbol")
                            name = this._eventRegistration[i].name;
                        break;
                    }
                }
            if (typeof name !== "string")
                throw new Error((typeof id === "symbol") ? "Event ID not registered" : "Event name not registered or is private");
            if (arguments.length > 3)
                $scope.$on(name, function (event, args) {
                    if (args.length == 1)
                        listener.call(thisArg, event, args[0]);
                    else if (args.length > 1)
                        listener.apply(thisArg, (args.length == 0) ? [event] : [event].concat(args));
                    else
                        listener.call(thisArg, event);
                });
            else
                $scope.$on(name, function (event, args) {
                    if (args.length == 1)
                        listener(event, args[0]);
                    else if (args.length > 1)
                        listener.apply(this, [event].concat(args));
                    else
                        listener(event);
                });
        }
        onRouteChangeSuccess(callback, thisArg) {
            if (arguments.length > 1)
                this.$rootScope.$on(EVENTNAME_ROUTECHANGESUCCESS, (event, current, previous) => {
                    callback.call(thisArg, event, current, previous);
                });
            else
                this.$rootScope.$on(EVENTNAME_ROUTECHANGESUCCESS, (event, current, previous) => {
                    callback(event, current, previous);
                });
        }
    }
    rootBroadcaster.Service = Service;
})(rootBroadcaster || (rootBroadcaster = {}));
var app;
(function (app) {
    app.MAIN_MODULE_NAME = "mainModule";
    app.MAIN_CONTROLLER_NAME = "mainController";
    app.MAIN_NAV_SERVICE_NAME = "mainNavigationService";
    app.MAIN_NAV_PROVIDER_NAME = app.MAIN_NAV_SERVICE_NAME + "Provider";
    app.whitespaceRe = /[\s\r\n]+/g;
    app.isTrueRe = /^(t(rue)?|y(es)?|1)$/g;
    app.isFalseRe = /^(f(alse)?|no?|o)$/g;
    app.trueFalseRe = /^((t(?:rue)?|y(?:es)?|1)|(f(?:alse)?|no?|0))$/g;
    const trimRightRe = /^((\s*\S+)(\s+\S+)*)\s*$/;
    const trimLeftRe = /^\s*(\S.*)$/;
    const falseStringRe = /^(f(alse)?|no?|0+(\.0+)?)([^\w-]|$)/i;
    const numberStringRe = /^\d+(\.\d+)$/i;
    /**
     * Determines if a value is null or undefined.
     * @param {*} value Value to test.
     * @returns {boolean} true if value was null or undefined; otherwise, false.
     */
    function isNil(value) { return typeof (value) === 'undefined' || value === null; }
    app.isNil = isNil;
    function isNilOrEmpty(value) {
        return (typeof (value) !== 'string' && (typeof (value) != 'object' || value === null || !Array.isArray(value))) || value.length == 0;
    }
    app.isNilOrEmpty = isNilOrEmpty;
    function isNilOrWhiteSpace(value) { return typeof (value) !== 'string' || value.trim().length == 0; }
    app.isNilOrWhiteSpace = isNilOrWhiteSpace;
    function notNil(obj) { return typeof (obj) !== 'undefined' && obj != null; }
    app.notNil = notNil;
    function notNilOrEmpty(value) {
        return (typeof (value) == 'string' || (typeof (value) == 'object' && value != null && Array.isArray(value))) && value.length > 0;
    }
    app.notNilOrEmpty = notNilOrEmpty;
    function notNilOrWhiteSpace(value) { return typeof (value) == 'string' && value.trim().length > 0; }
    app.notNilOrWhiteSpace = notNilOrWhiteSpace;
    function isNumber(value) { return typeof (value) === "number" && !isNaN(value); }
    app.isNumber = isNumber;
    /**
     * Determines if value's type is an object.
     * @param {*} value Value to test.
     * @param {boolean} [noArray=false] If value is an array, return false.
     * @returns {boolean} true if value was null or undefined; otherwise, false.
     */
    function isObject(value, noArray) { return (typeof (value) == "object" && value !== null && !(noArray && Array.isArray(value))); }
    app.isObject = isObject;
    function asNotNil(value, opt, trim) {
        if (typeof (value) === "undefined" || value === null)
            return (typeof (opt) !== 'undefined') ? opt : '';
        if (typeof (value) !== 'string')
            return value;
        return ((typeof (opt) === "boolean") ? opt : trim === true) ? value.trim() : value;
    }
    app.asNotNil = asNotNil;
    function asString(value, trim = false, spec = false) {
        if (isNil(value))
            return (typeof (trim) === 'string') ? trim : ((typeof (spec) === 'string') ? spec : ((spec) ? value : ""));
        if (typeof (value) != "string") {
            if (Array.isArray(value))
                value = value.join("\n");
            else {
                try {
                    value = value.toString();
                }
                catch (err) { /* okay to ignnore */ }
                if (isNil(value))
                    return (typeof (trim) === 'string') ? trim : ((typeof (spec) === 'string') ? spec : ((spec) ? value : ""));
                if (typeof (value) != "string") {
                    try {
                        value = Object.prototype.toString.call(value);
                        if (isNil(value))
                            return (typeof (trim) === 'string') ? trim : ((typeof (spec) === 'string') ? spec : ((spec) ? value : ""));
                    }
                    catch (err) {
                        try {
                            value = value + "";
                        }
                        catch (err) {
                            if (typeof (trim) === 'string')
                                return trim;
                            if (typeof (spec) === 'string')
                                return spec;
                            if (spec)
                                return;
                            return "";
                        }
                    }
                }
            }
        }
        if (typeof (trim) === 'boolean' && trim)
            return value.trim();
        return value;
    }
    app.asString = asString;
    /**
     * Ensures that a value is a floating-point number, converting it if necessary.
     * @param value
     * @param defaultValue
     * @returns {string} Input value converted to a floating-point number.
     */
    function asFloat(value, defaultValue = NaN) {
        if (typeof (value) === 'undefined' || value === null)
            return defaultValue;
        if (typeof (value) === 'number')
            return value;
        let n = parseFloat(value);
        if (isNaN(n))
            return defaultValue;
        return n;
    }
    app.asFloat = asFloat;
    /**
     * Ensures that a value is a whole number, converting it if necessary.
     * @param value
     * @param defaultValue
     * @returns {string} Input value converted to a whole number.
     */
    function asInt(value, defaultValue = NaN) {
        if (typeof (value) === 'undefined' || value === null)
            return defaultValue;
        if (typeof (value) === 'number')
            return value;
        let n = parseInt(value);
        if (isNaN(n))
            return defaultValue;
        return n;
    }
    app.asInt = asInt;
    /**
     * Trims trailing whitespace from text.
     * @param {string} text Text to trim.
     * @returns {string} Text with trailing whitespace removed.
     */
    function trimRight(text) {
        var m = trimRightRe.exec(asString(text));
        return (isNil(m)) ? "" : m[1];
    }
    app.trimRight = trimRight;
    /**
     * Trims leading whitespace from text.
     * @param {string} text Text to trim.
     * @returns {string} Text with leading whitespace removed.
     */
    function trimLeft(text) {
        var m = trimLeftRe.exec(asString(text));
        return (isNil(m)) ? "" : m[1];
    }
    app.trimLeft = trimLeft;
    function asBoolean(value, nilIsTrue = false) {
        if (isNil(value))
            return (nilIsTrue == true);
        if (typeof (value) == "boolean")
            return value;
        if (typeof (value) == "object") {
            if (!Array.isArray(value)) {
                if (typeof (value.valueOf) == "function") {
                    try {
                        value = value.valueOf();
                    }
                    catch (e) { }
                    if (isNil(value))
                        return (nilIsTrue == true);
                }
                if (typeof (value) != "object" || !Array.isArray(value))
                    value = [value];
                else if (value.length == 0)
                    return false;
            }
            else if (value.length == 0)
                return false;
        }
        else
            value = [value];
        if (nilIsTrue) {
            for (var i = 0; i < value.length; i++) {
                var v = value[i];
                if (isNil(v))
                    return true;
                if (typeof (v) == "boolean") {
                    if (v)
                        return true;
                    continue;
                }
                if (typeof (v) != "string") {
                    if (typeof (v.valueOf) == "function") {
                        try {
                            v = v.valueOf();
                        }
                        catch (e) { }
                        if (isNil(v))
                            return true;
                        if (typeof (v) == "boolean") {
                            if (v)
                                return true;
                            continue;
                        }
                    }
                    if (typeof (v) != "string")
                        v = asString(v);
                }
                if (v.length == 0 || (v = v.trim()).length == 0 || !falseStringRe.test(v))
                    return true;
            }
        }
        else {
            for (var i = 0; i < value.length; i++) {
                var v = value[i];
                if (isNil(v))
                    continue;
                if (typeof (v) == "boolean") {
                    if (v)
                        return true;
                    continue;
                }
                if (typeof (v) != "string") {
                    if (typeof (v.valueOf) == "function") {
                        try {
                            v = v.valueOf();
                        }
                        catch (e) { }
                        if (isNil(v))
                            continue;
                        if (typeof (v) == "boolean") {
                            if (v)
                                return true;
                            continue;
                        }
                    }
                    if (typeof (v) != "string")
                        v = asString(v);
                }
                if (v.length > 0 && (v = v.trim()).length > 0 && !falseStringRe.test(v))
                    return true;
            }
        }
        return false;
    }
    app.asBoolean = asBoolean;
    function notString(value) { return typeof (value) !== 'string'; }
    app.notString = notString;
    function asNotWhitespaceOrUndefined(value, trim) {
        if (typeof (value) === 'string') {
            if (trim === true) {
                if ((value = value.trim()).length > 0)
                    return value;
            }
            else if (value.trim().length > 0)
                return value;
        }
    }
    app.asNotWhitespaceOrUndefined = asNotWhitespaceOrUndefined;
    function asDefinedOrNull(value) { return (typeof (value) === undefined) ? null : value; }
    app.asDefinedOrNull = asDefinedOrNull;
    function asUndefinedIfNull(value) {
        if (typeof (value) !== undefined && value !== null)
            return value;
    }
    app.asUndefinedIfNull = asUndefinedIfNull;
    function asNotEmptyOrNull(value, trim) {
        if (typeof (value) === 'string') {
            if (trim) {
                if ((value = value.trim()).length > 0)
                    return value;
            }
            else if (value.trim().length > 0)
                return value;
        }
        return null;
    }
    app.asNotEmptyOrNull = asNotEmptyOrNull;
    function asNotWhitespaceOrNull(value, trim) {
        if (typeof (value) === 'string') {
            if (trim === true) {
                if ((value = value.trim()).length > 0)
                    return value;
            }
            else if (value.trim().length > 0)
                return value;
        }
        return null;
    }
    app.asNotWhitespaceOrNull = asNotWhitespaceOrNull;
    function asNotEmptyOrUndefined(value, trim) {
        if (typeof (value) !== 'undefined' && value !== null && value.length > 0)
            return (trim === true && typeof (value) === 'string') ? value.trim() : value;
    }
    app.asNotEmptyOrUndefined = asNotEmptyOrUndefined;
    function isError(value) {
        return typeof (value) == 'object' && value !== null && typeof (value.message) == 'string' && typeof (value.name) == 'string' &&
            (typeof (value.stack) == 'undefined' || value.stack === null || typeof (value.stack) == 'string');
    }
    app.isError = isError;
    function isIterable(value) {
        if (typeof (value) !== 'object' || value == null)
            return false;
        if (Array.isArray(value))
            return true;
        let fn = value[Symbol.iterator];
        return (typeof (fn) === 'function');
    }
    app.isIterable = isIterable;
    function asIterable(source, allowNull = false) {
        if (typeof (source) === 'undefined')
            return [];
        if (source === null)
            return (allowNull) ? [null] : [];
        return (Array.isArray(source)) ? source : ((isIterable(source)) ? source : [source]);
    }
    app.asIterable = asIterable;
    function asArray(source, allowNull = false) {
        if (typeof (source) === 'undefined')
            return [];
        if (source === null)
            return (allowNull) ? [null] : [];
        if (Array.isArray(source))
            return source;
        if (isIterable(source)) {
            let iterator;
            let fn = source[Symbol.iterator];
            try {
                iterator = fn();
            }
            catch ( /* okay to ignore */_a) { /* okay to ignore */ }
            if (typeof (iterator) === 'object' && iterator !== null) {
                let result = [];
                try {
                    let ir = iterator.next();
                    if (allowNull)
                        while (!ir.done) {
                            if (typeof (ir.value) !== 'undefined')
                                result.push(ir.value);
                            ir = iterator.next();
                        }
                    else
                        while (!ir.done) {
                            if (typeof (ir.value) !== 'undefined' && ir.value !== null)
                                result.push(ir.value);
                            ir = iterator.next();
                        }
                }
                catch ( /* okay to ignore */_b) { /* okay to ignore */ }
                return result;
            }
        }
        return [source];
    }
    app.asArray = asArray;
    function skipFirst(source, spec, thisArg) {
        let result = [];
        let iterator = source[Symbol.iterator]();
        let ir = iterator.next();
        if (typeof (spec) === 'number')
            while (!ir.done) {
                if (spec < 1) {
                    result.push(ir.value);
                    while (!(ir = iterator.next()).done)
                        result.push(ir.value);
                    break;
                }
                spec--;
                ir = iterator.next();
            }
        else {
            let index = 0;
            if (typeof (thisArg) === 'undefined')
                while (!ir.done) {
                    if (!spec(ir.value, index++, source)) {
                        result.push(ir.value);
                        while (!(ir = iterator.next()).done)
                            result.push(ir.value);
                        break;
                    }
                    ir = iterator.next();
                }
            else
                while (!ir.done) {
                    if (!spec.call(thisArg, ir.value, index++, source)) {
                        result.push(ir.value);
                        while (!(ir = iterator.next()).done)
                            result.push(ir.value);
                        break;
                    }
                    ir = iterator.next();
                }
        }
        return result;
    }
    app.skipFirst = skipFirst;
    function skipLast(source, spec, thisArg) {
        let result = reverse(source);
        if (typeof (spec) === 'number') {
            while (result.length > 0 && spec-- > 0)
                result.shift();
        }
        else if (typeof (thisArg) === 'undefined') {
            while (result.length > 0 && spec(result[0], result.length - 1, source))
                result.shift();
        }
        else {
            while (result.length > 0 && spec.call(thisArg, result[0], result.length - 1, source))
                result.shift();
        }
        return result.reverse();
    }
    app.skipLast = skipLast;
    function takeFirst(source, spec, thisArg) {
        let result = [];
        let iterator = source[Symbol.iterator]();
        let ir = iterator.next();
        if (typeof (spec) === 'number')
            while (!ir.done && spec-- > 0) {
                result.push(ir.value);
                ir = iterator.next();
            }
        else {
            let index = 0;
            if (typeof (thisArg) === 'undefined')
                while (!ir.done && spec(ir.value, index++, source)) {
                    result.push(ir.value);
                    ir = iterator.next();
                }
            else
                while (!ir.done && spec.call(thisArg, ir.value, index++, source)) {
                    result.push(ir.value);
                    ir = iterator.next();
                }
        }
        return result;
    }
    app.takeFirst = takeFirst;
    function takeLast(source, spec, thisArg) {
        let result = reverse(source);
        if (typeof (spec) === 'number')
            while (result.length > 0 && spec)
                result.pop();
        else if (typeof (thisArg) === 'undefined')
            while (result.length > 0 && spec(result[0], result.length - 1, source))
                result.shift();
        else
            while (result.length > 0 && spec.call(thisArg, result[0], result.length - 1, source))
                result.shift();
        return result.reverse();
    }
    app.takeLast = takeLast;
    function filter(source, callbackfn, thisArg) {
        let result = [];
        let iterator = source[Symbol.iterator]();
        let ir = iterator.next();
        let index = 0;
        if (typeof (thisArg) === 'undefined')
            while (!ir.done) {
                if (callbackfn(ir.value, index++, source))
                    result.push(ir.value);
                ir = iterator.next();
            }
        else
            while (!ir.done) {
                if (callbackfn.call(thisArg, ir.value, index++, source))
                    result.push(ir.value);
                ir = iterator.next();
            }
        return result;
    }
    app.filter = filter;
    function first(source, callbackfn, thisArg) {
        let iterator = source[Symbol.iterator]();
        let r = iterator.next();
        let index = 0;
        if (typeof (thisArg) !== 'undefined')
            while (!r.done) {
                if (callbackfn.call(thisArg, r.value, index++, source))
                    return r.value;
                r = iterator.next();
            }
        else
            while (!r.done) {
                if (callbackfn(r.value, index, source))
                    return r.value;
                r = iterator.next();
            }
    }
    app.first = first;
    function indexOf(source, callbackfn, thisArg) {
        let iterator = source[Symbol.iterator]();
        let r = iterator.next();
        let index = 0;
        if (typeof (thisArg) !== 'undefined')
            while (!r.done) {
                if (callbackfn.call(thisArg, r.value, index++, source))
                    return index;
                r = iterator.next();
            }
        else
            while (!r.done) {
                if (callbackfn(r.value, index, source))
                    return index;
                r = iterator.next();
            }
    }
    app.indexOf = indexOf;
    function last(source, callbackfn, thisArg) {
        let iterator = source[Symbol.iterator]();
        let r = iterator.next();
        let result;
        let index = 0;
        if (typeof (thisArg) !== 'undefined')
            while (!r.done) {
                if (callbackfn.call(thisArg, r.value, index++, source))
                    result = r.value;
                r = iterator.next();
            }
        else
            while (!r.done) {
                if (callbackfn(r.value, index++, source))
                    result = r.value;
                r = iterator.next();
            }
        return result;
    }
    app.last = last;
    function join(source, separator) {
        if (Array.isArray(source))
            return source.join(separator);
        let iterator = source[Symbol.iterator]();
        let r = iterator.next();
        let result = [];
        let index = 0;
        while (!r.done) {
            result.push(r.value);
            r = iterator.next();
        }
        return result.join(separator);
    }
    app.join = join;
    function reverse(source) {
        let result = [];
        let iterator = source[Symbol.iterator]();
        let ir = iterator.next();
        let index = 0;
        while (!ir.done) {
            result.unshift(ir.value);
            ir = iterator.next();
        }
        return result;
    }
    app.reverse = reverse;
    function indexOfAny(value, position, ...searchString) {
        let result;
        if (typeof (position) === 'number') {
            result = -1;
            searchString.forEach((s) => {
                if (s.length > 0) {
                    let i = value.indexOf(s, position);
                    if (i > -1 && (result < 0 || i < result))
                        result = i;
                }
            });
        }
        else {
            searchString.forEach((s) => {
                if (s.length > 0) {
                    let i = value.indexOf(s);
                    if (i > -1 && (result < 0 || i < result))
                        result = i;
                }
            });
        }
        return result;
    }
    app.indexOfAny = indexOfAny;
    function map(source, callbackfn, thisArg) {
        let iterator = source[Symbol.iterator]();
        let r = iterator.next();
        let result = [];
        let index = 0;
        if (typeof (thisArg) !== 'undefined')
            while (!r.done) {
                result.push(callbackfn.call(thisArg, r.value, index++, source));
                r = iterator.next();
            }
        else
            while (!r.done) {
                result.push(callbackfn(r.value, index++, source));
                r = iterator.next();
            }
        return result;
    }
    app.map = map;
    function every(source, callbackfn, thisArg) {
        let iterator = source[Symbol.iterator]();
        let r = iterator.next();
        let index = 0;
        if (typeof (thisArg) !== 'undefined')
            while (!r.done) {
                if (!callbackfn.call(thisArg, r.value, index++, source))
                    return false;
                r = iterator.next();
            }
        else
            while (!r.done) {
                if (!callbackfn(r.value, index++, source))
                    return false;
                r = iterator.next();
            }
        return true;
    }
    app.every = every;
    function some(source, callbackfn, thisArg) {
        let iterator = source[Symbol.iterator]();
        let r = iterator.next();
        let index = 0;
        if (typeof (thisArg) !== 'undefined')
            while (!r.done) {
                if (callbackfn.call(thisArg, r.value, index++, source))
                    return true;
                r = iterator.next();
            }
        else
            while (!r.done) {
                if (callbackfn(r.value, index++, source))
                    return true;
                r = iterator.next();
            }
        return true;
    }
    app.some = some;
    function forEach(source, callbackfn, thisArg) {
        let iterator = source[Symbol.iterator]();
        let r = iterator.next();
        let index = 0;
        if (typeof (thisArg) !== 'undefined')
            while (!r.done) {
                callbackfn.call(thisArg, r.value, index++, source);
                r = iterator.next();
            }
        else
            while (!r.done) {
                callbackfn(r.value, index++, source);
                r = iterator.next();
            }
    }
    app.forEach = forEach;
    function reduce(source, callbackfn, initialValue) {
        let iterator = source[Symbol.iterator]();
        let r = iterator.next();
        let result = initialValue;
        let index = 0;
        while (!r.done) {
            result = callbackfn(result, r.value, index++, source);
            r = iterator.next();
        }
        return result;
    }
    app.reduce = reduce;
    function unique(source, callbackfn, thisArg) {
        if (typeof (callbackfn) !== 'function')
            callbackfn = function (x, y) { return x === y; };
        let iterator = source[Symbol.iterator]();
        let r = iterator.next();
        let result = [];
        if (!r.done) {
            result.push(r.value);
            r = iterator.next();
            let index = 0;
            if (typeof (thisArg) !== 'undefined')
                while (!r.done) {
                    if (!result.some((value) => callbackfn.call(thisArg, r.value, value)))
                        result.push(r.value);
                    r = iterator.next();
                }
            else
                while (!r.done) {
                    if (!result.some((value) => callbackfn(r.value, value)))
                        result.push(r.value);
                    r = iterator.next();
                }
        }
        return result;
    }
    app.unique = unique;
    function areSequencesEqual(source, target, callbackfn, thisArg) {
        if (typeof (source) != typeof (target) || (Array.isArray(source) && Array.isArray(target) && source.length != target.length))
            return false;
        let iteratorX = source[Symbol.iterator]();
        let iteratorY = target[Symbol.iterator]();
        let resultX = iteratorX.next();
        let resultY = iteratorY.next();
        if (typeof (callbackfn) !== 'function')
            while (!resultX.done) {
                if (resultY.done || resultX.value !== resultY.value)
                    return false;
                resultX = iteratorX.next();
                resultY = iteratorY.next();
            }
        else if (typeof (thisArg) === 'undefined') {
            let index = -1;
            while (!resultX.done) {
                if (resultY.done || !callbackfn.call(thisArg, resultX.value, resultY.value, ++index))
                    return false;
                resultX = iteratorX.next();
                resultY = iteratorY.next();
            }
        }
        else {
            let index = -1;
            while (!resultX.done) {
                if (resultY.done || !callbackfn(resultX.value, resultY.value, ++index))
                    return false;
                resultX = iteratorX.next();
                resultY = iteratorY.next();
            }
        }
        return resultY.done;
    }
    app.areSequencesEqual = areSequencesEqual;
    function isEventPropagationStoppedFunc(event) {
        return typeof event === "object" && event !== null && typeof event.isPropagationStopped === "function" && event.isPropagationStopped();
    }
    app.isEventPropagationStoppedFunc = isEventPropagationStoppedFunc;
    function preventEventDefault(event, stopPropogation) {
        if (typeof event !== "object" || event === null)
            return;
        if (!event.defaultPrevented)
            event.preventDefault();
        if (stopPropogation === true && !isEventPropagationStoppedFunc(event))
            event.stopPropagation();
    }
    app.preventEventDefault = preventEventDefault;
    function stopEventPropagation(event, preventDefault) {
        if (typeof event !== "object" || event === null)
            return;
        if (!isEventPropagationStoppedFunc(event))
            event.stopPropagation();
        if (preventDefault === true && !event.defaultPrevented)
            event.preventDefault();
    }
    app.stopEventPropagation = stopEventPropagation;
    /**
     * Represents status HTTP response status codes.
     *
     * @enum
     * @description These were derrived from the following authoritative source: {@link https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html}.
     */
    let HttpResponseStatusCode;
    (function (HttpResponseStatusCode) {
        /**
         * The client SHOULD continue with its request.
         *
         * @member HttpResponseStatusCode
         * @description This interim response is used to inform the client that the initial part of the request has been received and has not yet been rejected by the server. The client SHOULD continue by sending the remainder of the request or, if the request has already been completed, ignore this response.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["continue"] = 100] = "continue";
        /**
         * The server understands and is willing to comply with the client's request for a change in the application protocol.
         *
         * @member HttpResponseStatusCode
         * @description The server understands and is willing to comply with the client's request, via the Upgrade Message Header field, for a change in the application protocol being used on this connection. The server will switch protocols to those defined by the response's Upgrade header field immediately after the empty line which terminates the 101 response.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["switchingProtocols"] = 101] = "switchingProtocols";
        /**
         * The request has succeeded.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["ok"] = 200] = "ok";
        /**
         * The request has been fulfilled and resulted in a new resource being created.
         *
         * @member HttpResponseStatusCode
         * @description The newly created resource can be referenced by the URI(s) returned in the entity of the response, with the most specific URI for the resource given by a Location header field. The response SHOULD include an entity containing a list of resource characteristics and location(s) from which the user or user agent can choose the one most appropriate. The entity format is specified by the media type given in the Content-Type header field.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["created"] = 201] = "created";
        /**
         * The request has been accepted for processing, but the processing has not been completed.
         *
         * @member HttpResponseStatusCode
         * @description The request might or might not eventually be acted upon, as it might be disallowed when processing actually takes place. There is no facility for re-sending a status code from an asynchronous operation such as this.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["accepted"] = 202] = "accepted";
        /**
         * The returned metainformation in the entity-header is not the definitive set as available from the origin server, but is gathered from a local or a third-party copy
         *
         * @member HttpResponseStatusCode
         * @description
         */
        HttpResponseStatusCode[HttpResponseStatusCode["nonAuthoritativeInformation"] = 203] = "nonAuthoritativeInformation";
        /**
         * The server has fulfilled the request but does not need to return an entity-body, and might want to return updated metainformation.
         *
         * @member HttpResponseStatusCode
         * @description The response MAY include new or updated metainformation in the form of entity-headers, which if present SHOULD be associated with the requested variant.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["noContent"] = 204] = "noContent";
        /**
         * The server has fulfilled the request and the user agent SHOULD reset the document view which caused the request to be sent.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["resetContent"] = 205] = "resetContent";
        /**
         * The server has fulfilled the partial GET request for the resource.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["partialContent"] = 206] = "partialContent";
        /**
         * Multiple resources correspond to the request.
         *
         * @member HttpResponseStatusCode
         * @description  The requested resource corresponds to any one of a set of representations, each with its own specific location, and agent- driven negotiation information (section 12) is being provided so that the user (or user agent) can select a preferred representation and redirect its request to that location.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["multipleChoices"] = 300] = "multipleChoices";
        /**
         * The requested resource is permanently located at another URI, usually provided in the Location response field.
         *
         * @member HttpResponseStatusCode
         * @description The requested resource has been assigned a new permanent URI and any future references to this resource SHOULD use one of the returned URIs. Clients with link editing capabilities ought to automatically re-link references to the Request-URI to one or more of the new references returned by the server, where possible.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["movedPermanently"] = 301] = "movedPermanently";
        /**
         * The requested resource is temporarily located at another URI, usually provided in the Location response field.
         *
         * @member HttpResponseStatusCode
         * @description The requested resource resides temporarily under a different URI. Since the redirection might be altered on occasion, the client SHOULD continue to use the Request-URI for future requests. This response is only cacheable if indicated by a Cache-Control or Expires header field.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["found"] = 302] = "found";
        /**
         * The response to the request can be found under a different URI, usually provided in the Location response field.
         *
         * @member HttpResponseStatusCode
         * @description The response to the request can be found under a different URI and SHOULD be retrieved using a GET method on that resource. This method exists primarily to allow the output of a POST-activated script to redirect the user agent to a selected resource. The new URI is not a substitute reference for the originally requested resource.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["seeOther"] = 303] = "seeOther";
        /**
         * The requested resource has not been modified.
         *
         * @member HttpResponseStatusCode
         * @description This response code usually results from a conditional request; otherwise, the server should not send this response.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["notModified"] = 304] = "notModified";
        /**
         * The requested resource MUST be accessed through the proxy given by the Location field.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["useProxy"] = 305] = "useProxy";
        /**
         * (unused redirection response code)
         *
         * @member HttpResponseStatusCode
         * @description This status code was used in a previous version of the specification, is no longer used, and the code is reserved.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["unusedRedirection"] = 306] = "unusedRedirection";
        /**
         * The requested resource resides temporarily under a different URI.
         *
         * @member HttpResponseStatusCode
         * @description Since the redirection MAY be altered on occasion, the client SHOULD continue to use the Request-URI for future requests. This response is only cacheable if indicated by a Cache-Control or Expires header field.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["temporaryRedirect"] = 307] = "temporaryRedirect";
        /**
         * The request could not be understood by the server due to malformed syntax.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["badRequest"] = 400] = "badRequest";
        /**
         * The request requires user authentication.
         *
         * @member HttpResponseStatusCode
         * @description The response MUST include a WWW-Authenticate header field (section 14.47) containing a challenge applicable to the requested resource. The client MAY repeat the request with a suitable Authorization header field (section 14.8). If the request already included Authorization credentials, then the 401 response indicates that authorization has been refused for those credentials.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["unauthorized"] = 401] = "unauthorized";
        /**
         * This code is reserved for future use.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["paymentRequired"] = 402] = "paymentRequired";
        /**
         * The server understood the request, but is refusing to fulfill it.
         *
         * @member HttpResponseStatusCode
         * @description Authorization will not help and the request SHOULD NOT be repeated.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["forbidden"] = 403] = "forbidden";
        /**
         * The server has not found anything matching the Request-URI.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["notFound"] = 404] = "notFound";
        /**
         * The method specified in the Request-Line is not allowed for the resource identified by the Request-URI.
         *
         * @member HttpResponseStatusCode
         * @description The response will include an Allow header containing a list of valid methods for the requested resource.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["methodNotAllowed"] = 405] = "methodNotAllowed";
        /**
         * The resource identified by the request is only capable of generating response entities which have content characteristics not acceptable according to the accept headers sent in the request.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["notAcceptable"] = 406] = "notAcceptable";
        /**
         * This code is similar to 401 (Unauthorized), but indicates that the client must first authenticate itself with the proxy.
         *
         * @member HttpResponseStatusCode
         * @description The proxy will return a Proxy-Authenticate header field containing a challenge applicable to the proxy for the requested resource. The client MAY repeat the request with a suitable Proxy-Authorization header field.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["proxyAuthenticationRequired"] = 407] = "proxyAuthenticationRequired";
        /**
         * The client did not produce a request within the time that the server was prepared to wait.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["requestTimeout"] = 408] = "requestTimeout";
        /**
         * The request could not be completed due to a conflict with the current state of the resource.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["conflict"] = 409] = "conflict";
        /**
         * The requested resource is no longer available at the server and no forwarding address is known.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["gone"] = 410] = "gone";
        /**
         * The server refuses to accept the request without a defined Content-Length.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["lengthRequired"] = 411] = "lengthRequired";
        /**
         * The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["preconditionFailed"] = 412] = "preconditionFailed";
        /**
         * The server is refusing to process a request because the request entity is larger than the server is willing or able to process.
         *
         * @member HttpResponseStatusCode
         * @description The server MAY close the connection to prevent the client from continuing the request.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["requestEntityTooLarge"] = 413] = "requestEntityTooLarge";
        /**
         * The server is refusing to service the request because the Request-URI is longer than the server is willing to interpret.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["requestUriTooLong"] = 414] = "requestUriTooLong";
        /**
         * The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["unsupportedMediaType"] = 415] = "unsupportedMediaType";
        /**
         * Range specified in request not viable.
         *
         * @member HttpResponseStatusCode
         * @description Request included a Range request-header field, and none of the range-specifier values in this field overlap the current extent of the selected resource, and the request did not include an If-Range request-header field.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["requestedRangeNotSatisfiable"] = 416] = "requestedRangeNotSatisfiable";
        /**
         * The expectation given in an Expect request-header field could not be met.
         *
         * @member HttpResponseStatusCode
         * @description The expectation given in an Expect request-header field could not be met by this server, or, if the server is a proxy, the server has unambiguous evidence that the request could not be met by the next-hop server.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["expectationFailed"] = 417] = "expectationFailed";
        /**
         * The server encountered an unexpected condition which prevented it from fulfilling the request.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["internalServerError"] = 500] = "internalServerError";
        /**
         * The server does not support the functionality required to fulfill the request.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["notImplemented"] = 501] = "notImplemented";
        /**
         * The server, while acting as a gateway or proxy, received an invalid response from the upstream server it accessed in attempting to fulfill the request.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["badGateway"] = 502] = "badGateway";
        /**
         * The server is currently unable to handle the request due to a temporary overloading or maintenance of the server.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["serviceUnavailable"] = 503] = "serviceUnavailable";
        /**
         * The server, while acting as a gateway or proxy, did not receive a timely response from the upstream server.
         *
         * @member HttpResponseStatusCode
         * @description The server, while acting as a gateway or proxy, did not receive a timely response from the upstream server specified by the URI (e.g. HTTP, FTP, LDAP) or some other auxiliary server (e.g. DNS) it needed to access in attempting to complete the request.
         */
        HttpResponseStatusCode[HttpResponseStatusCode["gatewayTimeout"] = 504] = "gatewayTimeout";
        /**
         * The server does not support, or refuses to support, the HTTP protocol version that was used in the request message.
         *
         * @member HttpResponseStatusCode
         */
        HttpResponseStatusCode[HttpResponseStatusCode["httpVersionNotSupported"] = 505] = "httpVersionNotSupported";
    })(HttpResponseStatusCode = app.HttpResponseStatusCode || (app.HttpResponseStatusCode = {}));
    let HttpResponseStatusClass;
    (function (HttpResponseStatusClass) {
        HttpResponseStatusClass[HttpResponseStatusClass["informational"] = 1] = "informational";
        HttpResponseStatusClass[HttpResponseStatusClass["successful"] = 2] = "successful";
        HttpResponseStatusClass[HttpResponseStatusClass["redirect"] = 3] = "redirect";
        HttpResponseStatusClass[HttpResponseStatusClass["clientError"] = 4] = "clientError";
        HttpResponseStatusClass[HttpResponseStatusClass["serverError"] = 5] = "serverError";
        HttpResponseStatusClass[HttpResponseStatusClass["NOT_NUMBER"] = -1] = "NOT_NUMBER";
        HttpResponseStatusClass[HttpResponseStatusClass["OUT_OF_RANGE"] = -2] = "OUT_OF_RANGE";
    })(HttpResponseStatusClass = app.HttpResponseStatusClass || (app.HttpResponseStatusClass = {}));
    let HttpResponseStatusRanges;
    (function (HttpResponseStatusRanges) {
        HttpResponseStatusRanges[HttpResponseStatusRanges["MINRANGE"] = 100] = "MINRANGE";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MINRANGE_INFORMATIONAL"] = 100] = "MINRANGE_INFORMATIONAL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXVALUE_INFORMATIONAL_EXCL"] = 102] = "MAXVALUE_INFORMATIONAL_EXCL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXRANGE_INFORMATIONAL_EXCL"] = 200] = "MAXRANGE_INFORMATIONAL_EXCL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MINRANGE_SUCCESSFUL"] = 200] = "MINRANGE_SUCCESSFUL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXVALUE_SUCCESSFUL_EXCL"] = 207] = "MAXVALUE_SUCCESSFUL_EXCL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXRANGE_SUCCESSFUL_EXCL"] = 300] = "MAXRANGE_SUCCESSFUL_EXCL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MINRANGE_REDIRECT"] = 300] = "MINRANGE_REDIRECT";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXVALUE_REDIRECT_EXCL"] = 308] = "MAXVALUE_REDIRECT_EXCL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXRANGE_REDIRECT_EXCL"] = 400] = "MAXRANGE_REDIRECT_EXCL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MINRANGE_CLIENT_ERROR"] = 400] = "MINRANGE_CLIENT_ERROR";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXVALUE_CLIENT_ERROR_EXCL"] = 418] = "MAXVALUE_CLIENT_ERROR_EXCL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXRANGE_CLIENT_ERROR_EXCL"] = 500] = "MAXRANGE_CLIENT_ERROR_EXCL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MINRANGE_SERVER_ERROR"] = 500] = "MINRANGE_SERVER_ERROR";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXVALUE_SERVER_ERROR_EXCL"] = 506] = "MAXVALUE_SERVER_ERROR_EXCL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXRANGE_SERVER_ERROR_EXCL"] = 600] = "MAXRANGE_SERVER_ERROR_EXCL";
        HttpResponseStatusRanges[HttpResponseStatusRanges["MAXRANGE_EXCL"] = 600] = "MAXRANGE_EXCL";
    })(HttpResponseStatusRanges || (HttpResponseStatusRanges = {}));
    let UserRole;
    (function (UserRole) {
        UserRole[UserRole["None"] = 0] = "None";
        UserRole[UserRole["Viewer"] = 1] = "Viewer";
        UserRole[UserRole["User"] = 2] = "User";
        UserRole[UserRole["Crawler"] = 3] = "Crawler";
        UserRole[UserRole["Admin"] = 4] = "Admin";
    })(UserRole = app.UserRole || (app.UserRole = {}));
    function __toHttpResponseStatusValueInRange(response) {
        if (response >= HttpResponseStatusRanges.MINRANGE && response < HttpResponseStatusRanges.MAXRANGE_EXCL)
            return Math.floor(response);
    }
    function __toHttpResponseStatusCode(response) {
        if (!isNaN(response) && response >= HttpResponseStatusRanges.MINRANGE_INFORMATIONAL) {
            if (response < HttpResponseStatusRanges.MAXRANGE_INFORMATIONAL_EXCL)
                return Math.floor(response);
            if (response >= HttpResponseStatusRanges.MINRANGE_SUCCESSFUL) {
                if (response < HttpResponseStatusRanges.MAXVALUE_SUCCESSFUL_EXCL)
                    return Math.floor(response);
                if (response >= HttpResponseStatusRanges.MINRANGE_REDIRECT) {
                    if (response < HttpResponseStatusRanges.MAXVALUE_REDIRECT_EXCL)
                        return Math.floor(response);
                    if ((response >= HttpResponseStatusRanges.MINRANGE_SERVER_ERROR) ? response < HttpResponseStatusRanges.MAXVALUE_SERVER_ERROR_EXCL :
                        response >= HttpResponseStatusRanges.MINRANGE_CLIENT_ERROR && response < HttpResponseStatusRanges.MAXVALUE_CLIENT_ERROR_EXCL)
                        return Math.floor(response);
                }
            }
        }
    }
    function toHttpResponseStatusClass(response) {
        if (typeof response == "number") {
            if (isNaN(response))
                return HttpResponseStatusClass.NOT_NUMBER;
            response = __toHttpResponseStatusValueInRange(response);
        }
        else if (typeof response === "object" && response !== null && typeof response.status === "number") {
            if (isNaN(response.status))
                return HttpResponseStatusClass.NOT_NUMBER;
            response = __toHttpResponseStatusValueInRange(response.status);
        }
        else
            return HttpResponseStatusClass.NOT_NUMBER;
        return (typeof response === "number") ? Math.floor(response / 100.0) : HttpResponseStatusClass.OUT_OF_RANGE;
    }
    app.toHttpResponseStatusClass = toHttpResponseStatusClass;
    function toHttpResponseStatusCode(response) {
        if (typeof response == "number") {
            if (!isNaN(response))
                return __toHttpResponseStatusCode(response);
        }
        else if (typeof response === "object" && response !== null && typeof response.status === "number") {
            if (isNaN(response.status))
                return __toHttpResponseStatusCode(response.status);
        }
    }
    app.toHttpResponseStatusCode = toHttpResponseStatusCode;
    function toHttpResponseStatusMessage(response) {
        if (typeof response == "number") {
            if (isNaN(response))
                return "#NaN";
        }
        else if (typeof response === "object" && response !== null) {
            if (typeof response.statusText === "string" && response.statusText.trim().length > 0)
                return response.statusText;
            if (typeof response.status !== "number")
                return "#Err";
            if (isNaN(response.status))
                return "#NaN";
            response = response.status;
        }
        else
            return "#Err";
        switch (Math.floor(response)) {
            case HttpResponseStatusCode.continue:
                return "Continue";
            case HttpResponseStatusCode.switchingProtocols:
                return "Switching Protocols";
            case HttpResponseStatusCode.ok:
                return "OK";
            case HttpResponseStatusCode.created:
                return "Created";
            case HttpResponseStatusCode.accepted:
                return "accepted";
            case HttpResponseStatusCode.nonAuthoritativeInformation:
                return "Non-Authoritative Information";
            case HttpResponseStatusCode.noContent:
                return "No Content";
            case HttpResponseStatusCode.resetContent:
                return "Reset Content";
            case HttpResponseStatusCode.partialContent:
                return "Partial Content";
            case HttpResponseStatusCode.multipleChoices:
                return "Multiple Choices";
            case HttpResponseStatusCode.movedPermanently:
                return "Moved Permanently";
            case HttpResponseStatusCode.found:
                return "Found";
            case HttpResponseStatusCode.seeOther:
                return "See Other";
            case HttpResponseStatusCode.notModified:
                return "Not Modified";
            case HttpResponseStatusCode.useProxy:
                return "Use Proxy";
            case HttpResponseStatusCode.unusedRedirection:
                return "Unused";
            case HttpResponseStatusCode.temporaryRedirect:
                return "Temporary Redirect";
            case HttpResponseStatusCode.badRequest:
                return "Bad Request";
            case HttpResponseStatusCode.unauthorized:
                return "Unauthorized";
            case HttpResponseStatusCode.paymentRequired:
                return "Payment Required";
            case HttpResponseStatusCode.forbidden:
                return "Forbidden";
            case HttpResponseStatusCode.notFound:
                return "Not Found";
            case HttpResponseStatusCode.methodNotAllowed:
                return "Method Not Allowed";
            case HttpResponseStatusCode.notAcceptable:
                return "Not Acceptable";
            case HttpResponseStatusCode.proxyAuthenticationRequired:
                return "Proxy Authentication Required";
            case HttpResponseStatusCode.requestTimeout:
                return "Request Timeout";
            case HttpResponseStatusCode.conflict:
                return "Conflict";
            case HttpResponseStatusCode.gone:
                return "Gone";
            case HttpResponseStatusCode.lengthRequired:
                return "Length Required";
            case HttpResponseStatusCode.preconditionFailed:
                return "Precondition Failed";
            case HttpResponseStatusCode.requestEntityTooLarge:
                return "Request Entity Too Large";
            case HttpResponseStatusCode.requestUriTooLong:
                return "Request Uri Too Long";
            case HttpResponseStatusCode.unsupportedMediaType:
                return "Unsupported Media Type";
            case HttpResponseStatusCode.requestedRangeNotSatisfiable:
                return "Requested Range Not Satisfiable";
            case HttpResponseStatusCode.expectationFailed:
                return "Expectation Failed";
            case HttpResponseStatusCode.internalServerError:
                return "Internal Server Error";
            case HttpResponseStatusCode.notImplemented:
                return "Not Implemented";
            case HttpResponseStatusCode.badGateway:
                return "Bad Gateway";
            case HttpResponseStatusCode.serviceUnavailable:
                return "Service Unavailable";
            case HttpResponseStatusCode.gatewayTimeout:
                return "Gateway Timeout";
            case HttpResponseStatusCode.httpVersionNotSupported:
                return "Http Version Not Supported";
        }
        if (response >= HttpResponseStatusRanges.MINRANGE_INFORMATIONAL) {
            if (response < HttpResponseStatusRanges.MAXRANGE_INFORMATIONAL_EXCL)
                return "Informational";
            if (response < HttpResponseStatusRanges.MAXVALUE_SUCCESSFUL_EXCL)
                return "Success";
            if (response < HttpResponseStatusRanges.MAXVALUE_REDIRECT_EXCL)
                return "Redirect";
            if (response < HttpResponseStatusRanges.MAXRANGE_CLIENT_ERROR_EXCL)
                return "Client Error";
            if (response < HttpResponseStatusRanges.MAXRANGE_SERVER_ERROR_EXCL)
                return "Server Error";
        }
        return "Unknown";
    }
    app.toHttpResponseStatusMessage = toHttpResponseStatusMessage;
    /**
     * Indicates whether the response is provisional, consisting only of the Status-Line and optional headers, and is terminated by an empty line.
     *
     * @param {number | ng.IHttpPromiseCallbackArg<any>} response - The response code or response object.
     * @returns {boolean} True if the response is provisional; otherwise, false.
     */
    function isInformationalHttpResponse(response) {
        return (typeof response === "number" || (typeof response == "object" && response !== null && typeof (response = response.status) === "number")) && !isNaN(response) &&
            response >= HttpResponseStatusRanges.MINRANGE_INFORMATIONAL && response < HttpResponseStatusRanges.MAXRANGE_INFORMATIONAL_EXCL;
    }
    app.isInformationalHttpResponse = isInformationalHttpResponse;
    /**
     * Indicates whether the client's request was successfully received, understood, and accepted.
     *
     * @param {number | ng.IHttpPromiseCallbackArg<any>} response - The response code or response object.
     * @returns {boolean} True if the client's request was successful; otherwise, false.
     */
    function isSuccessfulHttpResponse(response) {
        return (typeof response === "number" || (typeof response == "object" && response !== null && typeof (response = response.status) === "number")) && !isNaN(response) &&
            response >= HttpResponseStatusRanges.MINRANGE_SUCCESSFUL && response < HttpResponseStatusRanges.MAXRANGE_SUCCESSFUL_EXCL;
    }
    app.isSuccessfulHttpResponse = isSuccessfulHttpResponse;
    /**
     * Indicates whether further action needs to be taken by the user agent in order to fulfill the request.
     *
     * @param {number | ng.IHttpPromiseCallbackArg<any>} response - The response code or response object.
     * @returns {boolean} True if further action needs to be taken by the user agent in order to fulfill the request; otherwise, false.
     */
    function isRedirectionHttpResponse(response) {
        return (typeof response === "number" || (typeof response == "object" && response !== null && typeof (response = response.status) === "number")) && !isNaN(response) &&
            response >= HttpResponseStatusRanges.MINRANGE_REDIRECT && response < HttpResponseStatusRanges.MAXRANGE_REDIRECT_EXCL;
    }
    app.isRedirectionHttpResponse = isRedirectionHttpResponse;
    /**
     * Indicates whether there was an error in the client request.
     *
     * @param {number | ng.IHttpPromiseCallbackArg<any>} response - The response code or response object.
     * @returns {boolean} True if there was an error in the client request; otherwise, false.
     */
    function isClientErrorHttpResponse(response) {
        return (typeof response === "number" || (typeof response == "object" && response !== null && typeof (response = response.status) === "number")) && !isNaN(response) &&
            response >= HttpResponseStatusRanges.MINRANGE_CLIENT_ERROR && response < HttpResponseStatusRanges.MAXRANGE_CLIENT_ERROR_EXCL;
    }
    app.isClientErrorHttpResponse = isClientErrorHttpResponse;
    /**
     * Indicates whether the server encountered an unexpected condition which prevented it from fulfilling the request.
     *
     * @param {number | ng.IHttpPromiseCallbackArg<any>} response - The response code or response object.
     * @returns {boolean} True if the server encountered an unexpected condition which prevented it from fulfilling the request; otherwise, false.
     */
    function isServerErrorHttpResponse(response) {
        return (typeof response === "number" || (typeof response == "object" && response !== null && typeof (response = response.status) === "number")) && !isNaN(response) &&
            response >= HttpResponseStatusRanges.MINRANGE_SERVER_ERROR && response < HttpResponseStatusRanges.MAXRANGE_SERVER_ERROR_EXCL;
    }
    app.isServerErrorHttpResponse = isServerErrorHttpResponse;
    function logResponse(response, logService, messageOrForce, force) {
        if (((arguments.length > 3) ? force : messageOrForce) !== true && isSuccessfulHttpResponse(response))
            return;
        let outputObj = {};
        if (typeof messageOrForce === "string")
            outputObj.message = messageOrForce;
        if (typeof response === "number") {
            outputObj.statusCode = response;
            outputObj.status = toHttpResponseStatusMessage(response);
        }
        else {
            if (typeof response !== "object" || response === null)
                return;
            outputObj.statusCode = (typeof response.status !== "number") ? NaN : response.status;
            outputObj.status = toHttpResponseStatusMessage(response);
            if (typeof response.headers === "object" && response.headers !== null)
                outputObj.headers = response.headers;
        }
        if (toHttpResponseStatusCode(outputObj.statusCode) === HttpResponseStatusCode.noContent)
            logService.warn(angular.toJson(outputObj));
        else if (isSuccessfulHttpResponse(outputObj.statusCode) || isInformationalHttpResponse(outputObj.statusCode))
            logService.info(angular.toJson(outputObj));
        else
            logService.error(angular.toJson(outputObj));
    }
    app.logResponse = logResponse;
    class mainNavigationServiceProvider {
        constructor() {
            this[Symbol.toStringTag] = app.MAIN_NAV_PROVIDER_NAME;
            this._currentItemClass = [];
            this._currentParentClass = [];
            this._otherItemClass = [];
            let p = this;
            this.$get = ['$document', rootBroadcaster.SERVICE_NAME, function mainNavigationFactory($document, rootBroadcaster) {
                    if (p._currentItemClass.length == 0)
                        p._currentItemClass = ["nav-link", "active"];
                    if (p._currentParentClass.length == 0)
                        p._currentParentClass = ["nav-link", "active"];
                    if (p._otherItemClass.length == 0)
                        p._otherItemClass = ["nav-link"];
                    return new mainNavigationService($document, rootBroadcaster, p._currentItemClass, p._currentParentClass, p._otherItemClass);
                }];
        }
        currentItemClass(...css) {
            this._currentItemClass = [];
            if (typeof css !== "object" && css === null || css.length == 0)
                return this;
            let r = /[\s\r\n]+/;
            css.forEach((value) => {
                if (typeof value == "string" && (value = value.trim()).length > 0)
                    value.split(r).forEach((v) => { this._currentItemClass.push(v); });
            });
        }
        currentParentClass(...css) {
            this._currentParentClass = [];
            if (typeof css !== "object" && css === null || css.length == 0)
                return this;
            let r = /[\s\r\n]+/;
            css.forEach((value) => {
                if (typeof value == "string" && (value = value.trim()).length > 0)
                    value.split(r).forEach((v) => { this._currentParentClass.push(v); });
            });
        }
        otherItemClass(...css) {
            this._otherItemClass = [];
            if (typeof css !== "object" && css === null || css.length == 0)
                return this;
            let r = /[\s\r\n]+/;
            css.forEach((value) => {
                if (typeof value == "string" && (value = value.trim()).length > 0)
                    value.split(r).forEach((v) => { this._otherItemClass.push(v); });
            });
        }
    }
    app.mainNavigationServiceProvider = mainNavigationServiceProvider;
    class mainNavigationService {
        constructor($document, rootBroadcaster, _currentItemClass, _currentParentClass, _otherItemClass) {
            this.$document = $document;
            this.rootBroadcaster = rootBroadcaster;
            this._currentItemClass = _currentItemClass;
            this._currentParentClass = _currentParentClass;
            this._otherItemClass = _otherItemClass;
            this[Symbol.toStringTag] = app.MAIN_NAV_SERVICE_NAME;
            let element = $document.find('head').find('title');
            if (element.length == 0)
                element = $document.find('head').add('<title></title>');
            let pageTitle = element.text();
            if (typeof pageTitle !== "string" || (pageTitle = pageTitle.trim()).length == 0) {
                pageTitle = 'FS InfoCat';
                element.text(pageTitle);
            }
            this._pageTitle = this._defaultPageTitle = pageTitle;
            this._titleChangedEvent = this.rootBroadcaster.registerPrivateEventName(app.MAIN_NAV_SERVICE_NAME + ":TitleChanged");
            this._pageChangedEvent = this.rootBroadcaster.registerPrivateEventName(app.MAIN_NAV_SERVICE_NAME + ":PageChanged");
            this._subTitleChangedEvent = this.rootBroadcaster.registerPrivateEventName(app.MAIN_NAV_SERVICE_NAME + ":SubTitleChanged");
            this.rootBroadcaster.onRouteChangeSuccess(this.onRouteChangeSuccess, this);
        }
        defaultPageTitle() { return this._defaultPageTitle; }
        pageTitle(value) {
            if (typeof value === "string") {
                if ((value = value.trim()).length == 0)
                    value = this._defaultPageTitle;
                let oldValue = this._pageTitle;
                if (value !== oldValue) {
                    this._pageTitle = value;
                    if (this._pageTitle === this._defaultPageTitle)
                        this.$document.find('head').find('title').text(this._defaultPageTitle);
                    else
                        this.$document.find('head').find('title').text(this._defaultPageTitle + ": " + this._pageTitle);
                    this.rootBroadcaster.broadcastEvent(this._titleChangedEvent, value, oldValue);
                }
            }
            return this._pageTitle;
        }
        pageSubTitle(value) {
            if (typeof value === "string") {
                let oldValue = this._pageSubTitle;
                if ((value = value.trim()) !== oldValue) {
                    this._pageSubTitle = value;
                    this.rootBroadcaster.broadcastEvent(this._subTitleChangedEvent, value, oldValue);
                }
            }
            return this._pageSubTitle;
        }
        currentItemClass() { return this._currentItemClass; }
        currentParentClass() { return this._currentParentClass; }
        otherItemClass() { return this._otherItemClass; }
        onPageTitleChanged($scope, listener, thisArg) {
            if (arguments.length > 2)
                this.rootBroadcaster.onEvent($scope, this._titleChangedEvent, (event, newValue, oldValue) => { listener.call(thisArg, newValue, oldValue); });
            else
                this.rootBroadcaster.onEvent($scope, this._titleChangedEvent, (event, newValue, oldValue) => { listener(newValue, oldValue); });
        }
        onPageSubTitleChanged($scope, listener, thisArg) {
            if (arguments.length > 2)
                this.rootBroadcaster.onEvent($scope, this._subTitleChangedEvent, (event, newValue, oldValue) => { listener.call(thisArg, newValue, oldValue); });
            else
                this.rootBroadcaster.onEvent($scope, this._subTitleChangedEvent, (event, newValue, oldValue) => { listener(newValue, oldValue); });
        }
        onRouteChangeSuccess(event, current) {
            if (typeof current !== "object" || current === null)
                return;
            let newPage = current.__metaData;
            let arr = (typeof newPage === 'undefined' || null === newPage) ? ((typeof this._currentPage === 'undefined') ? [] : [this._currentPage, undefined]) : [this._currentPage, newPage];
            if (arr.length == 0)
                return;
            this._currentPage = newPage;
            try {
                this.pageTitle((typeof arr[0] === "undefined" || arr[0].pageTitle.length == 0) ? this._defaultPageTitle : arr[0].pageTitle);
            }
            finally {
                try {
                    this.pageTitle((typeof arr[0] === "undefined" || arr[0].pageTitle.length == 0) ? this._defaultPageTitle : arr[0].pageTitle);
                }
                finally {
                    this.rootBroadcaster.broadcastEvent(this._pageChangedEvent, arr[0], arr[1]);
                }
            }
        }
    }
    app.mainNavigationService = mainNavigationService;
    class mainControllerBase {
        constructor($scope, $window, $log, mainNavigation) {
            this.$scope = $scope;
            this.$window = $window;
            this.$log = $log;
            this.mainNavigation = mainNavigation;
            this[Symbol.toStringTag] = app.MAIN_CONTROLLER_NAME;
            $scope.pageTitle = mainNavigation.pageTitle();
            this.onSubTitleChanged(mainNavigation.pageSubTitle());
            mainNavigation.onPageTitleChanged($scope, this.onTitleChanged, this);
        }
        static baseGetControllerInjectable(c) {
            return ['$scope', '$window', '$log', app.MAIN_NAV_SERVICE_NAME, c];
        }
        onTitleChanged(newValue) { this.$scope.pageTitle = newValue; }
        onSubTitleChanged(newValue) {
            this.$scope.subTitle = (typeof newValue === "string") ? newValue : "";
            this.$scope.showSubtitle = this.$scope.subTitle.length > 0;
        }
        $doCheck() { }
    }
    app.mainControllerBase = mainControllerBase;
})(app || (app = {}));
