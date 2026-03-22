// ~/assets/js/validaciones/UserValidations.js

$(function () {
    $("#FormUpdateUser").validate({
        rules: {
            Nombre: {
                required: true,
              
                maxlength: 40
            },
            Apellido1: {
                required: true,
              
                maxlength: 30
            },
            UsuarioLogin: {
                required: true,
                minlength: 8,
                maxlength: 50
            },
            Email: {
                required: true,
                email: true,
                maxlength: 100
            }
        },
        messages: {
            Nombre: {
                required: "El nombre es obligatorio",
            
                maxlength: "Máximo 40 caracteres"
            },
            Apellido1: {
                required: "El apellido es obligatorio",

                maxlength: "Máximo 30 caracteres"
            },
            UsuarioLogin: {
                required: "El usuario es obligatorio",
                minlength: "Mínimo 8 caracteres",
                maxlength: "Máximo 50 caracteres"
            },
            Email: {
                required: "El correo es obligatorio",
                email: "Correo no válido",
                maxlength: "Máximo 100 caracteres"
            }
        },
        errorElement: "span",
        errorClass: "text-danger small d-block mt-1",
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
        }
    });
});