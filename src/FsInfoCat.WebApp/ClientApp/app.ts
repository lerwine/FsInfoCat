
module rootBroadcaster {
    export const SERVICE_NAME: string = "rootBroadcaster";
    const EVENTNAME_ROUTECHANGESUCCESS = "$routeChangeSuccess";

    interface IEventRegistration {
        id?: symbol;
        name: string;
    }

    export type RouteChangeSuccessListener<TRoute extends ng.route.ICurrentRoute> = { (event: ng.IAngularEvent, current?: TRoute, previous?: TRoute): void; } & Function;
    export type ThisRouteChangeSuccessListener<TRoute extends ng.route.ICurrentRoute, TThis> = { (this: TThis, event: ng.IAngularEvent, current?: TRoute, previous?: TRoute): void; } & Function;
    export type EventListener = { (event: ng.IAngularEvent, ...args: any[]): void; } & Function;
    export type ThisEventListener<T> = { (this: T, event: ng.IAngularEvent, ...args: any[]): void; } & Function;

    export function register(module: ng.IModule): ng.IModule { return module.service(SERVICE_NAME, ['$rootScope', Service]); }

    export class Service {
        private _eventRegistration: IEventRegistration[] = [];

        readonly [Symbol.toStringTag]: string = SERVICE_NAME;

        constructor(private $rootScope: ng.IRootScopeService) { }

        registerPrivateEventName(name: string, id?: symbol): symbol {
            if (typeof name !== "string")
                throw new Error("Name must be a string");
            if (name.length == 0)
                throw new Error("Name cannot be empty");
            if (name.startsWith("$"))
                throw new Error("Name cannot start with a \"$\" symbol");
            for (let i: number = 0; i < this._eventRegistration.length; i++) {
                if (this._eventRegistration[i].name === name) {
                    if (typeof id === "symbol" && id === this._eventRegistration[i].id)
                        return id;
                    throw new Error("That event name is already registered");
                }
            }
            let eventRegistration: IEventRegistration = { id: Symbol(), name: name };
            this._eventRegistration.push(eventRegistration);
            return eventRegistration.id;
        }

        registerSharedEventName(name: string): boolean {
            if (typeof name !== "string")
                throw new Error("Name must be a string");
            if (name.length == 0)
                throw new Error("Name cannot be empty");
            if (name.startsWith("$"))
                throw new Error("Name cannot start with a \"$\" symbol");
            for (let i: number = 0; i < this._eventRegistration.length; i++) {
                if (this._eventRegistration[i].name === name) {
                    if (typeof this._eventRegistration[i].id === "symbol")
                        throw new Error("That event name is not available");
                    return false;
                }
            }
            let eventRegistration: IEventRegistration = { name: name };
            this._eventRegistration.push(eventRegistration);
            return true;
        }

