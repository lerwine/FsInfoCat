var login;
(function (login) {
    class MainController {
        constructor($scope, $window, $log) {
            this.$scope = $scope;
            this.$window = $window;
            this.$log = $log;
            this[Symbol.toStringTag] = app.MAIN_CONTROLLER_NAME;
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
                    this.loginError = "Username not provided";
                    $scope.loginButtonDisabled = true;
                }
                else if (app.isNilOrWhiteSpace(this.$scope.password)) {
                    this.loginError = "Password not provided";
                    $scope.loginButtonDisabled = true;
                }
                else {
                    this.loginError = "";
                    $scope.loginButtonDisabled = false;
                }
                $scope.hasLoginErrorMessageDetail = false;
            });
            $scope.$watch("password", () => {
                $log.debug("Watch raised for 'mainController.password'");
                if (!app.isNilOrWhiteSpace(this.$scope.userName)) {
                    if (app.isNilOrWhiteSpace(this.$scope.password)) {
                        this.loginError = "Password not provided";
                        $scope.loginButtonDisabled = true;
                    }
                    else {
                        this.loginError = "";
                        $scope.loginButtonDisabled = false;
                    }
                    $scope.hasLoginErrorMessageDetail = false;
                }
            });
        }
        get loginError() { return this._loginError; }
        set loginError(value) {
            this._loginError = value;
            let message, details;
            if (app.isNilOrWhiteSpace(value)) {
                if (this.$scope.hasErrorMessage !== false)
                    this.$scope.hasErrorMessage = false;
                if (this.$scope.hasLoginErrorMessageDetail !== false)
                    this.$scope.hasLoginErrorMessageDetail = false;
                details = message = "";
            }
            else {
                if (typeof value === "string")
                    message = value;
                else {
                    let m = value.message;
                    let name = value.name;
                    if (typeof m === "string" && (m = m.trim()).length > 0) {
                        message = m;
                        details = value.data;
                        m = value.name;
                        if (typeof m === "string" && (m = m.trim()).length > 0)
                            name = m;
                    }
                    else if (typeof (m = value.name) === "string" && (m = m.trim()).length > 0)
                        name = m;
                    if (typeof message !== "string") {
                        if (typeof name === "string") {
                            details = value;
                            message = name;
                        }
                        else
                            message = angular.toJson(value);
                    }
                    else if (typeof name === "string")
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
                }
                else if (this.$scope.hasLoginErrorMessageDetail !== false)
                    this.$scope.hasLoginErrorMessageDetail = false;
                if (this.$scope.hasErrorMessage !== true)
                    this.$scope.hasErrorMessage = true;
            }
            if (this.$scope.errorMessage !== message)
                this.$scope.errorMessage = message;
            if (this.$scope.loginErrorDetail !== details)
                this.$scope.loginErrorDetail = details;
        }
        static getControllerInjectable() {
            return ['$scope', '$window', '$log', MainController];
        }
        doLogin(event) {
            try {
                app.preventEventDefault(event, true);
            }
            finally {
                this.$scope.loginButtonDisabled = true;
                this.$scope.inputControlsDisabled = true;
                this.$scope.hasErrorMessage = false;
                this.$scope.hasLoginErrorMessageDetail = false;
                const item = {
                    loginName: this.$scope.userName,
                    password: this.$scope.password
                };
                fetch("api/Account/login", {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(item)
                })
                    .then(response => response.json())
                    .then((data) => {
                    if (data.success) {
                        this.$window.location.href = 'index.html';
                    }
                    else {
                        this.$scope.loginButtonDisabled = false;
                        this.$scope.inputControlsDisabled = false;
                        this.loginError = data.message;
                    }
                    return data;
                })
                    .catch(error => {
                    this.$scope.loginButtonDisabled = false;
                    this.$scope.inputControlsDisabled = false;
                    this.$log.error('Unexpected error while attempting to log in.', error);
                    this.loginError = error;
                });
            }
        }
        $doCheck() { }
    }
    /**
    * The main module for this app.
    * @type {ng.IModule}
    */
    login.mainModule = rootBroadcaster.register(angular.module(app.MAIN_MODULE_NAME, []))
        .controller(app.MAIN_CONTROLLER_NAME, MainController.getControllerInjectable());
})(login || (login = {}));
