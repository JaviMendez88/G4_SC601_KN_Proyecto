// Crear familia
$(document).ready(function () {
    $('#createFamilyForm').submit(function (e) {
        e.preventDefault();

        var formData = {
            title: $('#familyTitle').val(),
            __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            url: '/Material/CreateFamily',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    // Limpiar formulario
                    $('#createFamilyForm')[0].reset();
                    $('#errorMessageFamily').addClass('d-none');

                    // Cerrar modal
                    var modalEl = document.getElementById('createFamilyModal');
                    if (modalEl) {
                        var modal = bootstrap.Modal.getOrCreateInstance(modalEl);
                        modal.hide();
                    }

                    // Mostrar alerta de éxito
                    Swal.fire({
                        icon: 'success',
                        title: '¡Éxito!',
                        text: 'Familia creada exitosamente',
                        confirmButtonText: 'OK'
                    }).then(function() {
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message
                    });
                    $('#errorMessageFamily').removeClass('d-none').text(response.message);
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al crear la familia. Intente de nuevo.'
                });
                $('#errorMessageFamily').removeClass('d-none').text('Error al crear la familia. Intente de nuevo.');
            }
        });
    });
});


// Crear parent
$(document).ready(function () {
    $('#createParentForm').submit(function (e) {
        e.preventDefault();

        var formData = {
            codigo: $('#parentCodigo').val(),
            __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            url: '/Material/CreateParent',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    // Limpiar formulario
                    $('#createParentForm')[0].reset();
                    $('#errorMessageParent').addClass('d-none');

                    // Cerrar modal
                    var modalEl = document.getElementById('createParentModal');
                    if (modalEl) {
                        var modal = bootstrap.Modal.getOrCreateInstance(modalEl);
                        modal.hide();
                    }

                    // Mostrar alerta de éxito
                    Swal.fire({
                        icon: 'success',
                        title: '¡Éxito!',
                        text: 'Parent creado exitosamente',
                        confirmButtonText: 'OK'
                    }).then(function() {
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message
                    });
                    $('#errorMessageParent').removeClass('d-none').text(response.message);
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al crear el parent. Intente de nuevo.'
                });
                $('#errorMessageParent').removeClass('d-none').text('Error al crear el parent. Intente de nuevo.');
            }
        });
    });
});

// Eliminar familia
function deleteFamily(id) {
    Swal.fire({
        title: '¿Está seguro?',
        text: '¿Desea eliminar esta familia?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            var token = $('[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: '/Material/DeleteFamily',
                type: 'POST',
                data: { id: id, __RequestVerificationToken: token },
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        $('#family-row-' + id).fadeOut(function () {
                            $(this).remove();
                        });
                        Swal.fire({
                            icon: 'success',
                            title: '¡Eliminado!',
                            text: response.message,
                            confirmButtonText: 'OK'
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message
                        });
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error al eliminar la familia.'
                    });
                }
            });
        }
    });
}

// Eliminar parent
function deleteParent(id) {
    Swal.fire({
        title: '¿Está seguro?',
        text: '¿Desea eliminar este parent?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            var token = $('[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: '/Material/DeleteParent',
                type: 'POST',
                data: { id: id, __RequestVerificationToken: token },
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        $('#parent-row-' + id).fadeOut(function () {
                            $(this).remove();
                        });
                        Swal.fire({
                            icon: 'success',
                            title: '¡Eliminado!',
                            text: response.message,
                            confirmButtonText: 'OK'
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message
                        });
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error al eliminar el parent.'
                    });
                }
            });
        }
    });
}
