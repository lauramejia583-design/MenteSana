flatpickr("#calendario", {
    inline: true,
    locale: "es",
    dateFormat: "Y-m-d",
    defaultDate: "today",
    onChange: function (selectedDates, dateStr) {
        fechaSeleccionada = dateStr || null;
        renderCitas(ordenarLista(filtrarPorFecha(fechaSeleccionada), criterio), contenedor);
    },
});