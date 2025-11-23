$(document).ready(function () {

    // Mostrar horas por defecto al cargar
    let horasDisponibles = [
        "08:00", "09:00", "10:00",
        "11:00", "14:00", "15:00",
        "16:00"
    ];

    function cargarHorasIniciales() {
        $("#hora").empty();
        $("#hora").append(`<option value="">Selecciona una hora</option>`);
        horasDisponibles.forEach(h => {
            $("#hora").append(`<option value="${h}">${h}</option>`);
        });
    }

    cargarHorasIniciales(); // ← esto llena el menú desde el inicio

    $("#fecha, #psicologo").change(function () {

        let fecha = $("#fecha").val();
        let psicologo = $("#psicologo").val();

        if (fecha && psicologo) {

            $.ajax({
                url: '/Citas/ObtenerHorasOcupadas',
                type: 'GET',
                data: { idPsicologo: psicologo, fecha: fecha },

                success: function (horasOcupadas) {

                    $("#hora").empty();
                    $("#hora").append(`<option value="">Selecciona una hora</option>`);

                    horasDisponibles.forEach(h => {
                        if (!horasOcupadas.includes(h)) {
                            $("#hora").append(`<option value="${h}">${h}</option>`);
                        }
                    });
                },

                error: function () {
                    console.log("Error cargando horas ocupadas.");
                }
            });
        }

    });

});
