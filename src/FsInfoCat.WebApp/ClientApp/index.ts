module index {
    class mainController extends app.mainControllerBase<app.IMainScope> {
        constructor($scope: app.IMainScope, mainNavigation: app.mainNavigationService) {
            super($scope, mainNavigation);
        }
    }

    /**
    * The main module for this app.
    * @type {ng.IModule}
    */
    export let mainModule: ng.IModule = rootBroadcaster.register(angular.module(app.MAIN_MODULE_NAME, ['ngRoute']))
        .provider(app.MAIN_NAV_SERVICE_NAME, app.mainNavigationServiceProvider)
        .controller(app.MAIN_CONTROLLER_NAME, ['$scope', app.MAIN_NAV_SERVICE_NAME, mainController])
        .config(['$routeProvider', '$locationProvider', app.MAIN_NAV_PROVIDER_NAME, function ($routeProvider: ng.route.IRouteProvider, mainNavigationProvider: app.mainNavigationServiceProvider): void {
            $routeProvider
                .when('/home', { templateUrl: 'Template/Home.htm' })
                .when('/error', { templateUrl: 'Template/Error.htm' })
                .otherwise({ redirectTo: '/home' });
        }]);
}
