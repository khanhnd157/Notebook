// SweetAlert2, for more examples you can check out https://github.com/sweetalert2/sweetalert2

var SweetalertExtend = (function () {
    // Set default properties
    var toast = Swal.mixin({
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-success m-1',
            cancelButton: 'btn btn-danger m-1',
            input: 'form-control'
        }
    });

    var typeMsg = {
        success: 'success',
        warning: 'warning',
        info: 'info',
        error: 'error',
        question: 'question'
    }

    function Success(message) {
        if (typeof message == 'undefined' || message == '') {
            message = 'Everything saved success!';
        }

        return toast.fire('Success', message, typeMsg.success);
    }


    function Error(message) {
        if (typeof message == 'undefined' || message == '') {
            message = 'Something went wrong!';
        }

        return toast.fire('Oops...', 'Something went wrong!', typeMsg.error);
    }

    function Warning(message) {
        if (typeof message == 'undefined' || message == '') {
            message = 'Something needs your attention!';
        }

        return toast.fire('Warning', message, typeMsg.warning);
    }

    function Info(message) {
        if (typeof message == 'undefined' || message == '') {
            message = 'Just an informational message!';
        }

        return toast.fire('Info', message, typeMsg.info);
    }

    function Question(message) {
        if (typeof message == 'undefined' || message == '') {
            message = 'Are you sure about that?';
        }

        return toast.fire('Question', message, typeMsg.question);
    }

    function ConfirmDelete(message, fucSuccess) {
        if (typeof message == 'undefined' || message.length == 0) {
            message = 'Are you sure you want to permanently remove this item?';
        }

        return toast.fire({
            title: 'Are you sure?',
            text: message,
            icon: 'warning',
            showCancelButton: true,
            customClass: {
                confirmButton: 'btn btn-danger m-1',
                cancelButton: 'btn btn-secondary m-1'
            },
            confirmButtonText: 'Yes, delete it!',
            html: false,
            preConfirm: e => {
                return new Promise(resolve => {
                    setTimeout(() => {
                        resolve();
                    }, 50);
                });
            }
        }).then(result => {
            if (result.value && fucSuccess()) {
                return toast.fire('Deleted!', 'Your item has been deleted.', 'success');
            }
            return false;
        });
    }

    return {
        Init: function () {
            return {
                Info: Info,
                Success: Success,
                Warning, Warning,
                Error: Error,
                Question: Question,
                ConfirmDelete: ConfirmDelete
            }
        }
    }

})();
// Initialize when page loads

$(function () {
    window.Sweetalert = SweetalertExtend.Init();
});