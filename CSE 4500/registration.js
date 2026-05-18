document.getElementById("registrationForm").addEventListener("submit", function (event) {
    event.preventDefault();

    let isValid = true;

    // Get values
    const firstName = document.getElementById("firstName").value.trim();
    const lastName = document.getElementById("lastName").value.trim();
    const dob = document.getElementById("dob").value;
    const email = document.getElementById("email").value.trim();
    const phone = document.getElementById("phone").value.trim();
    const country = document.getElementById("country").value;
    const gender = document.querySelector('input[name="gender"]:checked');

    // Regex
    const nameRegex = /^[A-Za-z]+$/;
    const phoneRegex = /^\d{3}[- ]?\d{3}[- ]?\d{4}$/;

    // Clear old errors
    document.getElementById("firstNameError").textContent = "";
    document.getElementById("lastNameError").textContent = "";
    document.getElementById("dobError").textContent = "";
    document.getElementById("emailError").textContent = "";
    document.getElementById("phoneError").textContent = "";
    document.getElementById("genderError").textContent = "";
    document.getElementById("countryError").textContent = "";

    // First name validation
    if (firstName === "") {
        document.getElementById("firstNameError").textContent = "First name cannot be empty.";
        isValid = false;
    } else if (!nameRegex.test(firstName)) {
        document.getElementById("firstNameError").textContent = "First name must contain only letters.";
        isValid = false;
    }

    // Last name validation
    if (lastName === "") {
        document.getElementById("lastNameError").textContent = "Last name cannot be empty.";
        isValid = false;
    } else if (!nameRegex.test(lastName)) {
        document.getElementById("lastNameError").textContent = "Last name must contain only letters.";
        isValid = false;
    }

    // DOB validation
    if (dob === "") {
        document.getElementById("dobError").textContent = "Please select a date of birth.";
        isValid = false;
    }

    // Email validation
    if (email === "") {
        document.getElementById("emailError").textContent = "Email cannot be empty.";
        isValid = false;
    }

    // Phone validation
    if (phone === "") {
        document.getElementById("phoneError").textContent = "Telephone number cannot be empty.";
        isValid = false;
    } else if (!phoneRegex.test(phone)) {
        document.getElementById("phoneError").textContent = "Enter phone as 123-456-7890.";
        isValid = false;
    }

    // Gender validation
    if (!gender) {
        document.getElementById("genderError").textContent = "Please select a gender.";
        isValid = false;
    }

    // Country validation
    if (country === "") {
        document.getElementById("countryError").textContent = "Please select a country.";
        isValid = false;
    }

    // Success
    if (isValid) {
        alert("Registration submitted successfully!");
        document.getElementById("registrationForm").submit();
    }
});