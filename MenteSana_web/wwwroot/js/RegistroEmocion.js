document.addEventListener("DOMContentLoaded", function () {

    // --- Selección de emoción ---
    const botones = document.querySelectorAll(".emoji-btn");
    const inputEmocion = document.getElementById("emocionSeleccionada");

    // DEBUG: ver cuántos botones encuentra
    console.log("emoji-btn encontrados:", botones.length);
    console.log("inputEmocion encontrado:", !!inputEmocion);

    botones.forEach(btn => {
        btn.addEventListener("click", function (e) {

            // Evitar que un button dentro de un form haga submit
            e.preventDefault();

            // Quitar selección anterior
            botones.forEach(b => b.classList.remove("selected"));

            // Marcar el botón seleccionado (visual)
            this.classList.add("selected");

            // Guardar el valor en el input oculto
            if (inputEmocion) {
                inputEmocion.value = this.dataset.value;
                console.log("emocionSeleccionada:", inputEmocion.value);
            }
        });
    });

    function mensaje(texto, tipo) {
        const cont = document.getElementById("respuesta");
        if (cont) {
            cont.innerHTML = `<div class="alert alert-${tipo}">${texto}</div>`;
        } else {
            alert(texto); // fallback
        }
    }

});