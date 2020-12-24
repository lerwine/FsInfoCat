var appError;
(function (appError) {
    class MainController {
        constructor($scope, $log, mainNavigation) {
            this.$scope = $scope;
            this.$log = $log;
            this.mainNavigation = mainNavigation;
            this[Symbol.toStringTag] = app.MAIN_CONTROLLER_NAME;
        }
        static getControllerInjectable() {
            return ['$scope', '$log', app.MAIN_NAV_SERVICE_NAME];
        }
        $doCheck() { }
    }
    /**
    * The main module for this app.
    * @type {ng.IModule}
    */
    appError.mainModule = rootBroadcaster.register(angular.module(app.MAIN_MODULE_NAME, ['ngRoute']))
        .provider(app.MAIN_NAV_SERVICE_NAME, app.mainNavigationServiceProvider)
        .controller(app.MAIN_CONTROLLER_NAME, MainController.getControllerInjectable())
        .config(['$routeProvider', '$locationProvider', app.MAIN_NAV_PROVIDER_NAME, function ($routeProvider, mainNavigationProvider) {
            $routeProvider
                .when('/error', { templateUrl: 'Template/Error.htm' })
                .when('/denied', { templateUrl: 'Template/Denied.htm' })
                .otherwise({ redirectTo: '/error' });
        }]);
})(appError || (appError = {}));
