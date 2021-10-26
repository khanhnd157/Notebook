$(function () {
    const formUpdate = document.querySelector('#modal-category-update'),
        formCategoryUpdate = formUpdate.querySelector('.form-category-update'),
        modaltitle = formUpdate.querySelector('h5.modal-title'),
        formBody = formUpdate.querySelector('.modal-body'),
        customizeControl = formUpdate.querySelector('#customize-control-for');

    customizeControl.addEventListener('change', (e) => {
        if (e.target.checked == true) {
            $('.group-customize-url').find('input').removeAttr('readonly');
        } else {
            $('.group-customize-url').find('input').prop('readonly', true);
        }
    });

    $(document).on('click', '.btn-category-update', function (e) {
        let _this = this;
        let _action = $(_this).attr('data-action');

        // Init create form
        if (_action == 'create') {
            modaltitle.innerHTML = 'Create Category';
            // Reset to default value form
            resetForm();
            return;
        }

        // Init update form and bind data
        if (_action == 'update') {
            modaltitle.innerHTML = 'Update Category';
            let _data = JSON.parse($(_this).attr('data-update'));
            $('.group-customize-url').show();
            bindingForm(_data);
            return;
        }
    });

    $(document).on('click', '.btn-category-submit', function (e) {
        e.preventDefault();
        let _id = $(formBody).find('input[name="Id"]').val();

        let api = (typeof _id != 'undefined' && _id != '') ? ('/api/category/update/' + _id) : '/api/category/create';

        var data = new FormData(jQuery(".form-category-update")[0]);
        jQuery.ajax({
            url: api,
            type: "POST",
            data: data,
            dataType: "json",
            contentType: false,
            processData: false,
            async: true,
            cache: false,
            success: function (response) {

                if (response != null && response.Successed == true) {
                    jQuery('#modal-category-update').modal('hide');

                    Sweetalert.Success("Data saved success!");

                    $(document).on('mouseup', '.swal2-confirm', function (e) {
                        location.reload();
                    });

                } else {
                    Sweetalert.Error();
                }

            },
            error: function (response) {
                Sweetalert.Error();
            }
        });
    })

    function bindingForm(_data) {
        if (typeof _data != 'undefined') {
            $(formBody).find('input[name="Id"]').val(_data.Id);
            $(formBody).find('#category-name').val(_data.DisplayName);
            $(formBody).find('#category-position').val(_data.Position);
            $(formBody).find('#category-note').val(_data.Note);
            $(formBody).find('#show-on-tab').prop('checked', _data.ShowOnTab);
            $(formBody).find('#publish-for').prop('checked', _data.Publish);
            $(formBody).find('input[name="url"]').val(_data.Url);
            $(formBody).find('input[name="code"]').val(_data.Code);
        }
    }

    function resetForm() {
        $('.group-customize-url').hide();
        $(formBody).find('#category-name').val('');
        $(formBody).find('#category-position').val('0');
        $(formBody).find('#category-note').val('');
        $(formBody).find('#show-on-tab').prop('checked', false);
        $(formBody).find('#publish-for').prop('checked', true);
    }

});