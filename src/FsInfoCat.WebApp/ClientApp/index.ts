module index {
    class MainController extends app.mainControllerBase<app.IMainScope> {
        static getControllerInjectable(): ng.Injectable<ng.IControllerConstructor> {
            return MainController.baseGetControllerInjectable(MainController);
        }

        constructor($scope: app.IMainScope, $window: ng.IWindowService, $log: ng.ILogService, mainNavigation: app.mainNavigationService) {
            super($scope, $window, $log, mainNavigation);
        }
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
                .when('/home', { templateUrl: 'Template/Home.htm' })
                .otherwise({ redirectTo: '/home' });
        }]);
}