        broadcastEvent(id: string | symbol, ...args: any[]): void {
            let name: string | undefined;
            if (typeof id === "symbol")
                for (let i: number = 0; i < this._eventRegistration.length; i++) {
                    if (this._eventRegistration[i].id === id) {
                        name = this._eventRegistration[i].name;
                        break;
                    }
                }
            else if (typeof id === "string")
                for (let i: number = 0; i < this._eventRegistration.length; i++) {
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

        onEvent<T>($scope: ng.IScope, id: string | symbol, listener: ThisEventListener<T>, thisArg: T): void;
        onEvent($scope: ng.IScope, id: string | symbol, listener: EventListener): void;
        onEvent($scope: ng.IScope, id: string | symbol, listener: EventListener | ThisEventListener<any>, thisArg?: any): void {
            let name: string | undefined;
            if (typeof id === "symbol")
                for (let i: number = 0; i < this._eventRegistration.length; i++) {
                    if (this._eventRegistration[i].id === id) {
                        name = this._eventRegistration[i].name;
                        break;
                    }
                }
            else if (typeof id === "string")
                for (let i: number = 0; i < this._eventRegistration.length; i++) {
                    if (this._eventRegistration[i].name === name) {
                        if (typeof this._eventRegistration[i].id !== "symbol")
                            name = this._eventRegistration[i].name;
                        break;
                    }
                }
            if (typeof name !== "string")
                throw new Error((typeof id === "symbol") ? "Event ID not registered" : "Event name not registered or is private");
            if (arguments.length > 3)
                $scope.$on(name, function (event: ng.IAngularEvent, args: any[]): void {
                    if (args.length == 1)
                        listener.call(thisArg, event, args[0]);
                    else if (args.length > 1)
                        listener.apply(thisArg, (args.length == 0) ? [event] : [event].concat(args));
                    else
                        listener.call(thisArg, event);
                });
            else
                $scope.$on(name, function (event: ng.IAngularEvent, args: any[]): void {
                    if (args.length == 1)
                        listener(event, args[0]);
                    else if (args.length > 1)
                        listener.apply(this, [event].concat(args));
                    else
                        listener(event);
                });
        }

        onRouteChangeSuccess<TRoute extends ng.route.ICurrentRoute, TThis>(callback: ThisRouteChangeSuccessListener<TRoute, any>, thisArg: TThis): void;
        onRouteChangeSuccess<T extends ng.route.ICurrentRoute>(callback: RouteChangeSuccessListener<T>): void;
        onRouteChangeSuccess<T extends ng.route.ICurrentRoute>(callback: RouteChangeSuccessListener<T> | ThisRouteChangeSuccessListener<T, any>, thisArg?: any): void {
            if (arguments.length > 1)
                this.$rootScope.$on(EVENTNAME_ROUTECHANGESUCCESS, (event: ng.IAngularEvent, current?: T, previous?: T) => {
                    callback.call(thisArg, event, current, previous);
                });
            else
                this.$rootScope.$on(EVENTNAME_ROUTECHANGESUCCESS, (event: ng.IAngularEvent, current?: T, previous?: T) => {
                    callback(event, current, previous);
                });
        }
    }
}

module app {

    const MAIN_MODULE_NAME: string = "mainModule";
    const MAIN_CONTROLLER_NAME: string = "mainController";
    const MAIN_NAV_SERVICE_NAME: string = "mainNavigationService";
    const MAIN_NAV_PROVIDER_NAME: string = MAIN_NAV_SERVICE_NAME + "Provider";

    export type PageTitleChangedEventListener = { (newValue: string, oldValue: string): void; } & Function;
    export type ThisPageTitleChangedEventListener<T> = { (this: T, newValue: string, oldValue: string): void; } & Function;

    export interface INavigationProperties<T> {
        pageTitle?: string;
        pageSubTitle?: string;
        id?: symbol;
    }
    export interface INavigationMetaData extends INavigationProperties<INavigationDefinition> { }
    export type INavigationDefinition = [string, ng.route.IRoute, INavigationMetaData];
    interface IRouteWithMetaData extends ng.route.IRoute { __metaData: INavigationMetaData }
    interface ICurrentRouteWithMetaData extends ng.route.ICurrentRoute, IRouteWithMetaData { }

    class mainNavigationServiceProvider implements ng.IServiceProvider {
        readonly [Symbol.toStringTag]: string = MAIN_NAV_PROVIDER_NAME;
        private _currentItemClass: string[] = [];
        private _currentParentClass: string[] = [];
        private _otherItemClass: string[] = [];

