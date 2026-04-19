$(document).ready(function () {

    $("#formSignup").validate({
        rules: {
            Nombre: { required: true },
            Apellido1: { required: true },

            UsuarioLogin: {
                required: true,
                minlength: 4
            },

            Email: {
                required: true,
                email: true,
                remote: {
                    url: "/Home/ValidarEmail",
                    type: "post",
                    data: {
                        Email: function () {
                            return $("#Email").val();
                        }
                    }
                }
            },

            Contrasena: {
                required: true,
                minlength: 6
            },

            ConfirmPassword: {
                required: true,
                equalTo: "#Contrasena"
            }
        },

        messages: {
            Nombre: "Ingrese su nombre",
            Apellido1: "Ingrese su apellido",

            UsuarioLogin: {
                required: "Ingrese un usuario",
                minlength: "Mínimo 4 caracteres"
            },

            Email: {
                required: "Ingrese un correo",
                email: "Formato de correo inválido",
                remote: "Correo existe en base de datos"
            },

            Contrasena: {
                required: "Ingrese una contraseña",
                minlength: "Mínimo 6 caracteres"
            },

            ConfirmPassword: {
                required: "Confirme la contraseña",
                equalTo: "Las contraseñas no coinciden"
            }
        },

        errorClass: "text-danger",
        errorElement: "small",

        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid").addClass("is-valid");
        }
    });

});