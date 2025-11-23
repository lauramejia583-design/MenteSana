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

    // --- Guardar emoción ---
    const btnGuardar = document.getElementById("btnGuardar");
    if (btnGuardar) {
        btnGuardar.addEventListener("click", function (ev) {
            ev.preventDefault();

            // leer desde el input oculto (imagen) OR fallback al select si existe
            let emocion = (document.getElementById("emocionSeleccionada") || {}).value || "";
            if (!emocion) {
                const select = document.getElementById("nombre_estado");
                if (select) emocion = select.value;
            }

            const nota = document.getElementById("nota") ? document.getElementById("nota").value : "";

            if (!emocion) {
                mensaje("Selecciona una emoción antes de guardar.", "danger");
                return;
            }

            fetch(RUTA_GUARDAR, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    nombre_estado: emocion,
                    nota: nota
                })
            })
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        mensaje("Emoción registrada con éxito.", "success");

                        // limpiar
                        if (document.getElementById("nota")) document.getElementById("nota").value = "";
                        if (document.getElementById("nombre_estado")) document.getElementById("nombre_estado").value = "";
                        if (document.getElementById("emocionSeleccionada")) document.getElementById("emocionSeleccionada").value = "";

                        botones.forEach(b => b.classList.remove("selected"));
                    } else {
                        mensaje("Error: " + (data.message || "No se pudo guardar"), "danger");
                    }
                })
                .catch(err => {
                    console.error(err);
                    mensaje("Error en la solicitud.", "danger");
                });
        });
    }

    function mensaje(texto, tipo) {
        const cont = document.getElementById("respuesta");
        if (cont) {
            cont.innerHTML = `<div class="alert alert-${tipo}">${texto}</div>`;
        } else {
            alert(texto); // fallback
        }
    }

});
