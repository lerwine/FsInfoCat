module appError {
    class MainController implements ng.IController {
        readonly [Symbol.toStringTag]: string = app.MAIN_CONTROLLER_NAME;

        static getControllerInjectable(): ng.Injectable<ng.IControllerConstructor> {
            return ['$scope', '$log', app.MAIN_NAV_SERVICE_NAME];
        }

        constructor(private $scope: app.IMainScope, private $log: ng.ILogService, private mainNavigation: app.mainNavigationService) {
        }

        $doCheck(): void { }
    }

    /**
    * The main module for this app.
    * @type {ng.IModule}
    */
    export let mainModule: ng.IModule = rootBroadcaster.register(angular.module(app.MAIN_MODULE_NAME, ['ngRoute']))
        .provider(app.MAIN_NAV_SERVICE_NAME, app.mainNavigationServiceProvider)
        .controller(app.MAIN_CONTROLLER_NAME, MainController.getControllerInjectable())
        .config(['$routeProvider', '$locationProvider', app.MAIN_NAV_PROVIDER_NAME, function ($routeProvider: ng.route.IRouteProvider, mainNavigationProvider: app.mainNavigationServiceProvider): void {
            $routeProvider
            .when('/error', { templateUrl: 'Template/Error.htm' })
            .when('/denied', { templateUrl: 'Template/Denied.htm' })
                .otherwise({ redirectTo: '/error' });
        }]);
}
