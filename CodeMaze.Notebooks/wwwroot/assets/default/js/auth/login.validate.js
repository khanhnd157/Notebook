class pageAuthLogin{
    /*
     * Init Sign In Form Validation, for more examples you can check out https://github.com/jzaefferer/jquery-validation
     *
     */
    static initValidation() {
        // Load default options for jQuery Validation plugin
        Mazek.helpers('validation');

        // Init Form Validation
        jQuery('.js-validation-login').validate({
            rules: {
                'username': {
                    required: true,
                    minlength: 3
                },
                'password': {
                    required: true,
                    minlength: 5
                }
            },
            messages: {
                'username': {
                    required: 'Please enter a username or email',
                    minlength: 'Your username must consist of at least 3 characters'
                },
                'password': {
                    required: 'Please provide a password',
                    minlength: 'Your password must be at least 5 characters long'
                }
            }
        });
    }

    /*
     * Init functionality
     *
     */
    static init() {
        this.initValidation();
    }
}

// Initialize when page loads
jQuery(() => { pageAuthLogin.init(); });