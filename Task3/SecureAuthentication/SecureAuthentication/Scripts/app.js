﻿function ViewModel() {
    var self = this;

    var tokenKey = 'accessToken';

    self.result = ko.observable();
    self.user = ko.observable();

    self.registerEmail = ko.observable();
    self.registerPassword = ko.observable();
    self.registerPassword2 = ko.observable();

    self.loginEmail = ko.observable();
    self.loginPassword = ko.observable();
    self.errors = ko.observableArray([]);

    function showError(jqXHR) {

        self.result(jqXHR.status + ': ' + jqXHR.statusText);

        var response = jqXHR.responseJSON;
        if (response) {
            if (response.Message) self.errors.push(response.Message);
            if (response.ModelState) {
                var modelState = response.ModelState;
                for (var prop in modelState) {
                    if (modelState.hasOwnProperty(prop)) {
                        var msgArr = modelState[prop]; // expect array here
                        if (msgArr.length) {
                            for (var i = 0; i < msgArr.length; ++i) self.errors.push(msgArr[i]);
                        }
                    }
                }
            }
            if (response.error) self.errors.push(response.error);
            if (response.error_description) self.errors.push(response.error_description);
        }
    }

    self.callApi = function () {
        self.result('');
        self.errors.removeAll();

        // If we already have a bearer token, set the Authorization header.
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: '/api/values',
            headers: headers
        }).done(function (data) {
            self.result(data);
        }).fail(showError);
    }

    self.register = function () {
        grecaptcha.ready(function () {
            grecaptcha.execute('6LdTSSUaAAAAAF7ELSG0Y_7xxDbH6dZbFLl7Q-rg', { action: 'submit' }).then(function (token) {
                // Add your logic to submit to your backend server here.
                console.log(token);
                var data = {
                    Token: token
                };
                //backend verification 
                $.ajax({
                    type: 'POST',
                    url: `/api/Account/Verify`,
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(data)
                }).done(function (data) {
                    console.log("Register verification result");
                    console.log(data);
                    if (data.success) {
                        self.result('');
                        self.errors.removeAll();

                        var data = {
                            Email: self.registerEmail(),
                            Password: self.registerPassword(),
                            ConfirmPassword: self.registerPassword2()
                        };

                        $.ajax({
                            type: 'POST',
                            url: `/api/Account/Register`,
                            contentType: 'application/json; charset=utf-8',
                            data: JSON.stringify(data)
                        }).done(function (data) {
                            self.result("Done!");
                        }).fail(showError);
                    }
                    else {
                        alert("The current environment is not secure. Please change browser or connect to private network for secure registration :)");
                    }
                }).fail(showError);
            });
        });

    }

    self.login = function () {
        grecaptcha.ready(function () {
            grecaptcha.execute('6LdTSSUaAAAAAF7ELSG0Y_7xxDbH6dZbFLl7Q-rg', { action: 'submit' }).then(function (token) {
                // Add your logic to submit to your backend server here.
                console.log(token);
                var data = {
                    Token: token
                };
                //backend verification 
                $.ajax({
                    type: 'POST',
                    url: `/api/Account/Verify`,
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(data)
                }).done(function (data) {
                    console.log("login verification result");
                    if (data.success) {
                        self.result('');
                        self.errors.removeAll();

                        var loginData = {
                            grant_type: 'password',
                            username: self.loginEmail(),
                            password: self.loginPassword()
                        };

                        $.ajax({
                            type: 'POST',
                            url: '/Token',
                            data: loginData
                        }).done(function (data) {
                            self.user(data.userName);
                            // Cache the access token in session storage.
                            sessionStorage.setItem(tokenKey, data.access_token);
                        }).fail(showError);
                    }
                    else {
                        alert("The current environment is not secure. Please change browser or connect to private network for secure login :)");
                    }
                }).fail(showError);
            });
        });
    }

    self.logout = function () {
        // Log out from the cookie based logon.
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'POST',
            url: '/api/Account/Logout',
            headers: headers
        }).done(function (data) {
            // Successfully logged out. Delete the token.
            self.user('');
            sessionStorage.removeItem(tokenKey);
        }).fail(showError);
    }
}

var app = new ViewModel();
ko.applyBindings(app);