let contador = 1;

function agregar() {
    const lista = document.getElementById('lista');
    const nuevoElemento = document.createElement('li');
    nuevoElemento.textContent = `Elemento${contador}`;
    lista.appendChild(nuevoElemento);
    contador++;
}

function cambiarFondo() {
    const body = document.body;
    if (body.style.backgroundColor === 'lightblue') {
        body.style.backgroundColor = 'white';
    } else {
        body.style.backgroundColor = 'lightblue';
    }
}

function borrar() {
    const lista = document.getElementById('lista');
    if (lista.lastElementChild) {
        lista.removeChild(lista.lastElementChild);
        contador = Math.max(1, contador - 1);
    }
}