        readonly $get: any;
        constructor() {
            let p: mainNavigationServiceProvider = this;
            this.$get = ['$document', rootBroadcaster.SERVICE_NAME, function mainNavigationFactory($document: ng.IDocumentService, rootBroadcaster: rootBroadcaster.Service) {
                if (p._currentItemClass.length == 0)
                    p._currentItemClass = ["nav-link", "active"];
                if (p._currentParentClass.length == 0)
                    p._currentParentClass = ["nav-link", "active"];
                if (p._otherItemClass.length == 0)
                    p._otherItemClass = ["nav-link"];
                return new mainNavigationService($document, rootBroadcaster, p._currentItemClass, p._currentParentClass, p._otherItemClass);
            }];
        }
        currentItemClass(...css: string[]): mainNavigationServiceProvider {
            this._currentItemClass = [];
            if (typeof css !== "object" && css === null || css.length == 0)
                return this;
            let r: RegExp = /[\s\r\n]+/;
            css.forEach((value: any) => {
                if (typeof value == "string" && (value = value.trim()).length > 0)
                    value.split(r).forEach((v: string) => { this._currentItemClass.push(v); });
            });
        }
        currentParentClass(...css: string[]): mainNavigationServiceProvider {
            this._currentParentClass = [];
            if (typeof css !== "object" && css === null || css.length == 0)
                return this;
            let r: RegExp = /[\s\r\n]+/;
            css.forEach((value: any) => {
                if (typeof value == "string" && (value = value.trim()).length > 0)
                    value.split(r).forEach((v: string) => { this._currentParentClass.push(v); });
            });
        }
        otherItemClass(...css: string[]): mainNavigationServiceProvider {
            this._otherItemClass = [];
            if (typeof css !== "object" && css === null || css.length == 0)
                return this;
            let r: RegExp = /[\s\r\n]+/;
            css.forEach((value: any) => {
                if (typeof value == "string" && (value = value.trim()).length > 0)
                    value.split(r).forEach((v: string) => { this._otherItemClass.push(v); });
            });
        }
    }

