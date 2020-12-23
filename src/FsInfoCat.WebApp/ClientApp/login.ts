module login {
    interface LoginScope extends ng.IScope {
        userName: string;
        password: string;
        hasErrorMessage: boolean;
        hasLoginErrorMessageDetail: boolean;
        errorMessage: string;
        loginErrorDetail: string;
        loginButtonDisabled: boolean;
        inputControlsDisabled: boolean;
    }

    class mainController implements ng.IController {
        readonly [Symbol.toStringTag]: string = app.MAIN_CONTROLLER_NAME;

        private _loginError: any | undefined;
        get loginError(): any | undefined { return this._loginError; }
        set loginError(value: any | undefined) {
            this._loginError = value;
            let message: string | undefined, details: any | undefined;
            if (app.isNilOrWhiteSpace(value)) {
                if (this.$scope.hasErrorMessage !== false)
                    this.$scope.hasErrorMessage = false;
                if (this.$scope.hasLoginErrorMessageDetail !== false)
                    this.$scope.hasLoginErrorMessageDetail = false;
                details = message = "";
            } else {
                if (typeof value === "string")
                    message = value;
                else {
                    let m: any = (<{ [index: string]: any }>value).message;
                    let name: any = (<{ [index: string]: any }>value).name;
                    if (typeof m === "string" && (m = m.trim()).length > 0) {
                        message = m;
                        details = (<{ [index: string]: any }>value).data;
                        m = (<{ [index: string]: any }>value).name;
                        if (typeof m === "string" && (m = m.trim()).length > 0)
                            name = m;
                    } else if (typeof (m = (<{ [index: string]: any }>value).name) === "string" && (m = m.trim()).length > 0)
                        name = m;
                    if (typeof message !== "string") {
                        if (typeof name === "string") {
                            details = value;
                            message = name;
                        } else
                            message = angular.toJson(value);
                    } else if (typeof name === "string")
                        message = name + ": " + name;
                }
                if (app.isNil(details))
                    details = "";
                else if (typeof details !== "string")
                    details = angular.toJson(details);
                else
                    details = details.trim();
                if (app.isNilOrWhiteSpace(message))
                    message = "An unspecified error has occurred.";
                if (details.length === 0) {
                    if (this.$scope.hasLoginErrorMessageDetail !== true)
                        this.$scope.hasLoginErrorMessageDetail = true;
                } else if (this.$scope.hasLoginErrorMessageDetail !== false)
                    this.$scope.hasLoginErrorMessageDetail = false;
                if (this.$scope.hasErrorMessage !== true)
                    this.$scope.hasErrorMessage = true;
            }
            if (this.$scope.errorMessage !== message)
                this.$scope.errorMessage = message;
            if (this.$scope.loginErrorDetail !== details)
                this.$scope.loginErrorDetail = details;
        }

        constructor(private $scope: LoginScope, private $log: ng.ILogService) {
            $scope.password = "";
            $scope.userName = "";
            $scope.hasErrorMessage = true;
            $scope.hasLoginErrorMessageDetail = false;
            $scope.errorMessage = "Username not provided";
            $scope.loginErrorDetail = "";
            $scope.loginButtonDisabled = true;
            $scope.inputControlsDisabled = false;
            $scope.$watch("userName", () => {
                $log.debug("Watch raised for 'mainController.userName'");
                if (app.isNilOrWhiteSpace(this.$scope.userName)) {
                    $scope.errorMessage = "Username not provided";
                    $scope.hasErrorMessage = true;
                    $scope.loginButtonDisabled = true;
                } else if (app.isNilOrWhiteSpace(this.$scope.password)) {
                    $scope.errorMessage = "Password not provided";
                    $scope.hasErrorMessage = true;
                    $scope.loginButtonDisabled = true;
                } else {
                    $scope.hasErrorMessage = false;
                    $scope.loginButtonDisabled = false;
                }
                $scope.hasLoginErrorMessageDetail = false;
            });
            $scope.$watch("password", () => {
                $log.debug("Watch raised for 'mainController.password'");
                if (!app.isNilOrWhiteSpace(this.$scope.userName)) {
                    if (app.isNilOrWhiteSpace(this.$scope.password)) {
                        $scope.errorMessage = "Password not provided";
                        $scope.hasErrorMessage = true;
                        $scope.loginButtonDisabled = true;
                    } else {
                        $scope.hasErrorMessage = false;
                        $scope.loginButtonDisabled = false;
                    }
                    $scope.hasLoginErrorMessageDetail = false;
                }
            });
        }

        doLogin(event: BaseJQueryEventObject): void {
            try { app.preventEventDefault(event, true); }
            finally {
                this.$scope.loginButtonDisabled = true;
                this.$scope.inputControlsDisabled = true;
                this.$scope.hasErrorMessage = false;
                this.$scope.hasLoginErrorMessageDetail = false;
                try {

                } finally {
                    if (this.$scope.hasErrorMessage) {
                        this.$scope.loginButtonDisabled = false;
                        this.$scope.inputControlsDisabled = false;
                    } else {
                        // TODO: Redirect for successful login.
                    }
                }
            }
        }

        $doCheck(): void { }
    }

    /**
    * The main module for this app.
    * @type {ng.IModule}
    */
    export let mainModule: ng.IModule = rootBroadcaster.register(angular.module(app.MAIN_MODULE_NAME, []))
        .controller(app.MAIN_CONTROLLER_NAME, ['$scope', app.MAIN_NAV_SERVICE_NAME, mainController]);
}
