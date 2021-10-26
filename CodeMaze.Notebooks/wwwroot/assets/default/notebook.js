jQuery(function () {
    var notebook = (function () {
        var _access = document.getElementById("note-access");
        var _password = document.getElementById("note-password");
        var _password_group = document.querySelector(".note-password-group");

        function setState() {
            let _access_val = jQuery(_access).val();

            if (_access_val == 'protected') {
                jQuery(_password).prop("required", true);
                jQuery(_password_group).slideDown(500);
            }
            else {
                jQuery(_password).prop("required", false);
                jQuery(_password_group).slideUp(500);
            }
        }

        function selectText(containerid) {
            if (document.selection) { // IE
                var range = document.body.createTextRange();
                range.moveToElementText(document.getElementById(containerid));
                range.select();
            } else if (window.getSelection) {
                var range = document.createRange();
                range.selectNode(document.getElementById(containerid));
                window.getSelection().removeAllRanges();
                window.getSelection().addRange(range);
            }
        }

        function deleteNote(noteId) {
            return $.ajax({
                url: "/api/note/" + noteId,
                dataType: "json",
                method: 'DELETE',
                data: {
                    id: noteId,
                    '__RequestVerificationToken': $("input[name='__RequestVerificationToken']").val()
                }
            }).done(function (res) {
                if (res == true) {
                    let id = '#_id' + noteId;
                    $(id).hide(300);
                    setTimeout(function () {
                        $(id).remove();
                    }, 300);
                }

                return res;
            });
        }

        jQuery(document).on("change", "#note-access", function () {
            setState();
        })

        jQuery(document).on("click", "#btn-copy-clipboard", function () {
            selectText('notebook-content');

            document.execCommand('copy');
        })

        $(document).on('click', '.btn-note-delete', function (e) {
            Sweetalert.ConfirmDelete('', function () {
                let noteId = $(e.target).attr('data-id');
                return deleteNote(noteId);
            });
        });

        function initialize() {
            setState();
        }

        return {
            Init: initialize
        }
    });

    notebook().Init();
})