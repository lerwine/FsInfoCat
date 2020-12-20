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
    const MAIN_MODULE_NAME = "mainModule";
    const MAIN_CONTROLLER_NAME = "mainController";
    const MAIN_NAV_SERVICE_NAME = "mainNavigationService";
    const MAIN_NAV_PROVIDER_NAME = MAIN_NAV_SERVICE_NAME + "Provider";
    class mainNavigationServiceProvider {
        constructor() {
            this[Symbol.toStringTag] = MAIN_NAV_PROVIDER_NAME;
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
    class mainNavigationService {
        constructor($document, rootBroadcaster, _currentItemClass, _currentParentClass, _otherItemClass) {
            this.$document = $document;
            this.rootBroadcaster = rootBroadcaster;
            this._currentItemClass = _currentItemClass;
            this._currentParentClass = _currentParentClass;
            this._otherItemClass = _otherItemClass;
            this[Symbol.toStringTag] = MAIN_NAV_SERVICE_NAME;
            let element = $document.find('head').find('title');
            if (element.length == 0)
                element = $document.find('head').add('<title></title>');
            let pageTitle = element.text();
            if (typeof pageTitle !== "string" || (pageTitle = pageTitle.trim()).length == 0) {
                pageTitle = 'Lenny\'s GitHub Pages';
                element.text(pageTitle);
            }
            this._pageTitle = this._defaultPageTitle = pageTitle;
            this._titleChangedEvent = this.rootBroadcaster.registerPrivateEventName(MAIN_NAV_SERVICE_NAME + ":TitleChanged");
            this._pageChangedEvent = this.rootBroadcaster.registerPrivateEventName(MAIN_NAV_SERVICE_NAME + ":PageChanged");
            this._subTitleChangedEvent = this.rootBroadcaster.registerPrivateEventName(MAIN_NAV_SERVICE_NAME + ":SubTitleChanged");
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
    class mainController {
        constructor($scope, mainNavigation) {
            this.$scope = $scope;
            this.mainNavigation = mainNavigation;
            this[Symbol.toStringTag] = MAIN_CONTROLLER_NAME;
            $scope.pageTitle = mainNavigation.pageTitle();
            this.onSubTitleChanged(mainNavigation.pageSubTitle());
            mainNavigation.onPageTitleChanged($scope, this.onTitleChanged, this);
        }
        onTitleChanged(newValue) { this.$scope.pageTitle = newValue; }
        onSubTitleChanged(newValue) {
            this.$scope.subTitle = (typeof newValue === "string") ? newValue : "";
            this.$scope.showSubtitle = this.$scope.subTitle.length > 0;
        }
        $doCheck() { }
    }
    /**
    * The main module for this app.
    * @type {ng.IModule}
    */
    app.mainModule = rootBroadcaster.register(angular.module(MAIN_MODULE_NAME, ['ngRoute']))
        .provider(MAIN_NAV_SERVICE_NAME, mainNavigationServiceProvider)
        .controller(MAIN_CONTROLLER_NAME, ['$scope', MAIN_NAV_SERVICE_NAME, mainController])
        .config(['$routeProvider', '$locationProvider', MAIN_NAV_PROVIDER_NAME, function ($routeProvider, mainNavigationProvider) {
            $routeProvider.when('/home', { template: 'Home sweet home' })
                .when('/error', { templateUrl: 'Template/Error.htm' })
                .otherwise({ redirectTo: '/home' });
        }]);
})(app || (app = {}));
