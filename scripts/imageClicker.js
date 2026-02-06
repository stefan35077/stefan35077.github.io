const images = document.querySelectorAll('.images');
let currentImageIndex = 0;

function showImage(index) {
    images.forEach((image, i) => {
        if (i === index) {
            image.style.display = 'block';
        } else {
            image.style.display = 'none';
        }
    });
}

function nextImage() {
    currentImageIndex++;
    if (currentImageIndex >= images.length) {
        currentImageIndex = 0;
    }
    showImage(currentImageIndex);
}

function prevImage() {
    currentImageIndex--;
    if (currentImageIndex < 0) {
        currentImageIndex = images.length - 1;
    }
    showImage(currentImageIndex);
}

showImage(currentImageIndex);