var appError;
(function (appError) {
    class mainController extends app.mainControllerBase {
        constructor($scope, mainNavigation) {
            super($scope, mainNavigation);
        }
    }
    /**
    * The main module for this app.
    * @type {ng.IModule}
    */
    appError.mainModule = rootBroadcaster.register(angular.module(app.MAIN_MODULE_NAME, ['ngRoute']))
        .provider(app.MAIN_NAV_SERVICE_NAME, app.mainNavigationServiceProvider)
        .controller(app.MAIN_CONTROLLER_NAME, ['$scope', app.MAIN_NAV_SERVICE_NAME, mainController])
        .config(['$routeProvider', '$locationProvider', app.MAIN_NAV_PROVIDER_NAME, function ($routeProvider, mainNavigationProvider) {
            $routeProvider
                .when('/error', { templateUrl: 'Template/Error.htm' })
                .when('/denied', { templateUrl: 'Template/Denied.htm' })
                .otherwise({ redirectTo: '/error' });
        }]);
})(appError || (appError = {}));
