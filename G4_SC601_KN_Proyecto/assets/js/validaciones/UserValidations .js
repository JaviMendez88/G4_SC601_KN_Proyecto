$(function () {
    $("FormUpdateUser").validate({
        rules: {
            Nombre: {
                required: true
            },
            Apellido1: {
                requerid: true
            },
            UsuarioLogin: {
                required: true
            },
            Email: {
                required: true,
                email: true
            }
        },
        messages: {
            Nombre: {
                required: "Campo Requerido"
            },
            Apellido1: {
                required: "Campo Requerido"
            },
            UsuarioLogin: {
                required: "Campo Requerido"
            },
            Email: {
                required: "Campo Requerido",
                email: "Ingrese un correo electrónico válido"
            }
            
        },
        errorElement: "span",
        errorClass: "text-danger",
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
        }
    });
    });