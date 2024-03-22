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
        currentImageIndex = 0; // Reset to first image
    }
    showImage(currentImageIndex);
}

function prevImage() {
    currentImageIndex--;
    if (currentImageIndex < 0) {
        currentImageIndex = images.length - 1; // Set to last image
    }
    showImage(currentImageIndex);
}

// Initially display the first image
showImage(currentImageIndex);