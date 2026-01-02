document.addEventListener("DOMContentLoaded", function () {
    const dropMenu = document.getElementById("dropMenu");
    const dropContent = document.getElementById("dropContent");

    dropMenu.addEventListener("click", function () {
        dropContent.classList.toggle("show");
    });


    window.addEventListener("click", function (event) {
        const clickedInsideMenu = dropContent.contains(event.target);
        const clickedToggle = event.target === dropMenu;

        if (dropContent.classList.contains("show") && !clickedInsideMenu && !clickedToggle) {
            dropContent.classList.remove("show");
        }
    });
});
