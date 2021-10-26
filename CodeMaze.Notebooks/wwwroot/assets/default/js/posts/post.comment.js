$(function () {
    jQuery('.from-comment-text').focus(function () {
        hideCommentForm();
        let _this = this;
        jQuery(_this).hide();
        let formAppend = jQuery(_this).closest('.comment-form');
        let template = jQuery('#post-comment-template')[0].innerHTML;
        formAppend.append(template);

        validateCMT();

        setTimeout(function () {
            formAppend.find('input[name="name"]').focus();
        }, 100);
    });

    jQuery(document).on('click', '.btn-comment-submit', function (e) {
        e.preventDefault();

        let valid = $("#comment-form").valid();
        if (typeof valid != 'undefined' && valid == true) {
            let tokenValue = jQuery("input[name='CSRF-ZANEK-FORM']").val();
            let token = { "Zanek-Token": tokenValue };

            let data = new FormData(jQuery("#comment-form")[0]);
            jQuery.ajax({
                url: "/api/comments/add",
                type: "POST",
                data: data,
                dataType: "json",
                contentType: false,
                processData: false,
                cache: false,
                headers: token,
                success: function (response) {
                    console.log(response);
                    if (typeof response != 'undefined' && response.Successed == true) {
                        Mazek.helpers('notify', { type: 'success', icon: 'fa fa-check mr-1', message: 'Your comment is saved, please wait approve, thanks!' });
                        setTimeout(function () {
                            hideCommentForm();
                            // location.reload();
                        }, 3000);
                    } else {
                        let message = 'Save your comment is failed, please check and try again!';
                        Mazek.helpers('notify', { type: 'danger', icon: 'fa fa-times mr-1', message: message });
                    }
                },
                error: function (response) {
                    let message = 'Save your comment is failed, please check and try again!';
                    if (typeof response != 'undefined' && response.Successed == false) {
                        message = response.Message;
                    }
                    Mazek.helpers('notify', { type: 'danger', icon: 'fa fa-times mr-1', message: message });
                }
            });
        }
    });

    jQuery(document).on('click', '.btn-comment-reply', function (e) {
        hideCommentForm();

        var _this = this;
        hideCommentForm();
        var commentId = jQuery(_this).attr('data-id');
        var commnetFor = jQuery(_this).closest('.media-body');
        var form = jQuery('#post-comment-template')[0].innerHTML;
        jQuery.find('inline-comment').forEach(function (element, index) {
            jQuery(element).empty();
        });
        commnetFor.find('inline-comment').first().html(form).find('input[name="CommentId"]').val(commentId);

        validateCMT();

        setTimeout(function () {
            commnetFor.find('input[name="name"]').focus();
        }, 10);
    });

    jQuery(document).on('click', '.btn-comment-cancel', function (e) {
        hideCommentForm();
    });

    function hideCommentForm() {
        var cmtF = jQuery('.comment-form').find('#comment-form');
        if (typeof cmtF != 'undefined' && cmtF.length == 1) {
            jQuery('.from-comment-text').show();
            cmtF.remove();
        }
        jQuery('.comment-data').find('#comment-form').remove();
    }

    var validateCMT = function () {
        // Load default options for jQuery Validation plugin
        jQuery("#comment-form").validate({
            rules: {
                name: {
                    required: true,
                },
                email: {
                    required: true,
                    email: true
                },
                comment: {
                    required: true,
                    minlength: 20
                }
            },
            messages: {
                name: "Please specify your name",
                email: {
                    required: "We need your email address to contact you",
                    email: "Your email address must be in the format of name@domain.com"
                },
                comment: {
                    required: "Please enter your comment",
                    minlength: jQuery.validator.format("At least {0} characters required!")
                }
            }
        });

        Mazek.helpers('validation');
    }
});