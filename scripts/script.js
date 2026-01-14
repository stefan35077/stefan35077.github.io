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
});
