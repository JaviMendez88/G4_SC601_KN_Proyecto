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

// Event listener para los botones "Ver detalles"
$(document).on('click', '.btn-ver-detalles', function (e) {
    e.preventDefault();

    const oldValues = $(this).attr('data-old-values');
    const newValues = $(this).attr('data-new-values');
    const accion = $(this).attr('data-accion');

    verDetalle(oldValues, newValues, accion);


});

function verDetalle(oldValues, newValues, accion) {
    const mensaje = "ANTES:\n" + oldValues + "\n\nDESPUÉS:\n" + newValues;

    Swal.fire({
        title: 'Cambios',
        text: mensaje,
    });
}





