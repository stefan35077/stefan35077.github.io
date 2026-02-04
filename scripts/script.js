document.addEventListener("DOMContentLoaded", function () {
    const dropMenu = document.getElementById("dropMenu");
    const dropContent = document.getElementById("dropContent");

    if (dropMenu && dropContent) {
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
    }

    const mediaImages = document.querySelectorAll("img.images");
    const extractYouTubeId = (value) => {
        if (!value) {
            return "";
        }

        const trimmed = value.trim();
        if (/^[a-zA-Z0-9_-]{11}$/.test(trimmed)) {
            return trimmed;
        }

        try {
            const url = new URL(trimmed, window.location.origin);
            const host = url.hostname.replace(/^www\./, "");

            if (host === "youtu.be") {
                return url.pathname.replace("/", "");
            }

            if (host.endsWith("youtube.com")) {
                if (url.searchParams.has("v")) {
                    return url.searchParams.get("v");
                }

                const pathMatch = url.pathname.match(/\/(embed|shorts)\/([^/?]+)/);
                if (pathMatch) {
                    return pathMatch[2];
                }
            }
        } catch (error) {
            return "";
        }

        const fallbackMatch = trimmed.match(/(?:v=|\/embed\/|\/shorts\/|youtu\.be\/)([a-zA-Z0-9_-]{11})/);
        return fallbackMatch ? fallbackMatch[1] : "";
    };

    mediaImages.forEach((img) => {
        const youtubeValue = img.getAttribute("data-youtube");
        const customVideo = img.getAttribute("data-video");

        if (youtubeValue) {
            const videoId = extractYouTubeId(youtubeValue);
            if (!videoId) {
                return;
            }

            const wrapper = document.createElement("div");
            wrapper.className = "video-embed";

            const iframe = document.createElement("iframe");
            iframe.src = `https://www.youtube.com/embed/${videoId}`;
            iframe.title = "YouTube video player";
            iframe.setAttribute(
                "allow",
                "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
            );
            iframe.setAttribute("allowfullscreen", "");

            wrapper.appendChild(iframe);
            img.replaceWith(wrapper);
            return;
        }

        if (!customVideo) {
            return;
        }

        const src = img.getAttribute("src") || "";
        const video = document.createElement("video");
        video.className = img.className;
        video.setAttribute("controls", "");
        video.setAttribute("preload", "metadata");
        video.setAttribute("playsinline", "");
        if (src) {
            video.setAttribute("poster", src);
        }
        video.src = customVideo;

        img.replaceWith(video);
    });

    if (!document.getElementById("scriptShelf")) {
        const scriptPrefix = window.location.pathname.includes("/pages/") ? "../scripts/" : "scripts/";
        const defaultScripts = [
            {
                file: "script.js",
                label: { en: "Main UI + media helper", nl: "Hoofd UI + media helper" }
            },
            {
                file: "language.js",
                label: { en: "Language switcher", nl: "Taal wisselaar" }
            },
            {
                file: "contact.js",
                label: { en: "Contact form handler", nl: "Contactformulier handler" }
            },
            {
                file: "imageClicker.js",
                label: { en: "Image click effects", nl: "Klik effecten voor images" }
            }
        ];

        const scripts = Array.isArray(window.projectScripts) ? window.projectScripts : defaultScripts;
        if (!scripts.length) {
            return;
        }

        const shelf = document.createElement("section");
        shelf.className = "script-shelf";
        shelf.id = "scriptShelf";

        const title = document.createElement("div");
        title.className = "script-shelf-title";
        title.innerHTML = `
            <h2 class="English">Project Scripts</h2>
            <h2 class="Dutch">Project scripts</h2>
            <p class="English">Open any script like you would in a game engine.</p>
            <p class="Dutch">Open een script alsof je in een game engine zit.</p>
        `;

        const list = document.createElement("div");
        list.className = "script-shelf-list";

        scripts.forEach((item) => {
            const card = document.createElement("div");
            card.className = "script-card";

            const meta = document.createElement("div");
            meta.className = "script-meta";

            const name = document.createElement("p");
            name.className = "script-name";
            name.innerHTML = `
                <span class="English">${item.label.en}</span>
                <span class="Dutch">${item.label.nl}</span>
            `;

            const file = document.createElement("p");
            file.className = "script-file";
            file.textContent = item.file;

            meta.appendChild(name);
            meta.appendChild(file);

            const action = document.createElement("a");
            action.className = "btn script-open";
            action.href = item.path ? item.path : `${scriptPrefix}${item.file}`;
            action.target = "_blank";
            action.rel = "noopener noreferrer";
            action.innerHTML = `
                <span class="English">Open Script</span>
                <span class="Dutch">Open Script</span>
            `;

            card.appendChild(meta);
            card.appendChild(action);
            list.appendChild(card);
        });

        shelf.appendChild(title);
        shelf.appendChild(list);
        document.body.appendChild(shelf);
    }
});