    class mainNavigationService {
        readonly [Symbol.toStringTag]: string = MAIN_NAV_SERVICE_NAME;
        private readonly _titleChangedEvent: symbol;
        private readonly _pageChangedEvent: symbol;
        private readonly _subTitleChangedEvent: symbol;
        private readonly _defaultPageTitle: string;
        private _pageTitle: string;
        private _pageSubTitle: string;
        private _currentPage: INavigationMetaData | undefined;
        defaultPageTitle(): string { return this._defaultPageTitle; }
        pageTitle(value?: string): string {
            if (typeof value === "string") {
                if ((value = value.trim()).length == 0)
                    value = this._defaultPageTitle;
                let oldValue: string = this._pageTitle;
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
        pageSubTitle(value?: string): string {
            if (typeof value === "string") {
                let oldValue: string = this._pageSubTitle;
                if ((value = value.trim()) !== oldValue) {

                    this._pageSubTitle = value;
                    this.rootBroadcaster.broadcastEvent(this._subTitleChangedEvent, value, oldValue);
                }
            }
            return this._pageSubTitle;
        }
        currentItemClass(): string[] { return this._currentItemClass; }
        currentParentClass(): string[] { return this._currentParentClass; }
        otherItemClass(): string[] { return this._otherItemClass; }
        constructor(private $document: ng.IDocumentService, private rootBroadcaster: rootBroadcaster.Service, private _currentItemClass: string[], private _currentParentClass: string[], private _otherItemClass: string[]) {
            let element: JQuery = $document.find('head').find('title');
            if (element.length == 0)
                element = $document.find('head').add('<title></title>');

            let pageTitle: string = element.text();
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
        onPageTitleChanged<T>($scope: ng.IScope, listener: ThisPageTitleChangedEventListener<T>, thisArg: T): void;
        onPageTitleChanged($scope: ng.IScope, listener: PageTitleChangedEventListener): void;
        onPageTitleChanged($scope: ng.IScope, listener: ThisPageTitleChangedEventListener<any> | PageTitleChangedEventListener, thisArg?: any): void {
            if (arguments.length > 2)
                this.rootBroadcaster.onEvent($scope, this._titleChangedEvent, (event: ng.IAngularEvent, newValue: string, oldValue: string): void => { listener.call(thisArg, newValue, oldValue); });
            else
                this.rootBroadcaster.onEvent($scope, this._titleChangedEvent, (event: ng.IAngularEvent, newValue: string, oldValue: string): void => { listener(newValue, oldValue); });
        }
        onPageSubTitleChanged<T>($scope: ng.IScope, listener: ThisPageTitleChangedEventListener<T>, thisArg: T): void;
        onPageSubTitleChanged($scope: ng.IScope, listener: PageTitleChangedEventListener): void;
        onPageSubTitleChanged($scope: ng.IScope, listener: ThisPageTitleChangedEventListener<any> | PageTitleChangedEventListener, thisArg?: any): void {
            if (arguments.length > 2)
                this.rootBroadcaster.onEvent($scope, this._subTitleChangedEvent, (event: ng.IAngularEvent, newValue: string, oldValue: string): void => { listener.call(thisArg, newValue, oldValue); });
            else
                this.rootBroadcaster.onEvent($scope, this._subTitleChangedEvent, (event: ng.IAngularEvent, newValue: string, oldValue: string): void => { listener(newValue, oldValue); });
        }
        private onRouteChangeSuccess(event: ng.IAngularEvent, current?: ICurrentRouteWithMetaData): void {
            if (typeof current !== "object" || current === null)
                return;
            let newPage: INavigationMetaData | undefined = current.__metaData;
            let arr: [INavigationMetaData, INavigationMetaData] | [INavigationMetaData, undefined] | [undefined, INavigationMetaData] | [] = (typeof newPage === 'undefined' || null === newPage) ? ((typeof this._currentPage === 'undefined') ? [] : [this._currentPage, undefined]) : [this._currentPage, newPage];
            if (arr.length == 0)
                return;
            this._currentPage = newPage;
            try { this.pageTitle((typeof arr[0] === "undefined" || arr[0].pageTitle.length == 0) ? this._defaultPageTitle : arr[0].pageTitle); }
            finally {
                try { this.pageTitle((typeof arr[0] === "undefined" || arr[0].pageTitle.length == 0) ? this._defaultPageTitle : arr[0].pageTitle); }
                finally { this.rootBroadcaster.broadcastEvent(this._pageChangedEvent, arr[0], arr[1]); }
            }
        }
    }

    interface IMainScope extends ng.IScope {
        pageTitle: string;
        showSubtitle: boolean;
        subTitle: string;
    }

    class mainController implements ng.IController {
        readonly [Symbol.toStringTag]: string = MAIN_CONTROLLER_NAME;
        constructor(private $scope: IMainScope, private mainNavigation: mainNavigationService) {
            $scope.pageTitle = mainNavigation.pageTitle();
            this.onSubTitleChanged(mainNavigation.pageSubTitle());
            mainNavigation.onPageTitleChanged($scope, this.onTitleChanged, this);
        }
        private onTitleChanged(newValue: string): void { this.$scope.pageTitle = newValue; }
        private onSubTitleChanged(newValue?: string): void {
            this.$scope.subTitle = (typeof newValue === "string") ? newValue : "";
            this.$scope.showSubtitle = this.$scope.subTitle.length > 0;
        }
        $doCheck(): void { }
    }

    /**
    * The main module for this app.
    * @type {ng.IModule}
    */
    export let mainModule: ng.IModule = rootBroadcaster.register(angular.module(MAIN_MODULE_NAME, ['ngRoute']))
        .provider(MAIN_NAV_SERVICE_NAME, mainNavigationServiceProvider)
        .controller(MAIN_CONTROLLER_NAME, ['$scope', MAIN_NAV_SERVICE_NAME, mainController])
        .config(['$routeProvider', '$locationProvider', MAIN_NAV_PROVIDER_NAME, function ($routeProvider: ng.route.IRouteProvider, mainNavigationProvider: mainNavigationServiceProvider): void {
            $routeProvider
                .when('/home', { templateUrl: 'Template/Home.htm' })
                .when('/error', { templateUrl: 'Template/Error.htm' })
                .otherwise({ redirectTo: '/home'});
        }]);
}
