class pageAuthReminder {
    /*
     * Init Password Reminder Form Validation, for more examples you can check out https://github.com/jzaefferer/jquery-validation
     *
     */
    static initValidation() {
        // Load default options for jQuery Validation plugin
        Mazek.helpers('validation');

        // Init Form Validation
        jQuery('.js-validation-reminder').validate({
            rules: {
                'credential': {
                    required: true,
                    minlength: 3
                }
            },
            messages: {
                'credential': {
                    required: 'Please enter a valid credential'
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
jQuery(() => { pageAuthReminder.init(); });
