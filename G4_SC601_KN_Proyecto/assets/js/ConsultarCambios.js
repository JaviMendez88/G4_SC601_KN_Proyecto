$(function () {

    var table = new DataTable('#tablaCambios', {
        language: {
            url: 'https://cdn.datatables.net/plug-ins/2.3.7/i18n/es-ES.json',
            "emptyTable": "No hay registros de auditoría",
        },
        columnDefs: [
            { targets: '_all', className: 'text-start' }
        ]
    });
});



