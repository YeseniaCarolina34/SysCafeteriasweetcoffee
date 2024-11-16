// validation.js

function isValidEmail(email) {
    const emailRegex = /^[^@]+@[^@]+\.[^@]+$/;
    return emailRegex.test(email);
}

function isValidPassword(password) {
    return password.length >= 8; // Contraseña debe tener al menos 8 caracteres
}

function validateForm(event) {
    let valid = true;

    const email = document.querySelector('input[name="Login"]').value;
    const password = document.querySelector('input[name="Password"]').value;

    // Limpiar mensajes anteriores
    const emailError = document.querySelector('#email-error');
    const passwordError = document.querySelector('#password-error');
    if (emailError) emailError.remove();
    if (passwordError) passwordError.remove();

    // Validación de correo
    if (!isValidEmail(email)) {
        valid = false;
        const errorMessage = document.createElement('span');
        errorMessage.id = 'email-error';
        errorMessage.classList.add('text-danger');
        errorMessage.textContent = "Por favor ingresa un correo electrónico válido.";
        document.querySelector('input[name="Login"]').parentNode.appendChild(errorMessage);
    }

    // Validación de la contraseña
    if (!isValidPassword(password)) {
        valid = false;
        const errorMessage = document.createElement('span');
        errorMessage.id = 'password-error';
        errorMessage.classList.add('text-danger');
        errorMessage.textContent = "La contraseña debe tener al menos 8 caracteres.";
        document.querySelector('input[name="Password"]').parentNode.appendChild(errorMessage);
    }

    // Si alguna validación falla, se previene el envío del formulario
    if (!valid) {
        event.preventDefault();
    }

    return valid;
}
