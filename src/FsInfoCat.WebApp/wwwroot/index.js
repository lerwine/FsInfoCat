var index;
(function (index) {
    class MainController extends app.mainControllerBase {
        static getControllerInjectable() {
            return MainController.baseGetControllerInjectable(MainController);
        }
        constructor($scope, $window, $log, mainNavigation) {
            super($scope, $window, $log, mainNavigation);
        }
    }
    /**
    * The main module for this app.
    * @type {ng.IModule}
    */
    index.mainModule = rootBroadcaster.register(angular.module(app.MAIN_MODULE_NAME, ['ngRoute']))
        .provider(app.MAIN_NAV_SERVICE_NAME, app.mainNavigationServiceProvider)
        .controller(app.MAIN_CONTROLLER_NAME, MainController.getControllerInjectable())
        .config(['$routeProvider', '$locationProvider', app.MAIN_NAV_PROVIDER_NAME, function ($routeProvider, mainNavigationProvider) {
            $routeProvider
                .when('/home', { templateUrl: 'Template/Home.htm' })
                .otherwise({ redirectTo: '/home' });
        }]);
})(index || (index = {}));
