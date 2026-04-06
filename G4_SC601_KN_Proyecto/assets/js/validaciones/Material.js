$(function() {
    $("#FormCreateMaterial").validate({
        rules: {
            idFamily: {
                required: true
            },
            idParent: {
                required: true
            },
            descriptionM: {
                required: true,
                minlength: 3
            },
            countOrder: {
                required: true,
                number: true,
                min: 1
            },
            Qty: {
                required: true,
                number: true,
                min: 1
            },
            labor: {
                required: true,
                number: true,
                min: 0
            },
            IndOVH: {
                required: true,
                number: true,
                min: 0
            },
            Scrap_Allowance: {
                required: true,
                number: true,
              
            },
            materialPrice: {
                required: true,
                number: true,
                min: 0
            },
            totalVariance: {
                number: true
            }
        },
        messages: {
            idFamily: "Selecciona una familia",
            idParent: "Selecciona un parent",
            descriptionM: {
                required: "La descripción es requerida",
                minlength: "La descripción debe tener al menos 3 caracteres"
            },
            countOrder: {
                required: "La cantidad de orden es requerida",
                number: "Debe ser un número",
                min: "Debe ser mayor a 0"
            },
            Qty: {
                required: "La cantidad es requerida",
                number: "Debe ser un número",
                min: "Debe ser mayor a 0"
            },
            labor: {
                required: "Labor es requerido",
                number: "Debe ser un número",
                min: "No puede ser negativo"
            },
            IndOVH: {
                required: "Indirect OVH es requerido",
                number: "Debe ser un número",
                min: "No puede ser negativo"
            },
            Scrap_Allowance: {
                required: "Scrap Allowance es requerido",
                number: "Debe ser un número",
                min: "No puede ser negativo"
            },
            materialPrice: {
                required: "El precio del material es requerido",
                number: "Debe ser un número",
                min: "No puede ser negativo"
            },
            totalVariance: {
                number: "Debe ser un número"
            }
        },
        errorClass: "text-danger",
        validClass: "text-success",
        errorElement: "span",
        highlight: function(element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function(element) {
            $(element).closest('.form-group').removeClass('has-error');
        }
    });

    


$(function () {

    var table = new DataTable('#tableMaterial', {
        language: {
            url: 'https://cdn.datatables.net/plug-ins/2.3.7/i18n/es-ES.json',
            "emptyTable": "No hay materiales",
        },
        columnDefs: [
            { targets: '_all', className: 'text-start' }
        ]
    });
});
