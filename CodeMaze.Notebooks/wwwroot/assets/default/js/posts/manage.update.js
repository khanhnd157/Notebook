var PostManage = (function () {
    var c_visibility = $('#post-visibility');
    var gr_protected = $('#group-post-protected');

    //function initCkEditor() {
    //    CKEDITOR.editorConfig = function (config) {
    //        config.toolbarGroups = [
    //            { name: 'document', groups: ['mode', 'document', 'doctools'] },
    //            { name: 'clipboard', groups: ['clipboard', 'undo'] },
    //            { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
    //            { name: 'forms', groups: ['forms'] },
    //            '/',
    //            { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
    //            { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
    //            { name: 'links', groups: ['links'] },
    //            { name: 'insert', groups: ['insert'] },
    //            '/',
    //            { name: 'styles', groups: ['styles'] },
    //            { name: 'colors', groups: ['colors'] },
    //            { name: 'tools', groups: ['tools'] },
    //            { name: 'others', groups: ['others'] }
    //        ];
    //        config.removeButtons = 'Language,About';
    //        config.height = '680';
    //    };
    //}

    function initPostDropzone() {
        let dropzoneElement = document.querySelector("#post-thumbnail-dropzone");
        var postDropzone = dropzoneElement.dropzone;

        postDropzone.options.url = "/storage/media/upload?type=post_thumbnail";

        postDropzone.options.addRemoveLinks = true;

        postDropzone.on('removedfile', function (file) {
            document.querySelector('.post-image-thumbnail').value = '';
        });

        postDropzone.on('success', function (file, response) {
            file.previewElement.classList.add("dz-success");
            $('.post-image-thumbnail').val(response);
        });

        postDropzone.on('processing', function (file) {
            console.log('addedfile');
            postDropzone.removeAllFiles();
        });

        let img = document.querySelector('.post-image-thumbnail').value;

        if (typeof img != 'undefined') {

            let mockFile = { name: "priview.png", size: 500, type: 'image/jpeg', accepted: true };

            postDropzone.emit("addedfile", mockFile);
            postDropzone.emit("thumbnail", mockFile, img);
            postDropzone.emit("complete", mockFile);
            postDropzone.emit("complete", mockFile);
            postDropzone.emit("addRemoveLinks", mockFile);
        }
    }

    function setSelected() {
        let select = $('.custom-select');

        select.find("option").filter(function (i, e) {
            if (select.attr('data-select') == $(e).val()) {
                $(e).prop('selected', true);
                $(e).attr('selected', 'selected');
            } else {
                $(e).prop('selected', false);
                $(e).removeAttr('selected');
            }
        });
    }

    function setChecked() {
        let checkbox = $('.custom-checkbox').find("input[type='checkbox']");

        checkbox.filter(function (i, e) {
            if ($(e).attr('data-checkbox') == 'True' || $(e).attr('data-checkbox') == 'true') {
                $(e).attr('checked', 'checked');
                $(e).prop('checked', true);
            } else {
                $(e).removeAttr('checked');
                $(e).prop('checked', false);
            }
        });
    }

    function eventProtected_Changed() {

        $(document).on('change', '#post-visibility', function () {

            let visibility = $(this).val().toLowerCase();

            showHideFormProteced(visibility);
        })
    }

    function setStateFormVisibility() {
        let visibility = c_visibility.attr('data-selected').toLowerCase();

        if (typeof visibility == 'undefined') visibility = 'publish';

        c_visibility.val(visibility); // Select the option with a value of '1'
        c_visibility.trigger('change'); // Notify any JS components that the value changed

        showHideFormProteced(visibility); // Manualy raise event show or hide form protected
    }

    function showHideFormProteced(visibility = 'publish') {
        let type_protect = "protected";
        if (visibility == type_protect)
            gr_protected.show();
        else
            gr_protected.hide();
    }

    function initialize() {
        Mazek.helpers(['ckeditor', 'select2']);

        setStateFormVisibility();

        initPostDropzone();
        setSelected();
        setChecked();

        eventProtected_Changed();
    }

    return {
        Init: initialize
    }
});

jQuery(() => { PostManage().Init() });