ajaxlogin.User = function () {
    var self = this;

    self.Email = ko.observable();

    self.IsAuthenticated = ko.observable(false);

    self.Update = function (data) {
        self.Email(data.Email);
        self.IsAuthenticated(data.IsAuthenticated);
    };
};

ajaxlogin.Account = function () {
    var self = this;

    self.User = new ajaxlogin.User();

    self.LogInDetails = {
        Email: ko.observable(),
        Password: ko.observable(),
        RememberMe: ko.observable(false)
    };

    self.SignUpDetails = {
        Email: ko.observable(),
        Password: ko.observable(),
        ConfirmPassword: ko.observable(),
        AgreeToLicence: ko.observable(false)
    };

    self.LogOut = function () {
        ajaxlogin.Xhr.Post(ajaxlogin.Routes.AccountLogOut, self.Refresh);
    };

    // The Xhr version of Log in is not currently used. As of 07/Oct/2012, Loggin in is done by a HTTP post
    self.LogIn = function () {
        ajaxlogin.Xhr.Post(
            ajaxlogin.Routes.AccountLogIn,
            {
                email: self.LogInDetails.Email(),
                password: self.LogInDetails.Password(),
                rememberMe: self.LogInDetails.RememberMe()
            },
            function (data) {
                if (data.IsLoggedIn) {
                    self.Refresh();
                } else {
                    self.OnError.LogIn && self.OnError.LogIn({ Email: data.Error, Password: data.Error });
                }
            });
    };

    self.SignUp = function () {
        ajaxlogin.Xhr.Post(
            ajaxlogin.Routes.AccountSignUp,
            {
                email: self.SignUpDetails.Email(),
                password: self.SignUpDetails.Password(),
                confirmPassword: self.SignUpDetails.ConfirmPassword()
            },
            function (data) {
                if (data.IsRegistered) {
                    self.Refresh();
                } else {
                    self.OnError.SignUp && self.OnError.SignUp({ Email: data.Error });
                }
            });
    };

    self.OnError = {};

    self.Refresh = function () {
        self.LogInDetails.Email(null);
        self.LogInDetails.Password(null);
        self.SignUpDetails.Email(null);
        self.SignUpDetails.Password(null);
        self.SignUpDetails.ConfirmPassword(null);
        ajaxlogin.Xhr.Get(ajaxlogin.Routes.AccountDetails, self.User.Update);
    };
};