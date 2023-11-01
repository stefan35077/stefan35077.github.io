let succesMessage = document.getElementById("succesfull");
succesMessage.style.display = 'none';

(function () {
    emailjs.init('n-W202hVhZQWKhVwh');

    document.getElementById('contact-form').addEventListener('submit', function (event) {
        event.preventDefault();

        emailjs.send('service_1tqgv0x', 'template_jqs7u2v', {
            name: document.getElementsByName('name')[0].value,
            email: document.getElementsByName('email')[0].value,
            subject: document.getElementsByName('subject')[0].value,
            message: document.getElementsByName('message')[0].value
        })
            .then(response => {
                console.log('Email sent!', response.status, response.text);
                succesMessage.style.display = 'flex';
                succesMessage.innerText = 'Email sent succesfully!';
                succesMessage.style.color = 'green';
                document.getElementById('contact-form').reset();
            }, error => {
                succesMessage.style.display = 'flex'
                succesMessage.innerText = 'An error occurred while sending the email. Please try again later.';
                succesMessage.style.color = 'red';
            });
    });
})();